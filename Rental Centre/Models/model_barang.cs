using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Rental_Centre.Models
{
    public class model_barang
    {
        RCDB _DB = new RCDB();
        public int count()
        {
            var msbarang = (from data in _DB.msbarang
                            select data);
            return msbarang.Count<msbarang>();
        }
        public IEnumerable<msbarang> getAllDataByIdRentGroupByKelompok(int logged_id, int id_kelompokjenis)
        {
            

            var msjenisbarang = (from data in _DB.msjenisbarang
                                 where data.id_kelompokjenis == id_kelompokjenis
                                 select data);

            var msbarang = (from data in _DB.msbarang
                            join jenis in msjenisbarang
                            on data.id_jenisbarang equals jenis.id_jenisbarang
                            where data.id_rental == logged_id
                            select data);
            return msbarang;
        }
        public IEnumerable<msbarang> getDataById(int id)
        {
            var barang = (from data in _DB.msbarang
                               where data.id_barang == id
                               select data);
            return barang;
        }
        public void updateData(msbarang new_msbarang)
        {
            msbarang barang = _DB.msbarang.Single(data => data.id_barang == new_msbarang.id_barang);
            //barang.id_barang = new_msbarang.id_barang;
            //barang.id_jenisbarang = new_msbarang.id_jenisbarang;
            //barang.id_rental = new_msbarang.id_rental;
            barang.link_gambar = new_msbarang.link_gambar;
            barang.nama_barang = new_msbarang.nama_barang;
            barang.stok_barang = new_msbarang.stok_barang;
            barang.deskripsi_barang = new_msbarang.deskripsi_barang;
            barang.harga_sewa = new_msbarang.harga_sewa;
            barang.modidate = DateTime.Now;
            barang.modiby = new_msbarang.modiby;

            _DB.SaveChanges();
        }
        public void createData(msbarang new_msbarang)
        {
            _DB.msbarang.Add(new_msbarang);

            _DB.SaveChanges();

        }
        public void editData(msbarang new_msbarang)
        {
            msbarang barang = _DB.msbarang.Single(data => data.id_barang == new_msbarang.id_barang);
            barang.nama_barang = new_msbarang.nama_barang;
            barang.stok_barang = new_msbarang.stok_barang;
            barang.harga_sewa = new_msbarang.harga_sewa;
            barang.deskripsi_barang = new_msbarang.deskripsi_barang;
            barang.modiby = new_msbarang.modiby;
            barang.modidate = new_msbarang.modidate;
            if (!String.IsNullOrEmpty(new_msbarang.link_gambar))
            {
                barang.link_gambar = new_msbarang.link_gambar;
            }            
            _DB.SaveChanges();
        }
        public void hapusData(int id)
        {
            msbarang barang = _DB.msbarang.Single(data => data.id_barang == id);
            barang.status = 0;
            _DB.SaveChanges();
        }

        public msbarang getBarang(int id)
        {
            msbarang barang = _DB.msbarang.Single(data => data.id_barang == id);
            return barang;
        }

        public int getJumlahbarangperJenis(int id_jenisbarang)
        {
            var barang = (from data in _DB.msbarang
                          where data.id_jenisbarang == id_jenisbarang
                          select data);
            int jumlah = barang.ToList<msbarang>().Count();
            return jumlah;
        }
        public IEnumerable<msbarang> getAllData()
        {
            var msbarang = (from data in _DB.msbarang
                            select data);
            return msbarang;
        }
    }
}