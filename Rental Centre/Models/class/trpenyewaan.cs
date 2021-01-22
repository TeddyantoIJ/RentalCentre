using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("trpenyewaan")]
    public class trpenyewaan
    {
        [Key]
        public int id_penyewewaan { get; set; }
        public int id_penyewa { get; set; }
        public Nullable<int> id_admin { get; set; }
        public DateTime tgl_penyewaan { get; set; }
        public DateTime tgl_pengembalian { get; set; }
        public int total_dp { get; set; }
        public int total_harga { get; set; }
        public int jenis_sewa { get; set; }
        public string alamat_tujuan { get; set; }
        public DateTime creadate { get; set; }
        public string status_transaksi { get; set; }
        // [PENDING],[DISIAPKAN],[BERJALAN],[SELESAI]
        public int status_dp { get; set; }
        public int status_pembayaran { get; set; }
    }
}