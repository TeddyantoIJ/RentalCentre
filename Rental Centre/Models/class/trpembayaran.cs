using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 
namespace Rental_Centre.Models
{
    [Table("trpembayaran")]
    public class trpembayaran
    {
        [Key]
        public int id_pembayaran { get; set; }
        public Nullable<int> id_penyewaan { get; set; }
        public int jenis_pembayaran { get; set; }
        //0 = DP, 1 = SISA
        public int jml_dibayar { get; set; }
        public string bukti_pembayaran { get; set; }
        public int validate { get; set; }
        // 0 blm diapprove , 1 sudah
        public Nullable<int> id_admin { get; set; }
        public DateTime tgl_validasi { get; set; }
        public DateTime creadate { get; set; }
    }
}