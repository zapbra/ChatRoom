using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthentication.Models
{
    [Table("external_providers")]
    public class ExternalProvider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("provider_name")]
        [Required(ErrorMessage = "You must provide a Provider Name")] 
        [MaxLength(50)]
        public string ProviderName { get; set; }

        [Column("ws_endpoint")]
        public string? WSEndpoint { get; set; }
        public ICollection<UserLoginDataExternal> UserLoginDataExternals { get; } = new List<UserLoginDataExternal>();

    }
}
