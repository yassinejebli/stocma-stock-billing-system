using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Generators
{
    public class DocNumberGenerator
    {

        public string getNumDocByCompany(int lastRef, string companyName, DateTime date)
        {
            var newRef = lastRef + 1;

            if(companyName == "SUIV" || companyName == "SBCIT")
                return companyName + "/" + date.ToString("yy") + "/" + date.ToString("MM") + String.Format("/BLD/{0:000000}", newRef);

            if (companyName == "AQK")
                return companyName+"/"+date.ToString("yyyyMM") + newRef;

            return newRef + "/" + date.ToString("yyyy"); ;
        }
    }
}