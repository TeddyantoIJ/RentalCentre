using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("trkritiksaran")]
    public class trkritiksaran
    {
        [Key]
        public int id_kritiksaran { get; set; }
        public Nullable<int> id_penyewa { get; set; }
        public Nullable<int> id_rental { get; set; }
        public Nullable<int> id_admin { get; set; }
        public string kritik_saran { get; set; }
        public DateTime creadate { get; set; }
    }
}