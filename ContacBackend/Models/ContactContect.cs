using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ContacBackend.Models
{
    public class ContactContect : DbContext
    {
               //Aqui me conecto a la BD:
        public ContactContect():base("DefaultConnection")
        {
            
        }

        public System.Data.Entity.DbSet<ContacBackend.Models.Contact> Contacts { get; set; }
    }
}