﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rental_Centre.Models.RentalModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RCDBEntities : DbContext
    {
        public RCDBEntities()
            : base("name=RCDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<mskelompokjeni> mskelompokjenis { get; set; }
        public virtual DbSet<mskodepos> mskodepos { get; set; }
        public virtual DbSet<msprovinsi> msprovinsis { get; set; }
        public virtual DbSet<msbarang> msbarangs { get; set; }
    }
}