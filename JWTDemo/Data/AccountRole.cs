using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JWTDemo.Data
{
    public class AccountRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public int AccountID { get; set; }
        [Required]
        public int RoleID { get; set; }

        [ForeignKey("AccountID")]
        public virtual Account Account { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
    }
}
