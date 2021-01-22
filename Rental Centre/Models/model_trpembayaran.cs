using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_trpembayaran
    {
        RCDB _DB = new RCDB();

        public void addData(trpembayaran trpembayaran)
        {
            _DB.trpembayaran.Add(trpembayaran);
            _DB.SaveChanges();
        }
    }
}