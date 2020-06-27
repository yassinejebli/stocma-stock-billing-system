using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class DepenseItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid IdTypeDepense { get; set; }
        public Guid IdDepense { get; set; }

        public float Montant { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual TypeDepense TypeDepense { get; set; }
        public virtual Depense Depense { get; set; }
    }
}
