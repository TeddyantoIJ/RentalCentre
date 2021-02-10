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
        public void dibatalkan(int id_penyewaan)
        {
            var dtdetailpenyewaan = (from data in _DB.dtdetailpenyewaan
                                                         where data.id_penyewaan == id_penyewaan
                                                         select data);
            foreach(var item in dtdetailpenyewaan)
            {
                item.status_barang = "DIBATALKAN";
            }            
            _DB.SaveChanges();
        }


        public List<item> JumlahPenjualan(int id_rental, DateTime? date1, DateTime? date2)
        {
            var pemesanan = (from detail in _DB.dtdetailpenyewaan
                             join penyewaan in _DB.trpenyewaan
                             on detail.id_penyewaan equals penyewaan.id_penyewaan
                             join barang in _DB.msbarang
                             on detail.id_barang equals barang.id_barang
                             where
                             barang.id_rental == id_rental &&
                             (penyewaan.tgl_penyewaan >= date1 && penyewaan.tgl_pengembalian <= date2) ||
                             (penyewaan.tgl_penyewaan <= date1 && penyewaan.tgl_pengembalian >= date1) ||
                             (penyewaan.tgl_penyewaan <= date2 && penyewaan.tgl_pengembalian >= date2)
                             && penyewaan.status_transaksi != "GAGAL"
                             group detail by detail.id_barang into id
                             select new { id = id.Key, jumlah = id.Sum(d => d.jml_barang)}).ToList();
            var brg = (from b in _DB.msbarang
                          select b);
            List<item> output = new List<item>();
            foreach (var data in pemesanan)
            {
                foreach (Rental_Centre.Models.msbarang item in brg)
                {
                    if(item.id_barang == data.id)
                    {
                        output.Add(new item(item.nama_barang, data.jumlah));
                    }
                }
            }
            return output;
        }
        public List<item> JumlahPenjualan(DateTime? date1, DateTime? date2)
        {
            var pemesanan = (from detail in _DB.dtdetailpenyewaan
                             join penyewaan in _DB.trpenyewaan
                             on detail.id_penyewaan equals penyewaan.id_penyewaan
                             join barang in _DB.msbarang
                             on detail.id_barang equals barang.id_barang
                             where                             
                             (penyewaan.tgl_penyewaan >= date1 && penyewaan.tgl_pengembalian <= date2) ||
                             (penyewaan.tgl_penyewaan <= date1 && penyewaan.tgl_pengembalian >= date1) ||
                             (penyewaan.tgl_penyewaan <= date2 && penyewaan.tgl_pengembalian >= date2)
                             && penyewaan.status_transaksi != "GAGAL"
                             group detail by detail.id_barang into id
                             select new { id = id.Key, jumlah = id.Sum(d => d.jml_barang) }).ToList();
            var brg = (from b in _DB.msbarang
                       select b);
            List<item> output = new List<item>();
            foreach (var data in pemesanan)
            {
                foreach (Rental_Centre.Models.msbarang item in brg)
                {
                    if (item.id_barang == data.id)
                    {
                        output.Add(new item(item.nama_barang, data.jumlah));
                    }
                }
            }
            return output;
        }
        public class item
        {
            public item(string a, int b)
            {
                this.nama = a;
                this.jumlah = b;
            }
            public string nama;
            public int jumlah;
        }
    }
}