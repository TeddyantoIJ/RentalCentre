using Rental_Centre.Models.RentalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rental_Centre.Models;
using System.IO;
using PagedList;
using System.Net.Mail;
using System.Net;

namespace Rental_Centre.Controllers.RentalController
{
    public class RentalController : Controller
    {
        // GET: Rental
        RCDB _DB = new RCDB();
        
        static int logged_id = -1;

        // MASTER
        model_jenisbarang msjenisbarang = new model_jenisbarang();
        model_kelompokjenis mskelompokjenis = new model_kelompokjenis();
        model_barang msbarang = new model_barang();
        model_msadmin msadmin = new model_msadmin();
        model_msprovinsi msprovinsi = new model_msprovinsi();
        model_mskodepos mskodepos = new model_mskodepos();
        model_msrental msrental = new model_msrental();
        model_mspenyewa mspenyewa = new model_mspenyewa();
        public RentalController()
        {            
            
            
        }
        #region index
        public ActionResult Index()
        {
            
            if(Session["id"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }
            else
            {
                logged_id = Convert.ToInt32(Session["id"].ToString());
            }                        
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(logged_id);

            return View();
        }
        #endregion

        #region page_penawaran
        
        #region view
        public ActionResult page_penawaran(int? id, string currentFilter, string searchString, int? page)
        {
            int idi = id ?? 1;
            

            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getDataByIdKelompok(idi).ToList<msjenisbarang>();
            
            ViewBag.totalbarang = this.msbarang.count();
            ViewBag.totaljenisbarang = this.msjenisbarang.count();
            ViewBag.logged_in = this.msrental.getRental(logged_id);

            var msbarang = this.msbarang.getAllDataByIdRentGroupByKelompok(logged_id, idi).Take(this.msbarang.getAllDataByIdRentGroupByKelompok(logged_id, idi).ToList<msbarang>().Count());
            //Pagging
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentId = idi;
            if (!String.IsNullOrEmpty(searchString))
            {
                msbarang = msbarang.Where(s => s.nama_barang.ToLower().Contains(searchString.ToLower()));
            }

            int pageSize = this.msbarang.getAllDataByIdRentGroupByKelompok(logged_id, idi).ToList<msbarang>().Count() ;
            int pageNumber = (page ?? 1);

            if(pageSize == 0)
            {
                return View(msbarang.ToPagedList(pageNumber, 1));
            }
            return View(msbarang.ToPagedList(pageNumber, pageSize));
            
        }
        #endregion
        
        #region add
        public ActionResult ajukan_penawaran()
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData();

            ViewBag.totalkelompokjenis = this.mskelompokjenis.count();
            ViewBag.logged_in = this.msrental.getRental(logged_id);

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
            barang.link_gambar = barang.link_gambar;

            barang.status = 1;
            barang.creaby = logged_id;
            barang.creadate = DateTime.Now;
            barang.modiby = null;
            barang.modidate = DateTime.Now;
            
            
            
            //this.msbarang.createData(barang);

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
        [HttpPost]
        public void uploadFileFix()
        {
            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
            string path = Path.Combine(Server.MapPath("~/Content/RoleRental/Image/Barang"),
                                               Path.GetFileName(file.FileName));

            file.SaveAs(path);
        }
        #region  edit
        //-- Edit barang 
        public ActionResult edit_penawaran(int id)
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData();

            ViewBag.totalkelompokjenis = this.mskelompokjenis.count();
            ViewBag.logged_in = this.msrental.getRental(logged_id);

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
            barang.link_gambar = barang.link_gambar;

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

        #region Account
        #region addAccount
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public ActionResult page_signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult page_signup(msrental msrental)
        {
            msrental.creadate = DateTime.Now;
            msrental.password = RandomString(10);

            msrental.jml_barang = 0;
            msrental.status = 1;
            msrental.saldo = 0;
            msrental.rating = 0;

            if (this.msadmin.adaUsername(msrental.username) || this.msrental.adaUsername(msrental.username) || this.mspenyewa.adaUsername(msrental.username))
            {
                ViewBag.error = "Username sudah digunakan";
                //View Bag Wajib ada untuk template
                return View();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("rentalcentreofficial@gmail.com", "Rental Centre");
                    var receiverEmail = new MailAddress(msrental.email, "Receiver");
                    var password = "@RentalCentre123";
                    var sub = "Rental Centre Official, Verifikasi Password";
                    var body = "<h2>Hello, " + msrental.nama_rental +
                            "</h2>Berkaitan dengan website Rental Centre, Berikut Terlampir detail informasi akun anda<br>"
                            + "Username : <b>" + msrental.username + "</b><br>Password   : <b>" + msrental.password +
                            "</b>Sekian info yang dapat kami sampaikan atas perhatiannya kami ucapkan terimakasih." +
                            "<br><br>Sekretaris";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body,
                        IsBodyHtml = true
                    })
                    {
                        smtp.Send(mess);
                    }
                    this.msrental.addData(msrental);
                    return View();
                } 
            }
            catch (Exception ex)
            {
                ViewBag.error = "Alamat email tidak tepat";
                return View();
            }
            return View();
        }

        [HttpPost]
        public void uploadFileAkun()
        {
            HttpPostedFileBase profil, berkas1, berkas2;
            string path;


            profil = Request.Files[0]; //Uploaded file
            berkas1 = Request.Files[1]; //Uploaded file
            berkas2 = Request.Files[2]; //Uploaded file

            path = Path.Combine(Server.MapPath("~/Content/RoleRental/Image/Profil"),
                                               Path.GetFileName(profil.FileName));
            profil.SaveAs(path);

            path = Path.Combine(Server.MapPath("~/Content/RoleRental/Image/Berkas1"),
                                               Path.GetFileName(berkas1.FileName));
            berkas1.SaveAs(path);

            path = Path.Combine(Server.MapPath("~/Content/RoleRental/Image/Berkas2"),
                                               Path.GetFileName(berkas2.FileName));
            berkas2.SaveAs(path);

        }
        #endregion
        #region profil
        public ActionResult page_profil()
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(logged_id);

            msrental msrental = this.msrental.getRental(logged_id);

            return View(msrental);
        }
        #endregion
        #region edit Account
        public ActionResult edit_profil()
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(logged_id);

            msrental msrental = this.msrental.getRental(logged_id);

            return View(msrental);
        }
        [HttpPost]
        public ActionResult edit_profil(msrental msrental)
        {
            msrental.modidate = DateTime.Now;
            this.msrental.editData(msrental);
            return RedirectToAction("Index");
        }
        public void uploadProfil()
        {
            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
            string path = Path.Combine(Server.MapPath("~/Content/RoleRental/Image/Profil"),
                                               Path.GetFileName(file.FileName));
            file.SaveAs(path);

            //return RedirectToAction("page_penawaran");
        }
        public void uploadBerkas1()
        {
            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
            string path = Path.Combine(Server.MapPath("~/Content/RoleRental/Image/Berkas1"),
                                               Path.GetFileName(file.FileName));
            file.SaveAs(path);

            //return RedirectToAction("page_penawaran");
        }
        public void uploadBerkas2()
        {
            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
            string path = Path.Combine(Server.MapPath("~/Content/RoleRental/Image/Berkas2"),
                                               Path.GetFileName(file.FileName));
            file.SaveAs(path);

            //return RedirectToAction("page_penawaran");
        }
        #endregion
        #region edit password
        public ActionResult edit_password()
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(logged_id);

            msrental msrental = this.msrental.getRental(logged_id);

            return View(msrental);
        }
        [HttpPost]
        public ActionResult edit_password(msrental msrental)
        {
            msrental.modidate = DateTime.Now;
            this.msrental.editPassword(msrental);
            return RedirectToAction("Index");
        }

        #endregion

        #region hapus akun
        public ActionResult hapus_akun()
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(logged_id);

            msrental msrental = this.msrental.getRental(logged_id);

            return View(msrental);
        }
        [HttpPost]
        public ActionResult hapus_akun(msrental msrental)
        {
            this.msrental.hapusData(msrental.id_rental);
            logged_id = -1;
            return RedirectToAction("Index", "Penyewa", new { id = msrental.id_rental });
        }
        #endregion
        #endregion

        #region Dan lain lain
        public ActionResult logout()
        {
            logged_id = -1;
            return RedirectToAction("Index", "Penyewa");
        }
        #endregion
    }
}