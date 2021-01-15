using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Rental_Centre.Models
{
    public class RCDB : DbContext
    {
        public RCDB() : base("RCDB") { }
        public DbSet<mskelompokjenis> mskelompokjenis { get; set; }
        public DbSet<msjenisbarang> msjenisbarang { get; set; }
        public DbSet<msbarang> msbarang { get; set; }
        public DbSet<msadmin> msadmin { get; set; }
        public DbSet<msprovinsi> msprovinsi { get; set; }
        public DbSet<mspenyewa> mspenyewa { get; set; }
        public DbSet<mskodepos> mskodepos { get; set; }
    }
}