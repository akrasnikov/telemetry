using Artel.Telemetry.Domain.BaseClasses;
using Artel.Telemetry.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artel.Telemetry.Domain.Data
{
    public class ProductRepository : IProductRepository
    {
        private PQMSEntities db;

        public ProductRepository()
        {
            this.db = new PQMSEntities();
        }
        public Line GetProductByBarcode(string barcode)
        {
            var line = db.Lines.FirstOrDefault(x => x.Barcode == barcode);
            return line;
        }

        public void Dispose()
        {
            db.Dispose();
            GC.SuppressFinalize(db);
        }

        public string[] Picture(long barcode)
        {
            throw new NotImplementedException();
        }

        public void ClearBarcodeCRLF()
        {
            var barcode = db.Lines.ToList();
            foreach (var b in barcode)
            {
                var s = b.Barcode;
                s = s.TrimEnd(new char[] { '\r', '\n' });
                b.Barcode = s;
                //db.Lines.Update(b);               
            }
            db.SaveChanges();
        }
    }
}
