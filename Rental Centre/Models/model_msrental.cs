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
        model_dtmutasisaldo dtmutasisaldo = new model_dtmutasisaldo();

        public msrental getRental(int id)
        {
            msrental msrental = _DB.msrental.Single<msrental>(s => s.id_rental == id);
            return msrental;
        }
        public msrental getRentalUsername(string username)
        {
            msrental msrental = _DB.msrental.SingleOrDefault<msrental>(s => s.username == username);
            return msrental;
        }
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
        public void editPassword(msrental msrental)
        {
            msrental oldData = _DB.msrental.Single<msrental>(s => s.id_rental == msrental.id_rental);
            oldData.password = msrental.password;
            _DB.SaveChanges();
        }
        public void editData(msrental msrental)
        {
            msrental oldData = _DB.msrental.Single<msrental>(s => s.id_rental == msrental.id_rental);

            oldData.nama_rental = msrental.nama_rental;
            oldData.nama_toko = msrental.nama_toko;
            oldData.no_telp = msrental.no_telp;
            oldData.NIK = msrental.NIK;
            oldData.email = msrental.email;
            oldData.tempat_lahir = msrental.tempat_lahir;
            oldData.tgl_lahir = msrental.tgl_lahir;
            oldData.alamat = msrental.alamat;
            oldData.alamat_toko = msrental.alamat_toko;
            oldData.kodepos = msrental.kodepos;
            oldData.jenis_kelamin = msrental.jenis_kelamin;            
            oldData.nama_bank = msrental.nama_bank;
            oldData.no_rek = msrental.no_rek;            
            oldData.modidate = msrental.modidate;
            if(msrental.profil != null)
            {
                oldData.profil = msrental.profil;
            }
            if(msrental.berkas1 != null)
            {
                oldData.berkas1 = msrental.berkas1;
            }
            if(msrental.berkas2 != null)
            {
                oldData.berkas2 = msrental.berkas2;
            }                        
            _DB.SaveChanges();

        }
        public void hapusData(int id)
        {
            msrental msrental = _DB.msrental.Single<msrental>(s => s.id_rental == id);
            msrental.modidate = DateTime.Now;
            msrental.status = 0;
            _DB.SaveChanges();
        }
        public bool adaUsername(string username)
        {
            msrental msrental = _DB.msrental.SingleOrDefault(s => s.username == username);
            if (msrental == null)
            {
                return false;
            }
            return true;
        }
        public void topup(int jumlah_uang, int? id_rental)
        {
            msrental msrental = _DB.msrental.Single<msrental>(s => s.id_rental == id_rental);
            msrental.saldo = msrental.saldo + jumlah_uang;
            _DB.SaveChanges();
            this.dtmutasisaldo.rental_mutasi(id_rental, jumlah_uang, "TOP UP");
        }
        public void saldo_tambah(int uang, int id_rental)
        {
            msrental msrental = _DB.msrental.Single<msrental>(s => s.id_rental == id_rental);
            msrental.saldo = msrental.saldo + uang;
            _DB.SaveChanges();

            this.dtmutasisaldo.penyewa_mutasi(id_rental, uang, "TERIMA TRANSFER");
        }
        public void saldo_kurang(int uang, int id_rental)
        {
            msrental msrental = _DB.msrental.Single<msrental>(s => s.id_rental == id_rental);
            msrental.saldo = msrental.saldo - uang;
            _DB.SaveChanges();
                
            this.dtmutasisaldo.penyewa_mutasi(id_rental, uang, "TERIMA TRANSFER");
        }
    }
}