using System;
using System.Collections.Generic;
using System.Text;
using Artel.Telemetry.Domain.Models;

namespace Artel.Telemetry.Domain.BaseClasses
{
    public interface IFarmacyRepository : IDisposable
    {
        void Save();
        void Create(Pharmacies pharmacies);

        IEnumerable<Pharmacies> GetPharmaciesByPosition(double Latitude, double Longitude, double distance);
       
        Pharmacies GetPharmacyPosition(float latitude, float longitude);
    }
}
