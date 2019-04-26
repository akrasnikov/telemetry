using Artel.Telemetry.Domain.Model;
using System;


namespace Artel.Telemetry.Domain.BaseClasses
{
    public interface ICustomerRepository : IDisposable
    {
        Diller GetCustomer(int id);
        void Create(string contactName, string contactPhone, int telegramid, bool isEnabled, DateTime dateOfCreation);
        bool CustomerIsEnabled(Int64 id);
        string Language(long telegramid);
        void SetLanguage(long id, string language);
    }
}
