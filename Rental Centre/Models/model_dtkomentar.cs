using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_dtkomentar
    {
        RCDB _DB = new RCDB();

        public void add(dtkomentar komen)
        {            
            _DB.dtkomentar.Add(komen);
            _DB.SaveChanges();
        }
        public IEnumerable<dtkomentar> GetDtkomentars(List<int> id_komentar)
        {

            List<dtkomentar> output = new List<dtkomentar>();
            foreach (int item in id_komentar as List<int>)
            {
                List<dtkomentar> dt = (from data in _DB.dtkomentar
                          where data.id_komentar == item
                          select data).ToList();
                foreach (dtkomentar balasan in dt)
                {
                    output.Add(balasan);
                }                
            }

            return output;
        }
        public void delete(int id_komentar)
        {
            var dt = (from data in _DB.dtkomentar
                      where data.id_komentar == id_komentar
                      select data);
            foreach (dtkomentar item in dt)
            {
                _DB.dtkomentar.Remove(item);
            }
            _DB.SaveChanges();
        }
        public dtkomentar getKomentarById(int id_dtkomentar)
        {
            return _DB.dtkomentar.Find(id_dtkomentar);
        }
        public void delete(dtkomentar dtkomentar)
        {
            _DB.dtkomentar.Remove(dtkomentar);
            _DB.SaveChanges();
        }
    }
}