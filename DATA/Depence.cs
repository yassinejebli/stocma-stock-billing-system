using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
  public class Depence
  {
    [Key]
    public Guid Id { get; set; }

    public Guid IdTypeDepence { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string Comment { get; set; }

    [Required]
    public float Montant { get; set; }

    public virtual TypeDepence TypeDepence { get; set; }
  }
}
