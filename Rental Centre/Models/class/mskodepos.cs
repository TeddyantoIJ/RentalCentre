using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    [Table("mskodepos")]
    public class mskodepos
    {
        [Key]
        public int id_kodepos { get; set; }
        public string kodepos { get; set; }
        public string kelurahan { get; set; }
        public string kecamatan { get; set; }
        public string kota { get; set; }
        public string provinsi { get; set; }
    }
}