﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserAuthentication.Models
{
    [Table("user_login_data_external")]
    public class UserLoginDataExternal
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

        [Column("external_provider_id")]
        public long ExternalProviderId { get; set; }

        [ForeignKey(nameof(ExternalProviderId))]
        public ExternalProvider ExternalProvider { get; set; }
    }
}
