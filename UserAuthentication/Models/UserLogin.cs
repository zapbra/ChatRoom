using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UserAuthentication.Utilities;


namespace UserAuthentication.Models
{
    [Table("user_login")]
    public class UserLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        [Required(ErrorMessage = "You must provide a user")]
        public long UserId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Column("username")]
        [Required(ErrorMessage = "You must provide a username")]
        [MaxLength(50)]
        public string Username { get; set; }

        [Column("password_hash")]
        [Required(ErrorMessage = "You must provide a password")]
        [MaxLength(250)]
        public string PasswordHash { get; set; }

        [Column("password_salt")]
        [Required(ErrorMessage = "You must provide a password salt")]
        [MaxLength(250)]
        public string PasswordSalt { get; set; }

        [Column("email")]
        [Required(ErrorMessage = "You must provide an email address")]
        [MaxLength(255)]
        public string Email { get; set; }

        [Column("confirmation_token")]
        [MaxLength(100)]
        public string ConfirmationToken { get; set; } = TokenGenerator.GenerateConfirmationToken();

        [Column("token_generation_time")]
        public DateTime TokenGenerationTime { get; set; } = DateTime.UtcNow;

        [Column("hash_algorithm_id")]
        public long? HashAlgorithmId {  get; set; }

        [ForeignKey(nameof(HashAlgorithmId))]
        public HashAlgorithm? HashAlgorithm { get; set; }

        [Column("email_validation_status_id")]
        public long EmailValidationId { get; set; }

        [ForeignKey(nameof(EmailValidationId))]
        public EmailValidationStatus EmailValidationStatus { get; set; } = new EmailValidationStatus(); // Creates the default, which is "Pending" and false values

        [Column("password_recovery_token")]
        public string? PasswordRecoveryToken { get; set; }

        [Column("recovery_token_time")]
        public DateTime? RecoveryTokenTime { get; set; }

    }
}
