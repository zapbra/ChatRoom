using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthentication.Models
{
    [Table("granted_permissions")]
    public class GrantedPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("role_id")]
        [Required(ErrorMessage = "You must provide a role")]
        public long RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        [Column("permission_id")]
        [Required(ErrorMessage = "You must provide a permission")]
        public long PermissionId { get; set; }

        [ForeignKey(nameof(PermissionId))]
        public Permission Permission { get; set; }

    }
}
