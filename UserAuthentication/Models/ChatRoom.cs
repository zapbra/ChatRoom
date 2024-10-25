using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UserAuthentication.Models
{
    [Table("chatroom")]
    public class ChatRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [Required(ErrorMessage = "You must provide a chatroom name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("owner_id")]
        [Required(ErrorMessage = "You must provide an owner (user)")]
        public long OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; }


        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")] // last time there was activity in the chat room
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Column("is_private")] // whether it's public or private (restrictred to certain users)
        public bool IsPrivate { get; set; } = false;

        [Column("is_active")] // whether it's currently active or archived
        public Boolean isActive { get; set; } = true;

        [Column("max_participants")]
        public int MaxParticipants { get; set; }

        [Column("is_password_protected")]
        public bool PasswordProtected { get; set; } = false;

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("topic")]
        public string Topic { get; set; }

        [Column("thumbnail_image_url")]
        public string ThumbnailImageUrl { get; set; }

        public ICollection<ChatRoomUser> ChatRoomUsers { get; } = new List<ChatRoomUser>();


    }
}
