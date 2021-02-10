using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trtopup
    {
        RCDB _DB = new RCDB();
        model_mspenyewa mspenyewa = new model_mspenyewa();
        model_msrental msrental = new model_msrental();

        public IEnumerable<trtopup> getAll()
        {
            var trtopup = (from data in _DB.trtopup
                           orderby data.creadate descending
                           where data.status == 1
                            select data                            
                            );
            return trtopup;
        }
        public void addData(trtopup topup)
        {
            _DB.trtopup.Add(topup);
            _DB.SaveChanges();
        }
        public void valid(int id_topup, int admin)
        {
            trtopup trtopup = _DB.trtopup.Single<trtopup>(s => s.id_topup == id_topup);
            trtopup.status = 0;
            trtopup.validate = 1;
            trtopup.id_admin = admin;
            trtopup.tgl_validasi = DateTime.Now;
            
            _DB.SaveChanges();
            if(trtopup.id_penyewa != null)
            {                
                this.mspenyewa.topup(trtopup.jml_topup,trtopup.id_penyewa);
            }
            if (trtopup.id_rental != null)
            {
                this.msrental.topup(trtopup.jml_topup, trtopup.id_rental);
            }
        }
        public void invalid(int id_topup, int admin)
        {
            trtopup trtopup = _DB.trtopup.Single<trtopup>(s => s.id_topup == id_topup);
            trtopup.status = 0;
            trtopup.validate = 0;
            trtopup.id_admin = admin;
            trtopup.tgl_validasi = DateTime.Now;

            _DB.SaveChanges();            
        }
    }
}