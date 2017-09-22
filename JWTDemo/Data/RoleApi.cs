using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JWTDemo.Data
{
    public class RoleApi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public int RoleID { get; set; }
        [Required]
        public int ApiID { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
        [ForeignKey("ApiID")]
        public virtual ApiCollection Apis { get; set; }
    }
}
