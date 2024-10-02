using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthentication.Models
{
    [Table("permissions")]
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("permissions_description")]
        [MaxLength(100)]
        [Required(ErrorMessage = "You must provide a permissions description")]
        public string PermissionDescription { get; set; }
        public ICollection<GrantedPermission> GrantedPermissions { get; } = new List<GrantedPermission>();
    }
}
