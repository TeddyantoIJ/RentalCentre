using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trwishlist
    {
        RCDB _DB = new RCDB();

        public IEnumerable<trwishlist> getAll(int id_penyewa)
        {
            var wish = _DB.trwishlist.Where<trwishlist>(s => s.id_penyewa == id_penyewa);
            return wish;
        }
        public bool ada(int id_barang, int id_penyewa)
        {
            var data = _DB.trwishlist.SingleOrDefault<trwishlist>(s => s.id_barang == id_barang && s.id_penyewa == id_penyewa);
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public void add(trwishlist trwishlist)
        {
            _DB.trwishlist.Add(trwishlist);
            _DB.SaveChanges();
        }
        public void remove(int id_penyewa, int id_barang)
        {
            trwishlist trwishlist = _DB.trwishlist.SingleOrDefault<trwishlist>(t => t.id_penyewa == id_penyewa && t.id_barang == id_barang);
            _DB.trwishlist.Remove(trwishlist);
            _DB.SaveChanges();
        }
    }
}