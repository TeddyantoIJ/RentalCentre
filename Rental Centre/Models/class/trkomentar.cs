using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("trkomentar")]
    public class trkomentar
    {
        [Key]
        public int id_komentar { get; set; }
        public int id_barang { get; set; }
        public Nullable<int> id_penyewa { get; set; }
        public Nullable<int> id_rental { get; set; }
        public string isi_komentar { get; set; }
        public DateTime creadate { get; set; }
    }
}