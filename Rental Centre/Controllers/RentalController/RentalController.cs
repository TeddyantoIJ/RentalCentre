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
        
        //static int Convert.ToInt32(Session["logged_id"]) = -1;

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
        model_trpembayaran trpembayaran = new model_trpembayaran();
        model_trtopup trtopup = new model_trtopup();
        model_dtmutasisaldo dtmutasisaldo = new model_dtmutasisaldo();
        model_trtransfer trtransfer = new model_trtransfer();
        model_trpenyewaan trpenyewaan = new model_trpenyewaan();
        model_dtdetailpenyewaan dtdetailpenyewaan = new model_dtdetailpenyewaan();
        model_trkritiksaran trkritiksaran = new model_trkritiksaran();
        model_trpencairan trpencairan = new model_trpencairan();
        model_dtchat dtchat = new model_dtchat();
        model_trkomentar trkomentar = new model_trkomentar();
        model_dtkomentar dtkomentar = new model_dtkomentar();

        #region index
        public class produk
        {
            public produk(string label, int data)
            {
                this.label = label;
                this.data = data;
            }
            public string label;
            public int data;
        }
        public class pendapatan
        {
            public pendapatan(string label, int data)
            {
                this.label = label;
                this.data = data;
            }
            public string label;
            public int data;
        }
        public class transaksi
        {
            public transaksi(string label, int data)
            {
                this.label = label;
                this.data = data;
            }
            public string label;
            public int data;
        }
        public ActionResult Index(DateTime? date1, DateTime? date2)
        {
            
            if(Session["id"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }
            else
            {
                Session["logged_id"] = Convert.ToInt32(Session["id"].ToString());
            }                        
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            msrental rental = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));
            Session["Nama_user"] = rental.nama_rental;
            Session["username"] = rental.username;
            Session["Profile"] = rental.profil;
            Session["Toko"] = rental.nama_toko;

            Session["total_pengajuan"] = this.trpenyewaan.getAllDataRental(rental.id_rental, "DISIAPKAN").ToList<trpenyewaan>().Count();
            Session["total_siap"] = this.trpenyewaan.getAllDataRental(rental.id_rental, "SIAP / DIKIRIM").ToList<trpenyewaan>().Count();
            Session["total_pelunasan"] = this.trpenyewaan.getAllDataRental(rental.id_rental, "VALIDASI TRANSFER").ToList<trpenyewaan>().Count();
            Session["total_berjalan"] = this.trpenyewaan.getAllDataRental(rental.id_rental, "BERJALAN").ToList<trpenyewaan>().Count();
            Session["total_selesai"] = this.trpenyewaan.getAllDataRental(rental.id_rental, "SELESAI").ToList<trpenyewaan>().Count();

            getjumlahpesan();

            if (date1 == null || date2 == null)
            {
                date1 = DateTime.Now.AddMonths(-1);
                date2 = DateTime.Now;
            }            

            // UNTUK JUMLAH PRODUK

            var prd = this.dtdetailpenyewaan.JumlahPenjualan(Convert.ToInt32(Session["logged_id"]), date1, date2);
            List<produk> produks = new List<produk>();
            int jumlah = 0;
            foreach (var item in prd)
            {
                jumlah += item.jumlah;
                produks.Add(new produk(item.nama, item.jumlah));
            }
            ViewBag.produk = produks.ToArray();
            ViewBag.produks = prd;
            // UNTUK JUMLAH TRANSAKSI            
            var penyewaan = this.trpenyewaan.getAllDataByRange(Convert.ToInt32(Session["logged_id"]), date1, date2).ToList();
            List<transaksi> transaksi = new List<transaksi>();
            foreach (var item in penyewaan)
            {
                transaksi.Add(new transaksi(item.a.ToString("dd-MM-yyyy"), item.b));
            }
            ViewBag.transaksi = transaksi.ToArray();

            // UNTUK PENDAPATAN
            var pendapatan = this.trpenyewaan.getPenghasilanByRange(Convert.ToInt32(Session["logged_id"]), date1, date2).ToList();
            List<pendapatan> pemasukkan = new List<pendapatan>();
            foreach (dtmutasisaldo item in pendapatan)
            {
                pemasukkan.Add(new pendapatan(item.creadate.ToString("dd-MM-yyyy"), item.jumlah_transaksi));
            }
            ViewBag.pemasukkan = pemasukkan.ToArray();

            // RESUME ALL
            ViewBag.total_transaksi = penyewaan.Count();
            ViewBag.total_product_sewa = jumlah;
            ViewBag.total_uang_masuk = this.trpenyewaan.getPenghasilan(Convert.ToInt32(Session["logged_id"]), date1, date2);

            ViewBag.date1 = date1;
            ViewBag.date2 = date2;

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
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            var msbarang = this.msbarang.getAllDataByIdRentGroupByKelompok(Convert.ToInt32(Session["logged_id"]), idi).Take(this.msbarang.getAllDataByIdRentGroupByKelompok(Convert.ToInt32(Session["logged_id"]), idi).ToList<msbarang>().Count());
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

            int pageSize = this.msbarang.getAllDataByIdRentGroupByKelompok(Convert.ToInt32(Session["logged_id"]), idi).ToList<msbarang>().Count() ;
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
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            return View();
        }
        [HttpPost]
        public ActionResult ajukan_penawaran(msbarang barang)
        {

            //ViewBag

            barang.id_jenisbarang = barang.id_jenisbarang;
            barang.id_rental = Convert.ToInt32(Session["logged_id"]);
            barang.nama_barang = barang.nama_barang;
            barang.harga_sewa = barang.harga_sewa;
            barang.stok_barang = barang.stok_barang;
            barang.deskripsi_barang = barang.deskripsi_barang;
            barang.link_gambar = barang.link_gambar;

            barang.status = 1;
            barang.creaby = Convert.ToInt32(Session["logged_id"]);
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
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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

            barang.modiby = Convert.ToInt32(Session["logged_id"]);
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

        #region Products

        public ActionResult Product(int id)
        {
            if (Session["logged_id"] == null)
            {
                return RedirectToAction("Index");
            }
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            
            msbarang msbarang = this.msbarang.getBarang(id);
            
            int id_rental = msbarang.id_rental ?? 0;

            ViewBag.rental = this.msrental.getRental(id_rental);
            ViewBag.trkomentar = this.trkomentar.GetTrkomentar(id).ToList();
            List<int> id_komentar = this.trkomentar.GetTrkomentar(id).Select(s => s.id_komentar).ToList<int>();
            ViewBag.dtkomentar = this.dtkomentar.GetDtkomentars(id_komentar).ToList();
            ViewBag.msrental = this.msrental.getAllData();
            ViewBag.mspenyewa = this.mspenyewa.getAllData();            

            return View(msbarang);
        }

        #region Komentar

        [HttpPost]
        public ActionResult Komentar(trkomentar komen, string filter)
        {
            komen.id_rental = Convert.ToInt32(Session["logged_id"]);
            komen.creadate = DateTime.Now;
            this.trkomentar.add(komen);

            return RedirectToAction("Product", "Rental", new { id = komen.id_barang, filter = filter });
        }
        public ActionResult Balas_komentar(dtkomentar balasan, string filter, int id_barang)
        {
            balasan.id_rental = Convert.ToInt32(Session["logged_id"]);
            balasan.creadate = DateTime.Now;

            if (balasan.isi_komentar != "")
            {
                this.dtkomentar.add(balasan);
            }
            return RedirectToAction("Product", "Rental", new { id = id_barang, filter = filter });
        }
        [HttpGet]
        public ActionResult Hapus_komentar(int id)
        {
            trkomentar trkomentar = this.trkomentar.getKomentarbyId(id);
            this.trkomentar.delete(trkomentar);
            this.dtkomentar.delete(id);
            return RedirectToAction("Product", "Rental", new { id = trkomentar.id_barang });
        }
        [HttpGet]
        public ActionResult Hapus_balasan(int id, int id_barang)
        {
            dtkomentar dtkomentar = this.dtkomentar.getKomentarById(id);
            this.dtkomentar.delete(dtkomentar);
            return RedirectToAction("Product", "Rental", new { id = id_barang });
        }
        #endregion

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
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            msrental msrental = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            return View(msrental);
        }
        #endregion
        #region edit Account
        public ActionResult edit_profil()
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            msrental msrental = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            msrental msrental = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            msrental msrental = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            return View(msrental);
        }
        [HttpPost]
        public ActionResult hapus_akun(msrental msrental)
        {
            this.msrental.hapusData(msrental.id_rental);
            Session["logged_id"] = null;
            return RedirectToAction("Index", "Penyewa", new { id = msrental.id_rental });
        }
        #endregion
        #endregion

        #region DANA
        public ActionResult cek_saldo()
        {

            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            msrental msrental = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));
            return View(msrental);

        }

        #region TOP UP
        public ActionResult top_up()
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            return View();
        }
        [HttpPost]
        public ActionResult top_up(FormCollection data)
        {
            trtopup trtopup = new trtopup();
            trtopup.jml_topup = Convert.ToInt32(data["jml_dibayar"]);
            trtopup.bukti_topup = data["bukti_pembayaran"];
            trtopup.creadate = DateTime.Now;
            trtopup.validate = 0;
            trtopup.id_rental = Convert.ToInt32(Session["logged_id"]);
            trtopup.status = 1;

            this.trtopup.addData(trtopup);

            return RedirectToAction("cek_saldo");
        }
        #endregion

        #region Transfer
        public ActionResult transfer()
        {
            if (Convert.ToInt32(Session["logged_id"]) == -1)
            {
                return RedirectToAction("Index");
            }
            //Viewbag wajib ada untuk template
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            //Viewbag dibutuhkan
            msrental a = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));            
            ViewBag.password = a.password;
            ViewBag.saldp = a.saldo;

            return View();
        }
        [HttpPost]
        public ActionResult transfer(FormCollection data)
        {
            string username_tujuan = data["username_tujuan"];
            mspenyewa penyewa = this.mspenyewa.getPenyewaUsername(username_tujuan);
            msrental rental = this.msrental.getRentalUsername(username_tujuan);

            trtransfer trtransfer = new trtransfer();

            trtransfer.id_pengirim = Convert.ToInt32(Session["logged_id"]);
            if (penyewa != null)
            {
                trtransfer.id_penerima = penyewa.id_penyewa;
                trtransfer.jenis_transfer = 21;
            }
            if (rental != null)
            {
                trtransfer.id_penerima = rental.id_rental;
                trtransfer.jenis_transfer = 22;
            }
            if (rental == null && penyewa == null)
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

        #region Mutasi
        public ActionResult lihat_mutasi(int? page)
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            //Viewbag dibutuhkan            
            var dtmutasisaldo = this.dtmutasisaldo.getAllRental(Convert.ToInt32(Session["logged_id"])).Take(this.dtmutasisaldo.getAllRental(Convert.ToInt32(Session["logged_id"])).ToList<dtmutasisaldo>().Count());

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(dtmutasisaldo.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region Uangkan
        public ActionResult Uangkan()
        {
            //ViewBag WAJIB ADA            
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            msrental msrental = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));
            return View(msrental);
        }
        [HttpPost]
        public ActionResult Uangkan(FormCollection data)
        {
            int uang = Convert.ToInt32(data["uangkan"]);
            this.msrental.saldo_kurang(uang, Convert.ToInt32(Session["logged_id"]), "PENCAIRAN");

            trpencairan trpencairan = new trpencairan();
            trpencairan.creadate = DateTime.Now;
            trpencairan.id_rental = Convert.ToInt32(Session["logged_id"]);
            trpencairan.jml_pencairan = uang;
            trpencairan.no_rek = data["no_rek"];
            trpencairan.status_pencairan = 0;
            this.trpencairan.add(trpencairan);
            return RedirectToAction("cek_saldo");
        }            
        #endregion
        #endregion

        #region PENYEWAAN

        #region Pengajuan sewa
        public ActionResult Pengajuan_sewa(int? page)
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            //View bag dibutuhkan
            var trpenyewaan = this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "DISIAPKAN").Take(this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]),"DISIAPKAN").ToList<trpenyewaan>().Count());
            ViewBag.mspenyewa = this.mspenyewa.getAllData();
            ViewBag.trpembayaran = this.trpembayaran.getAll();            
            int pageNumber = page ?? 1;
            int pageSize = 10;

            return View(trpenyewaan.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult Pengemasan(FormCollection data)
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            //UPDATE DATA DIKEMAS
            if(data["selesai"] != null)
            {
                int id_penyewaan = Convert.ToInt32(data["selesai"]);
                var detail = this.dtdetailpenyewaan.getAllData(id_penyewaan);
                bool doneall = true;
                foreach (var item in detail)
                {
                    if(item.status_barang == "DIPROSES")
                    {
                        doneall = false;
                        break;
                    }
                }
                if (doneall)
                {
                    this.trpenyewaan.ubahSiap(id_penyewaan);
                    refresh_penyewaan();
                }
                return RedirectToAction("Pengajuan_sewa");
            }
            else
            {
                if (data["id_barang"] != null)
                {
                    dtdetailpenyewaan detail = new dtdetailpenyewaan();                    
                    detail.id_penyewaan = Convert.ToInt32(data["id_penyewaan"]);
                    detail.id_barang = Convert.ToInt32(data["id_barang"]);

                    var dt = this.dtdetailpenyewaan.getAllData(detail.id_penyewaan);
                    foreach (var item in dt)
                    {
                        if(item.id_barang == detail.id_barang)
                        {
                            msbarang barang = this.msbarang.getBarang(detail.id_barang);
                            barang.stok_barang -= item.jml_barang;
                            this.msbarang.editData(barang);
                            break;
                        }
                    }
                    this.dtdetailpenyewaan.dikemas(detail);                    
                }



                int id_penyewaan = Convert.ToInt32(data["id_penyewaan"]);
                var dtdetailpenyewaan = this.dtdetailpenyewaan.getAllData(id_penyewaan).ToList<dtdetailpenyewaan>();
                ViewBag.msbarang = this.msbarang.getAllData();
                ViewBag.trpenyewaan = this.trpenyewaan.getPenyewaan(id_penyewaan);
                ViewBag.mspenyewa = this.mspenyewa.getPenyewa(this.trpenyewaan.getPenyewaan(id_penyewaan).id_penyewa);
                if (this.trpenyewaan.getPenyewaan(id_penyewaan).jenis_sewa == 0)
                {
                    ViewBag.jenis_sewa = "DIKIRIM KE ALAMAT";
                }
                else
                {
                    ViewBag.jenis_sewa = "DIAMBIL DI TOKO";
                }
                return View(dtdetailpenyewaan);
            }            
        }
        #endregion

        #region SIAP / DIKIRIM
        public ActionResult Siap_kirim(int? page)
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            //View bag dibutuhkan
            var trpenyewaan = this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "SIAP / DIKIRIM").Take(this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "SIAP / DIKIRIM").ToList<trpenyewaan>().Count());
            ViewBag.mspenyewa = this.mspenyewa.getAllData();
            ViewBag.trpembayaran = this.trpembayaran.getAll();
            int pageNumber = page ?? 1;
            int pageSize = 10;

            return View(trpenyewaan.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region Validiasi Pelunasan
        public ActionResult Pelunasan(int? page)
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            //View bag dibutuhkan
            var trpenyewaan = this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "VALIDASI TRANSFER").Take(this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "VALIDASI TRANSFER").ToList<trpenyewaan>().Count());
            ViewBag.mspenyewa = this.mspenyewa.getAllData();
            ViewBag.trpembayaran = this.trpembayaran.getAll();
            int pageNumber = page ?? 1;
            int pageSize = 10;

            return View(trpenyewaan.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region Berjalan
        public ActionResult Berjalan(int? page)
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            //View bag dibutuhkan
            var trpenyewaan = this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "BERJALAN").Take(this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "BERJALAN").ToList<trpenyewaan>().Count());
            ViewBag.mspenyewa = this.mspenyewa.getAllData();
            ViewBag.trpembayaran = this.trpembayaran.getAll();
            int pageNumber = page ?? 1;
            int pageSize = 10;

            return View(trpenyewaan.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult konfirmasi_selesai(FormCollection data)
        {
            int id_penyewaan = Convert.ToInt32(data["id"]);
            this.trpenyewaan.ubahSelesai(id_penyewaan);
            List<dtdetailpenyewaan> detail = this.dtdetailpenyewaan.getAllData(id_penyewaan).ToList<dtdetailpenyewaan>();
            int uang = 0;
            foreach (var item in detail)
            {
                uang += item.harga_total;
                this.dtdetailpenyewaan.dikembalikan(item);
                msbarang barang = this.msbarang.getBarang(item.id_barang);
                barang.stok_barang += item.jml_barang;
                this.msbarang.editData(barang);
            }
            uang = uang * 90 / 100;
            this.msrental.saldo_tambah(uang, Convert.ToInt32(Session["logged_id"]), "PENYEWAAN");
            refresh_penyewaan();
            return RedirectToAction("Selesai");
        }

        #endregion

        #region Selesai
        public ActionResult Selesai(int? page)
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            //View bag dibutuhkan
            var trpenyewaan = this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "SELESAI").Take(this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "SELESAI").ToList<trpenyewaan>().Count());
            ViewBag.mspenyewa = this.mspenyewa.getAllData();
            ViewBag.trpembayaran = this.trpembayaran.getAll();
            int pageNumber = page ?? 1;
            int pageSize = 10;

            return View(trpenyewaan.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region Detail
        [HttpPost]
        public ActionResult Pemesanan_details(int id)
        {
            if (Session["logged_id"] == null)
            {
                return RedirectToAction("Index", "Rental");
            }
            
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();

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

        #endregion

        #region Chatting

        #region Pesan_baru

        public ActionResult Pesan_baru(string username)
        {
            if (Session["logged_id"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            dtchat.username_pengirim = this.msrental.getRental(Convert.ToInt32(Session["logged_id"])).username;

            var mspenyewas = this.mspenyewa.getAllData();            
            var msrentals = this.msrental.getAllData();

            if(!msrentals.Any(s=>s.username == data.username_penerima) && !mspenyewas.Any(s => s.username == data.username_penerima))
            {
                //ViewBag WAJIB ADA
                ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
                ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

                //viewbag kebutuhan
                ViewBag.mspenyewa = this.mspenyewa.getAllData();
                ViewBag.msadmin = this.msadmin.getAllData();
                ViewBag.msrental = this.msrental.getAllData();

                ViewBag.notif = "Username tidak ditemukan!";

                return View("Pesan_baru",data);
            }            
            this.dtchat.add(dtchat);            

            return RedirectToAction("Obrolan");
        }
        #endregion

        #region Obrolan
        public ActionResult Obrolan()
        {
            if(Session["logged_id"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            ViewBag.mspenyewa = this.mspenyewa.getAllData();
            ViewBag.msadmin = this.msadmin.getAllData();
            ViewBag.msrental = this.msrental.getAllData();

            ViewBag.dtchat = this.dtchat.getChat(this.msrental.getRental(Convert.ToInt32(Session["logged_id"])).username).ToList<dtchat>();
            ViewBag.dtchatdescen = this.dtchat.getChatDesc(this.msrental.getRental(Convert.ToInt32(Session["logged_id"])).username).ToList<dtchat>();
            ViewBag.listobrolan = this.dtchat.getList(this.msrental.getRental(Convert.ToInt32(Session["logged_id"])).username);
            
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
            dtchat.username_pengirim = this.msrental.getRental(Convert.ToInt32(Session["logged_id"])).username;
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
            if (Session["logged_id"] == null)
            {
                return null;
            }
            var dtpesan = this.dtchat.getChatDesc(this.msrental.getRental(Convert.ToInt32(Session["logged_id"])).username).ToList<dtchat>();
            var list = ViewBag.listobrolan = this.dtchat.getList(this.msrental.getRental(Convert.ToInt32(Session["logged_id"])).username);
            int jumlah = 0;
            List<dtchat> chat = new List<dtchat>();
            foreach (var item in list)
            {
                foreach (var pesan in dtpesan)
                {
                    if(pesan.username_pengirim == item && pesan.dibaca == 0)
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
            if (Session["logged_id"] == null)
            {
                return null;
            }
            var dtpesan = this.dtchat.getChatDesc(this.msrental.getRental(Convert.ToInt32(Session["logged_id"])).username).ToList<dtchat>();
            var list = ViewBag.listobrolan = this.dtchat.getList(this.msrental.getRental(Convert.ToInt32(Session["logged_id"])).username);
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
                        if(jumlah == 1)
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
            string username_login = this.msrental.getRental(Convert.ToInt32(Session["logged_id"].ToString())).username;
            this.dtchat.hapus(username, username_login);
            return RedirectToAction("Obrolan");
        }
        #endregion
        #endregion

        #region Kritik Saran
        #region add KritikSaran
        public ActionResult kritik_saran()
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_id = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            return View();

        }
        [HttpPost]
        public ActionResult kritik_saran(trkritiksaran trkritiksaran)
        {
            trkritiksaran.id_rental = Convert.ToInt32(Session["logged_id"]);
            trkritiksaran.creadate = DateTime.Now;
            this.trkritiksaran.addData(trkritiksaran);

            return RedirectToAction("kritik_saran");
        }
        #endregion
        #endregion

        #region Laporan
        public ActionResult laporan_penyewaan(int? page)
        {
            if (Session["logged_id"] == null)
            {
                return RedirectToAction("Index", "Penyewa");
            }

            // View bag wajib
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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

        #region Dan lain lain
        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Penyewa");
        }

        public void refresh_penyewaan()
        {
            Session["total_pengajuan"] = this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "DISIAPKAN").ToList<trpenyewaan>().Count();
            Session["total_siap"] = this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "SIAP / DIKIRIM").ToList<trpenyewaan>().Count();
            Session["total_pelunasan"] = this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "VALIDASI TRANSFER").ToList<trpenyewaan>().Count();
            Session["total_berjalan"] = this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "BERJALAN").ToList<trpenyewaan>().Count();
            Session["total_selesai"] = this.trpenyewaan.getAllDataRental(Convert.ToInt32(Session["logged_id"]), "SELESAI").ToList<trpenyewaan>().Count();
        }
        #endregion
    }
}