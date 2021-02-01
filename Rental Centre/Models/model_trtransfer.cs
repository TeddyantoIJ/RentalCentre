using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trtransfer
    {
        RCDB _DB = new RCDB();
        model_mspenyewa mspenyewa = new model_mspenyewa();
        model_msrental msrental = new model_msrental();
        model_msadmin msadmin = new model_msadmin();

        public void addData(trtransfer trtransfer)
        {
            _DB.trtransfer.Add(trtransfer);
            _DB.SaveChanges();
            if(trtransfer.jenis_transfer == 11)
            {
                mspenyewa pengirim = this.mspenyewa.getPenyewa(trtransfer.id_pengirim);
                mspenyewa penerima = this.mspenyewa.getPenyewa(trtransfer.id_penerima);

                this.mspenyewa.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_pengirim, penerima.username);
                this.mspenyewa.saldo_tambah(trtransfer.jml_transfer, trtransfer.id_penerima, pengirim.username);
            }
            if (trtransfer.jenis_transfer == 12)
            {
                mspenyewa pengirim = this.mspenyewa.getPenyewa(trtransfer.id_pengirim);
                msrental penerima = this.msrental.getRental(trtransfer.id_penerima);

                this.mspenyewa.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_pengirim, penerima.username);
                this.msrental.saldo_tambah(trtransfer.jml_transfer, trtransfer.id_penerima, pengirim.username);
            }
            if (trtransfer.jenis_transfer == 13)
            {
                mspenyewa pengirim = this.mspenyewa.getPenyewa(trtransfer.id_pengirim);
                msadmin penerima = this.msadmin.getAdmin(trtransfer.id_penerima);

                this.mspenyewa.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_pengirim, penerima.username);
                //this.msadmin.saldo_tambah(trtransfer.jml_transfer, trtransfer.id_pengirim, penerima.username);
            }
            if (trtransfer.jenis_transfer == 21)
            {
                msrental pengirim = this.msrental.getRental(trtransfer.id_pengirim);                
                mspenyewa penerima = this.mspenyewa.getPenyewa(trtransfer.id_penerima);

                this.msrental.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_pengirim, penerima.username);
                this.mspenyewa.saldo_tambah(trtransfer.jml_transfer, trtransfer.id_penerima, pengirim.username);
            }
            if (trtransfer.jenis_transfer == 22)
            {
                msrental pengirim = this.msrental.getRental(trtransfer.id_pengirim);
                msrental penerima = this.msrental.getRental(trtransfer.id_penerima);

                this.msrental.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_pengirim, penerima.username);
                this.msrental.saldo_tambah(trtransfer.jml_transfer, trtransfer.id_penerima, pengirim.username);
            }
            if (trtransfer.jenis_transfer == 23)
            {
                msrental pengirim = this.msrental.getRental(trtransfer.id_pengirim);
                msadmin penerima = this.msadmin.getAdmin(trtransfer.id_penerima);

                this.msrental.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_penerima, penerima.username);
            }
            /*
            if (trtransfer.jenis_transfer == 31)
            {
                msadmin pengirim = this.msadmin.getAdmin(trtransfer.id_pengirim);
                mspenyewa penerima = this.mspenyewa.getPenyewa(trtransfer.id_penerima);

                this.mspenyewa.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_pengirim, penerima.username);
                this.mspenyewa.saldo_tambah(trtransfer.jml_transfer, trtransfer.id_penerima, pengirim.username);
            }
            if (trtransfer.jenis_transfer == 32)
            {
                mspenyewa pengirim = this.mspenyewa.getPenyewa(trtransfer.id_pengirim);
                msrental penerima = this.msrental.getRental(trtransfer.id_penerima);

                
            }
            if (trtransfer.jenis_transfer == 33)
            {
                msrental pengirim = this.mspenyewa.getPenyewa(trtransfer.id_pengirim);
                mspenyewa penerima = this.mspenyewa.getPenyewa(trtransfer.id_penerima);

                
            }*/
        }
    }
}