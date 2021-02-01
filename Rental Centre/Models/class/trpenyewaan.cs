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
        public int id_penyewaan { get; set; }
        public int id_penyewa { get; set; }
        public Nullable<int> id_admin { get; set; }
        public DateTime tgl_penyewaan { get; set; }
        public DateTime tgl_pengembalian { get; set; }
        public int total_dp { get; set; }
        public int total_harga { get; set; }
        // 0 DIKIRIM, 1 DIJEMPUT
        public int jenis_sewa { get; set; }
        public string alamat_tujuan { get; set; }
        public string kodepos { get; set; }
        public DateTime creadate { get; set; }
        public string status_transaksi { get; set; }
        // [PEMESANAN],[VALIDASI DP], [DISIAPKAN],[SIAP / DIKIRIM],[BERJALAN],[SELESAI], [GAGAL]
        public int status_dp { get; set; }
        // 0 = BLM, 1 = SUDAH
        public int status_pembayaran { get; set; }
    }
}