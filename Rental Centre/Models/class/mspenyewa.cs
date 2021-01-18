using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("mspenyewa")]
    public class mspenyewa
    {
        [Key]
        public int id_penyewa { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string nama_penyewa { get; set; }
        public string profil { get; set; }
        public string alamat { get; set; }
        public string kodepos { get; set; }
        public int jenis_kelamin { get; set; }
        public string tempat_lahir { get; set; }
        public DateTime tgl_lahir { get; set; }
        public string email { get; set; }
        public string no_telepon { get; set; }
        public int saldo { get; set; }
        public string NIK { get; set; }
        public string berkas1 { get; set; }
        public string berkas2 { get; set; }
        public int status { get; set; }
        public string creaby { get; set; }
        public DateTime creadate { get; set; }
        public string modiby { get; set; }
        public Nullable<DateTime> modidate { get; set; }
        public Nullable<int> modiadminby { get; set; }
        public Nullable<DateTime> modiadmindate { get; set; }
    }
}