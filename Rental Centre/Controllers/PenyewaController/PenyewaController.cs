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
        static int logged_id = 1;

        // MASTER
        model_jenisbarang msjenisbarang = new model_jenisbarang();
        model_kelompokjenis mskelompokjenis = new model_kelompokjenis();
        model_barang msbarang = new model_barang();
        model_msadmin msadmin = new model_msadmin();
        model_msprovinsi msprovinsi = new model_msprovinsi();
        model_mspenyewa mspenyewa = new model_mspenyewa();

        public ActionResult Index()
        {
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            return View();
        }
        #region MyAccount
        #region View / sudah login
        public ActionResult page_myAccount()
        {
            if(logged_id.ToString() == "")
            {
                return View("add_myAccount");
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
        [HttpPost]
        public ActionResult add_myAccount(mspenyewa mspenyewa)
        {
            mspenyewa.creaby = "teddyanto.ij@gmail.com";
            mspenyewa.creadate = DateTime.Now;
            mspenyewa.status = 1;
            mspenyewa.saldo = 0;
            mspenyewa.nama_penyewa = mspenyewa.nama_penyewa;
            mspenyewa.password = RandomString(10);
             
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
            if (logged_id.ToString() == "")
            {
                return View("add_myAccount");
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

        #region hapus
        public ActionResult hapus_myAccount(int id)
        {
            if (logged_id.ToString() == "")
            {
                return View("add_myAccount");
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
    }
}