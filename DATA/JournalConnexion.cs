﻿// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.JournalConnexion
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
  public class JournalConnexion
  {
    [Key]
    public Guid Id { get; set; }

    public string User { get; set; }

    public DateTime Date { get; set; }
  }
}
