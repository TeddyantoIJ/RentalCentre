using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trpencairan
    {
        RCDB _DB = new RCDB();

        public void add(trpencairan trpencairan)
        {
            _DB.trpencairan.Add(trpencairan);
            _DB.SaveChanges();
        }
        public IEnumerable<trpencairan> getAll()
        {
            var trpencairan = (from data in _DB.trpencairan
                               orderby data.creadate descending
                               select data);
            trpencairan = trpencairan.OrderBy(n => n.status_pencairan);
            return trpencairan;
        }
        public void konfirmasi(int id_pencairan)
        {
            trpencairan trpencairan = _DB.trpencairan.SingleOrDefault<trpencairan>(s => s.id_pencairan == id_pencairan);
            trpencairan.status_pencairan = 1;
            _DB.SaveChanges();
        }
    }
}