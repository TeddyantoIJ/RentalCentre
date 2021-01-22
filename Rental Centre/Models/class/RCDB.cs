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
        public DbSet<msrental> msrental { get; set; }

        //Transaksi

        public DbSet<dtchat> dtchat { get; set; }
        public DbSet<dtdetailpenyewaan> dtdetailpenyewaan { get; set; }
        public DbSet<dtkomentar> dtkomentar { get; set; }
        public DbSet<dtmutasisaldo> dtmutasisaldo { get; set; }
        public DbSet<dttracking> dttracking { get; set; }
        public DbSet<trkeranjang> trkeranjang { get; set; }
        public DbSet<trkomentar> trkomentar { get; set; }
        public DbSet<trkritiksaran> trkritiksaran { get; set; }
        public DbSet<trnotifikasi> trnotifikasi { get; set; }
        public DbSet<trpembayaran> trpembayaran { get; set; }
        public DbSet<trpenyewaan> trpenyewaan { get; set; }
        public DbSet<trwishlist> trwishlist { get; set; }
        public DbSet<trtopup> trtopup { get; set; }
        public DbSet<trtransfer> trtransfer { get; set; }

    }
}