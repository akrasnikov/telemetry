using Artel.Telemetry.Common.BaseClasses;
using Artel.Telemetry.Domain.BaseClasses;
using Artel.Telemetry.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artel.Telemetry.Domain.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private ABUMedrepContext db;

        public CustomerRepository()
        {
            this.db = new ABUMedrepContext();
        }

        public IEnumerable<Customers> GetCustomers()
        {
            return db.Customers;
        }
        public Customers GetCustomer(int id)
        {
            return db.Customers.Find(id);
        }

        public void Create(string contactName, string contactPhone, int telegramid, bool isEnabled, DateTime dateOfCreation)
        {
            Customers customer = new Customers
            {
                ContactName = contactName,
                ContactPhone = contactPhone,
                TelegramId = telegramid,
                IsEnabled = isEnabled,
                DateOfCreation = dateOfCreation
            };
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public void Update(Customers customer)
        {
            db.Entry(customer).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Customers customer = db.Customers.Find(id);
            if (customer != null)
                db.Customers.Remove(customer);
        }

        public void Save()
        {
            db.SaveChanges();
        }


        public bool CustomerIsEnabled(Int64 id)
        {

            var customer = db.Customers.FirstOrDefault(x => x.TelegramId == id);
            return customer is Customers ? customer.IsEnabled : false;            
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}
