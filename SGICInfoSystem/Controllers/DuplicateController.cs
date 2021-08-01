using PagedList;
using SGICInfoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGICInfoSystem.Controllers
{
    [Authorize(Users = "Admin")]
    public class DuplicateController : Controller
    {
        GeologiyaContext db = new GeologiyaContext();
        // GET: Duplicate
        public ActionResult Index(int? page)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);

            ViewBag.Count = db.Duplicates.ToList().Count;
            return View(db.Duplicates.OrderBy(d => d.nomer).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult IndexUz(int? page)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);

            ViewBag.Count = db.UzDuplicates.ToList().Count;
            return View(db.UzDuplicates.OrderBy(d => d.nomer).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Essay essay = db.Essays.Find(id);
            if (essay == null)
                return HttpNotFound();
            return View(essay);
        }
        [HttpGet]
        public ActionResult DeleteUz(int id)
        {
            UzEssay essay = db.UzEssays.Find(id);
            if (essay == null)
                return HttpNotFound();
            return View(essay);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Essay essay = db.Essays.Find(id);
            if (essay == null)
                return HttpNotFound();
            db.Essays.Remove(essay);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmedUz(int id)
        {
            UzEssay essay = db.UzEssays.Find(id);
            if (essay == null)
                return HttpNotFound();
            db.UzEssays.Remove(essay);
            db.SaveChanges();
            return RedirectToAction("IndexUz");
        }
        [HttpGet]
        public ActionResult EditEssay(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Essay essay = db.Essays.Find(id);
            if (essay != null)
                return View(essay);
            return HttpNotFound();
        }
        [HttpGet]
        public ActionResult EditEssayUz(int? id)
        {
            if (id == null)
                return HttpNotFound();
            UzEssay essay = db.UzEssays.Find(id);
            if (essay != null)
                return View(essay);
            return HttpNotFound();
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult EditEssay(Essay essay)
        {
            db.Entry(essay).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult EditEssayUz(UzEssay essay)
        {
            db.Entry(essay).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexUz");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Essay essay = db.Essays.Find(id);
            if (essay != null)
                return View(essay);
            return HttpNotFound();
        }
        public ActionResult DetailsUz(int? id)
        {
            if (id == null)
                return HttpNotFound();
            UzEssay essay = db.UzEssays.Find(id);
            if (essay != null)
                return View(essay);
            return HttpNotFound();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}