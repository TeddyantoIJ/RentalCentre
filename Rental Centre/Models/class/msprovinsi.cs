using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("msprovinsi")]
    public class msprovinsi
    {
        [Key]
        public int id_provinsi { get; set; }
        public string nama_provinsi { get; set; }
    }
}