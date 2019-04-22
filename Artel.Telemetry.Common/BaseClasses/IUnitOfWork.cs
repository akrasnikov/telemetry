using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artel.Telemetry.Common.BaseClasses
{
    public interface IUnitOfWork:IDisposable
    {
        void Commit();
        void Rollback();
    }
}
