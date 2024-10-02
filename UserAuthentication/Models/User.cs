using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthentication.Models
{
    [Table("chat_user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("role_id")]
        [Required(ErrorMessage = "You must provide a user role")]
        public long RoleId { get; set; }

        [ForeignKey(nameof(RoleId))] 
        public Role? Role { get; set; }

        public UserLoginDataExternal? UserLoginDataExternal { get; set; }
        public UserLogin? UserLogin { get; set; }

        public UserState UserState { get; set; }
    }
}
