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
        public ActionResult RegistrerFilm()
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
        public ActionResult RegistrerFilm(Filmer nyFilm, HttpPostedFileBase file)
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

        [HttpGet]
        public ActionResult RedigerFilm(int id)
        {
            using (var dbkobling = new FilmKveldDBEntities())
            {
                var redigerFilm = (from film in dbkobling.Filmer
                                 where film.Film_Id == id
                                 select film).SingleOrDefault();


                return View(redigerFilm);
            }
        }

        [HttpPost]
        public ActionResult RedigerFilm(Filmer film)
        {
            using (var dbkobling = new FilmKveldDBEntities())
            {
                var redigerFilm = (from films in dbkobling.Filmer
                                 where films.Film_Id == film.Film_Id
                                 select films).SingleOrDefault();

                redigerFilm.Film_Id = film.Film_Id;
                redigerFilm.Tittel = film.Tittel;
                redigerFilm.Bilde = film.Bilde;
                redigerFilm.Utgivelsesaar = film.Utgivelsesaar;
                redigerFilm.Beskrivelse = film.Beskrivelse;
                redigerFilm.Kategori_Id = film.Kategori_Id;

                dbkobling.SaveChanges();

                return RedirectToAction("VisAlleFilmer");
            }
        }

        [HttpGet]
        public ActionResult SlettFilm(int id)
        {
            using (var dbkobling = new FilmKveldDBEntities())
            {
                var slettFilm = (from film in dbkobling.Filmer
                                   where film.Film_Id == id
                                   select film).SingleOrDefault();


                return View(slettFilm);
            }
        }

        [HttpPost]
        public ActionResult SlettFilm(Filmer film)
        {
            using (var dbkobling = new FilmKveldDBEntities())
            {
                var slettFilm = (from films in dbkobling.Filmer
                                   where films.Film_Id == film.Film_Id
                                   select films).SingleOrDefault();

                dbkobling.Filmer.Remove(slettFilm);
                dbkobling.SaveChanges();

                return RedirectToAction("VisAlleFilmer");
            }
        }
    }
}