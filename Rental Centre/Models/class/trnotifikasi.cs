using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("trnotifikasi")]
    public class trnotifikasi
    {
        [Key]
        public int id_notifikasi { get; set; }
        public Nullable<int> id_rental { get; set; }
        public Nullable<int> id_penyewaan { get; set; }
        public Nullable<int> id_admin { get; set; }
        public string isi_notifikasi { get; set; }
        public int dilihat { get; set; }
        public DateTime creadate { get; set; }
    }
}