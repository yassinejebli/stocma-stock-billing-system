// Decompiled with JetBrains decompiler
// Type: WebApplication1.StatistiqueEspeceParMois
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

namespace WebApplication1
{
  public class StatistiqueEspeceParMois
  {
    public string Date { get; set; }

    public float totalDepense { get; set; }

    public float totalCredit { get; set; }

    public StatistiqueEspeceParMois(string Date, float totalDepense, float totalCredit)
    {
      this.Date = Date;
      this.totalDepense = totalDepense;
      this.totalCredit = totalCredit;
    }
  }
}
