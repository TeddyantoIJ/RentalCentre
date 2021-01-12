using Rental_Centre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Rental_Centre.Controllers.AdminController
{
    public class AdminController : Controller
    {
        // GET: Admin
        RCDB _DB = new RCDB();
        static int logged_id = 1;

        // MASTER
        model_jenisbarang msjenisbarang = new model_jenisbarang();
        model_kelompokjenis mskelompokjenis = new model_kelompokjenis();
        model_barang msbarang = new model_barang();
        model_msadmin msadmin = new model_msadmin();

        public ActionResult Index()
        {
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            return View();
        }
        #region Kelompok jenis barang
        #region view
        public ActionResult page_kelompokjenisbarang()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View bag dibutuhkan
            ViewBag.msadmin = this.msadmin.getAllData().ToList<msadmin>();
            var mskelompokjenis = this.mskelompokjenis.getAllData();

            return View(mskelompokjenis.ToList<mskelompokjenis>());
        }
        #endregion
        #region tambah
        public ActionResult add_kelompokjenisbarang()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            return View();
        }
        [HttpPost]
        public ActionResult add_kelompokjenisbarang(mskelompokjenis new_mskelompokjenis)
        {
            new_mskelompokjenis.nama_kelompokjenis = new_mskelompokjenis.nama_kelompokjenis.ToUpper();
            new_mskelompokjenis.deskripsi = new_mskelompokjenis.deskripsi;
            new_mskelompokjenis.status = 1;
            new_mskelompokjenis.creaby = logged_id;
            new_mskelompokjenis.creadate = DateTime.Now;

            this.mskelompokjenis.addData(new_mskelompokjenis);

            return RedirectToAction("page_kelompokjenisbarang");
        }
        #endregion
        #region edit
        public ActionResult edit_kelompokjenisbarang(int id)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View bag yang dibutuhkan        
            mskelompokjenis mskelompokjenis = this.mskelompokjenis.getKelompok(id);            
            return View(mskelompokjenis);
        }
        [HttpPost]
        public ActionResult edit_kelompokjenisbarang(mskelompokjenis new_mskelompokjenis)
        {
            new_mskelompokjenis.id_kelompokjenis = new_mskelompokjenis.id_kelompokjenis;
            new_mskelompokjenis.nama_kelompokjenis = new_mskelompokjenis.nama_kelompokjenis.ToUpper();
            new_mskelompokjenis.deskripsi = new_mskelompokjenis.deskripsi;
            new_mskelompokjenis.modiby = logged_id;
            new_mskelompokjenis.modidate = DateTime.Now;

            //save edit
            this.mskelompokjenis.editData(new_mskelompokjenis);
            return RedirectToAction("page_kelompokjenisbarang");
        }
        #endregion
        #region hapus
        public ActionResult hapus_kelompokjenisbarang(int id)
        {
            this.mskelompokjenis.deleteData(id);
            return RedirectToAction("page_kelompokjenisbarang");
        }
        #endregion
        #endregion
        
        #region Jenis barang
        #region upload file
        //public ActionResult m()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult m(mHttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //        try
        //        {
        //            string path = Path.Combine(Server.MapPath("~/Content/RoleUser/images"),
        //                                       Path.GetFileName(file.FileName));
        //            file.SaveAs(path);
        //            ViewBag.Message = "File uploaded successfully";
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Message = "ERROR:" + ex.Message.ToString();
        //        }
        //    else
        //    {
        //        ViewBag.Message = "You have not specified a file.";
        //    }
        //    return View();
        //}
        #endregion
        #region view
        public ActionResult page_jenisbarang()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View Bag yang dibutuhkan
            ViewBag.msadmin = this.msadmin.getAllData().ToList<msadmin>();
            ViewBag.jumlahkelompok = this.mskelompokjenis.count();
            return View(this.msjenisbarang.getAllData().ToList<msjenisbarang>());
        }
        #endregion
        #region Add
        public ActionResult add_jenisbarang()
        { 
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            return View();
        }
        [HttpPost]
        public ActionResult add_jenisbarang(msjenisbarang msjenisbarang)
        {
            msjenisbarang.id_kelompokjenis = msjenisbarang.id_kelompokjenis;
            msjenisbarang.nama_jenisbarang = msjenisbarang.nama_jenisbarang;
            msjenisbarang.deskripsi = msjenisbarang.deskripsi;
            msjenisbarang.status = 1;
            msjenisbarang.creaby = logged_id;
            msjenisbarang.creadate = DateTime.Now;

            this.msjenisbarang.addData(msjenisbarang);
            return RedirectToAction("page_jenisbarang");
        }
        #endregion
        #region Edit
        public ActionResult edit_jenisbarang(int id)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View bag dibutuhkan
            return View(this.msjenisbarang.getJenisbarang(id));
        }
        [HttpPost]
        public ActionResult edit_jenisbarang(msjenisbarang msjenisbarang)
        {
            msjenisbarang.id_kelompokjenis = msjenisbarang.id_kelompokjenis;
            msjenisbarang.nama_jenisbarang = msjenisbarang.nama_jenisbarang;
            msjenisbarang.deskripsi = msjenisbarang.deskripsi;
            msjenisbarang.modiby = logged_id;
            msjenisbarang.modidate = DateTime.Now;

            this.msjenisbarang.editData(msjenisbarang);
            return RedirectToAction("page_jenisbarang");
        }
        #endregion
        #region Delete
        public ActionResult hapus_jenisbarang(int id)
        {
            this.msjenisbarang.hapusData(id);
            return RedirectToAction("page_jenisbarang");
        }
        #endregion
        #endregion
    }
}