﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Artel.Telemetry.Domain.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PQMSEntities : DbContext
    {
        public PQMSEntities()
            : base("name=PQMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Line> Lines { get; set; }
        public virtual DbSet<Operator> Operators { get; set; }
        public virtual DbSet<Diller> Dillers { get; set; }
        public virtual DbSet<HistoryDiller> HistoryDillers { get; set; }
    }
}
