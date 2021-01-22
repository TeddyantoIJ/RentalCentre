using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("dttracking")]
    public class dttracking
    {
        [Key]
        public int id_tracking { get; set; }
        public int id_penyewaan { get; set; }
        public string deskripsi { get; set; }
        public int creaby { get; set; }
        public DateTime creadate { get; set; }
    }
}