using Rental_Centre.Models.RentalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rental_Centre.Models;
using System.IO;

namespace Rental_Centre.Controllers.RentalController
{
    public class RentalController : Controller
    {
        // GET: Rental
        RCDB _DB = new RCDB();
        static int logged_id = 1;

        // MASTER
        model_jenisbarang msjenisbarang = new model_jenisbarang();
        model_kelompokjenis mskelompokjenis = new model_kelompokjenis();
        model_barang msbarang = new model_barang();


        #region index
        public ActionResult Index()
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            return View();
        }
        #endregion

        #region page_penawaran

        #region view
        public ActionResult page_penawaran(int id)
        {


            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getDataByIdKelompok(id).ToList<msjenisbarang>();
            ViewBag.msbarang = this.msbarang.getAllDataByIdRentGroupByKelompok(logged_id, id).ToList<msbarang>();

            ViewBag.totalbarang = this.msbarang.count();
            ViewBag.totaljenisbarang = this.msjenisbarang.count();

            //set ViewBag total barang per jenis
            //int[] sumBarangPerJenis = null;
            
            //foreach(msjenisbarang i in ViewBag.msjenisbarang)
            //{
            //    sumBarangPerJenis[ = this.msbarang.getJumlahbarangperJenis(i.id_jenisbarang);
            //}

            return View();
        }
        #endregion
        
        #region add
        public ActionResult ajukan_penawaran()
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData();

            ViewBag.totalkelompokjenis = this.mskelompokjenis.count();

            return View();
        }
        [HttpPost]
        public ActionResult ajukan_penawaran(msbarang barang)
        {

            //ViewBag

            barang.id_jenisbarang = barang.id_jenisbarang;
            barang.id_rental = logged_id;
            barang.nama_barang = barang.nama_barang;
            barang.harga_sewa = barang.harga_sewa;
            barang.stok_barang = barang.stok_barang;
            barang.deskripsi_barang = barang.deskripsi_barang;
            barang.link_gambar = "/Content/RoleAdmin/img/"+barang.link_gambar;

            barang.status = 1;
            barang.creaby = logged_id;
            barang.creadate = DateTime.Now;
            barang.modiby = null;
            barang.modidate = DateTime.Now;
            
            
            
            this.msbarang.createData(barang);

            return RedirectToAction("page_penawaran", "Rental", new { id = 1 });

        }
        #endregion
        [HttpPost]
        public void uploadFile()
        {
            // Hapus file yang ada di temp
            //DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Content/Temp"));

            //foreach (FileInfo n in di.GetFiles())
            //{
            //    n.Delete();
            //}

            //Upload file di temmp
            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
            string path = Path.Combine(Server.MapPath("~/Content/Temp"),
                                               Path.GetFileName(file.FileName));
            file.SaveAs(path);

            //return RedirectToAction("page_penawaran");
        }
        #region  edit
        //-- Edit barang 
        public ActionResult edit_penawaran(int id)
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData();

            ViewBag.totalkelompokjenis = this.mskelompokjenis.count();

            //Define all data needed
            msbarang barang = this.msbarang.getBarang(id);
            msjenisbarang jenisbarang = this.msjenisbarang.getJenisbarang(Convert.ToInt32(barang.id_jenisbarang));
            mskelompokjenis kelompokjenis = this.mskelompokjenis.getKelompok(Convert.ToInt32(jenisbarang.id_kelompokjenis));

            ViewBag.editBarang = barang;
            ViewBag.editJenisbarang = jenisbarang;
            ViewBag.editKelompokjenis = kelompokjenis;
            return View(barang);
        }
        [HttpPost]
        public ActionResult edit_penawaran(msbarang barang)
        {
            
            barang.nama_barang = barang.nama_barang;
            barang.harga_sewa = barang.harga_sewa;
            barang.stok_barang = barang.stok_barang;
            barang.deskripsi_barang = barang.deskripsi_barang;
            barang.link_gambar = "/Content/RoleAdmin/img/" + barang.link_gambar;

            barang.modiby = logged_id;
            barang.modidate = DateTime.Now;
            
            this.msbarang.editData(barang);

            return RedirectToAction("page_penawaran", "Rental", new { id = 1 });
        }
        #endregion
        
        #region hapus
        //-- HAPUS BARANG
        public ActionResult hapus_penawaran(int id)
        {
            this.msbarang.hapusData(id);
            return RedirectToAction("page_penawaran", "Rental", new { id = 1 });
        }
        #endregion

        #region uploadItem

        #endregion
        #endregion
    }
}