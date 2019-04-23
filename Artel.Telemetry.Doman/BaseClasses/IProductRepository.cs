using Artel.Telemetry.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artel.Telemetry.Domain.BaseClasses
{
    public interface IProductRepository : IDisposable
    {
        string[] Picture(long barcode);
        Line GetProductByBarcode(string barcode);
    }
}
