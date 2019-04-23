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
            return db.Lines.FirstOrDefault(x => x.Barcode == barcode);
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
    }
}
