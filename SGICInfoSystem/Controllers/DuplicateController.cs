using PagedList;
using SGICInfoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGICInfoSystem.Controllers
{
    public class DuplicateController : Controller
    {
        GeologiyaContext db = new GeologiyaContext();
        // GET: Duplicate
        public ActionResult Index(int? page)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            ViewBag.Count = db.Duplictes.ToList().Count;
            return View(db.Duplictes.OrderBy(d => d.nomer).ToPagedList(pageNumber, pageSize));
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}