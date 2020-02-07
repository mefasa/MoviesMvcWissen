﻿using _036_MoviesMvcWissen.Contexts;
using _036_MoviesMvcWissen.Entities;
using _036_MoviesMvcWissen.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _036_MoviesMvcWissen.Controllers
{
    public class MoviesController : Controller
    {
        MoviesContext db = new MoviesContext();
        // GET: Movies
        public ViewResult Index()
        {
            //var model = db.Movies.ToList();
            var model = GetList();
            //ViewBag.count = model.Count;
            ViewData["count"] = model.Count;
            return View(model);
        }

        [NonAction]//url üzerinden çağıralamaz hale getirmek için.
        public List<Movie> GetList(bool removeSession = true)
        {
            List<Movie> entities;
            if (removeSession)
            {
                Session.Remove("movies");
            }
            if (Session["movies"] == null || removeSession)
            {
                entities = db.Movies.ToList();
                Session["movies"] = entities;
            }
            else
            {
                entities = Session["movies"] as List<Movie>;
            }
            return entities;
        }
        
        public ActionResult GetMoviesFromSession()
        {
            var model = GetList(false);
            ViewBag.count = model.Count;
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Message = "Please enter movie information...";
            //return View();
            return new ViewResult();
        }

        [HttpPost]
        public RedirectToRouteResult Add(string Name, string ProductionYear, string BoxOfficeReturn)
        {
            
            var entity = new Movie()
            {
                Name = Name,
                ProductionYear = ProductionYear,

                //BoxOfficeReturn = Convert.ToDouble(BoxOfficeReturn.Replace(",","."), CultureInfo.InvariantCulture)
            };
            if (BoxOfficeReturn == "")
            {
                entity.BoxOfficeReturn = null;
            }
            else
            {
                entity.BoxOfficeReturn = Convert.ToDouble(BoxOfficeReturn.Replace(",", "."), CultureInfo.InvariantCulture);
            }
            db.Movies.Add(entity);
            db.SaveChanges();
            TempData["Info"] = "Record successfully saved to database.";
            return RedirectToAction("Index"); 

        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Movies.Find(id.Value);
            List<SelectListItem> years = new List<SelectListItem>();
            SelectListItem year;
            for (int i = DateTime.Now.Year; i >= 2000 ; i--)
            {
                year = new SelectListItem() { Value = i.ToString(), Text = i.ToString() };
                years.Add(year);
            }
            ViewBag.Years = new SelectList(years, "Value", "Text", model.ProductionYear);
            return View(model);

        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,ProductionYear")]Movie movie, string BoxOfficeReturn)
        {
            var entity = db.Movies.SingleOrDefault(e => e.Id == movie.Id);
            entity.Name = movie.Name;
            entity.ProductionYear = movie.ProductionYear;
            entity.BoxOfficeReturn = Convert.ToDouble(BoxOfficeReturn.Replace(",", "."), CultureInfo.InvariantCulture);
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Info"] = "Record successfully updated in database.";
            return RedirectToRoute(new { controller = "Movies", action = "Index" });
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(400,"Id is required!");
            }
            var model = db.Movies.FirstOrDefault(e => e.Id == id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var entity = db.Movies.Find(id);
            db.Movies.Remove(entity);
            db.SaveChanges();
            TempData["Info"] = "Record successfully deleted from database.";
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Id is not required!");
            }
            var model = db.Movies.Find(id.Value);
            return View(model);
        }
        public ActionResult Welcome()
        {
            var result = "Welcome to Movies MVC";
            //return Content(result);

            return PartialView("_Welcome", result);

        }
    }
}