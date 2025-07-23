using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities {
    [Table("tGitHubOptions")]
    public class tGitHubOptions {
        public string UserName { get; set; }
        public string BToken { get; set; }
        public string PageSize { get; set; }
    }
    public class tGitHubOptionsConfiguration : IEntityTypeConfiguration<tGitHubOptions> {
        public void Configure(EntityTypeBuilder<tGitHubOptions> builder) {
            builder.HasKey(r => r.UserName);
        }
    }
}