using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("dtchat")]
    public class dtchat
    {  
        [Key]
        public int id_chat { get; set; }
        public Nullable<int> id_penyewa { get; set; }
        public Nullable<int> id_rental { get; set; }
        public Nullable<int> id_admin { get; set; }
        public string isi_pesan { get; set; }
        public int dibaca { get; set; }
        public DateTime creadate { get; set; }
    }
}