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
        public string username_pengirim { get; set; }
        public string username_penerima { get; set; }        
        public string isi_pesan { get; set; }
        public int dibaca { get; set; }
        public DateTime creadate { get; set; }
    }
}