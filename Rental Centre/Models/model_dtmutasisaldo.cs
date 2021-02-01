using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_dtmutasisaldo
    {
        RCDB _DB = new RCDB();

        public IEnumerable<dtmutasisaldo> getAll()
        {
            var dtmutasisaldo = (from data in _DB.dtmutasisaldo                                 
                                 select data);
            return dtmutasisaldo;
        }
        
        public IEnumerable<dtmutasisaldo> getAllPenyewa(int id_penyewa)
        {
            var dtmutasisaldo = (from data in _DB.dtmutasisaldo
                                 where data.id_penyewa == id_penyewa
                                 select data);
            return dtmutasisaldo;
        }
        public IEnumerable<dtmutasisaldo> getAllRental(int id_rental)
        {
            var dtmutasisaldo = (from data in _DB.dtmutasisaldo
                                 where data.id_rental == id_rental
                                 select data);
            return dtmutasisaldo;
        }


        public void penyewa_mutasi(int? id_penyewa, int uang, string jenis, int total)
        {
            dtmutasisaldo mutasi = new dtmutasisaldo();
            mutasi.creadate = DateTime.Now;
            mutasi.id_penyewa = id_penyewa;
            mutasi.jenis_transaksi = jenis;
            mutasi.jumlah_transaksi = uang;
            mutasi.jumlah_saldo = total;

            _DB.dtmutasisaldo.Add(mutasi);
            _DB.SaveChanges();
        }
        public void rental_mutasi(int? id_rental, int uang, string jenis, int total)
        {
            dtmutasisaldo mutasi = new dtmutasisaldo();
            mutasi.creadate = DateTime.Now;
            mutasi.id_rental = id_rental;
            mutasi.jenis_transaksi = jenis;
            mutasi.jumlah_transaksi = uang;
            mutasi.jumlah_saldo = total;

            _DB.dtmutasisaldo.Add(mutasi);
            _DB.SaveChanges();
        }
    }
}