// Decompiled with JetBrains decompiler
// Type: WebApplication1.ConvertisseurChiffresLettres
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

namespace WebApplication1
{
  internal class ConvertisseurChiffresLettres
  {
    public static string converti(int chiffre)
    {
      bool flag = false;
      string str = "";
      int num1 = chiffre / 1;
      int num2 = 1000000000;
      while (num2 >= 1)
      {
        int num3 = num1 / num2;
        if (num3 != 0)
        {
          int num4 = num3 / 100;
          int num5 = (num3 - num4 * 100) / 10;
          int num6 = num3 - num4 * 100 - num5 * 10;
          switch (num4)
          {
            case 1:
              str += "cent ";
              break;
            case 2:
              str = num5 != 0 || num6 != 0 ? str + "deux cent " : str + "deux cents ";
              break;
            case 3:
              str = num5 != 0 || num6 != 0 ? str + "trois cent " : str + "trois cents ";
              break;
            case 4:
              str = num5 != 0 || num6 != 0 ? str + "quatre cent " : str + "quatre cents ";
              break;
            case 5:
              str = num5 != 0 || num6 != 0 ? str + "cinq cent " : str + "cinq cents ";
              break;
            case 6:
              str = num5 != 0 || num6 != 0 ? str + "six cent " : str + "six cents ";
              break;
            case 7:
              str = num5 != 0 || num6 != 0 ? str + "sept cent " : str + "sept cents ";
              break;
            case 8:
              str = num5 != 0 || num6 != 0 ? str + "huit cent " : str + "huit cents ";
              break;
            case 9:
              str = num5 != 0 || num6 != 0 ? str + "neuf cent " : str + "neuf cents ";
              break;
          }
          switch (num5)
          {
            case 1:
              flag = true;
              break;
            case 2:
              str += "vingt ";
              break;
            case 3:
              str += "trente ";
              break;
            case 4:
              str += "quarante ";
              break;
            case 5:
              str += "cinquante ";
              break;
            case 6:
              str += "soixante ";
              break;
            case 7:
              flag = true;
              str += "soixante ";
              break;
            case 8:
              str += "quatre-vingt ";
              break;
            case 9:
              flag = true;
              str += "quatre-vingt ";
              break;
          }
          switch (num6)
          {
            case 0:
              if (flag)
              {
                str += "dix ";
                break;
              }
              break;
            case 1:
              str = !flag ? str + "un " : str + "onze ";
              break;
            case 2:
              str = !flag ? str + "deux " : str + "douze ";
              break;
            case 3:
              str = !flag ? str + "trois " : str + "treize ";
              break;
            case 4:
              str = !flag ? str + "quatre " : str + "quatorze ";
              break;
            case 5:
              str = !flag ? str + "cinq " : str + "quinze ";
              break;
            case 6:
              str = !flag ? str + "six " : str + "seize ";
              break;
            case 7:
              str = !flag ? str + "sept " : str + "dix-sept ";
              break;
            case 8:
              str = !flag ? str + "huit " : str + "dix-huit ";
              break;
            case 9:
              str = !flag ? str + "neuf " : str + "dix-neuf ";
              break;
          }
          if (num2 != 1000)
          {
            if (num2 != 1000000)
            {
              if (num2 == 1000000000)
                str = num3 <= 1 ? str + "milliard " : str + "milliards ";
            }
            else
              str = num3 <= 1 ? str + "million " : str + "millions ";
          }
          else
            str += "mille ";
        }
        num1 -= num3 * num2;
        flag = false;
        num2 /= 1000;
      }
      if (str.Length == 0)
        str += "zero";
      return str;
    }
  }
}
