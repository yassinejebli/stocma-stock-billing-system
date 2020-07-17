using System;

namespace WebApplication1
{
  public class StatistiqueEspece
  {
    public DateTime Date { get; set; }

    public float totalDepense { get; set; }

    public float totalCredit { get; set; }

    public StatistiqueEspece(DateTime Date, float totalDepense, float totalCredit)
    {
      this.Date = Date;
      this.totalDepense = totalDepense;
      this.totalCredit = totalCredit;
    }
  }
}
