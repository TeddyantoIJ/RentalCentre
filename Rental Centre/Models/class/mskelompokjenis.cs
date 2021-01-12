using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rental_Centre.Models
{
    [Table("mskelompokjenis")]

    public class mskelompokjenis
    {
        [Key]
        public int id_kelompokjenis { get; set; }
        public string nama_kelompokjenis { get; set; }
        public string deskripsi { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> creaby { get; set; }
        public Nullable<System.DateTime> creadate { get; set; }
        public Nullable<int> modiby { get; set; }
        public Nullable<System.DateTime> modidate { get; set; }
    }
}