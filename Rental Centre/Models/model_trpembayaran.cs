using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trpembayaran
    {
        RCDB _DB = new RCDB();

        public IEnumerable<trpembayaran> getAll()
        {
            var pem = (from data in _DB.trpembayaran                       
                       select data);
            return pem;
        }

        public IEnumerable<trpembayaran> getAll(int id_penyewa)
        {            
            var pem = (from data in _DB.trpembayaran
                       join sewa in _DB.trpenyewaan
                       on data.id_penyewaan equals sewa.id_penyewaan
                       where sewa.id_penyewa == id_penyewa
                       select data);
            return pem;
        }

        public void addData(trpembayaran trpembayaran)
        {
            _DB.trpembayaran.Add(trpembayaran);
            _DB.SaveChanges();
        }
        public void ValidasiDP(trpembayaran pembayaran)
        {
            trpembayaran trpembayaran = _DB.trpembayaran.Single<trpembayaran>(s => s.id_penyewaan == pembayaran.id_penyewaan && s.jenis_pembayaran == 0);
            trpembayaran.validate = pembayaran.validate;
            trpembayaran.tgl_validasi = pembayaran.tgl_validasi;
            trpembayaran.id_admin = pembayaran.id_admin;

            _DB.SaveChanges();
        }
        public void ValidasiSisa(trpembayaran pembayaran)
        {
            trpembayaran trpembayaran = _DB.trpembayaran.Single<trpembayaran>(s => s.id_penyewaan == pembayaran.id_penyewaan && s.jenis_pembayaran == 1);
            trpembayaran.validate = pembayaran.validate;
            trpembayaran.tgl_validasi = pembayaran.tgl_validasi;
            trpembayaran.id_admin = pembayaran.id_admin;

            _DB.SaveChanges();
        }
    }
}