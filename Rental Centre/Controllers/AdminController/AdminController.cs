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
        //static int Convert.ToInt32(Session["admin"]) = -1;

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
        model_trpenyewaan trpenyewaan = new model_trpenyewaan();
        model_trpembayaran trpembayaran = new model_trpembayaran();
        model_dtdetailpenyewaan dtdetailpenyewaan = new model_dtdetailpenyewaan();
        model_trkritiksaran trkritiksaran = new model_trkritiksaran();
        model_trpencairan trpencairan = new model_trpencairan();
        model_dtchat dtchat = new model_dtchat();

        #region INDEX
        public class items
        {
            public items(string label, int data)
            {
                this.label = label;
                this.data = data;
            }
            public string label;
            public int data;
        }
        public ActionResult Index(DateTime? date1, DateTime? date2)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }            
            
            // VIEW BAG WAJIB
            Session["total_penyewaan"] = this.trpenyewaan.getAllData().ToList<trpenyewaan>().Count();
            Session["total_konfirmasi"] = this.trpenyewaan.getAllData("VALIDASI").ToList<trpenyewaan>().Count();
            Session["total_diproses"] = this.trpenyewaan.getAllData("DISIAPKAN").ToList<trpenyewaan>().Count();
            Session["total_siap"] = this.trpenyewaan.getAllData("SIAP / KIRIM").ToList<trpenyewaan>().Count();
            Session["total_berjalan"] = this.trpenyewaan.getAllData("BERJALAN").ToList<trpenyewaan>().Count();
            Session["total_selesai"] = this.trpenyewaan.getAllData("SELESAI").ToList<trpenyewaan>().Count();
            Session["total_gagal"] = this.trpenyewaan.getAllData("GAGAL").ToList<trpenyewaan>().Count();
            
            msadmin admin = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));
            Session["nama_admin"] = admin.nama_admin;
            Session["profil"] = admin.profil;
            Session["username"] = admin.username;

            // DASHBOARD
            if (date1 == null || date2 == null)
            {
                date1 = DateTime.Now.AddMonths(-1);
                date2 = DateTime.Now;
            }

            // DATA MASTER
            ViewBag.jum_kelompokjenisbarang = this.mskelompokjenis.getAllData().ToList().Count();
            ViewBag.jum_jenisbarang = this.msjenisbarang.getAllData().ToList().Count();
            ViewBag.jum_barang = this.msbarang.getAllData().ToList().Count();
            ViewBag.jum_provinsi = this.msprovinsi.getAllProvinsi().ToList().Count();
            ViewBag.jum_kodepos = this.mskodepos.getAllKodePos().ToList().Count();


            ViewBag.jum_penyewa = this.mspenyewa.getAllDataAktif().ToList().Count();
            ViewBag.jum_rental = this.msrental.getAllAktif().ToList().Count();
            ViewBag.jum_admin = this.msadmin.getAllDataAktif().ToList().Count();

            // DATA STATISTIK

            // UNTUK JUMLAH PRODUK
            var prd = this.dtdetailpenyewaan.JumlahPenjualan(date1, date2);
            List<items> produks = new List<items>();
            int jumlah = 0;
            foreach (var item in prd)
            {
                jumlah += item.jumlah;
                produks.Add(new items(item.nama, item.jumlah));
            }
            ViewBag.produk = produks.ToArray();
            ViewBag.produks = prd;
            // UNTUK JUMLAH TRANSAKSI            
            var penyewaan = this.trpenyewaan.getAllDataByRange(date1, date2).ToList();
            List<items> items = new List<items>();
            foreach (var item in penyewaan)
            {
                items.Add(new items(item.a.ToString("dd-MM-yyyy"), item.b));
            }
            ViewBag.transaksi = items.ToArray();

            // UNTUK PENDAPATAN
            var pendapatan = this.trpenyewaan.getPenghasilanByRange(date1, date2).ToList();
            List<items> pemasukkan = new List<items>();
            int uang_masuk = 0;
            foreach (dtmutasisaldo item in pendapatan)
            {
                pemasukkan.Add(new items(item.creadate.ToString("dd-MM-yyyy"), item.jumlah_transaksi / 9));
                uang_masuk += (item.jumlah_transaksi / 9);
            }
            ViewBag.pemasukkan = pemasukkan.ToArray();

            // RESUME ALL
            ViewBag.total_transaksi = penyewaan.Count();
            ViewBag.total_product_sewa = jumlah;
            ViewBag.total_uang_masuk = uang_masuk;

            ViewBag.date1 = date1;
            ViewBag.date2 = date2;

            return View();
        }
        #endregion

        #region MASTER
       
        #region Kelompok jenis barang
        #region view
        public ActionResult page_kelompokjenisbarang()
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //View bag dibutuhkan
            ViewBag.msadmin = this.msadmin.getAllData().ToList<msadmin>();
            var mskelompokjenis = this.mskelompokjenis.getAllData();

            

            

            return View(mskelompokjenis.ToList<mskelompokjenis>());
        }
        [HttpGet]
        public ActionResult page_kelompokjenisbarang(string currentFilter, string searchString, int? page)
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            return View();
        }
        [HttpPost]
        public ActionResult add_kelompokjenisbarang(mskelompokjenis new_mskelompokjenis)
        {
            new_mskelompokjenis.nama_kelompokjenis = new_mskelompokjenis.nama_kelompokjenis.ToUpper();
            new_mskelompokjenis.deskripsi = new_mskelompokjenis.deskripsi;
            new_mskelompokjenis.status = 1;
            new_mskelompokjenis.creaby = Convert.ToInt32(Session["admin"]);
            new_mskelompokjenis.creadate = DateTime.Now;

            this.mskelompokjenis.addData(new_mskelompokjenis);

            return RedirectToAction("page_kelompokjenisbarang");
        }
        #endregion
        #region edit
        public ActionResult edit_kelompokjenisbarang(int id)
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            new_mskelompokjenis.modiby = Convert.ToInt32(Session["admin"]);
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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //View Bag yang dibutuhkan
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msadmin = this.msadmin.getAllData().ToList<msadmin>();
            ViewBag.jumlahkelompok = this.mskelompokjenis.count();
            return View(this.msjenisbarang.getAllData().ToList<msjenisbarang>());
        }
        [HttpGet]
        public ActionResult page_jenisbarang(string currentFilter, string searchString, int? page)
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //View Bag yang dibutuhkan
            ViewBag.msadmin = this.msadmin.getAllData().ToList<msadmin>();
            ViewBag.jumlahkelompok = this.mskelompokjenis.count();
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            return View();
        }
        [HttpPost]
        public ActionResult add_jenisbarang(msjenisbarang msjenisbarang)
        {
            msjenisbarang.id_kelompokjenis = msjenisbarang.id_kelompokjenis;
            msjenisbarang.nama_jenisbarang = msjenisbarang.nama_jenisbarang;
            msjenisbarang.deskripsi = msjenisbarang.deskripsi;
            msjenisbarang.status = 1;
            msjenisbarang.creaby = Convert.ToInt32(Session["admin"]);
            msjenisbarang.creadate = DateTime.Now;

            this.msjenisbarang.addData(msjenisbarang);
            return RedirectToAction("page_jenisbarang");
        }
        #endregion
        #region Edit
        public ActionResult edit_jenisbarang(int id)
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //View bag dibutuhkan
            return View(this.msjenisbarang.getJenisbarang(id));
        }
        [HttpPost]
        public ActionResult edit_jenisbarang(msjenisbarang msjenisbarang)
        {
            msjenisbarang.id_kelompokjenis = msjenisbarang.id_kelompokjenis;
            msjenisbarang.nama_jenisbarang = msjenisbarang.nama_jenisbarang;
            msjenisbarang.deskripsi = msjenisbarang.deskripsi;
            msjenisbarang.modiby = Convert.ToInt32(Session["admin"]);
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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //View bag yang dibutuhkan / Data
            var msprovinsi = this.msprovinsi.getAllProvinsi();
            return View(msprovinsi.ToList<msprovinsi>());
        }
        [HttpGet]
        public ActionResult page_provinsi(string currentFilter, string searchString, int? page)
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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


        #region admin
        public ActionResult view_admin()
        {
            //View Bag Wajib ada untuk template

            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //data kebutuhan
            msadmin msadmin = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));
            return View(msadmin);
        }

        public ActionResult page_admin()
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // ViewBag dibutuhkan
            ViewBag.msadmin = this.msadmin.getAllData().ToList<msadmin>();

            return View();
        }
        [HttpGet]
        public ActionResult page_admin(string currentFilter, string searchString, int? page)
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));
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

            msadmin.creaby = Convert.ToInt32(Session["admin"]);
            msadmin.creadate = DateTime.Now;
            msadmin.status = 1;

            msadmin.password = RandomString(10);

            if(this.msadmin.adaUsername(msadmin.username) || this.msrental.adaUsername(msadmin.username) || this.mspenyewa.adaUsername(msadmin.username))
            {
                ViewBag.error = "Username sudah digunakan";
                //View Bag Wajib ada untuk template
                
                ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
                            "</b><br>Sekian info yang dapat kami sampaikan atas perhatiannya kami ucapkan terimakasih.";
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
            string date = DateTime.Now.ToString();
            string b = string.Empty;
            for (int i = 0; i < date.Length; i++)
            {
                if (Char.IsDigit(date[i]))
                    b += date[i];
            }
            Session["filename"] = b;
            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
            string path = Path.Combine(Server.MapPath("~/Content/Temp"),b);

            file.SaveAs(path);
        }        
        [HttpPost]
        public void uploadFileFix()
        {
            string date = DateTime.Now.ToString();
            string b = string.Empty;
            for (int i = 0; i < date.Length; i++)
            {
                if (Char.IsDigit(date[i]))
                    b += date[i];
            }
            Session["filename"] = b;
            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
            string path = Path.Combine(Server.MapPath("~/Content/RoleAdmin/img"),b);

            file.SaveAs(path);
        }
        #endregion
        #region edit        
        public ActionResult edit_admin()
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // View bag dibutuhkan
            msadmin msadmin = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));
            return View(msadmin);
        }
        [HttpPost]
        public ActionResult edit_admin(msadmin msadmin)
        {
            msadmin.modiby = Convert.ToInt32(Session["admin"]);
            msadmin.modidate = DateTime.Now;
            msadmin.profil = msadmin.profil;
            this.msadmin.editData(msadmin);
            return RedirectToAction("view_admin");
        }
        #endregion
        #region hapus
        public ActionResult hapus_admin()
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // View bag dibutuhkan
            msadmin msadmin = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));
            return View(msadmin);
        }
        [HttpPost]
        public ActionResult hapus_admin(msadmin msadmin)
        {
            msadmin.modiby = Convert.ToInt32(Session["admin"]);
            msadmin.modidate = DateTime.Now;
            this.msadmin.hapusData(msadmin.id_admin);
            return RedirectToAction("Index");
        }
        #endregion
        #region ubah password
        public ActionResult password_admin()
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // View bag dibutuhkan
            msadmin msadmin = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));
            return View(msadmin);
        }
        [HttpPost]
        public ActionResult password_admin(msadmin msadmin)
        {
            msadmin.modiby = Convert.ToInt32(Session["admin"]);
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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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

        #region Kodepos
        #region View
        public ActionResult page_kodepos()
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //view bag yang dibutuhkan / data
            List<mskodepos> mskodepos = this.mskodepos.getAllKodePos();
            return View(mskodepos);
        }
        [HttpGet]
        public ActionResult page_kodepos(string currentFilter, string searchString, string searchBy, int? page)
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
            this.trtopup.valid(id_topup, Convert.ToInt32(Session["admin"]));
            return RedirectToAction("user_top_up");
        }
        public ActionResult topup_invalid(FormCollection data)
        {
            int id_topup = Convert.ToInt32(data["id_topup"]);
            this.trtopup.valid(id_topup, Convert.ToInt32(Session["admin"]));
            return RedirectToAction("user_top_up");
        }
        #endregion

        #region cek saldo
        public ActionResult cek_saldo()
        {
            //View Bag Wajib ada untuk template            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

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
        #endregion

        #region mutasi
        [HttpGet]
        public ActionResult mutasi(int? page, DateTime? awal, DateTime? akhir, string kelompok)
        {
            if(Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.msrental = this.msrental.getAllData().ToList<msrental>();
            var dtmutasisaldo = this.dtmutasisaldo.getAll().Take(this.dtmutasisaldo.getAll().Count());

            if (awal == null || akhir == null)
            {
                awal = DateTime.Now.AddMonths(-1);
                akhir = DateTime.Now;
            }            
            
            if (kelompok == null)
            {
                kelompok = "A";
            }
            if (kelompok == "A")
            {                
                dtmutasisaldo = this.dtmutasisaldo.getAll().Where(s => s.creadate >= awal && s.creadate <= akhir).Take(this.dtmutasisaldo.getAll().Count());
            }
            else if (kelompok == "TR")
            {                
                dtmutasisaldo = this.dtmutasisaldo.getAll().Where(s => s.jenis_transaksi.Contains("TRANSFER") && DateTime.Compare(s.creadate, awal??DateTime.Now) > 0 && DateTime.Compare(s.creadate, akhir ?? DateTime.Now) < 0).Take(this.dtmutasisaldo.getAll().Count());
            }
            else if (kelompok == "TU")
            {                
                dtmutasisaldo = this.dtmutasisaldo.getAll().Where(s => s.jenis_transaksi.Contains("TOP UP") && DateTime.Compare(s.creadate, awal ?? DateTime.Now) > 0 && DateTime.Compare(s.creadate, akhir ?? DateTime.Now) < 0).Take(this.dtmutasisaldo.getAll().Count());
            }
            else if (kelompok == "DP")
            {
                dtmutasisaldo = this.dtmutasisaldo.getAll().Where(s => s.jenis_transaksi.Contains("DP") && DateTime.Compare(s.creadate, awal ?? DateTime.Now) > 0 && DateTime.Compare(s.creadate, akhir ?? DateTime.Now) < 0).Take(this.dtmutasisaldo.getAll().Count());
            }
            else if (kelompok == "PL")
            {
                dtmutasisaldo = this.dtmutasisaldo.getAll().Where(s => s.jenis_transaksi.Contains("PELUNASAN") && DateTime.Compare(s.creadate, awal ?? DateTime.Now) > 0 && DateTime.Compare(s.creadate, akhir ?? DateTime.Now) < 0).Take(this.dtmutasisaldo.getAll().Count());
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.awal = awal;
            ViewBag.akhir = akhir;
            ViewBag.kelompok = kelompok;

            return View(dtmutasisaldo.ToPagedList<dtmutasisaldo>(pageNumber, pageSize));
        }

        //public ActionResult mutasi(int? page)
        //{
        //    //View Bag Wajib ada untuk template
            
        //    ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

        //    //View bag dibutuhkan
        //    ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
        //    ViewBag.msrental = this.msrental.getAllData().ToList<msrental>();

        //    var dtmutasisaldo = this.dtmutasisaldo.getAll().Where(s => s.creadate > DateTime.Now.AddMonths(-1) && s.creadate < DateTime.Now).Take(this.dtmutasisaldo.getAll().Count());


        //    int pageSize = 10;
        //    int pageNumber = (page ?? 1);
            
        //    return View(dtmutasisaldo.ToPagedList<dtmutasisaldo>(pageNumber, pageSize));
        //}
        #endregion

        #region pencairan
        public ActionResult pencairan(int? page)
        {
            //View Bag Wajib ada untuk template            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //data dibutuhkan
            ViewBag.msrental = this.msrental.getAllData();
            ViewBag.mspenyewa = this.mspenyewa.getAllData();

            var trpencairan = this.trpencairan.getAll().Take(this.trpencairan.getAll().Count<trpencairan>());

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpencairan.ToPagedList(pageNumber, pageSize));

        }

        [HttpPost]
        public ActionResult pencairan(FormCollection data)
        {
            int id_pencairan = Convert.ToInt32(data["id_pencairan"]);
            this.trpencairan.konfirmasi(id_pencairan);
            return RedirectToAction("pencairan");
        }
        #endregion
        #endregion

        #region PENYEWAAN

        #region Konfirmasi
        public ActionResult Konfirmasi(int? page)
        {
            if(Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }

            // View bag wajib
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.trpembayaran = this.trpembayaran.getAll().ToList<trpembayaran>();

            var trpenyewaan = this.trpenyewaan.getAllData("VALIDASI").Take(this.trpenyewaan.getAllData("VALIDASI").ToList<trpenyewaan>().Count());
            

            // Page

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpenyewaan.ToPagedList<trpenyewaan>(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Konfirmasi(FormCollection data)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }
            
            if(data["submit"] == "VALID")
            {
                // UBAH STATUS DP JADI VALID 
                trpembayaran trpembayaran = new trpembayaran();
                trpembayaran.id_penyewaan = Convert.ToInt32(data["id_penyewaan"]);
                trpembayaran.id_admin = Convert.ToInt32(Session["admin"]);
                trpembayaran.tgl_validasi = DateTime.Now;
                trpembayaran.validate = 1;
                
                // UBAH STATUS PENYEWAAN JADI DISIAPKAN dan STATUS DP JADI 1
                if (data["jenis_pembayaran"] == "0")
                {
                    this.trpenyewaan.ubahDisiapkan(Convert.ToInt32(data["id_penyewaan"]),Convert.ToInt32(Session["admin"]));
                    this.trpenyewaan.bayarDP(Convert.ToInt32(data["id_penyewaan"]));
                    this.trpembayaran.ValidasiDP(trpembayaran);
                }
                else
                {
                    this.trpenyewaan.ubahBerjalan(Convert.ToInt32(data["id_penyewaan"]), Convert.ToInt32(Session["admin"]));
                    this.trpenyewaan.bayarSisa(Convert.ToInt32(data["id_penyewaan"]));
                    this.trpembayaran.ValidasiSisa(trpembayaran);
                }                
            }
            else
            {
                // UBAH STATUS PENYEWAAN JADI GAGAL
                //this.trpenyewaan.ubahGagal(Convert.ToInt32(data["id_penyewaan"]), Convert.ToInt32(Session["admin"]));
                

                // UBAH STATUS DP JADI VALID 
                trpembayaran trpembayaran = new trpembayaran();
                trpembayaran.id_penyewaan = Convert.ToInt32(data["id_penyewaan"]);
                trpembayaran.id_admin = Convert.ToInt32(Session["admin"]);
                trpembayaran.tgl_validasi = DateTime.Now;
                trpembayaran.validate = 0;
                if (data["jenis_pembayaran"] == "0")
                {
                    this.trpembayaran.ValidasiDP(trpembayaran);
                }
                else
                {
                    this.trpembayaran.ValidasiSisa(trpembayaran);
                }
                    
            }
            refresh_penyewaan();
            return RedirectToAction("Konfirmasi");
        }
        [HttpPost]
        public ActionResult Pemesanan_details(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }

            // View bag wajib
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //View bag diwajibkan
            var detail = this.dtdetailpenyewaan.getAllData(id).ToList<dtdetailpenyewaan>();
            ViewBag.msbarang = this.msbarang.getAllData();
            ViewBag.trpenyewaan = this.trpenyewaan.getPenyewaan(id);
            ViewBag.mspenyewa = this.mspenyewa.getPenyewa(this.trpenyewaan.getPenyewaan(id).id_penyewa);
            if (this.trpenyewaan.getPenyewaan(id).jenis_sewa == 0)
            {
                ViewBag.jenis_sewa = "DIKIRIM KE ALAMAT";
            }
            else
            {
                ViewBag.jenis_sewa = "DIAMBIL DI TOKO";
            }
            ViewBag.msbarang = this.msbarang.getAllData().ToList<msbarang>();
            return View(detail);
        }
        #endregion

        #region Siap dikirim

        public ActionResult Siap_kirim(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }

            // View bag wajib
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.trpembayaran = this.trpembayaran.getAll().ToList<trpembayaran>();

            var trpenyewaan = this.trpenyewaan.getAllData("SIAP / KIRIM").Take(this.trpenyewaan.getAllData("SIAP / KIRIM").ToList<trpenyewaan>().Count());


            // Page

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpenyewaan.ToPagedList<trpenyewaan>(pageNumber, pageSize));
        }

        #endregion

        #region DIPROSES
        public ActionResult Diproses(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }

            // View bag wajib
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.trpembayaran = this.trpembayaran.getAll().ToList<trpembayaran>();

            var trpenyewaan = this.trpenyewaan.getAllData("DISIAPKAN").Take(this.trpenyewaan.getAllData("DISIAPKAN").ToList<trpenyewaan>().Count());


            // Page

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpenyewaan.ToPagedList<trpenyewaan>(pageNumber, pageSize));
        }
        #endregion

        #region Berjalan
        public ActionResult Berjalan(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }

            // View bag wajib
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.trpembayaran = this.trpembayaran.getAll().ToList<trpembayaran>();

            var trpenyewaan = this.trpenyewaan.getAllData("BERJALAN").Take(this.trpenyewaan.getAllData("BERJALAN").ToList<trpenyewaan>().Count());


            // Page

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpenyewaan.ToPagedList<trpenyewaan>(pageNumber, pageSize));
        }
        #endregion

        #region Selesai
        public ActionResult Selesai(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }

            // View bag wajib
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.trpembayaran = this.trpembayaran.getAll().ToList<trpembayaran>();

            var trpenyewaan = this.trpenyewaan.getAllData("SELESAI").Take(this.trpenyewaan.getAllData("SELESAI").ToList<trpenyewaan>().Count());


            // Page

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpenyewaan.ToPagedList<trpenyewaan>(pageNumber, pageSize));
        }
        #endregion

        #region Gagal
        public ActionResult Gagal(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }

            // View bag wajib
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.trpembayaran = this.trpembayaran.getAll().ToList<trpembayaran>();

            var trpenyewaan = this.trpenyewaan.getAllData("GAGAL").Take(this.trpenyewaan.getAllData("GAGAL").ToList<trpenyewaan>().Count());


            // Page

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpenyewaan.ToPagedList<trpenyewaan>(pageNumber, pageSize));
        }
        #endregion

        #endregion

        #region kritik_saran
        public ActionResult kritik_saran(int? page, int? jumlah)
        {
            //View Bag Wajib ada untuk template
            
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.msrental = this.msrental.getAllData().ToList<msrental>();

            var trkritiksaran = this.trkritiksaran.getAll().Take(this.trkritiksaran.getAll().Count());


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.jumlah = (jumlah ?? 0);
            return View(trkritiksaran.ToPagedList<trkritiksaran>(pageNumber, pageSize));
        }
        #endregion

        #region Laporan mutasi
        public ActionResult laporan_mutasi(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }
            //View Bag Wajib ada untuk template

            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            //View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.msrental = this.msrental.getAllData().ToList<msrental>();

            var dtmutasisaldo = this.dtmutasisaldo.getAll().Where(s => s.creadate > DateTime.Now.AddMonths(-1) && s.creadate < DateTime.Now).ToList();


            //int pageSize = 10;
            //int pageNumber = (page ?? 1);

            //return View(dtmutasisaldo.ToPagedList<dtmutasisaldo>(pageNumber, pageSize));
            return View(dtmutasisaldo);
        }

        public ActionResult laporan_penyewaan(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }

            // View bag wajib
            ViewBag.logged_in = this.msadmin.getAdmin(Convert.ToInt32(Session["admin"]));

            // View bag dibutuhkan
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();
            ViewBag.trpembayaran = this.trpembayaran.getAll().ToList<trpembayaran>();

            var trpenyewaan = this.trpenyewaan.getAllData("SELESAI").Take(this.trpenyewaan.getAllData("SELESAI").ToList<trpenyewaan>().Count());


            // Page

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpenyewaan.ToPagedList<trpenyewaan>(pageNumber, pageSize));
        }

        #endregion

        #endregion

        #region Pesan tesk NONAKTIF

        #region Pesan_baru

        public ActionResult Pesan_baru(string username)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }            
            //viewbag kebutuhan

            ViewBag.mspenyewa = this.mspenyewa.getAllData();
            ViewBag.msadmin = this.msadmin.getAllData();
            ViewBag.msrental = this.msrental.getAllData();

            ViewBag.username_penerima = username;
            dtchat dtchat = new dtchat();

            return View(dtchat);
        }
        [HttpPost]
        public ActionResult sendChatBaru(dtchat data)
        {
            dtchat dtchat = new dtchat();
            dtchat.creadate = DateTime.Now;
            dtchat.dibaca = 0;
            dtchat.isi_pesan = data.isi_pesan;
            dtchat.username_penerima = data.username_penerima;
            dtchat.username_pengirim = Session["username"].ToString();

            var mspenyewas = this.mspenyewa.getAllData();
            var msrentals = this.msrental.getAllData();

            if (!msrentals.Any(s => s.username == data.username_penerima) && !mspenyewas.Any(s => s.username == data.username_penerima))
            {                

                //viewbag kebutuhan
                ViewBag.mspenyewa = this.mspenyewa.getAllData();
                ViewBag.msadmin = this.msadmin.getAllData();
                ViewBag.msrental = this.msrental.getAllData();

                ViewBag.notif = "Username tidak ditemukan!";

                return View("Pesan_baru", data);
            }
            this.dtchat.add(dtchat);

            return RedirectToAction("Obrolan");
        }
        #endregion

        #region Obrolan
        public ActionResult Obrolan()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }            

            ViewBag.mspenyewa = this.mspenyewa.getAllData();
            ViewBag.msadmin = this.msadmin.getAllData();
            ViewBag.msrental = this.msrental.getAllData();

            ViewBag.dtchat = this.dtchat.getChat(Session["username"].ToString()).ToList<dtchat>();
            ViewBag.dtchatdescen = this.dtchat.getChatDesc(Session["username"].ToString()).ToList<dtchat>();
            ViewBag.listobrolan = this.dtchat.getList(Session["username"].ToString());

            return View();
        }
        [HttpPost]
        public JsonResult sendChat(dtchat data)
        {
            dtchat dtchat = new dtchat();
            dtchat.creadate = DateTime.Now;
            dtchat.dibaca = 0;
            dtchat.isi_pesan = data.isi_pesan;
            dtchat.username_penerima = data.username_penerima;
            dtchat.username_pengirim = Session["username"].ToString();
            this.dtchat.add(dtchat);

            dtchat.username_pengirim = dtchat.creadate.ToString("HH:mm");
            dtchat.username_penerima = dtchat.creadate.ToString("dd MMM");

            return Json(dtchat, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void baca(string username)
        {
            this.dtchat.baca(username);
        }
        [HttpPost]
        public JsonResult getjumlahpesan()
        {
            if (Session["admin"] == null)
            {
                return null;
            }
            var dtpesan = this.dtchat.getChatDesc(Session["username"].ToString()).ToList<dtchat>();
            var list = ViewBag.listobrolan = this.dtchat.getList(Session["username"].ToString());
            int jumlah = 0;
            List<dtchat> chat = new List<dtchat>();
            foreach (var item in list)
            {
                foreach (var pesan in dtpesan)
                {
                    if (pesan.username_pengirim == item && pesan.dibaca == 0)
                    {
                        jumlah++;
                        chat.Add(pesan);
                    }
                }
            }
            Session["pesan"] = chat;
            Session["jumlah_pesan"] = jumlah;
            var data = jumlah;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getpesan()
        {
            if (Session["admin"] == null)
            {
                return null;
            }
            var dtpesan = this.dtchat.getChatDesc(Session["username"].ToString()).ToList<dtchat>();
            var list = ViewBag.listobrolan = this.dtchat.getList(Session["username"].ToString());
            int jumlah = 0;
            List<dtchat> chat = new List<dtchat>();
            var data = new dtchat();
            foreach (var item in list)
            {
                foreach (var pesan in dtpesan)
                {
                    if (pesan.username_pengirim == item && pesan.dibaca == 0)
                    {
                        jumlah++;
                        chat.Add(pesan);
                        if (jumlah == 1)
                        {
                            pesan.username_penerima = pesan.creadate.ToString("HH:mm • dd MMM");
                            data = pesan;
                        }
                    }
                }
            }
            Session["pesan"] = chat;
            Session["jumlah_pesan"] = jumlah;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Pesan_hapus
        public ActionResult Pesan_hapus(string username)
        {
            string username_login = Session["username"].ToString();
            this.dtchat.hapus(username, username_login);
            return RedirectToAction("Obrolan");
        }
        #endregion
        #endregion

        #region Dan lain lain
        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Penyewa");
        }

        public void refresh_penyewaan()
        {
            Session["total_penyewaan"] = this.trpenyewaan.getAllData().ToList<trpenyewaan>().Count();
            Session["total_konfirmasi"] = this.trpenyewaan.getAllData("VALIDASI").ToList<trpenyewaan>().Count();
            Session["total_diproses"] = this.trpenyewaan.getAllData("DISIAPKAN").ToList<trpenyewaan>().Count();
            Session["total_berjalan"] = this.trpenyewaan.getAllData("BERJALAN").ToList<trpenyewaan>().Count();
            Session["total_selesai"] = this.trpenyewaan.getAllData("SELESAI").ToList<trpenyewaan>().Count();
            Session["total_gagal"] = this.trpenyewaan.getAllData("GAGAL").ToList<trpenyewaan>().Count();
            Session["total_siap"] = this.trpenyewaan.getAllData("SIAP / KIRIM").ToList<trpenyewaan>().Count();
        }
        #endregion
    }
}