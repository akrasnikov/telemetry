using Artel.Telemetry.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artel.Telemetry.Domain.BaseClasses
{
    public interface ICustomerRepository:IDisposable
    {
        Customers GetCustomer(int id);
        void Create(string contactName, string contactPhone, int telegramid, bool isEnabled, DateTime dateOfCreation);
        bool CustomerIsEnabled(Int64 id);
    }
}
