using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("msjenisbarang")]
    public class msjenisbarang
    {
        [Key]
        public int id_jenisbarang { get; set; }
        public Nullable<int> id_kelompokjenis { get; set; }
        public string nama_jenisbarang { get; set; }
        public string deskripsi { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> creaby { get; set; }
        public Nullable<System.DateTime> creadate { get; set; }
        public Nullable<int> modiby { get; set; }
        public Nullable<System.DateTime> modidate { get; set; }

    }
}