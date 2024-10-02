using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthentication.Models
{
    [Table("hashing_algorithms")]
    public class HashAlgorithm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("algorithm_name")]
        [Required(ErrorMessage = "You must provide an algorithm name ")]
        public string AlgorithmName { get; set; }
    }
}
