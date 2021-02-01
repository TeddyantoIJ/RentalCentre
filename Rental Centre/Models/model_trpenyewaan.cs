using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trpenyewaan
    {
        RCDB _DB = new RCDB();        

        public void add(trpenyewaan trpenyewaan)
        {
            _DB.trpenyewaan.Add(trpenyewaan);
            _DB.SaveChanges();            
        }
        public int getLastId()
        {
            int id = (from data in _DB.trpenyewaan select data.id_penyewaan).Max();
            return id;
        }
        public IEnumerable<trpenyewaan> getAllData()
        {
            var trpenyewaan = (from data in _DB.trpenyewaan                               
                               select data);
            return trpenyewaan;
        }
        public IEnumerable<trpenyewaan> getAllData(string status_transaksi)
        {
            var trpenyewaan = (from data in _DB.trpenyewaan
                               where data.status_transaksi == status_transaksi
                               select data);
            return trpenyewaan;
        }
        public IEnumerable<trpenyewaan> getAllDataPenyewa(int id_penyewa)
        {
            var trpenyewaan = (from data in _DB.trpenyewaan
                               where data.id_penyewa == id_penyewa
                               select data);
            return trpenyewaan;
        }
        public IEnumerable<trpenyewaan> getAllDataRental(int id_rental)
        {
            
            var trpenyewaan = (from penyewaan in _DB.trpenyewaan
                          join detail in _DB.dtdetailpenyewaan
                               on penyewaan.id_penyewaan equals detail.id_penyewaan
                          join barang in _DB.msbarang
                               on detail.id_barang equals barang.id_barang
                          join rental in _DB.msrental
                               on barang.id_rental equals rental.id_rental                            
                          where rental.id_rental == id_rental                            
                            
                               select penyewaan);
            trpenyewaan = trpenyewaan.GroupBy(s => s.id_penyewaan).Select(g => g.FirstOrDefault());

            
            return trpenyewaan;
        }
        public trpenyewaan getPenyewaan(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.SingleOrDefault<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            return trpenyewaan;
        }
        public trpenyewaan getPenyewaan(int id_penyewaan, int id_penyewa)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.SingleOrDefault<trpenyewaan>(s => s.id_penyewaan == id_penyewaan && s.id_penyewa == id_penyewa);
            return trpenyewaan;
        }
        public void ubahSiap(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "SIAP / DIKIRIM";
            _DB.SaveChanges();
        }
        public void ubahDisiapkan(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "DISIAPKAN";
            _DB.SaveChanges();
        }
        public void ubahDisiapkan(int id_penyewaan, int id_admin)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "DISIAPKAN";
            trpenyewaan.id_admin = id_admin;            
            _DB.SaveChanges();
        }
        public void ubahValidasiDP(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "VALIDASI DP";
            _DB.SaveChanges();
        } 
        public void ubahGagal(int id_penyewaan, int id_admin)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "GAGAL";
            trpenyewaan.id_admin = id_admin;
            _DB.SaveChanges();
        }
        public void bayarDP(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_dp = 1;
            _DB.SaveChanges();
        }        
    }
}