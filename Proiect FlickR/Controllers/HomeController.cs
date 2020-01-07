using Proiect_FlickR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_FlickR.Controllers
{
    public class HomeController : Controller
         
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Picture");
            }

            var pictures = (from picture in db.Pictures
                            select picture);

            ViewBag.FirstPicture = pictures.First();
            ViewBag.Articles = pictures.OrderBy(o => o.Time).Skip(1).Take(2);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Stock pictures for everyone";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Melinceanu Cristiana Rang Razvan";

            return View();
        }
    }
}