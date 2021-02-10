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
        public IEnumerable<trpenyewaan> getAllData(int id_penyewa)
        {
            var trpenyewaan = (from data in _DB.trpenyewaan
                               orderby data.creadate descending
                               where data.id_penyewa == id_penyewa
                               select data);
            return trpenyewaan;
        }
        public IEnumerable<trpenyewaan> getAllData()
        {
            var trpenyewaan = (from data in _DB.trpenyewaan
                               orderby data.creadate descending
                               select data);
            return trpenyewaan;
        }
        public IEnumerable<trpenyewaan> getAllData(string status_transaksi)
        {
            var trpenyewaan = (from data in _DB.trpenyewaan
                               orderby data.creadate descending
                               where data.status_transaksi.Contains(status_transaksi)
                               select data);
            return trpenyewaan;
        }
        public IEnumerable<trpenyewaan> getAllDataPenyewa(int id_penyewa)
        {
            var trpenyewaan = (from data in _DB.trpenyewaan
                               orderby data.creadate descending
                               where data.id_penyewa == id_penyewa
                               select data);
            return trpenyewaan;
        }
        public IEnumerable<trpenyewaan> getAllDataRental(int id_rental, string status)
        {
            
            var trpenyewaan = (from penyewaan in _DB.trpenyewaan
                          join detail in _DB.dtdetailpenyewaan
                               on penyewaan.id_penyewaan equals detail.id_penyewaan
                          join barang in _DB.msbarang
                               on detail.id_barang equals barang.id_barang
                          join rental in _DB.msrental
                               on barang.id_rental equals rental.id_rental
                               orderby penyewaan.creadate descending
                               where rental.id_rental == id_rental && penyewaan.status_transaksi.Contains(status)                               
                               select penyewaan);
            trpenyewaan = trpenyewaan.GroupBy(s => s.id_penyewaan).Select(g => g.FirstOrDefault());
            
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
                               orderby penyewaan.creadate descending
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
        public void ubahBerjalan(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "BERJALAN";
            _DB.SaveChanges();
        }
        public void ubahSelesai(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "SELESAI";
            _DB.SaveChanges();
        }
        public void ubahBerjalan(int id_penyewaan, int id_admin)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "BERJALAN";
            trpenyewaan.id_admin = id_admin;
            _DB.SaveChanges();
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
        public void ubahValidasiTransfer(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "VALIDASI TRANSFER";
            _DB.SaveChanges();
        }
        public void ubahGagal(int id_penyewaan, int id_admin)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "GAGAL";
            trpenyewaan.id_admin = id_admin;
            _DB.SaveChanges();
        }
        public void ubahGagal(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_transaksi = "GAGAL";            
            _DB.SaveChanges();
        }
        public void bayarDP(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_dp = 1;
            _DB.SaveChanges();
        }
        public void bayarSisa(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_pembayaran = 1;
            _DB.SaveChanges();
        }
        public void beriUlasan(int id_penyewaan)
        {
            trpenyewaan trpenyewaan = _DB.trpenyewaan.Single<trpenyewaan>(s => s.id_penyewaan == id_penyewaan);
            trpenyewaan.status_ulasan = 1;
            _DB.SaveChanges();
        }        
        public int getPenghasilan(int id_rental, DateTime? date1, DateTime? date2)
        {
            var penghasilan = (from data in _DB.dtmutasisaldo
                               where data.creadate >= date1 && data.creadate <= date2 &&
                               data.id_rental == id_rental && data.jenis_transaksi.Contains("TERIMA TRANSFER PENYEWAAN")
                          group data by data.id_mutasisaldo into a
                          select new { a = a.Sum(d => d.jumlah_transaksi) });
            int jumlah = 0;
            foreach (var item in penghasilan)
            {
                jumlah += item.a;
            }
            return jumlah;
        }
        public IEnumerable<dtmutasisaldo> getPenghasilanByRange(int id_rental, DateTime? date1, DateTime? date2)
        {
            var penghasilan = (from data in _DB.dtmutasisaldo
                               where data.creadate >= date1 && data.creadate <= date2
                               && data.id_rental == id_rental && data.jenis_transaksi.Contains("TERIMA TRANSFER PENYEWAAN")
                               select data);

            return penghasilan;
        }
        public IEnumerable<dtmutasisaldo> getPenghasilanByRange(DateTime? date1, DateTime? date2)
        {
            var penghasilan = (from data in _DB.dtmutasisaldo
                               where data.creadate >= date1 && data.creadate <= date2
                               && data.jenis_transaksi.Contains("TERIMA TRANSFER PENYEWAAN")
                               select data);

            return penghasilan;
        }
        public List<PenyewaanRange> getAllDataByRange(int id_rental, DateTime? date1, DateTime? date2)
        {            
            var trpenyewaan = (from penyewaan in _DB.trpenyewaan
                               join detail in _DB.dtdetailpenyewaan
                                    on penyewaan.id_penyewaan equals detail.id_penyewaan
                               join barang in _DB.msbarang
                                    on detail.id_barang equals barang.id_barang
                               join rental in _DB.msrental
                                    on barang.id_rental equals rental.id_rental
                               orderby penyewaan.creadate descending
                               where rental.id_rental == id_rental &&
                               penyewaan.tgl_penyewaan >= date1 && penyewaan.tgl_penyewaan <= date2
                               && penyewaan.status_transaksi != "GAGAL"
                               select penyewaan).Distinct().ToList();
            var sewa = (from data in trpenyewaan
                        group data by data.tgl_penyewaan into da
                        select new { id = da.Key, jumlah = da.Count() }).ToList();
            List<PenyewaanRange> output = new List<PenyewaanRange>();            
            foreach (var item in sewa)
            {
                output.Add(new PenyewaanRange(item.id, item.jumlah));
            }
            return output;
        }
        public List<PenyewaanRange> getAllDataByRange(DateTime? date1, DateTime? date2)
        {
            var trpenyewaan = (from penyewaan in _DB.trpenyewaan
                               join detail in _DB.dtdetailpenyewaan
                                    on penyewaan.id_penyewaan equals detail.id_penyewaan
                               join barang in _DB.msbarang
                                    on detail.id_barang equals barang.id_barang
                               join rental in _DB.msrental
                                    on barang.id_rental equals rental.id_rental
                               orderby penyewaan.creadate descending
                               where
                               penyewaan.tgl_penyewaan >= date1 && penyewaan.tgl_penyewaan <= date2
                               && penyewaan.status_transaksi != "GAGAL"
                               select penyewaan).Distinct().ToList();
            var sewa = (from data in trpenyewaan
                        group data by data.tgl_penyewaan into da
                        select new { id = da.Key, jumlah = da.Count() }).ToList();
            List<PenyewaanRange> output = new List<PenyewaanRange>();
            foreach (var item in sewa)
            {
                output.Add(new PenyewaanRange(item.id, item.jumlah));
            }
            return output;
        }
        public class PenyewaanRange
        {
            public PenyewaanRange(DateTime a , int b)
            {
                this.a = a;
                this.b = b;
            }
            public DateTime a;
            public int b;
        }
    }
}