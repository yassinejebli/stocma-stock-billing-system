using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.DATA;

namespace WebApplication1.Generators
{
    public class DocNumberGenerator
    {
        MySaniSoftContext db = new MySaniSoftContext();
        public string getNumDocByCompany(int lastRef, DateTime date)
        {
            var company = db.Companies.FirstOrDefault();
            var companyName = company.Name.ToUpper();

            var newRef = lastRef + 1;

            if(companyName == "SUIV" || companyName == "SBCIT")
                return companyName + "/" + date.ToString("yy") + "/" + date.ToString("MM") + String.Format("/{0:00000}", newRef);

            if (companyName == "AQK")
                return companyName+"/"+date.ToString("yyyyMM") + newRef;

            return newRef + "/" + date.ToString("yyyy"); ;
        }
    }
}