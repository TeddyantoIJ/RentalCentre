using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rental_Centre.Models
{
    [Table("msadmin")]
    public class msadmin
    {
        [Key]
        public int id_admin { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string nama_admin { get; set; }
        public int jenis_kelamin { get; set; }
        public string profil { get; set; }
        public string email { get; set; }
        public string tempat_lahir { get; set; }
        public System.DateTime tgl_lahir { get; set; }
        public int status { get; set; }
        public int creaby { get; set; }
        public System.DateTime creadate { get; set; }
        public Nullable<int> modiby { get; set; }
        public Nullable<System.DateTime> modidate { get; set; }
    }
}