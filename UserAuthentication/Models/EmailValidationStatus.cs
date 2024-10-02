using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthentication.Models
{
    [Table("email_validation_status")]
    public class EmailValidationStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("status_description")]
        public string? StatusDescription { get; set; } = "Pending";

        [Column("isValidated")]
        public bool? IsValidated { get; set; } = false;
    }
}
