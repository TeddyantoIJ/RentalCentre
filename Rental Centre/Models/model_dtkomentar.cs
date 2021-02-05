using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_dtkomentar
    {
        RCDB _DB = new RCDB();

        public void add(dtkomentar komen)
        {
            _DB.dtkomentar.Add(komen);
            _DB.SaveChanges();
        }
    }
}