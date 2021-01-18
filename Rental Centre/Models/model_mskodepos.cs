using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Rental_Centre.Models
{
    public class model_mskodepos
    {
        RCDB _DB = new RCDB();

        public int count()
        {
            var mskodepos = (from data in _DB.mskodepos
                             where data.provinsi == "DKI Jakarta"
                             select data);
            return mskodepos.ToList<mskodepos>().Count();
        }
        public List<mskodepos> getAllKodePos()
        {
            var mskodepos = (from data in _DB.mskodepos
                             where data.provinsi == "DKI Jakarta"
                             select data);

            return mskodepos.ToList<mskodepos>();
        }

        public void addData(mskodepos mskodepos)
        {
            _DB.mskodepos.Add(mskodepos);
            _DB.SaveChanges();
        }

        public void editData(mskodepos mskodepos)
        {
            var oldkodepos = _DB.mskodepos.Single(data => data.id_kodepos == mskodepos.id_kodepos);
            oldkodepos.kodepos = mskodepos.kodepos;
            oldkodepos.kelurahan = mskodepos.kelurahan;
            oldkodepos.kecamatan = mskodepos.kecamatan;
            oldkodepos.kota = mskodepos.kota;
            oldkodepos.provinsi = mskodepos.provinsi;

            _DB.SaveChanges();
        }
        public void hapusData(int id)
        {
            var kodepos = _DB.mskodepos.Single(data => data.id_kodepos == id);
            kodepos.kodepos = "-";
            kodepos.kelurahan = "-";
            kodepos.kecamatan = "-";
            kodepos.kota = "-";
            kodepos.provinsi = "-";
            _DB.SaveChanges();
        }
        public mskodepos getKodepos(int id)
        {
            mskodepos kodepos = _DB.mskodepos.Single(data => data.id_kodepos == id);
            return kodepos;
        }
    }
}