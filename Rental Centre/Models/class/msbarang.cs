using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("msbarang")]
    public class msbarang
    {
        [Key]
        public int id_barang { get; set; }
        public Nullable<int> id_jenisbarang { get; set; }
        public Nullable<int> id_rental { get; set; }
        public string nama_barang { get; set; }
        public Nullable<int> harga_sewa { get; set; }
        public Nullable<int> stok_barang { get; set; }
        public string deskripsi_barang { get; set; }
        public string link_gambar { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> creaby { get; set; }
        public Nullable<System.DateTime> creadate { get; set; }
        public Nullable<int> modiby { get; set; }
        public Nullable<System.DateTime> modidate { get; set; }
    }
}