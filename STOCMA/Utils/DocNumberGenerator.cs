using System;
using System.Linq;
using STOCMA.Data;

namespace STOCMA.Utils
{
    public class DocNumberGenerator
    {
        private readonly ApplicationDbContext db;
        public DocNumberGenerator(ApplicationDbContext db)
        {
            this.db = db;
        }
        public string getNumDocByCompany(int lastRef, DateTime date)
        {
            var company = db.Companies.FirstOrDefault();
            var companyName = company.Name;

            var newRef = lastRef + 1;

            if (companyName == "SUIV" || companyName == "SBCIT")
                return companyName + "/" + date.ToString("yy") + "/" + date.ToString("MM") + String.Format("/{0:00000}", newRef);

            if (companyName == "AQK")
                return companyName + "/" + date.ToString("yyyyMM") + "0" + newRef;

            return newRef + "/" + date.ToString("yyyy"); ;
        }
    }
}
