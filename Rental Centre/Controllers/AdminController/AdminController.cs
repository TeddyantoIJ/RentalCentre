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
        model_msprovinsi msprovinsi = new model_msprovinsi();
        model_mskodepos mskodepos = new model_mskodepos();

        public ActionResult Index()
        {
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

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

        #region Provinsi
        #region View
        public ActionResult page_provinsi()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View bag yang dibutuhkan / Data
            var msprovinsi = this.msprovinsi.getAllProvinsi();
            return View(msprovinsi.ToList<msprovinsi>());
        }
        #endregion
        #region add
        public ActionResult add_provinsi()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            return View();
        }
        [HttpPost]
        public ActionResult add_provinsi(msprovinsi msprovinsi)
        {
            var a = msprovinsi.nama_provinsi;
            this.msprovinsi.addData(msprovinsi);

            return RedirectToAction("page_provinsi");
        }
        #endregion
        #region edit
        public ActionResult edit_provinsi(int id)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            // Data yang dibutuhkan
            msprovinsi msprovinsi = this.msprovinsi.getProvinsi(id);
            return View(msprovinsi);
        }
        [HttpPost]
        public ActionResult edit_provinsi(msprovinsi msprovinsi)
        {
            this.msprovinsi.editData(msprovinsi);

            return RedirectToAction("page_provinsi");
        }
        #endregion
        #region hapus
        public ActionResult hapus_provinsi(int id)
        {
            this.msprovinsi.hapusData(id);
            return RedirectToAction("page_provinsi");
        }
        #endregion
        #endregion

        #region pengguna
        #region view
        public ActionResult view_admin(int id)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //data kebutuhan
            msadmin msadmin = this.msadmin.getAdmin(id);
            return View(msadmin);
        }
        


        public ActionResult page_pengguna()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            // ViewBag dibutuhkan
            ViewBag.msadmin = this.msadmin.getAllData().ToList<msadmin>();

            return View();
        }
        #region admin
        #region add
        public ActionResult add_admin()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            return View();
        }
        [HttpPost]
        public ActionResult add_admin(msadmin msadmin)
        {
            msadmin.nama_admin = msadmin.nama_admin;
            msadmin.username = msadmin.username;
            msadmin.password = msadmin.password;
            msadmin.tempat_lahir = msadmin.tempat_lahir;
            msadmin.jenis_kelamin = msadmin.jenis_kelamin;
            msadmin.email = msadmin.email;

            msadmin.creaby = logged_id;
            msadmin.creadate = DateTime.Now;
            msadmin.status = 1;
            this.msadmin.addData(msadmin);
                
            return RedirectToAction("page_pengguna");
        }
        [HttpPost]
        public void uploadFile()
        {
            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
            string path = Path.Combine(Server.MapPath("~/Content/Temp"),
                                               Path.GetFileName(file.FileName));

            file.SaveAs(path);
        }
        [HttpPost]
        public void uploadFileFix()
        {
            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
            string path = Path.Combine(Server.MapPath("~/Content/RoleAdmin/img"),
                                               Path.GetFileName(file.FileName));

            file.SaveAs(path);
        }
        #endregion
        #region edit
        public ActionResult edit_admin(int id)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            // View bag dibutuhkan
            msadmin msadmin = this.msadmin.getAdmin(id);
            return View(msadmin);
        }
        [HttpPost]
        public ActionResult edit_admin(msadmin msadmin)
        {
            msadmin.modiby = logged_id;
            msadmin.modidate = DateTime.Now;

            this.msadmin.editData(msadmin);
            return RedirectToAction("page_pengguna");
        }
        #endregion
        #region hapus
        public ActionResult hapus_admin(int id)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            // View bag dibutuhkan
            msadmin msadmin = this.msadmin.getAdmin(id);
            return View(msadmin);
        }
        [HttpPost]
        public ActionResult hapus_admin(msadmin msadmin)
        {
            msadmin.modiby = logged_id;
            msadmin.modidate = DateTime.Now;
            this.msadmin.hapusData(msadmin.id_admin);
            return RedirectToAction("Index");
        }
        #endregion
        #region ubah password
        public ActionResult password_admin(int id)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            // View bag dibutuhkan
            msadmin msadmin = this.msadmin.getAdmin(logged_id);
            return View(msadmin);
        }
        [HttpPost]
        public ActionResult password_admin(msadmin msadmin)
        {
            msadmin.modiby = logged_id;
            msadmin.modidate = DateTime.Now;

            this.msadmin.ubahPassword(msadmin);
            return RedirectToAction("Index");
        }
        #endregion
        #endregion
        #endregion
        #endregion

        #region Kodepos
        #region View
        public ActionResult page_kodepos()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //view bag yang dibutuhkan / data
            List<mskodepos> mskodepos = this.mskodepos.getAllKodePos();
            return View(mskodepos);
        }
        #endregion
        #region Tambah
        public ActionResult add_kodepos()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            return View();
        }
        [HttpPost]
        public ActionResult add_kodepos(mskodepos mskodepos)
        {
            mskodepos.kodepos = mskodepos.kodepos;
            mskodepos.kelurahan = mskodepos.kelurahan;
            mskodepos.kecamatan = mskodepos.kecamatan;
            mskodepos.kota = mskodepos.kota;
            mskodepos.provinsi = mskodepos.provinsi;

            this.mskodepos.addData(mskodepos);
            return RedirectToAction("page_kodepos");
        }
        #endregion
        #region Edit
        public ActionResult edit_kodepos(int id)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View bag dibutuhkan
            return View(this.mskodepos.getKodepos(id));
        }
        [HttpPost]
        public ActionResult edit_kodepos(mskodepos mskodepos)
        {
            mskodepos.id_kodepos = mskodepos.id_kodepos;
            mskodepos.kodepos = mskodepos.kodepos;
            mskodepos.kelurahan = mskodepos.kelurahan;
            mskodepos.kecamatan = mskodepos.kecamatan;
            mskodepos.kota = mskodepos.kota;
            mskodepos.provinsi = mskodepos.provinsi;

            this.mskodepos.editData(mskodepos);
            return RedirectToAction("page_kodepos");
        }
        #endregion
        #region Hapus
        public ActionResult hapus_kodepos(int id)
        {
            this.mskodepos.hapusData(id);
            return RedirectToAction("page_kodepos");
        }
        #endregion
        #endregion
    }
}