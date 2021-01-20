using Rental_Centre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Rental_Centre.Controllers
{
    public class HomeController : Controller
    {
        RCDB _DB = new RCDB();
        model_jenisbarang msjenisbarang = new model_jenisbarang();
        model_kelompokjenis mskelompokjenis = new model_kelompokjenis();
        model_barang msbarang = new model_barang();
        model_msadmin msadmin = new model_msadmin();
        model_msprovinsi msprovinsi = new model_msprovinsi();
        model_mskodepos mskodepos = new model_mskodepos();
        model_msrental msrental = new model_msrental();
        model_mspenyewa mspenyewa = new model_mspenyewa();


        public ActionResult Login(string username, string password)
        {            
            msadmin msadmin = this.msadmin.getAllData().SingleOrDefault<msadmin>(s => s.username == username && s.password == password);
            mspenyewa mspenyewa = this.mspenyewa.getAllData().SingleOrDefault<mspenyewa>(s => s.username == username && s.password == password);
            msrental msrental = this.msrental.getAllData().SingleOrDefault<msrental>(s => s.username == username && s.password == password);
            if (msadmin != null && msadmin.status == 1)
            {
                Session["id"] = msadmin.id_admin;
                return RedirectToAction("Index", "Admin");
            }                        
            if (mspenyewa != null && mspenyewa.status == 1)
            {
                Session["id"] = mspenyewa.id_penyewa;
                return RedirectToAction("Index", "Penyewa");
            }                        
            if (msrental != null && msrental.status == 1)
            {
                Session["id"] = msrental.id_rental;
                return RedirectToAction("Index", "Rental");
            }
            return RedirectToAction("Index", "Penyewa");
        }        
    }
}