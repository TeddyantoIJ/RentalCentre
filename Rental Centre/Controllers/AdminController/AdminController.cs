using Rental_Centre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;
using System.Net.Mail;
using System.Net;

namespace Rental_Centre.Controllers.AdminController
{   
    public class AdminController : Controller
    {
        // GET: Admin
        RCDB _DB = new RCDB();
        static int logged_id = -1;

        // MASTER
        model_msjenisbarang msjenisbarang = new model_msjenisbarang();
        model_mskelompokjenis mskelompokjenis = new model_mskelompokjenis();
        model_msbarang msbarang = new model_msbarang();
        model_msadmin msadmin = new model_msadmin();
        model_msprovinsi msprovinsi = new model_msprovinsi();
        model_mskodepos mskodepos = new model_mskodepos();
        model_msrental msrental = new model_msrental();
        model_mspenyewa mspenyewa = new model_mspenyewa();

        //TRANSAKSI
        model_trtopup trtopup = new model_trtopup();
        model_dtmutasisaldo dtmutasisaldo = new model_dtmutasisaldo();

        public ActionResult Index()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }
            else
            {
                logged_id = Convert.ToInt32(Session["id"].ToString());
            }

            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            return View();
        }

        #region MASTER
       
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
        [HttpGet]
        public ActionResult page_kelompokjenisbarang(string currentFilter, string searchString, int? page)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View bag dibutuhkan
            ViewBag.msadmin = this.msadmin.getAllData().ToList<msadmin>();
            var mskelompokjenis = this.mskelompokjenis.getAllData().Take(this.mskelompokjenis.count());


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
            if (!String.IsNullOrEmpty(searchString))
            {
                mskelompokjenis = mskelompokjenis.Where(s => s.nama_kelompokjenis.ToLower().Contains(searchString.ToLower()));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            
            return View(mskelompokjenis.ToPagedList(pageNumber,pageSize));
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
        [HttpGet]
        public ActionResult page_jenisbarang(string currentFilter, string searchString, int? page)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View Bag yang dibutuhkan
            ViewBag.msadmin = this.msadmin.getAllData().ToList<msadmin>();
            ViewBag.jumlahkelompok = this.mskelompokjenis.count();

            var msjenisbarang = this.msjenisbarang.getAllData()
                        .Where(s => s.status == 1)
                        .Take(this.msjenisbarang.count());

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
            if (!String.IsNullOrEmpty(searchString))
            {

                msjenisbarang = msjenisbarang.Where(s => s.nama_jenisbarang.ToLower().Contains(searchString.ToLower()));
            }
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(msjenisbarang.ToPagedList(pageNumber, pageSize));
            
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

        #region barang
        [HttpGet]
        public ActionResult page_barang(string currentFilter, string searchString, int? page, string searchBy)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View Bag yang dibutuhkan
            ViewBag.msrental = this.msrental.getAllData().ToList<msrental>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            var msbarang = this.msbarang.getAllData().Take(this.msbarang.getAllData().ToList<msbarang>().Count());

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
            if (!String.IsNullOrEmpty(searchString))
            {
                msbarang = msbarang.Where(s => s.nama_barang.ToLower().Contains(searchString.ToLower()));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(msbarang.ToPagedList(pageNumber, pageSize));

        }
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
        [HttpGet]
        public ActionResult page_provinsi(string currentFilter, string searchString, int? page)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View bag yang dibutuhkan / Data
            var msprovinsi = this.msprovinsi.getAllProvinsi().Take(this.msprovinsi.getAllProvinsi().ToList<msprovinsi>().Count());

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
            if (!String.IsNullOrEmpty(searchString))
            {
                msprovinsi = msprovinsi.Where(s => s.nama_provinsi.ToLower().Contains(searchString.ToLower()));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(msprovinsi.ToPagedList(pageNumber, pageSize));
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

        #region admin
        public ActionResult page_admin()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            // ViewBag dibutuhkan
            ViewBag.msadmin = this.msadmin.getAllData().ToList<msadmin>();

            return View();
        }
        [HttpGet]
        public ActionResult page_admin(string currentFilter, string searchString, int? page)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            // ViewBag dibutuhkan
            var msadmin = this.msadmin.getAllData().Take(this.msadmin.getAllData().ToList<msadmin>().Count());

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
            if (!String.IsNullOrEmpty(searchString))
            {
                msadmin = msadmin.Where(s => s.nama_admin.ToLower().Contains(searchString.ToLower()));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(msadmin.ToPagedList(pageNumber, pageSize));
        }

        #region add
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult add_admin()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);
            msadmin msadmin = new msadmin();
            return View(msadmin);
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

            msadmin.password = RandomString(10);

            if(this.msadmin.adaUsername(msadmin.username) || this.msrental.adaUsername(msadmin.username) || this.mspenyewa.adaUsername(msadmin.username))
            {
                ViewBag.error = "Username sudah digunakan";
                //View Bag Wajib ada untuk template
                ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
                ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

                return View(msadmin);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("rentalcentreofficial@gmail.com", "Rental Centre");
                    var receiverEmail = new MailAddress(msadmin.email, "Receiver");
                    var password = "@RentalCentre123";
                    var sub = "Rental Centre Official, Verifikasi Password";
                    var body = "<h2>Hello, " + msadmin.nama_admin +
                            "</h2>Berkaitan dengan website Rental Centre, Berikut Terlampir detail informasi akun anda<br>"
                            + "Username : <b>" + msadmin.username + "</b><br>Password   : <b>" + msadmin.password +
                            "</b><br>Sekian info yang dapat kami sampaikan atas perhatiannya kami ucapkan terimakasih." +
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
                    //this.msadmin.addPenyewa(msadmin);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Maaf email tidak ditemukan ";
                return View();
            }


            this.msadmin.addData(msadmin);
                
            return RedirectToAction("page_admin");
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

        #region rental
        [HttpGet]
        public ActionResult page_rental(string currentFilter, string searchString, int? page)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View Bag yang dibutuhkan            
            var msrental = this.msrental.getAllData().Take(this.msrental.getAllData().ToList<msrental>().Count());

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
            if (!String.IsNullOrEmpty(searchString))
            {
                msrental = msrental.Where(s => s.nama_rental.ToLower().Contains(searchString.ToLower()));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(msrental.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region penyewa
        [HttpGet]
        public ActionResult page_penyewa(string currentFilter, string searchString, int? page)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View Bag yang dibutuhkan            
            var mspenyewa = this.mspenyewa.getAllData().Take(this.mspenyewa.getAllData().ToList<mspenyewa>().Count());

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
            if (!String.IsNullOrEmpty(searchString))
            {
                mspenyewa = mspenyewa.Where(s => s.nama_penyewa.ToLower().Contains(searchString.ToLower()));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(mspenyewa.ToPagedList(pageNumber, pageSize));
        }
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
        [HttpGet]
        public ActionResult page_kodepos(string currentFilter, string searchString, string searchBy, int? page)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //view bag yang dibutuhkan / data
            var mskodepos = this.mskodepos.getAllKodePos().Take(this.mskodepos.getAllKodePos().ToList<mskodepos>().Count());

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
            ViewBag.CurrentGroup = searchBy;
            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchBy == "Kodepos")
                {
                    mskodepos = mskodepos.Where(s => s.kodepos.Contains(searchString));
                }
                if (searchBy == "Kelurahan")
                {
                    mskodepos = mskodepos.Where(s => s.kelurahan.ToLower().Contains(searchString.ToLower()));
                }
                if (searchBy == "Kecamatan")
                {
                    mskodepos = mskodepos.Where(s => s.kecamatan.ToLower().Contains(searchString.ToLower()));
                }
                if (searchBy == "Kota")
                {
                    mskodepos = mskodepos.Where(s => s.kota.ToLower().Contains(searchString.ToLower()));
                }
                if (searchBy == "Provinsi")
                {
                    mskodepos = mskodepos.Where(s => s.provinsi.ToLower().Contains(searchString.ToLower()));
                }
            }
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(mskodepos.ToPagedList(pageNumber, pageSize));
            
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

        #endregion

        #region TRANSAKSI

        #region REKENING

        #region user top up        
        public ActionResult user_top_up(int? page)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.msrental = this.msrental.getAllData().ToList<msrental>();

            var trtopup = this.trtopup.getAll().Take(this.trtopup.getAll().ToList<trtopup>().Count());


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(trtopup.ToPagedList<trtopup>(pageNumber,pageSize));
        }
        public ActionResult topup_valid(FormCollection data)
        {
            int id_topup = Convert.ToInt32(data["id_topup"]);
            this.trtopup.valid(id_topup, logged_id);
            return RedirectToAction("user_top_up");
        }
        public ActionResult topup_invalid(FormCollection data)
        {
            int id_topup = Convert.ToInt32(data["id_topup"]);
            this.trtopup.valid(id_topup, logged_id);
            return RedirectToAction("user_top_up");
        }
        #endregion

        public ActionResult cek_saldo()
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            var mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            var msrental = this.msrental.getAllData().ToList<msrental>();

            int saldo = 0;
            foreach (var item in mspenyewa)
            {
                saldo += item.saldo;
            }
            foreach (var item in msrental)
            {
                saldo += item.saldo;
            }
            ViewBag.saldo = saldo;
            return View();
        }

        public ActionResult mutasi(int? page, int? jumlah)
        {
            //View Bag Wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msadmin.getAdmin(logged_id);

            //View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.msrental = this.msrental.getAllData().ToList<msrental>();

            var dtmutasisaldo = this.dtmutasisaldo.getAll().Take(this.dtmutasisaldo.getAll().Count());


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.jumlah = (jumlah ?? 0);
            return View(dtmutasisaldo.ToPagedList<dtmutasisaldo>(pageNumber, pageSize));
        }
        #endregion

        #endregion
        #region Dan lain lain
        public ActionResult logout()
        {
            logged_id = -1;
            Session["id"] = null;
            return RedirectToAction("Index", "Penyewa");
        }
        #endregion
    }
}