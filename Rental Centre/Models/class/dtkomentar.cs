using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("dtkomentar")]
    public class dtkomentar
    {
        [Key]
        public int id_dtkomentar { get; set; }
        public int id_komentar { get; set; }
        public Nullable<int> id_penyewa { get; set; }
        public Nullable<int> id_rental { get; set; }
        public string isi_komentar { get; set; }
        public DateTime creadate { get; set; }
    }
}