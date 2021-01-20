using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_msadmin
    {
        RCDB _DB = new RCDB();


        public void addData(msadmin msadmin)
        {
            _DB.msadmin.Add(msadmin);
            _DB.SaveChanges();
        }
        public void editData(msadmin msadmin)
        {
            msadmin oldData = _DB.msadmin.Single<msadmin>(data => data.id_admin == msadmin.id_admin);
            oldData.nama_admin = msadmin.nama_admin;
            oldData.password = msadmin.password;
            oldData.profil = msadmin.profil;
            oldData.tempat_lahir = msadmin.tempat_lahir;
            oldData.tgl_lahir = msadmin.tgl_lahir;
            oldData.email = msadmin.email;

            _DB.SaveChanges();
        }
        public void ubahPassword(msadmin msadmin)
        {
            msadmin oldData = _DB.msadmin.Single<msadmin>(data => data.id_admin == msadmin.id_admin);
            oldData.password = msadmin.password;

            _DB.SaveChanges();
        }
        public void hapusData(int id)
        {
            msadmin oldData = _DB.msadmin.Single<msadmin>(data => data.id_admin == id);
            oldData.status = 0;
            _DB.SaveChanges();
        }
        public msadmin getAdmin(int id)
        {
            msadmin admin = _DB.msadmin.Single(data => data.id_admin == id);

            return admin;
        }
        public IEnumerable<msadmin> getAllData()
        {
            var msadmin = (from data in _DB.msadmin select data);

            return msadmin;
        }
        public bool adaUsername(string username)
        {
            msadmin msadmin = _DB.msadmin.SingleOrDefault(s => s.username == username);
            if(msadmin == null)
            {
                return false;
            }
            return true;
        }
    }
}