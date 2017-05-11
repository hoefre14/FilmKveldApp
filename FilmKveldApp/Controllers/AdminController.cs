using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FilmKveldApp.Models;
using System.IO;


namespace FilmKveldApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult VisAlleFilmer()
        {
            try {
                using (var dbkobling = new FilmKveldDBEntities())
                   {
                    var filmliste = (from films in dbkobling.Filmer
                                   select films).ToList();
                    return View(filmliste);
                   } 
                } catch (Exception exeption)
            {
                ViewBag.Feilmelding = exeption.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult LoggInnAdmin()
        {
                return View();
        }
            
        [HttpPost]
        public ActionResult LoggInnAdmin(Brukere loggInnAdmin)
        {
            String adminBruker = "HouseSchiffer";

            if (loggInnAdmin.Brukernavn == adminBruker)
            {
                Session["LoggetInn"] = adminBruker;
                ViewBag.LoggetInn = true;
                return RedirectToAction("VisAlleFilmer");
            }
            else
            {
                ViewBag.LoggetInn = false;
            }
            return View();
        }

        [HttpGet]
        public ActionResult RegisterFilm()
        {
            if (Session["LoggetInn"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoggInnAdmin");
            }

        }

        [HttpPost]
        public ActionResult RegisterFilm(Filmer nyFilm, HttpPostedFileBase file)
        {
            if (file != null)
            {
                // needs using system.io; for Path. For å finne riktig fil
                String filnavn = Path.GetFileName(file.FileName);

                String filsti = Path.Combine(Server.MapPath("~/Bilder"), filnavn);

                file.SaveAs(filsti);

                nyFilm.Bilde = filnavn;
            }

            using (var dbkobling = new FilmKveldDBEntities())
            {
                dbkobling.Filmer.Add(nyFilm);
                dbkobling.SaveChanges();
            }
            return View();
        }
    }
}