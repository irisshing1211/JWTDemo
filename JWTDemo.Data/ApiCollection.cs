using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JWTDemo.Data
{
    public class ApiCollection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string ApiName { get; set; }
        [Required]
        public string Path { get; set; }

        public virtual ICollection<RoleApi> RoleApi { get; set; }
    }
}
