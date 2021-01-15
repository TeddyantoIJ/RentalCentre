using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Rental_Centre.Models
{
    public class model_mspenyewa
    {
        RCDB _DB = new RCDB();

        public void addPenyewa(mspenyewa mspenyewa)
        {
            _DB.mspenyewa.Add(mspenyewa);
            _DB.SaveChanges();
        }
        public void editPenyewa(mspenyewa mspenyewa)
        {
            mspenyewa oldData = _DB.mspenyewa.Single<mspenyewa>(data => data.id_penyewa == mspenyewa.id_penyewa);
            oldData.password = mspenyewa.password;
            oldData.nama_penyewa = mspenyewa.nama_penyewa;
            oldData.profil = mspenyewa.profil;
            oldData.kodepos = mspenyewa.kodepos;
            oldData.jenis_kelamin = mspenyewa.jenis_kelamin;
            oldData.tempat_lahir = mspenyewa.tempat_lahir;
            oldData.tgl_lahir = mspenyewa.tgl_lahir;
            oldData.email = mspenyewa.email;
            oldData.no_telepon = mspenyewa.no_telepon;
            oldData.saldo = mspenyewa.saldo;
            oldData.NIK = mspenyewa.NIK;
            oldData.SKCK = mspenyewa.SKCK;
            oldData.berkas1 = mspenyewa.berkas1;
            oldData.berkas2 = mspenyewa.berkas2;
            oldData.modiby = mspenyewa.modiby;
            oldData.modidate = mspenyewa.modidate;

            oldData.modiadminby = mspenyewa.modiadminby;
            oldData.modiadmindate = mspenyewa.modiadmindate;

            _DB.SaveChanges();
        }
        public void hapusPenyewaSelf(int id, string email)
        {
            mspenyewa mspenyewa = _DB.mspenyewa.Single<mspenyewa>(data => data.id_penyewa == id);
            mspenyewa.modiby = email;
            mspenyewa.modidate = DateTime.Now;
            mspenyewa.status = 0;
            _DB.SaveChanges();
        }

        public mspenyewa getPenyewa(int id)
        {
            mspenyewa penyewa = _DB.mspenyewa.Single<mspenyewa>(data => data.id_penyewa == id);
            return (penyewa);
        }
        
    }
}