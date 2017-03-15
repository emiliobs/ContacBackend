using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ContacBackend.Class;
using ContacBackend.Models;

namespace ContacBackend.Controllers.API
{
    public class ContactsController : ApiController
    {
        private ContactContect db = new ContactContect();

        // GET: api/Contacts
        public IQueryable<Contact> GetContacts()
        {
            return db.Contacts;
        }

        // GET: api/Contacts/5
        [ResponseType(typeof(Contact))]
        public async Task<IHttpActionResult> GetContact(int id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // PUT: api/Contacts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContact(int id, ContactRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.ContactId)
            {
                return BadRequest();
            }

            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
               var stream = new MemoryStream(request.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Images";
                var fullPath = $"{folder}{file}";
                var response = FileHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    request.Image = fullPath;
                }
            
            }

            var contact = ToContact(request);

            db.Entry(contact).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private Contact  ToContact(ContactRequest request)
        {
           return  new Contact()
            {
                Image = request.Image,
                ContactId = request.ContactId,
                EmailAddress = request.EmailAddress,
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                
            };
        }

        // POST: api/Contacts
        [ResponseType(typeof(Contact))]
        public async Task<IHttpActionResult> PostContact(ContactRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(request.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Images";
                var fullPath = $"{folder}{file}";
                var response = FileHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    request.Image = fullPath;
                }

            }

            var contact = ToContact(request);

            db.Contacts.Add(contact);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = contact.ContactId }, contact);
        }

        // DELETE: api/Contacts/5
        [ResponseType(typeof(Contact))]
        public async Task<IHttpActionResult> DeleteContact(int id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            db.Contacts.Remove(contact);
            await db.SaveChangesAsync();

            return Ok(contact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactExists(int id)
        {
            return db.Contacts.Count(e => e.ContactId == id) > 0;
        }
    }
}