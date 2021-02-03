﻿using System;
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

        
        #region index
        public ActionResult Index()
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
            Session["Profile"] = rental.profil;
            Session["Toko"] = rental.nama_toko;

            Session["total_pengajuan"] = this.trpenyewaan.getAllDataRental(rental.id_rental, "DISIAPKAN").ToList<trpenyewaan>().Count();
            Session["total_siap"] = this.trpenyewaan.getAllDataRental(rental.id_rental, "SIAP / DIKIRIM").ToList<trpenyewaan>().Count();
            Session["total_pelunasan"] = this.trpenyewaan.getAllDataRental(rental.id_rental, "VALIDASI TRANSFER").ToList<trpenyewaan>().Count();
            Session["total_berjalan"] = this.trpenyewaan.getAllDataRental(rental.id_rental, "BERJALAN").ToList<trpenyewaan>().Count();
            Session["total_selesai"] = this.trpenyewaan.getAllDataRental(rental.id_rental, "SELESAI").ToList<trpenyewaan>().Count();

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            msrental msrental = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            return View(msrental);
        }
        #endregion
        #region edit Account
        public ActionResult edit_profil()
        {
            //ViewBag
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            msrental msrental = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));
            return View(msrental);

        }

        #region TOP UP
        public ActionResult top_up()
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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

        public ActionResult lihat_mutasi(int? page)
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

            //Viewbag dibutuhkan            
            var dtmutasisaldo = this.dtmutasisaldo.getAllRental(Convert.ToInt32(Session["logged_id"])).Take(this.dtmutasisaldo.getAllRental(Convert.ToInt32(Session["logged_id"])).ToList<dtmutasisaldo>().Count());

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(dtmutasisaldo.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region PENYEWAAN

        #region Pengajuan sewa
        public ActionResult Pengajuan_sewa(int? page)
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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

        #region Kritik Saran
        #region add KritikSaran
        public ActionResult kritik_saran()
        {
            //ViewBag WAJIB ADA
            ViewBag.mskelompokjenis = this.mskelompokjenis.getAllData().ToList<mskelompokjenis>();
            ViewBag.logged_in = this.msrental.getRental(Convert.ToInt32(Session["logged_id"]));

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

        #region Dan lain lain
        public ActionResult logout()
        {
            Session["logged_id"] = null;
            Session["id"] = null;
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