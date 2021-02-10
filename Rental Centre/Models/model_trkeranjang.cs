using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trkeranjang
    {
        RCDB _DB = new RCDB();

        public IEnumerable<trkeranjang> getAllByPenyewa(int id)
        {
            var a = (from data in _DB.trkeranjang                     
                     where data.id_penyewa == id
                        select data);
            return a;
        }
        public void add(trkeranjang trkeranjang)
        {
            _DB.trkeranjang.Add(trkeranjang);
            try
            {
                _DB.SaveChanges();
            }
            catch
            {

            }
            
        }
        public bool ada(int id_barang, int id_penyewa)
        {
            var data = _DB.trkeranjang.SingleOrDefault<trkeranjang>(s => s.id_barang == id_barang && s.id_penyewa == id_penyewa);
            if(data == null)
            {
                return false;
            }
            return true;
        }
        public void remove(int id)
        {
            var data = _DB.trkeranjang.Where<trkeranjang>(s => s.id_penyewa == id);
            foreach (var item in data)
            {
                _DB.trkeranjang.Remove(item);
            }
            _DB.SaveChanges();
        }
        public void remove(int id_penyewa, int id_barang)
        {
            trkeranjang data = _DB.trkeranjang.SingleOrDefault<trkeranjang>(s => s.id_penyewa == id_penyewa && s.id_barang == id_barang);                        
            if(data != null)
            {
                _DB.trkeranjang.Remove(data);
                _DB.SaveChanges();
            }            
        }
    }
}