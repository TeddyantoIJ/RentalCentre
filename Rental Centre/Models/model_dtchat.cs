using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rental_Centre.Models
{
    public class model_dtchat
    {
        RCDB _DB = new RCDB();

        public void add(dtchat dtchat)
        {
            _DB.dtchat.Add(dtchat);
            _DB.SaveChanges();
        }
        public void baca(string username_pengirim)
        {
            var chat = (from data in _DB.dtchat
                        where data.username_pengirim == username_pengirim
                        select data);
            foreach (var item in chat)
            {
                item.dibaca = 1;
            }
            _DB.SaveChanges();
        }
        public IEnumerable<dtchat> getChat(string username)
        {
            var chat = (from data in _DB.dtchat
                        where data.username_penerima == username || data.username_pengirim == username
                        orderby data.creadate ascending
                        select data);
            return chat;
        }
        public IEnumerable<dtchat> getChatDesc(string username)
        {
            var chat = (from data in _DB.dtchat
                        where data.username_penerima == username || data.username_pengirim == username
                        orderby data.creadate descending
                        select data);
            return chat;
        }
        public List<string> getList(string username)
        {
            var data1 = (from data in _DB.dtchat
                         where data.username_penerima == username || data.username_pengirim == username
                         orderby data.creadate descending
                         select new { username = data.username_penerima, waktu = data.creadate });
            var data2 = (from data in _DB.dtchat
                         where data.username_penerima == username || data.username_pengirim == username
                         orderby data.creadate descending
                         select new { username = data.username_pengirim, waktu = data.creadate });

            var dabes = data1.Concat(data2);            
            var output = dabes.OrderByDescending(s => s.waktu);
            var output1 = output.Select(s => s.username);
            List<string> hasil = new List<string>();
            foreach (var item in output1)
            {                
                if (!hasil.Any(x => x == item))
                {
                    hasil.Add(item);
                }
            }
            
            hasil.Remove(username);
            return hasil;
        }
        public void hapus(string username_hapus, string username_login)
        {

            var data = (from b in _DB.dtchat
                        where b.username_penerima == username_login && b.username_pengirim == username_hapus
                        select b);
            foreach(var item in data)
            {
                _DB.dtchat.Remove(item);
            }
            data = (from b in _DB.dtchat
                        where b.username_penerima == username_hapus && b.username_pengirim == username_login
                    select b);
            foreach (var item in data)
            {
                _DB.dtchat.Remove(item);
            }
            _DB.SaveChanges();
        }
    }
}