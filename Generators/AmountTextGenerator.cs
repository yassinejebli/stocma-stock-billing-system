using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Generators
{
    public class AmountTextGenerator
    {
        public string DecimalToWords(Decimal number)
        {
            if (number == Decimal.Zero)
                return "zero";
            if (number < Decimal.Zero)
                return "moins " + this.DecimalToWords(Math.Abs(number));
            int chiffre = (int)number;
            int num = (int)((number - chiffre) * 100);
            string str = ConvertisseurChiffresLettres.converti(chiffre) + "dhs TTC";
            if (num > 0)
            {
                str = str.Replace("TTC", "");
                str = str + " " + num.ToString() + "cts TTC";
            }
            str = str.ToUpper();
            string prefix = "UN";
            return str.StartsWith(prefix) ? str.Substring(prefix.Length) : str;
        }
    }
}