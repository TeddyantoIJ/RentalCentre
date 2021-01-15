using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Rental_Centre.Models
{
    public class model_msprovinsi
    {
        RCDB _DB = new RCDB();

        public List<msprovinsi> getAllProvinsi()
        {
            var msprovinsi = (from data in _DB.msprovinsi
                              select data);
            return msprovinsi.ToList<msprovinsi>();
        }
        public void addData(msprovinsi msprovinsi)
        {
            _DB.msprovinsi.Add(msprovinsi);
            _DB.SaveChanges();
        }
        public void editData(msprovinsi msprovinsi)
        {
            msprovinsi oldData = _DB.msprovinsi.Single<msprovinsi>(a => a.id_provinsi == msprovinsi.id_provinsi);
            oldData.nama_provinsi = msprovinsi.nama_provinsi;
            _DB.SaveChanges();
        }
        public void hapusData(int id)
        {
            msprovinsi oldData = _DB.msprovinsi.Single<msprovinsi>(a => a.id_provinsi == id);
            oldData.nama_provinsi = "-";
            _DB.SaveChanges();
        }
        public msprovinsi getProvinsi(int id)
        {
            msprovinsi data = _DB.msprovinsi.Single<msprovinsi>(a => a.id_provinsi == id);
            return data;
        }
    }
}