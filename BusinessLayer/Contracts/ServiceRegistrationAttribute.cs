using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BusinessLayer.Contracts;
// 1. Classe Base per l'Attributo (Sostituisce ValidationAttribute)
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public abstract class ServiceRegistrationAttribute : Attribute {
    // Tipi necessari per la registrazione condizionale
    public Type InterfaceType { get; }
    public Type ImplementationType { get; }
    public bool DisableServiceRegistrationOnFailure { get; set; }

    // Tipo del logger/reporter per scrivere la regola di mancata convalida
    public Type LoggerType { get; }

    public ServiceRegistrationAttribute(
        Type interfaceType,
        Type implementationType,
        Type loggerType) {
        InterfaceType = interfaceType ?? throw new ArgumentNullException(nameof(interfaceType));
        ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        LoggerType = loggerType ?? throw new ArgumentNullException(nameof(loggerType));
    }

    // Metodo Astratto: La validazione specifica della regola deve essere implementata qui.
    // Restituisce il messaggio di errore se fallisce, altrimenti null o stringa vuota.
    public abstract string Validate(object value, string propertyName);

    // Metodo Concreto: Esegue la scrittura della regola in caso di fallimento.
    public void WriteFailureRule(string propertyName, object value, string errorMessage) {
        // Questo richiede di creare un'istanza del logger (fuori dal container DI)
        if (Activator.CreateInstance(LoggerType) is IConfigurationLogger logger) {
            logger.WriteValidationErrorRule(propertyName, value, errorMessage);
        }
    }
}

// 2. Attributo Concreto: Implementazione della regola di Lunghezza Minima
public class MinLengthRegistrationAttribute : ServiceRegistrationAttribute {
    private readonly int _minLength;

    public MinLengthRegistrationAttribute(
        int minLength,
        Type interfaceType,
        Type implementationType,
        Type loggerType)
        : base(interfaceType, implementationType, loggerType) {
        _minLength = minLength;
    }

    // Implementazione del metodo astratto: la logica di validazione vera e propria
    public override string Validate(object value, string propertyName) {
        if (value is string str && str.Length < _minLength) {
            return $"[{propertyName}] Fallimento: La stringa deve avere almeno {_minLength} caratteri, ma ne ha {str.Length}.";
        }
        return null; // Successo
    }
}

// Interfacce e Loggers (come definiti nel flusso precedente)
public interface IFeatureService { void Run(); }
public class DefaultFeatureService : IFeatureService { public void Run() => Console.WriteLine("Running Default."); }
public interface IConfigurationLogger { void WriteValidationErrorRule(string propertyName, object value, string errorMessage); }
public class ConsoleLogger : IConfigurationLogger {
    public void WriteValidationErrorRule(string propertyName, object value, string errorMessage) =>
        GlobalLogger.Errors.Add(new LogErrorEntry {
            ContextInfo = $"Validation failed for {propertyName} with value '{value}'",
            ErrorMessage = errorMessage,
            SinkName = "ConfigurationValidation",
            Timestamp = DateTime.UtcNow
        });
}

// 3. Classe di configurazione che usa il nuovo attributo
public class FeatureSettings {
    // SCENARIO 1: Successo (La stringa è più lunga di 5)
    // IFeatureService verrà registrato
    [MinLengthRegistration(
        minLength: 5,
        interfaceType: typeof(IFeatureService),
        implementationType: typeof(DefaultFeatureService),
        loggerType: typeof(ConsoleLogger))]
    public string FeatureName { get; set; } = "HighAvailability";

    // SCENARIO 2: Fallimento (La stringa "KEY" è lunga 3)
    // Disattivazione attiva -> ISecretService NON verrà registrato
    [MinLengthRegistration(
        minLength: 10,
        interfaceType: typeof(ISecretService),
        implementationType: typeof(SecretManagerService),
        loggerType: typeof(ConsoleLogger),
        DisableServiceRegistrationOnFailure = true)]
    public string SecretKey { get; set; } = "KEY";
}

public interface ISecretService { }
public class SecretManagerService : ISecretService { }


public static class ServiceCollectionExtensions {
    public static IServiceCollection AddCustomValidatedServices<TConfig>(
        this IServiceCollection services,
        TConfig configInstance) where TConfig : class {
        var configType = typeof(TConfig);

        foreach (var property in configType.GetProperties()) {
            // Cerchiamo il nostro attributo base personalizzato
            var attribute = property.GetCustomAttributes(typeof(ServiceRegistrationAttribute), true)
                                    .FirstOrDefault() as ServiceRegistrationAttribute;

            if (attribute == null)
                continue;

            object propertyValue = property.GetValue(configInstance);
            string propertyName = property.Name;

            // 1. Esecuzione della validazione tramite il metodo astratto dell'attributo
            string errorMessage = attribute.Validate(propertyValue, propertyName);
            bool validationSucceeded = string.IsNullOrEmpty(errorMessage);

            // 2. Scrittura della regola di mancata convalida (se fallita)
            if (!validationSucceeded) {
                attribute.WriteFailureRule(propertyName, propertyValue, errorMessage);
            }

            // 3. Controllo della Registrazione
            bool shouldRegister = validationSucceeded || !attribute.DisableServiceRegistrationOnFailure;

            if (shouldRegister) {
                // Registra l'interfaccia e la sua concretizzazione
                services.AddSingleton(attribute.InterfaceType, attribute.ImplementationType);
                Console.WriteLine($"✅ Registrazione DI: {attribute.InterfaceType.Name}");
            } else {
                services.RemoveAll(attribute.InterfaceType);
                // Skip della registrazione per violazione e DisableServiceRegistrationOnFailure = true
                Console.WriteLine($"❌ SKIP Registrazione DI: {attribute.InterfaceType.Name} causa violazione su {propertyName}.");
            }

            // (Opzionale) Assicurati che il logger sia sempre registrato
            if (!services.Any(s => s.ServiceType == attribute.LoggerType)) {
                services.AddSingleton(attribute.LoggerType);
            }
        }
        return services;
    }
}