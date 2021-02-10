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
        static string error = "";
        //static int Convert.ToInt32(Session["penyewa"].ToString()) = -1;

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
        model_dtkomentar dtkomentar = new model_dtkomentar();
        model_trkomentar trkomentar = new model_trkomentar();
        model_trwishlist trwishlist = new model_trwishlist();
        model_trpencairan trpencairan = new model_trpencairan();
        model_dtchat dtchat = new model_dtchat();

        #region INDEX
        public ActionResult Index()
        {
            
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            ViewBag.msbarang = this.msbarang.getAllData().ToList<msbarang>();

            if(Session["penyewa"] != null || Session["username"] != null)
            {                                
                ViewBag.cart = this.trkeranjang.getAllByPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).ToList<trkeranjang>();
                ViewBag.allBarang = this.msbarang.getAllData().ToList<msbarang>();
                Session["username"] = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).username;
            }
            if(error != "")
            {
                ViewBag.error = error;
                error = "";
            }
            if(Session["error"] != null)
            {
                ViewBag.error = Session["error"].ToString();
                Session["error"] = null;
            }
            return View();
            
        }
        public ActionResult All(string filter)
        {
            if (Session["penyewa"] != null)
            {
                
                ViewBag.cart = this.trkeranjang.getAllByPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).ToList<trkeranjang>();
                ViewBag.allBarang = this.msbarang.getAllData().ToList<msbarang>();
                Session["username"] = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).username;
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            ViewBag.msbarang = this.msbarang.getAllData(DateTime.Now, 1).ToList<msbarang>();            
            Session["filter"] = null;
            if (filter != null)
            {
                Session["filter"] = filter;
            }
            
            
            return View();
        }
        [HttpPost]
        public ActionResult All(DateTime tgl_sewa, int lama_sewa)
        {
            if (Session["penyewa"] != null)
            {
                
                ViewBag.cart = this.trkeranjang.getAllByPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).ToList<trkeranjang>();
                ViewBag.allBarang = this.msbarang.getAllData().ToList<msbarang>();
                Session["username"] = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).username;
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            ViewBag.tgl_sewa = tgl_sewa;
            ViewBag.lama_sewa = lama_sewa;
            tgl_sewa = tgl_sewa.AddDays(lama_sewa);
            ViewBag.msbarang = this.msbarang.getAllData(tgl_sewa,lama_sewa).ToList<msbarang>();
            
            return View();
        }

        #endregion
        #region Product

        public ActionResult Product(int? id, string filter)
        {
            if (Session["penyewa"] != null)
            {                
                ViewBag.cart = this.trkeranjang.getAllByPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).ToList<trkeranjang>();
                ViewBag.allBarang = this.msbarang.getAllData().ToList<msbarang>();
                Session["username"] = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).username;
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            int idb = id ?? 1;
            msbarang msbarang = this.msbarang.getBarang(idb);
            ViewBag.currentFilter = filter;
            int id_rental = msbarang.id_rental ?? 0;

            ViewBag.rental = this.msrental.getRental(id_rental);
            ViewBag.trkomentar = this.trkomentar.GetTrkomentar(idb).ToList();
            List<int> id_komentar = this.trkomentar.GetTrkomentar(idb).Select(s => s.id_komentar).ToList<int>();
            ViewBag.dtkomentar = this.dtkomentar.GetDtkomentars(id_komentar).ToList();
            ViewBag.msrental = this.msrental.getAllData();
            ViewBag.mspenyewa = this.mspenyewa.getAllData();

            ViewBag.currentFilter = filter;

            return View(msbarang);
        }
        #region Komentar

        [HttpPost]
        public ActionResult Komentar(trkomentar komen, string filter)
        {
            komen.id_penyewa = Convert.ToInt32(Session["penyewa"]);
            komen.creadate = DateTime.Now;
            this.trkomentar.add(komen);

            return RedirectToAction("Product", new { id = komen.id_barang, filter = filter });
        }
        public ActionResult Balas_komentar(dtkomentar balasan, string filter, int id_barang)
        {            
            balasan.id_penyewa = Convert.ToInt32(Session["penyewa"]);
            balasan.creadate = DateTime.Now;

            if(balasan.isi_komentar != "")
            {
                this.dtkomentar.add(balasan);
            }            
            return RedirectToAction("Product", new { id = id_barang, filter = filter });
        }
        [HttpGet]
        public ActionResult Hapus_komentar(int id)
        {
            trkomentar trkomentar = this.trkomentar.getKomentarbyId(id);            
            this.trkomentar.delete(trkomentar);
            this.dtkomentar.delete(id);
            return RedirectToAction("Product", new { id = trkomentar.id_barang });
        }
        [HttpGet]
        public ActionResult Hapus_balasan(int id, int id_barang)
        {
            dtkomentar dtkomentar = this.dtkomentar.getKomentarById(id);
            this.dtkomentar.delete(dtkomentar);                        
            return RedirectToAction("Product", new { id = id_barang });
        }
        #endregion

        #region Rental Profile
        public ActionResult Rental_profile(string username)
        {
            msrental msrental = this.msrental.getRentalUsername(username);
            ViewBag.msbarang = this.msbarang.getAllDataByRental(msrental.id_rental).ToList();
            return View(msrental);
        }
        #endregion

        #endregion
        #region MyAccount
        #region View / sudah login
        public ActionResult page_myAccount()
        {
            if(Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"]));
            return View(mspenyewa);
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
                error = "Username sudah digunakan";
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
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString()));
            return View(mspenyewa);
        }
        [HttpPost]
        public ActionResult edit_myAccount(mspenyewa mspenyewa)
        {
            mspenyewa.modiby = "emailedit@gmail.com";
            mspenyewa.modidate = DateTime.Now;

            this.mspenyewa.editPenyewa(mspenyewa);

            return RedirectToAction("page_myAccount");
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
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString()));
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
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString()));
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

        #region cek saldo
        public ActionResult cek_saldo()
        {
            if(Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            
            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString()));

            return View(mspenyewa);
        }

        #endregion
        
        #region Top up
        public ActionResult top_up()
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            mspenyewa mspenyewa = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString()));

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
            trtopup.id_penyewa = Convert.ToInt32(Session["penyewa"].ToString());
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

        #region mutasi
        public ActionResult lihat_mutasi(int? page)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            //Viewbag dibutuhkan            
            var dtmutasisaldo = this.dtmutasisaldo.getAllPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).Take(this.dtmutasisaldo.getAllPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).ToList<dtmutasisaldo>().Count());

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(dtmutasisaldo.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region Transfer

        public ActionResult transfer()
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            //Viewbag dibutuhkan
            mspenyewa a = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString()));
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
            
            trtransfer.id_pengirim = Convert.ToInt32(Session["penyewa"].ToString());
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

        #region Uangkan
        public ActionResult Uangkan()
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            ViewBag.penyewa = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString()));
            return View(mspenyewa);
        }
        [HttpPost]
        public ActionResult Uangkan(FormCollection data)
        {
            int uang = Convert.ToInt32(data["uangkan"]);
            this.mspenyewa.saldo_kurang(uang, Convert.ToInt32(Session["penyewa"]), "PENCAIRAN");

            trpencairan trpencairan = new trpencairan();
            trpencairan.creadate = DateTime.Now;
            trpencairan.id_penyewa = Convert.ToInt32(Session["penyewa"]);
            trpencairan.jml_pencairan = uang;
            trpencairan.no_rek = data["no_rek"];
            trpencairan.status_pencairan = 0;
            this.trpencairan.add(trpencairan);
            return RedirectToAction("cek_saldo");
        }
        #endregion

        #endregion

        #region Penyewaan

        #region Cart
        [HttpGet]
        public ActionResult Cart(DateTime? date1, DateTime? date2)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            ViewBag.cart = this.trkeranjang.getAllByPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).ToList<trkeranjang>();
            ViewBag.countCart = this.trkeranjang.getAllByPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).ToList<trkeranjang>().Count();
            
            DateTime date11 = date1 ?? DateTime.Now;
            DateTime date12 = date2 ?? DateTime.Now;
            
            int selanghari = (date12 - date11).Days;            
            
            ViewBag.allBarang = this.msbarang.getAllData(date11,selanghari).ToList<msbarang>();

            return View();
        }
        
        [HttpGet]
        public ActionResult add_ToCart(int id)
        {
            if (Session["penyewa"] == null)
            {                                
                return RedirectToAction("Index");
            }
            if (this.trkeranjang.ada(id,Convert.ToInt32(Session["penyewa"].ToString())))
            {                
                return RedirectToAction("Cart");
            }
            
            trkeranjang trkeranjang = new trkeranjang();
            trkeranjang.id_barang = id;
            trkeranjang.id_penyewa = Convert.ToInt32(Session["penyewa"].ToString());
            trkeranjang.id_keranjang = Convert.ToInt32(Session["penyewa"].ToString()) + "_" + id;
            this.trwishlist.remove(Convert.ToInt32(Session["penyewa"].ToString()), id);
            this.trkeranjang.add(trkeranjang);            
            return RedirectToAction("Cart");
        }
        [HttpGet]
        public ActionResult add_ToCart1(int id)
        {
            if (Session["penyewa"] == null)
            {                
                return RedirectToAction("Index");
            }
            trkeranjang trkeranjang = new trkeranjang();
            trkeranjang.id_barang = id;
            trkeranjang.id_penyewa = Convert.ToInt32(Session["penyewa"].ToString());
            trkeranjang.id_keranjang = Convert.ToInt32(Session["penyewa"].ToString()) + "_" + id;
            this.trwishlist.remove(Convert.ToInt32(Session["penyewa"].ToString()), id);
            this.trkeranjang.add(trkeranjang);            
            return RedirectToAction("WishList");
        }
        public ActionResult dcart(int id)
        {
            this.trkeranjang.remove(Convert.ToInt32(Session["penyewa"].ToString()), id);
            return RedirectToAction("Cart");
        }
        #endregion

        #region Wishlist
        public ActionResult WishList()
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            
                        
            ViewBag.allBarang = this.msbarang.getAllData().ToList<msbarang>();
            var wishlist = this.trwishlist.getAll(Convert.ToInt32(Session["penyewa"].ToString()));
            return View(wishlist);
        }
        [HttpGet]
        public ActionResult add_toWishList0(int id_barang, string filter)
        {
            if (Session["penyewa"] == null)
            {                
                return RedirectToAction("Index");
            }
            if (this.trwishlist.ada(id_barang, Convert.ToInt32(Session["penyewa"].ToString())))
            {             
                return RedirectToAction("Wishlist");
            }
            trwishlist trwishlist = new trwishlist();
            trwishlist.id_barang = id_barang;
            trwishlist.id_penyewa = Convert.ToInt32(Session["penyewa"].ToString());
            trwishlist.id_wishlist = Convert.ToInt32(Session["penyewa"].ToString()) + "_" + id_barang;
            this.trwishlist.add(trwishlist);
            this.trkeranjang.remove(trwishlist.id_penyewa, id_barang);            
            return RedirectToAction("Wishlist");
        }
        [HttpGet]
        public ActionResult add_toWishList1(int id_barang)
        {
            if (Session["penyewa"] == null)
            {                
                return RedirectToAction("Index");
            }            
            trwishlist trwishlist = new trwishlist();
            trwishlist.id_barang = id_barang;
            trwishlist.id_penyewa = Convert.ToInt32(Session["penyewa"].ToString());
            trwishlist.id_wishlist = Convert.ToInt32(Session["penyewa"].ToString()) + "_" + id_barang;
            this.trwishlist.add(trwishlist);
            this.trkeranjang.remove(trwishlist.id_penyewa, id_barang);
            return RedirectToAction("Cart");
        }
        public ActionResult dwish(int id)
        {
            this.trwishlist.remove(Convert.ToInt32(Session["penyewa"].ToString()),id);
            return RedirectToAction("Wishlist");
        }
        #endregion

        #region Checkout
        public ActionResult Checkout(int? page)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            var trpenyewaan = this.trpenyewaan.getAllDataPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).Take(this.trpenyewaan.getAllDataPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).ToList<trpenyewaan>().Count());
            
            // Page

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpenyewaan.ToPagedList<trpenyewaan>(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult checkout(FormCollection data)
        {
            if(this.trkeranjang.getAllByPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).ToList<trkeranjang>().Count() == 0)
            {
                return RedirectToAction("Index");
            }
            trpenyewaan trpenyewaan = new trpenyewaan();
            trpenyewaan.id_penyewa = Convert.ToInt32(Session["penyewa"].ToString());
            trpenyewaan.jenis_sewa = Convert.ToInt32(data["jenis_penyewaan"]);
            if(trpenyewaan.jenis_sewa == 0)
            {
                trpenyewaan.alamat_tujuan = data["alamat_tujuan"];
                trpenyewaan.kodepos = data["kodepos"];
            }
            else
            {
                trpenyewaan.alamat_tujuan = "-";
                trpenyewaan.kodepos = "-";
            }
            trpenyewaan.creadate = DateTime.Now;
            trpenyewaan.tgl_penyewaan = DateTime.Parse(data["tgl_penyewaan"]);
            trpenyewaan.tgl_pengembalian = DateTime.Parse(data["tgl_pengembalian"]);
            trpenyewaan.total_dp = Convert.ToInt32(data["total_dp"]);
            trpenyewaan.total_harga = Convert.ToInt32(data["total_harga"]);
            trpenyewaan.status_pembayaran = 0;
            trpenyewaan.status_dp = 0;
            trpenyewaan.status_ulasan = 0;
            trpenyewaan.status_transaksi = "PEMESANAN";

            // SIMPAN DATA KE DALAM TABLE PENYEWAAN
            this.trpenyewaan.add(trpenyewaan);

            // PERULANGAN UNTUK MENYIMPAN KE DALAM DETAIL
            dtdetailpenyewaan dtdetailpenyewaan = new dtdetailpenyewaan();
            msbarang barang = new msbarang();
            for(int i = 1; i <= this.trkeranjang.getAllByPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).ToList<trkeranjang>().Count(); i++)
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
            this.trkeranjang.remove(Convert.ToInt32(Session["penyewa"].ToString()));

            return RedirectToAction("Checkout");
        }

        #endregion

        #region Checkout details
        public ActionResult Checkout_details(int id)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

            var detail = this.dtdetailpenyewaan.getAllData(id).ToList<dtdetailpenyewaan>();
            ViewBag.msbarang = this.msbarang.getAllData();
            return View(detail);
        }
        #endregion

        #region batal transaksi
        [HttpPost]
        public ActionResult Batal_transaksi(int id)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            
            this.trpenyewaan.ubahGagal(id);
            this.dtdetailpenyewaan.dibatalkan(id);
            
            return RedirectToAction("Checkout");
        }
        #endregion

        #region Konfirmasi DP

        public ActionResult Konfirmasi_dp(int id)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            ViewBag.dana = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).saldo;
            trpenyewaan trpenyewaan = this.trpenyewaan.getPenyewaan(id,Convert.ToInt32(Session["penyewa"].ToString()));
            if(trpembayaran == null)
            {
                return RedirectToAction("Ceckout");
            }

            return View(trpenyewaan);
        }
        [HttpPost]
        public ActionResult Konfirmasi_dp_fix(FormCollection data)
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
                this.trpenyewaan.ubahValidasiDP(trpembayaran.id_penyewaan);
            }
            else
            {                
                trpembayaran.bukti_pembayaran = "DENGAN DANA";                
                trpembayaran.validate = 1;                
                this.mspenyewa.bayar_dp(trpembayaran.jml_dibayar, Convert.ToInt32(Session["penyewa"].ToString()));
                this.trpenyewaan.bayarDP(trpembayaran.id_penyewaan);
                this.trpenyewaan.ubahDisiapkan(trpembayaran.id_penyewaan);
            }            
            this.trpembayaran.addData(trpembayaran);
            
            return RedirectToAction("Checkout");
        }
        #endregion

        #region Konfirmasi_terima
        [HttpPost]
        public ActionResult Konfirmasi_terima(int id)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            
            ViewBag.dana = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"].ToString())).saldo;
            trpenyewaan trpenyewaan = this.trpenyewaan.getPenyewaan(id);
            return View(trpenyewaan);
        }
        [HttpPost]
        public ActionResult Konfirmasi_terima_fix(FormCollection data)
        {
            trpembayaran trpembayaran = new trpembayaran();
            trpembayaran.id_penyewaan = Convert.ToInt32(data["id_penyewaan"]);
            trpembayaran.jml_dibayar = Convert.ToInt32(data["total_dp"]);
            trpembayaran.creadate = DateTime.Now;
            trpembayaran.jenis_pembayaran = 1;

            if (data["pilihan_bayar"] == "2")
            {
                trpembayaran.bukti_pembayaran = data["bukti_pembayaran"];
                trpembayaran.validate = -1;
                this.trpenyewaan.ubahValidasiTransfer(trpembayaran.id_penyewaan);
            }
            else
            {
                trpembayaran.bukti_pembayaran = "DENGAN DANA";
                trpembayaran.validate = 1;
                this.mspenyewa.bayar_sisa(trpembayaran.jml_dibayar, Convert.ToInt32(Session["penyewa"].ToString()));
                this.trpenyewaan.bayarSisa(trpembayaran.id_penyewaan);
                this.trpenyewaan.ubahBerjalan(trpembayaran.id_penyewaan);
            }            
            this.trpembayaran.addData(trpembayaran);

            return RedirectToAction("Checkout");
        }
        #endregion

        #region Riwayat pembayaran
        public ActionResult Riwayat_pembayaran(int? page)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            var trpembayaran = this.trpembayaran.getAll(Convert.ToInt32(Session["penyewa"])).Take<trpembayaran>(this.trpembayaran.getAll(Convert.ToInt32(Session["penyewa"])).ToList<trpembayaran>().Count());
            ViewBag.trpenyewaan = this.trpenyewaan.getAllData(Convert.ToInt32(Session["penyewa"]));
            // Page

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(trpembayaran.ToPagedList<trpembayaran>(pageNumber, pageSize));            
        }

        #endregion

        #region Beri_ulasan
        public ActionResult Beri_ulasan(int id)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();


            int id_penyewaan = id;
            trpenyewaan trpenyewaan = this.trpenyewaan.getPenyewaan(id_penyewaan);
            ViewBag.dtdetailpenyewaan = this.dtdetailpenyewaan.getAllData(id_penyewaan);
            ViewBag.msrental = this.dtdetailpenyewaan.getAllDataRentalByIdPenyewaan(id_penyewaan);
            ViewBag.barang = this.msbarang.getAllData();

            return View(trpenyewaan);
        }
        [HttpPost]
        public ActionResult Beri_ulasan_fix(FormCollection data)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            int id_penyewaan = Convert.ToInt32(data["id_penyewaan"]);            

            var msrental = this.dtdetailpenyewaan.getAllDataRentalByIdPenyewaan(id_penyewaan);
            var dtdetailpenyewaan = this.dtdetailpenyewaan.getAllData(id_penyewaan);

            foreach (var item in msrental)
            {
                this.msrental.beriRating(Convert.ToInt32(data["rating_" + item.id_rental]), item.id_rental);
            }
            this.trpenyewaan.beriUlasan(id_penyewaan);
            foreach (var item in dtdetailpenyewaan)
            {
                trkomentar komen = new trkomentar();                
                komen.id_penyewa = Convert.ToInt32(Session["penyewa"]);
                komen.isi_komentar = data["bar_"+item.id_barang];
                komen.creadate = DateTime.Now;
                komen.id_barang = item.id_barang;
                if(komen.isi_komentar != "")
                {
                    this.trkomentar.add(komen);
                }                
            }            
            return RedirectToAction("Checkout");
        }
        #endregion

        #endregion

        #region Chatting

        #region Pesan_baru

        public ActionResult Pesan_baru(string username)
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
                        

            //viewbag kebutuhan

            ViewBag.mspenyewa = this.mspenyewa.getAllData();
            ViewBag.msadmin = this.msadmin.getAllData();
            ViewBag.msrental = this.msrental.getAllData();

            ViewBag.username_penerima = username;
            dtchat dtchat = new dtchat();
            dtchat.username_penerima = username;
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
            dtchat.username_pengirim = this.mspenyewa.getPenyewa(Convert.ToInt32(Session["penyewa"])).username;

            var mspenyewas = this.mspenyewa.getAllData();
            var msrentals = this.msrental.getAllData();

            if (!msrentals.Any(s => s.username == data.username_penerima) && !mspenyewas.Any(s => s.username == data.username_penerima))
            {
                //Viewbag wajib ada untuk template
                ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
                ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
                

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
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();
            

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
        #region ga kepake
        [HttpPost]
        public JsonResult getjumlahpesan()
        {
            if (Session["penyewa"] == null)
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
            if (Session["penyewa"] == null)
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
                            pesan.username_penerima = pesan.creadate.ToString("HH:mm | dd MMM");
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

        #region kritik_saran
        #region add kritik_saran
        public ActionResult kritik_saran()
        {
            if (Session["penyewa"] == null)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.msjenisbarang = this.msjenisbarang.getAllData().ToList<msjenisbarang>();

            return View();

        }
        [HttpPost]
        public ActionResult kritik_saran(trkritiksaran trkritiksaran)
        {
            trkritiksaran.id_penyewa = Convert.ToInt32(Session["penyewa"].ToString());
            trkritiksaran.creadate = DateTime.Now;
            this.trkritiksaran.addData(trkritiksaran);

            return RedirectToAction("Index");
        }
        #endregion
        #endregion

        #region Dan lain lain
        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }        
        #endregion
    }
}