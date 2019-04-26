
using Artel.Telemetry.Domain.BaseClasses;
using Artel.Telemetry.Domain.Model;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Artel.Telemetry.Domain.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private PQMSEntities db;

        public CustomerRepository()
        {
            this.db = new PQMSEntities();
        }

        public IEnumerable<Diller> GetCustomers()
        {
            return db.Dillers;
        }
        public Diller GetCustomer(int id)
        {
            return db.Dillers.Find(id);
        }

        public void Create(string contactName, string contactPhone, int telegramid, bool isEnabled, DateTime dateOfCreation)
        {
            Diller customer = new Diller
            {
                ContactName = contactName,
                ContactPhone = contactPhone,
                TelegramId = telegramid,
                IsEnabled = isEnabled,
                DateTime = dateOfCreation
            };
            db.Dillers.Add(customer);
            db.SaveChanges();
        }

        public void Update(Diller customer)
        {
            db.Entry(customer).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Diller customer = db.Dillers.Find(id);
            if (customer != null)
                db.Dillers.Remove(customer);
        }

        public void Save()
        {
            db.SaveChanges();
        }


        public bool CustomerIsEnabled(Int64 id)
        {

            var customer = db.Dillers.FirstOrDefault(x => x.TelegramId == id);
            return customer is Diller ? (bool)customer.IsEnabled : false;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public string Language(long telegramid)
        {
            var customer = db.Dillers
                .Where(c => c.TelegramId == telegramid).FirstOrDefault();
            return customer.Language;
        }

        public void SetLanguage(long id, string language)
        {
            var customer = db.Dillers
                .Where(c => c.TelegramId == id).FirstOrDefault();
            customer.Language = language;
            db.SaveChangesAsync();
        }
    }
}
