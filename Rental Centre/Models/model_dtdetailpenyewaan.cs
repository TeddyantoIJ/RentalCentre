using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_dtdetailpenyewaan
    {
        RCDB _DB = new RCDB();

        public IEnumerable<dtdetailpenyewaan> getAllData(int id_penyewaan)
        {
            var detail = (from data in _DB.dtdetailpenyewaan
                          where data.id_penyewaan == id_penyewaan
                          select data);
            return detail;
        }
        public IEnumerable<msrental> getAllDataRentalByIdPenyewaan(int id_penyewaan)
        {
            var detail = (from data in _DB.dtdetailpenyewaan
                          join barang in _DB.msbarang 
                                on data.id_barang equals barang.id_barang
                          join rental in _DB.msrental
                                on barang.id_rental equals rental.id_rental                            
                          where data.id_penyewaan == id_penyewaan
                          select rental);
            detail = detail.GroupBy(s => s.id_rental).Select(g => g.FirstOrDefault());
            return detail;
        }
        public void add(dtdetailpenyewaan dtdetailpenyewaan)
        {
            _DB.dtdetailpenyewaan.Add(dtdetailpenyewaan);
            _DB.SaveChanges();            
        }
        public void dikemas(dtdetailpenyewaan baru)
        {
            dtdetailpenyewaan dtdetailpenyewaan = _DB.dtdetailpenyewaan.Single<dtdetailpenyewaan>(s => s.id_penyewaan == baru.id_penyewaan && s.id_barang == baru.id_barang);
            dtdetailpenyewaan.status_barang = "DIKEMAS";
            _DB.SaveChanges();
        }
        public void dikembalikan(dtdetailpenyewaan baru)
        {
            dtdetailpenyewaan dtdetailpenyewaan = _DB.dtdetailpenyewaan.Single<dtdetailpenyewaan>(s => s.id_penyewaan == baru.id_penyewaan && s.id_barang == baru.id_barang);
            dtdetailpenyewaan.status_barang = "DIKEMBALIKAN";
            _DB.SaveChanges();
        }
    }
}