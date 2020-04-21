using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication1.DATA
{
    public class Revendeur
    {
        [Key]
        public Guid Id { get; set; }


        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Name { get; set; }

        public virtual ICollection<Client> Clients { get; set; }


    }
}