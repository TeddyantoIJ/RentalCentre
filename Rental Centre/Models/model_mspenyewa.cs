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
        model_dtmutasisaldo mutasisaldo = new model_dtmutasisaldo();

        public void addPenyewa(mspenyewa mspenyewa)
        {
            _DB.mspenyewa.Add(mspenyewa);
            _DB.SaveChanges();
        }
        public void editPenyewa(mspenyewa mspenyewa)
        {
            mspenyewa oldData = _DB.mspenyewa.Single<mspenyewa>(data => data.id_penyewa == mspenyewa.id_penyewa);

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
        public void ubahPass(mspenyewa mspenyewa)
        {
            mspenyewa oldData = _DB.mspenyewa.SingleOrDefault<mspenyewa>(data => data.id_penyewa == mspenyewa.id_penyewa);
            oldData.modiby = mspenyewa.modiby;
            oldData.modidate = DateTime.Now;
            oldData.password = mspenyewa.password;
            _DB.SaveChanges();
        }
        public mspenyewa getPenyewaUsername(string username)
        {
            mspenyewa penyewa = _DB.mspenyewa.SingleOrDefault<mspenyewa>(data => data.username == username);
            return (penyewa);
        }
        public mspenyewa getPenyewa(int id_penyewa)
        {
            mspenyewa penyewa = _DB.mspenyewa.SingleOrDefault<mspenyewa>(data => data.id_penyewa == id_penyewa);
            return (penyewa);
        }

        public IEnumerable<mspenyewa> getAllData()
        {
            var mspenyewa = (from data in _DB.mspenyewa
                             select data);

            return mspenyewa;
        }
        public bool adaUsername(string username)
        {
            mspenyewa mspenyewa = _DB.mspenyewa.SingleOrDefault(s => s.username == username);
            if (mspenyewa == null)
            {
                return false;
            }
            return true;
        }
        public void topup(int uang, int? id_penyewa)
        {
            mspenyewa mspenyewa = _DB.mspenyewa.Single<mspenyewa>(s => s.id_penyewa == id_penyewa);
            mspenyewa.saldo = mspenyewa.saldo + uang;
            _DB.SaveChanges();

            this.mutasisaldo.penyewa_mutasi(id_penyewa, uang, "TOP UP", mspenyewa.saldo);
        }
        public void saldo_tambah(int uang, int id_penyewa, string penerima)
        {
            mspenyewa mspenyewa = _DB.mspenyewa.Single<mspenyewa>(s => s.id_penyewa == id_penyewa);
            mspenyewa.saldo = mspenyewa.saldo + uang;
            _DB.SaveChanges();

            this.mutasisaldo.penyewa_mutasi(id_penyewa, uang, "TERIMA TRANSFER DARI "+penerima.ToUpper(), mspenyewa.saldo);
        }
        public void saldo_kurang(int uang, int id_penyewa, string penerima)
        {
            mspenyewa mspenyewa = _DB.mspenyewa.Single<mspenyewa>(s => s.id_penyewa == id_penyewa);
            mspenyewa.saldo = mspenyewa.saldo - uang;
            _DB.SaveChanges();

            this.mutasisaldo.penyewa_mutasi(id_penyewa, uang, "KIRIM TRANSFER KE "+penerima.ToUpper(),mspenyewa.saldo);
        }
        public void bayar_dp(int uang, int id_penyewa)
        {
            mspenyewa mspenyewa = _DB.mspenyewa.Single<mspenyewa>(s => s.id_penyewa == id_penyewa);
            mspenyewa.saldo = mspenyewa.saldo - uang;
            _DB.SaveChanges();

            this.mutasisaldo.penyewa_mutasi(id_penyewa, uang, "DP PENYEWAAN", mspenyewa.saldo);
        }
    }
}