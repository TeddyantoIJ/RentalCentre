using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_jenisbarang
    {
        RCDB _DB = new RCDB();

        public void addData(msjenisbarang msjenisbarang)
        {
            _DB.msjenisbarang.Add(msjenisbarang);
            _DB.SaveChanges();
        }
        public void editData(msjenisbarang msjenisbarang)
        {
            var oldBarang = _DB.msjenisbarang.Single(data => data.id_jenisbarang == msjenisbarang.id_jenisbarang);
            oldBarang.nama_jenisbarang = msjenisbarang.nama_jenisbarang;
            oldBarang.id_kelompokjenis = msjenisbarang.id_kelompokjenis;
            oldBarang.deskripsi = msjenisbarang.deskripsi;
            oldBarang.modiby = msjenisbarang.modiby;
            oldBarang.modidate = msjenisbarang.modidate;

            _DB.SaveChanges();
        }
        public void hapusData(int id)
        {
            var oldBarang = _DB.msjenisbarang.Single(data => data.id_jenisbarang == id);
            oldBarang.status = 0;
            _DB.SaveChanges();
        }
        public int count()
        {
            var msjenisbarang = (from data in _DB.msjenisbarang
                                 select data);
            return msjenisbarang.Count<msjenisbarang>();
        }
        public IEnumerable<msjenisbarang> getAllData()
        {
            var msjenisbarang = (from data in _DB.msjenisbarang
                                 select data);

            return msjenisbarang;
        }
        public IEnumerable<msjenisbarang> getDataByIdKelompok(int id)
        {
            var msjenisbarang = (from data in _DB.msjenisbarang
                                 where data.id_kelompokjenis == id
                                 select data);
            return msjenisbarang;
        }
        public msjenisbarang getJenisbarang(int id)
        {
            msjenisbarang jenisbarang = _DB.msjenisbarang.Single(data => data.id_jenisbarang == id);
            return jenisbarang;
        }
    }
}