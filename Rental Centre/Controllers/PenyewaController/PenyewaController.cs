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
        model_trtransfer trtransfer = new model_trtransfer();
        model_trkeranjang trkeranjang = new model_trkeranjang();
        model_trpenyewaan trpenyewaan = new model_trpenyewaan();
        model_dtdetailpenyewaan dtdetailpenyewaan = new model_dtdetailpenyewaan();
        model_trkritiksaran trkritiksaran = new model_trkritiksaran();

        public ActionResult Index()
        {
            
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            ViewBag.msbarang = this.msbarang.getAllData().ToList<msbarang>().Take(20);

            if(Session["id"] != null)
            {
                logged_id = Convert.ToInt32(Session["id"].ToString());
                ViewBag.logged_in = "hidden";
                ViewBag.cart = this.trkeranjang.getAllByPenyewa(logged_id).ToList<trkeranjang>();
                ViewBag.allBarang = this.msbarang.getAllData().ToList<msbarang>();
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
                trtransfer.jenis_transfer = 11;
            }
            if(rental != null)
            {
                trtransfer.id_penerima = rental.id_rental;
                trtransfer.jenis_transfer = 12;
            }
            if(rental == null && penyewa == null)
            {
                
                return RedirectToAction("transfer");
            }
                       
            trtransfer.jml_transfer = Convert.ToInt32(data["jml_transfer"]);            
            trtransfer.deskripsi = data["deskripsi"];
            trtransfer.creadate = DateTime.Now;

            this.trtransfer.addData(trtransfer);
            
            return RedirectToAction("transfer");
        }
        #endregion

        #region Penyewaan

        #region Cart
        public ActionResult Cart()
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            ViewBag.logged_in = "hidden";
            ViewBag.cart = this.trkeranjang.getAllByPenyewa(logged_id).ToList<trkeranjang>();
            ViewBag.countCart = this.trkeranjang.getAllByPenyewa(logged_id).ToList<trkeranjang>().Count();
            ViewBag.allBarang = this.msbarang.getAllData().ToList<msbarang>();

            return View();
        }


        [HttpPost]
        public bool add_ToCart(int id)
        {
            if (logged_id == -1)
            {                
                return false;
            }
            if (this.trkeranjang.ada(id,logged_id))
            {
                return false;
            }
            
            trkeranjang trkeranjang = new trkeranjang();
            trkeranjang.id_barang = id;
            trkeranjang.id_penyewa = logged_id;
            trkeranjang.id_keranjang = logged_id + "_" + id;
            this.trkeranjang.add(trkeranjang);
            return true;
        }

        #endregion        

        #region Checkout
        public ActionResult Checkout(int? page)
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            ViewBag.logged_in = "hidden";

            var trpenyewaan = this.trpenyewaan.getAllDataPenyewa(logged_id).Take(this.trpenyewaan.getAllDataPenyewa(logged_id).ToList<trpenyewaan>().Count());
            
            // Page

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpenyewaan.ToPagedList<trpenyewaan>(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult checkout(FormCollection data)
        {
            trpenyewaan trpenyewaan = new trpenyewaan();
            trpenyewaan.id_penyewa = logged_id;
            trpenyewaan.jenis_sewa = Convert.ToInt32(data["jenis_sewa"]);
            if(trpenyewaan.jenis_sewa == 0)
            {
                trpenyewaan.alamat_tujuan = data["alamat_tujuan"];
                trpenyewaan.kodepos = data["kodepos"];
            }
            trpenyewaan.creadate = DateTime.Now;
            trpenyewaan.tgl_penyewaan = DateTime.Parse(data["tgl_penyewaan"]);
            trpenyewaan.tgl_pengembalian = DateTime.Parse(data["tgl_pengembalian"]);
            trpenyewaan.total_dp = Convert.ToInt32(data["total_dp"]);
            trpenyewaan.total_harga = Convert.ToInt32(data["total_harga"]);
            trpenyewaan.status_pembayaran = 0;
            trpenyewaan.status_dp = 0;
            trpenyewaan.status_transaksi = "PEMESANAN";

            // SIMPAN DATA KE DALAM TABLE PENYEWAAN
            this.trpenyewaan.add(trpenyewaan);

            // PERULANGAN UNTUK MENYIMPAN KE DALAM DETAIL
            dtdetailpenyewaan dtdetailpenyewaan = new dtdetailpenyewaan();
            msbarang barang = new msbarang();
            for(int i = 1; i <= this.trkeranjang.getAllByPenyewa(logged_id).ToList<trkeranjang>().Count(); i++)
            {
                barang = this.msbarang.getBarang(Convert.ToInt32(data["id_" + i]));
                dtdetailpenyewaan.creadate = DateTime.Now;                
                dtdetailpenyewaan.jml_barang = Convert.ToInt32(data["jumlah_" + i]);
                dtdetailpenyewaan.id_barang = barang.id_barang;
                dtdetailpenyewaan.harga_total = barang.harga_sewa * dtdetailpenyewaan.jml_barang;
                dtdetailpenyewaan.id_penyewaan = this.trpenyewaan.getLastId();
                dtdetailpenyewaan.status_barang = "DIPROSES";
                this.dtdetailpenyewaan.add(dtdetailpenyewaan);
            }

            // PENGHAPUSAN DATA KERANJANG KARENA SUDAH CHECKOUT
            this.trkeranjang.remove(logged_id);

            return RedirectToAction("Checkout");
        }

        public ActionResult Checkout_details(int id)
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            ViewBag.logged_in = "hidden";

            var detail = this.dtdetailpenyewaan.getAllData(id).ToList<dtdetailpenyewaan>();
            ViewBag.msbarang = this.msbarang.getAllData();
            return View(detail);
        }
        #endregion

        #region Konfirmasi DP

        public ActionResult Konfirmasi_dp(int id)
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.logged_in = "hidden";
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            ViewBag.dana = this.mspenyewa.getPenyewa(logged_id).saldo;
            trpenyewaan trpenyewaan = this.trpenyewaan.getPenyewaan(id,logged_id);
            if(trpembayaran == null)
            {
                return RedirectToAction("Ceckout");
            }

            return View(trpenyewaan);
        }
        [HttpPost]
        public ActionResult Konfirmasi_dp(FormCollection data)
        {
            trpembayaran trpembayaran = new trpembayaran();
            trpembayaran.id_penyewaan = Convert.ToInt32(data["id_penyewaan"]);
            trpembayaran.jml_dibayar = Convert.ToInt32(data["total_dp"]);            
            trpembayaran.creadate = DateTime.Now;
            trpembayaran.jenis_pembayaran = 0;

            
            if (data["pilihan_bayar"] == "2")
            {                                
                trpembayaran.bukti_pembayaran = data["bukti_pembayaran"];                
                trpembayaran.validate = -1;                
            }
            else
            {                
                trpembayaran.bukti_pembayaran = "DENGAN DANA";                
                trpembayaran.validate = 1;                
                this.mspenyewa.bayar_dp(trpembayaran.jml_dibayar, logged_id);
                this.trpenyewaan.bayarDP(trpembayaran.id_penyewaan);
            }
            this.trpenyewaan.ubahDisiapkan(trpembayaran.id_penyewaan);
            this.trpembayaran.addData(trpembayaran);
            
            return RedirectToAction("Checkout");
        }
        #endregion
        #endregion

        #region kritik_saran
        #region add kritik_saran
        public ActionResult kritik_saran()
        {
            if (logged_id == -1)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.logged_in = "hidden";
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            return View();

        }
        [HttpPost]
        public ActionResult kritik_saran(trkritiksaran trkritiksaran)
        {
            trkritiksaran.id_penyewa = logged_id;
            trkritiksaran.creadate = DateTime.Now;
            this.trkritiksaran.addData(trkritiksaran);

            return RedirectToAction("Index");
        }
        #endregion
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