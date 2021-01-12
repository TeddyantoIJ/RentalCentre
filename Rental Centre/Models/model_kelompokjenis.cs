using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_kelompokjenis
    {
        RCDB _DB = new RCDB();

        public void addData(mskelompokjenis data)
        {
            _DB.mskelompokjenis.Add(data);
            _DB.SaveChanges();
        }
        public void editData(mskelompokjenis data)
        {
            var oldData = _DB.mskelompokjenis.Single(x => x.id_kelompokjenis == data.id_kelompokjenis);
            oldData.nama_kelompokjenis = data.nama_kelompokjenis;
            oldData.deskripsi = data.deskripsi;
            oldData.modiby = data.modiby;
            oldData.modidate = data.modidate;

            _DB.SaveChanges();
        }
        public void deleteData(int id)
        {
            var oldData = _DB.mskelompokjenis.Single(x => x.id_kelompokjenis == id);
            oldData.status = 0;
            _DB.SaveChanges();
        }

        public int count()
        {
            var kelompokjenis = (from data in _DB.mskelompokjenis
                                 select data);
            return kelompokjenis.Count<mskelompokjenis>();
        }
        public IEnumerable<mskelompokjenis> getAllData()
        {
            var kelompokjenis = (from data in _DB.mskelompokjenis
                                 select data);
            return kelompokjenis;
        }
        public mskelompokjenis getKelompok(int id)
        {
            mskelompokjenis kelompok = _DB.mskelompokjenis.Single(data => data.id_kelompokjenis == id);
            return kelompok;
        }

    }
}