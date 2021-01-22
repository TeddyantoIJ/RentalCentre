using PagedList;
using Rental_Centre.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Rental_Centre.Controllers.PenyewaController
{
    public class PenyewaController : Controller
    {
        // GET: Penyewa
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

        //Transaksi
        model_trpembayaran trpembayaran = new model_trpembayaran();
        model_trtopup trtopup = new model_trtopup();
        model_dtmutasisaldo dtmutasisaldo = new model_dtmutasisaldo();
        model_trtransaksi trtransaksi = new model_trtransaksi();

        public ActionResult Index()
        {
            
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            if(Session["id"] != null)
            {
                logged_id = Convert.ToInt32(Session["id"].ToString());
                ViewBag.logged_in = "hidden";
            }
            return View();
            
        }
        
        #region MyAccount
        #region View / sudah login
        public ActionResult page_myAccount()
        {
            if(logged_id != -1)
            {
                return RedirectToAction("Index");
            }
            return View("add_myAccount");
        }
        #endregion
        #region add
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public ActionResult add_myAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add_myAccount(mspenyewa mspenyewa)
        {
            mspenyewa.creaby = "teddyanto.ij@gmail.com";
            mspenyewa.creadate = DateTime.Now;
            mspenyewa.status = 1;
            mspenyewa.saldo = 0;
            mspenyewa.nama_penyewa = mspenyewa.nama_penyewa;
            mspenyewa.password = RandomString(10);

            if (this.msadmin.adaUsername(mspenyewa.username) || this.msrental.adaUsername(mspenyewa.username) || this.mspenyewa.adaUsername(mspenyewa.username))
            {
                ViewBag.error = "Username sudah digunakan";
                //View Bag Wajib ada untuk template
                

                return View();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("rentalcentreofficial@gmail.com","Rental Centre");
                    var receiverEmail = new MailAddress(mspenyewa.email, "Receiver");
                    var password = "@RentalCentre123";
                    var sub = "Rental Centre Official, Verifikasi Password";
                    var body = "<h2>Hello, " + mspenyewa.nama_penyewa +
                            "</h2>Berkaitan dengan website Rental Centre, Berikut Terlampir detail informasi akun anda<br>"
                            + "Username : <b>" + mspenyewa.username + "</b><br>Password   : <b>" + mspenyewa.password +
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
                    
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Some Error"+ex.Message;
            }
            this.mspenyewa.addPenyewa(mspenyewa);
            return RedirectToAction("Index");
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
            HttpPostedFileBase profil, berkas1, berkas2;
            string path;

            
            profil = Request.Files[0]; //Uploaded file
            berkas1 = Request.Files[1]; //Uploaded file
            berkas2 = Request.Files[2]; //Uploaded file

            path = Path.Combine(Server.MapPath("~/Content/RoleUser/images/profil_user"),
                                               Path.GetFileName(profil.FileName));
            profil.SaveAs(path);

            path = Path.Combine(Server.MapPath("~/Content/RoleUser/images/berkas_1"),
                                               Path.GetFileName(berkas1.FileName));
            berkas1.SaveAs(path);

            path = Path.Combine(Server.MapPath("~/Content/RoleUser/images/berkas_2"),
                                               Path.GetFileName(berkas2.FileName));
            berkas2.SaveAs(path);

        }
        #endregion

        #region edit
        public ActionResult edit_myAccount()
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(logged_id);
            return View(mspenyewa);
        }
        [HttpPost]
        public ActionResult edit_myAccount(mspenyewa mspenyewa)
        {
            mspenyewa.modiby = "emailedit@gmail.com";
            mspenyewa.modidate = DateTime.Now;

            this.mspenyewa.editPenyewa(mspenyewa);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public void uploadprofil()
        {
            HttpPostedFileBase berkas;
            string path;


            berkas = Request.Files[0]; //Uploaded file
            path = Path.Combine(Server.MapPath("~/Content/RoleUser/images/profil_user"),
                                               Path.GetFileName(berkas.FileName));
            berkas.SaveAs(path);
        }
        [HttpPost]
        public void uploadberkas1()
        {
            HttpPostedFileBase berkas;
            string path;


            berkas = Request.Files[0]; //Uploaded file
            path = Path.Combine(Server.MapPath("~/Content/RoleUser/images/berkas_1"),
                                               Path.GetFileName(berkas.FileName));
            berkas.SaveAs(path);
        }
        [HttpPost]
        public void uploadberkas2()
        {
            HttpPostedFileBase berkas;
            string path;


            berkas = Request.Files[0]; //Uploaded file
            path = Path.Combine(Server.MapPath("~/Content/RoleUser/images/berkas_2"),
                                               Path.GetFileName(berkas.FileName));
            berkas.SaveAs(path);
        }
        #endregion
        #region ubah Password
        public ActionResult edit_password()
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(logged_id);
            return View(mspenyewa);
        }
        [HttpPost]
        public ActionResult edit_password(mspenyewa mspenyewa)
        {
            string emaillogin = "login@gmail.com";
            mspenyewa.modiby = emaillogin;
            mspenyewa.modidate = DateTime.Now;
            this.mspenyewa.ubahPass(mspenyewa);

            return RedirectToAction("Index");
        }
        #endregion
        #region hapus
        public ActionResult hapus_myAccount()
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(logged_id);
            return View(mspenyewa);
        }
        [HttpPost]
        public ActionResult hapus_myAccount(mspenyewa mspenyewa)
        {
            string emailLogin = "emailhapus@gmail.com";
            this.mspenyewa.hapusPenyewaSelf(mspenyewa.id_penyewa, emailLogin);
            return RedirectToAction("Index");
        }
        #endregion
        #endregion

        #region Saldo
        public ActionResult cek_saldo()
        {
            if(logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            ViewBag.logged_in = "hidden";
            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(logged_id);

            return View(mspenyewa);
        }
        
        #region Top up
        public ActionResult top_up()
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            ViewBag.logged_in = "hidden";

            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(logged_id);

            return View(mspenyewa);
        }
        [HttpPost]
        public ActionResult top_up(FormCollection data)
        {
            trtopup trtopup = new trtopup();
            trtopup.jml_topup = Convert.ToInt32(data["jml_dibayar"]);
            trtopup.bukti_topup = data["bukti_pembayaran"];            
            trtopup.creadate = DateTime.Now;
            trtopup.validate = 0;
            trtopup.id_penyewa = logged_id;
            trtopup.status = 1;

            this.trtopup.addData(trtopup);            

            return RedirectToAction("cek_saldo");
        }
        public void uploadBukti()
        {
            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
            string path = Path.Combine(Server.MapPath("~/Content/RoleAdmin/img/bukti_transfer"),
                                               Path.GetFileName(file.FileName));

            file.SaveAs(path);
        }
        #endregion

        public ActionResult lihat_mutasi(int? page)
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            ViewBag.logged_in = "hidden";

            //Viewbag dibutuhkan            
            var dtmutasisaldo = this.dtmutasisaldo.getAllPenyewa(logged_id).Take(this.dtmutasisaldo.getAllPenyewa(logged_id).ToList<dtmutasisaldo>().Count());

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(dtmutasisaldo.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult transfer()
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            ViewBag.logged_in = "hidden";

            //Viewbag dibutuhkan
            mspenyewa a = this.mspenyewa.getPenyewa(logged_id);
            ViewBag.saldo = a.saldo;
            ViewBag.password = a.password;
            ViewBag.mspenyewa = this.mspenyewa.getAllData().ToList<mspenyewa>();

            return View();
        }
        [HttpPost]
        public ActionResult transfer(FormCollection data)
        {
            string username_tujuan = data["username_tujuan"];
            mspenyewa penyewa = this.mspenyewa.getPenyewaUsername(username_tujuan);
            msrental rental = this.msrental.getRentalUsername(username_tujuan);

            trtransfer trtransfer = new trtransfer();
            
            trtransfer.id_pengirim = logged_id;
            if(penyewa != null)
            {
                trtransfer.id_penerima = penyewa.id_penyewa;
                trtransfer.jenis_transfer = 1;
            }
            if(rental != null)
            {
                trtransfer.id_penerima = rental.id_rental;
                trtransfer.jenis_transfer = 2;
            }
            if(rental == null && penyewa == null)
            {
                
                return RedirectToAction("transfer");
            }
            

            trtransfer.jml_transfer = Convert.ToInt32(data["jml_transfer"]);            
            trtransfer.deskripsi = data["deskripsi"];
            trtransfer.creadate = DateTime.Now;

            this.trtransaksi.addData(trtransfer);
            
            return RedirectToAction("transfer");
        }
        #endregion

        #region Dan lain lain
        public ActionResult logout()
        {
            logged_id = -1;
            Session["id"] = null;
            return RedirectToAction("Index");
        }        
        #endregion
    }
}