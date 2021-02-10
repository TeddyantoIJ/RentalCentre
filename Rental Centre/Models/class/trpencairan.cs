using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("trpencairan")]
    public class trpencairan
    {
        [Key]
        public int id_pencairan { get; set; }
        public Nullable<int> id_rental { get; set; }
        public Nullable<int> id_penyewa { get; set; }
        public string no_rek {get;set;}
        public int jml_pencairan {get;set;}
        public int status_pencairan {get;set;}
        public DateTime creadate { get; set; }
    }
}