using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("msrental")]
    public class msrental
    {
        [Key]
        public int id_rental { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string nama_rental { get; set; }
        public string nama_toko { get; set; }
        public string no_telp { get; set; }
        public string NIK { get; set; }
        public string email { get; set; }
        public string tempat_lahir { get; set; }
        public DateTime tgl_lahir { get; set; }
        public string alamat { get; set; }
        public string alamat_toko { get; set; }
        public string kodepos { get; set; }
        public int jenis_kelamin { get; set; }
        public string profil { get; set; }
        public string nama_bank { get; set; }
        public string no_rek { get; set; }
        public int jml_barang { get; set; }
        public int status { get; set; }
        public int saldo { get; set; }
        public double rating { get; set; }
        public DateTime creadate { get; set; }
        public Nullable<DateTime> modidate { get; set; }
        public string berkas1 { get; set; }
        public string berkas2 { get; set; }
    }
}