using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("trtransfer")]
    public class trtransfer
    {
        [Key]
        public int id_trtransfer { get; set; }
        public int id_pengirim { get; set; }
        public int id_penerima { get; set; }
        
        /*
        1 = Penyewa -> Penyewa
        2 = Penyewa -> Rental
        3 = Rental -> Rental
        4 = Rental -> Penyewa
        */
        public int jenis_transfer { get; set; }
        public string deskripsi { get; set; }
        public int jml_transfer { get; set; }
        public DateTime creadate { get; set; }
    }
}