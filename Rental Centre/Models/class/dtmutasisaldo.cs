using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Rental_Centre.Models
{
    [Table("dtmutasisaldo")]
    public class dtmutasisaldo
    {
        [Key]
        public int id_mutasisaldo { get; set; }
        public Nullable<int> id_penyewa { get; set; }
        public Nullable<int> id_rental { get; set; }
        public Nullable<int> id_admin { get; set; }
        public int jumlah_transaksi { get; set; }
        public int jumlah_saldo { get; set; }
        public string jenis_transaksi { get; set; }
        public DateTime creadate { get; set; }
    }
}