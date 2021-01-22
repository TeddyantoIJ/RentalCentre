using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trtransaksi
    {
        RCDB _DB = new RCDB();
        model_mspenyewa mspenyewa = new model_mspenyewa();
        model_msrental msrental = new model_msrental();

        public void addData(trtransfer trtransfer)
        {
            _DB.trtransfer.Add(trtransfer);
            _DB.SaveChanges();
            if(trtransfer.jenis_transfer == 1)
            {
                this.mspenyewa.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_pengirim);
                this.mspenyewa.saldo_tambah(trtransfer.jml_transfer, trtransfer.id_penerima);
            }
            if (trtransfer.jenis_transfer == 2)
            {
                this.mspenyewa.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_pengirim);
                this.msrental.saldo_tambah(trtransfer.jml_transfer, trtransfer.id_penerima);
            }
            if (trtransfer.jenis_transfer == 3)
            {
                this.msrental.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_pengirim);
                this.mspenyewa.saldo_tambah(trtransfer.jml_transfer, trtransfer.id_penerima);
            }
            if (trtransfer.jenis_transfer == 4)
            {
                this.msrental.saldo_kurang(trtransfer.jml_transfer, trtransfer.id_pengirim);
                this.msrental.saldo_tambah(trtransfer.jml_transfer, trtransfer.id_penerima);
            }
        }
    }
}