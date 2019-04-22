using Artel.Telemetry.Domain.BaseClasses;
using Artel.Telemetry.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artel.Telemetry.Domain.Data
{
    public class PharmacyRepository : IFarmacyRepository
    {
        private ABUMedrepContext db;

        public PharmacyRepository()
        {
            this.db = new ABUMedrepContext();
        }
       

        public void Dispose()
        {
            db.Dispose();
        }
        public void Create(Pharmacies pharmacies)
        {           
            db.Pharmacies.Add(pharmacies);
            var i = db.SaveChanges();
        }


        public void Save()
        {
            db.SaveChangesAsync();
        }

        public IEnumerable<Pharmacies> GetPharmaciesByPosition(double latitude, double longitude , double distance)
        {
            return from p in db.Pharmacies
                   where p.Latitude <= latitude + distance
                   where p.Latitude >= latitude - distance

                   where p.Longitude <= longitude + distance
                   where p.Longitude >= longitude - distance
                   select p;
                           
        }

        public Pharmacies GetPharmacyPosition(float latitude, float longitude)
        {
            throw new NotImplementedException();
        }
    }
}
