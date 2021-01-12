using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_msadmin
    {
        RCDB _DB = new RCDB();
        public msadmin getAdmin(int id)
        {
            msadmin admin = _DB.msadmin.Single(data => data.id_admin == id);

            return admin;
        }
        public IEnumerable<msadmin> getAllData()
        {
            var msadmin = (from data in _DB.msadmin select data);

            return msadmin;
        }
    }
}