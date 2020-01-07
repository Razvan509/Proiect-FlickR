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
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using Proiect_FlickR.ImageHandler;
using Microsoft.AspNet.Identity;


namespace Proiect_FlickR.Controllers
{
   
    public class PictureController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pictures
        public ActionResult Index(string search, int? category)
        {
            var pictures = db.Pictures;
            var categories = GetAllCategories();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            if (category != null)
            {
                var picturess = pictures.Where(x => x.CategoryId == category);
                ViewBag.Pictures = picturess.Where(x => x.Name.Contains(search) || search == null).ToList();
                ViewBag.Categories = categories;
                return View();
            }
            ViewBag.Pictures = pictures.Where(x => x.Name.Contains(search) || search == null).ToList();
            ViewBag.Categories = categories;

            return View();
            //return View(db.Pictures.ToList());
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
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Create()

        {
            Picture picture = new Picture();

            // preluam lista de categorii din metoda GetAllCategories()
            picture.Categories = GetAllCategories();
            // Preluam ID-ul utilizatorului curent
            picture.UserId = User.Identity.GetUserId();


            return View(picture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Create(Picture picture, HttpPostedFileBase file)
        {

            picture.Categories = GetAllCategories();

            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            
            string extension = Path.GetExtension(file.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            picture.Path = "~/Content/" + fileName;
            picture.ThumbPath = "~/Upload/thumb/" + fileName;
            UploadImage.Crop(96, 72, file.InputStream, Path.Combine(Server.MapPath("~/Upload/thumb/") + fileName));

            fileName = Path.Combine(Server.MapPath("~/Content"), fileName);
            file.SaveAs(fileName);
           // picture.Time = DateTime.Now;

            if (picture.Name != null && picture.UserId!=null && picture.Path != "Content" + "\\")
            {
                db.Pictures.Add(picture);
                db.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("Index", "Picture");
        }

        // GET: Pictures/Edit/5
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int? id)
        {
            Picture pictures = db.Pictures.Find(id);
            ViewBag.Picture = pictures;
            pictures.Categories = GetAllCategories();
            
            if (pictures.UserId == User.Identity.GetUserId() ||
                User.IsInRole("Administrator"))
            {
                return View(pictures);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Index");
            }
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "Id,Name,Path")]
        public ActionResult Edit( int id,Picture requestPicture)
        {
            //picture.Categories = GetAllCategories();

            try
            {
                if (ModelState.IsValid)
                {
                    Picture picture = db.Pictures.Find(id);
                    if (picture.UserId == User.Identity.GetUserId() ||   User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(picture))
                        {
                            picture.Name = requestPicture.Name;
                  
                            //picture.Date = requestPicture.Date;
                           // picture.CategoryId = requestPicture.CategoryId;
                            db.SaveChanges();
                            TempData["message"] = "Articolul a fost modificat!";
                        }
                    ModelState.Clear();
                    return RedirectToAction("Index", "Picture");
                  
                   }
                    else
                    {
                       TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    return View(requestPicture);
                }

            }
            catch (Exception e)
            {
                // return View(requestPicture);
               return  RedirectToAction("Index","Picture");
            }
        }


        /* // GET: Pictures/Delete/5
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
         */


        [HttpDelete]
        //[Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(int id)
        {
            Picture picture = db.Pictures.Find(id);
            if (picture.UserId == User.Identity.GetUserId() ||
                User.IsInRole("Administrator"))
            {
                db.Pictures.Remove(picture);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un articol care nu va apartine!";
                return RedirectToAction("Index");
            }

        }
        
        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name.ToString()
                });
            }

            // returnam lista de categorii
            return selectList;
        }
    }
}
