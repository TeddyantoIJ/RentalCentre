using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trkomentar
    {
        RCDB _DB = new RCDB();

        public void add(trkomentar komentar)
        {
            _DB.trkomentar.Add(komentar);
            _DB.SaveChanges();
        }
        public IEnumerable<trkomentar> GetTrkomentar(int id_barang)
        {
            var trkomentar = (from data in _DB.trkomentar
                              where data.id_barang == id_barang
                              orderby data.creadate descending
                              select data);
            return trkomentar;
        }
        public void delete(trkomentar trkomentar)
        {
            _DB.trkomentar.Remove(trkomentar);
            _DB.SaveChanges();
        }
        public trkomentar getKomentarbyId(int id)
        {
            return _DB.trkomentar.Find(id);
        }
    }

    
}