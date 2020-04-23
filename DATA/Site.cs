using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.DATA
{
    public class Site
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = false)]
        [StringLength(200)]
        public string Name { get; set; }

        public string Address { get; set; }

        public virtual ICollection<ArticleSite> ArticleSites { get; set; }


    }
}
