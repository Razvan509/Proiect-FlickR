using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proiect_FlickR.Models;
using System.IO;

namespace Proiect_FlickR.Controllers
{
    public class PicturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pictures
        public ActionResult Index()
        {
            return View(db.Pictures.ToList());
        }

        // GET: Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: Pictures/Create
        [HttpGet]
        public ActionResult Create()
        {
            Picture picture = new Picture();
            return View(picture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Picture picture, HttpPostedFileBase file)
        {
             
            if (file != null)
            {
                //file.SaveAs(HttpContext.Server.MapPath("~/Pictures/")+ file.FileName);
                //picture.Path = file.FileName;

                //var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Path.GetExtension(file.FileName)
                var path = Path.Combine(Server.MapPath("~/Pictures"), file.FileName);

                file.SaveAs(path);
                picture.Path = file.FileName;
                picture.Id = 1;
                picture.Name = "meli";
                    
            }

            if (ModelState.IsValid)
            {
                db.Pictures.Add(picture);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
             
             
    
        }

            // GET: Pictures/Edit/5
            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Path")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(picture);
        }

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Pictures.Find(id);
            db.Pictures.Remove(picture);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
