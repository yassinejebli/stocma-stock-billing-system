﻿// Decompiled with JetBrains decompiler
// Type: WebApplication1.MargeSomme
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

namespace WebApplication1
{
  public class MargeSomme
  {
    public int Mois { get; set; }

    public float somme { get; set; }

    public float margeBL { get; set; }

    public MargeSomme(int Mois, float somme, float margeBL)
    {
      this.Mois = Mois;
      this.somme = somme;
      this.margeBL = margeBL;
    }
  }
}
