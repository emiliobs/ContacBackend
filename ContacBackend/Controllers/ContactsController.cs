using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer;
using ContacBackend.Class;
using ContacBackend.Models;

namespace ContacBackend.Controllers
{
    public class ContactsController : Controller
    {
        #region Atributtes
        private ContactContect db = new ContactContect(); 
        #endregion

        #region ActionResult

        // GET: Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactView view)
        {
            if (ModelState.IsValid)
            {

                var picture = string.Empty;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    picture = FileHelper.UploadPhoto(view.ImageFile, folder);
                    picture =  $"{folder}/{picture}";
                }

                var contact = ToContact(view);
                contact.Image = picture;


                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(view);
        }

      

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contact contact = db.Contacts.Find(id);

            if (contact == null)
            {
                return HttpNotFound();
            }

            var view = ToView(contact);

            return View(view);
        }
                                   
        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactView contactView)
        {
            if (ModelState.IsValid)
            {

                var picture = contactView.Image;
                var folder = "~/Content/Images";

                if (contactView.ImageFile != null)
                {
                    picture = FileHelper.UploadPhoto(contactView.ImageFile, folder);
                    picture = $"{folder}/{picture}";
                }

                var contact = ToContact(contactView);

                contact.Image = picture;

                db.Entry(contact).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(contactView);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private Contact ToContact(ContactView view)
        {
            return new Contact()
            {

                ContactId = view.ContactId,
                EmailAddress = view.EmailAddress,
                FirstName = view.FirstName,
                Image = view.Image,
                LastName = view.LastName,
                PhoneNumber = view.PhoneNumber,

            };

        }

        private ContactView ToView(Contact contact)
        {
            return new ContactView()
            {
                Image = contact.Image,
                ContactId = contact.ContactId,
                EmailAddress = contact.EmailAddress,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                PhoneNumber = contact.PhoneNumber,

            };
        }

        #endregion
    }
}
