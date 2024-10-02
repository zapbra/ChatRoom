using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthentication.Models
{
    [Table("user_roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("role_name")]
        [Required(ErrorMessage = "You must provide a role name")]
        [MaxLength(20)]
        public string RoleName { get; set; } 
        public ICollection<User>? Users { get; } = new List<User>();
        public ICollection<GrantedPermission>? GrantedPermissions { get; } = new List<GrantedPermission>();
    }
}
