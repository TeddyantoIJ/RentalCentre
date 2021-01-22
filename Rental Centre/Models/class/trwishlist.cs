using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("trwishlist")]
    public class trwishlist
    {
        [Key]
        public int id_wishlist { get; set; }
        public int id_penyewa { get; set; }        
        public int id_barang { get; set; }
    }
}