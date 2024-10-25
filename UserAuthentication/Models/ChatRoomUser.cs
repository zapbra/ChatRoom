using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserAuthentication.Models
{

    [Table("chatroom_user")]
    public class ChatRoomUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("chatroom_id")]
        [Required(ErrorMessage = "You must provide a chatroom")]
        public long ChatRoomId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ChatRoomId))]
        public ChatRoom ChatRoom { get; set; }

        [Column("user_id")]
        [Required(ErrorMessage = "You must provide a user")]
        public long UserId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Column("role")]
        public string Role { get; set; }



    }
}
