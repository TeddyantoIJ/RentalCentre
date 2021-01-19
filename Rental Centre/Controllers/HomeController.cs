using Rental_Centre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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


        public ActionResult Login(string role, string username, string password)
        {
            if(role == "a")
            {
                msadmin msadmin = this.msadmin.getAllData().Single<msadmin>(s => s.username == username && s.password == password);
                if(msadmin != null)
                {
                    Session["logged_in"] = msadmin;

                    ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
                    ViewBag.logged_in = this.msadmin.getAdmin(msadmin.id_admin);

                    return View("~/Views/Admin/Index.cshtml");
                }
            }
            if (role == "p")
            {
                mspenyewa mspenyewa = this.mspenyewa.getAllData().Single<mspenyewa>(s => s.username == username && s.password == password);
                if (mspenyewa != null)
                {
                    Session["logged_in"] = mspenyewa;

                    ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
                    ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

                    return View("~/Views/Penyewa/Index.cshtml");
                }
            }
            if (role == "r")
            {
                msrental msrental = this.msrental.getAllData().Single<msrental>(s => s.username == username && s.password == password);
                if (msrental != null)
                {
                    
                    TempData["logged_in"] = msrental;
                    ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
                    ViewBag.logged_in = msrental;
                    return View("~/Views/Rental/Index.cshtml");
                }
            }
            return View();
        }        
    }
}