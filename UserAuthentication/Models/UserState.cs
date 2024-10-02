using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserAuthentication.Models
{
    [Table("user_state")]
    public class UserState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("state_uuid")]
        public string StateUUID { get; set; } = Guid.NewGuid().ToString();

        [Column("expiry_time")]
        public DateTime ExpiryTime { get; set; }
        public bool Authenticated { get; set; } = true;
        [Column("user_id")]
        [Required(ErrorMessage = "You must provide a user id")]
        public long UserId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

    }
}
