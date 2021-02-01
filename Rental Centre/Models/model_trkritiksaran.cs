using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trkritiksaran
    {
        RCDB _DB = new RCDB();
        model_mspenyewa mspenyewa = new model_mspenyewa();
        model_msrental msrental = new model_msrental();

        public void addData(trkritiksaran kritiksaran)
        {
            _DB.trkritiksaran.Add(kritiksaran);
            _DB.SaveChanges();
        }

        public IEnumerable<trkritiksaran> getAll()
        {
            var trkritiksaran = (from data in _DB.trkritiksaran
                                 select data);
            return trkritiksaran;
        }
        public IEnumerable<trkritiksaran> getAllPenyewa(int id_penyewa)
        {
            var trkritiksaran = (from data in _DB.trkritiksaran
                                 where data.id_penyewa == id_penyewa
                                 select data);
            return trkritiksaran;
        }
        public IEnumerable<trkritiksaran> getAllRental(int id_rental)
        {
            var trkritiksaran = (from data in _DB.trkritiksaran
                                 where data.id_rental == id_rental
                                 select data);
            return trkritiksaran;
        }
    }
}