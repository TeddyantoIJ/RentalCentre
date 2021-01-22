using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("trtopup")]
    public class trtopup
    {
        [Key]
        public int id_topup { get; set; }
        public Nullable<int> id_rental { get; set; }
        public Nullable<int> id_penyewa { get; set; }
        public Nullable<int> id_admin { get; set; }
        public int jml_topup { get; set; }
        public string bukti_topup { get; set; }
        public DateTime creadate { get; set; }
        public int validate { get; set; }
        public Nullable<DateTime> tgl_validasi { get; set; }
        public int status { get; set; }        
    }
}