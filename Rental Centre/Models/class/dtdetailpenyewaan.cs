using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("dtdetailpenyewaan")]
    public class dtdetailpenyewaan
    {
        [Key]
        public int id_detailpenyewaan { get; set; }
        public int id_penyewaan { get; set; }
        public int id_barang { get; set; }
        public int jml_barang { get; set; }
        public int harga_total { get; set; }
        public DateTime creadate { get; set; }
    }
}