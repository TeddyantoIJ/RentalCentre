using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Rental_Centre.Models
{
    public class model_msrental
    {
        RCDB _DB = new RCDB();
        public IEnumerable<msrental> getAllData()
        {
            var msrental = (from data in _DB.msrental
                            select data);
            return msrental;
        }
        public void addData(msrental msrental)
        {
            _DB.msrental.Add(msrental);
            _DB.SaveChanges();
        }
        public void editData(msrental msrental)
        {
            msrental oldData = _DB.msrental.Single<msrental>(s => s.id_rental == msrental.id_rental);
            oldData.alamat = msrental.alamat;
            oldData.alamat_toko = msrental.alamat_toko;
            oldData.berkas1 = msrental.berkas1;
            oldData.berkas2 = msrental.berkas2;
            oldData.email = msrental.email;
            oldData.jenis_kelamin = msrental.jenis_kelamin;
            oldData.modiby = msrental.modiby;
            oldData.modidate = msrental.modidate;
            oldData.nama_bank = msrental.nama_bank;
            oldData.nama_rental = msrental.nama_rental;
            oldData.nama_toko = msrental.nama_toko;
            oldData.NIK = msrental.NIK;
            oldData.no_rek = msrental.no_rek;
            oldData.no_telp = msrental.no_telp;

            _DB.SaveChanges();

        }
        public void hapusData(int id)
        {
            msrental msrental = _DB.msrental.Single<msrental>(s => s.id_rental == id);
            msrental.status = 0;
            _DB.SaveChanges();
        }
    }
}