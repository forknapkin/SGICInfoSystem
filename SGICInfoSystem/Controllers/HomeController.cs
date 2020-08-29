using SGICInfoSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace SGICInfoSystem.Controllers
{
    public class HomeController : Controller
    {
        //Создаем контекст БД
        GeologiyaContext db = new GeologiyaContext();
       
        public ActionResult Index()
        {
            return View();
        }
        //Таблица с записями
        [Authorize(Users = "Admin, User")]
        [HttpGet]
        public ActionResult Table(int? page)
        {
            //int pageSize = 50;
            //IEnumerable<Essay> essaysPerPage = db.Essays.OrderBy(essay => essay.nomer).Skip((page - 1) * pageSize).Take(pageSize);
            //PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Essays.Count()};
            //PageViewModel pvm = new PageViewModel {PageInfo = pageInfo, Essays = essaysPerPage };
            //return View(pvm);
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            ViewBag.Count = db.Essays.ToList().Count;
            return View(db.Essays.OrderBy(e => e.nomer).ToPagedList(pageNumber, pageSize));
            //var essays = db.Essays;
            //return View(essays.ToList().Take(50));

        }
        [HttpGet]
        public ActionResult List()
        {
            var essays = db.Essays;
            return View(essays.ToList().Take(500));
        }
        [HttpPost, ActionName("Table")]
        public ActionResult Search()
        {
            SqlGenerationModel sqlModel = new SqlGenerationModel(Request.Form);

            string sqlQuery = sqlModel.GenerateSqlQuery();
            Debug.WriteLine(sqlQuery);
            var essays = db.Essays.SqlQuery(sqlQuery, sqlModel.SqlParams.ToArray());
            return View(essays.ToList());
        }
        [HttpPost]
        public ActionResult EssaySearch()
        {
            SqlGenerationModel sqlModel = new SqlGenerationModel(Request.Form);

            string sqlQuery = sqlModel.GenerateSqlQuery();
            Debug.WriteLine(sqlQuery);
            var essays = db.Essays.SqlQuery(sqlQuery, sqlModel.SqlParams.ToArray());
            return PartialView("Search", essays.ToList());
        }
        //[HttpPost, ActionName("Table")]
        //public ActionResult Search(FormCollection form)
        //{
        //    SqlGenerationModel sqlModel = new SqlGenerationModel(form);

        //    string sqlQuery = sqlModel.GenerateSqlQuery();
        //    Debug.WriteLine(sqlQuery);
        //    var essays = db.Essays.SqlQuery(sqlQuery, sqlModel.SqlParams.ToArray());
        //    return View(essays.ToList());
        //}

        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult Create(Essay essay)
        {
            db.Essays.Add(essay);
            db.SaveChanges();

            return RedirectToAction("Table");
        }
        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult Delete(int id)
        {
            Essay essay = db.Essays.Find(id);
            if (essay == null)
                return HttpNotFound();
            return View(essay);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Users = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Essay essay = db.Essays.Find(id);
            if (essay == null)
                return HttpNotFound();
            db.Essays.Remove(essay);
            db.SaveChanges();
            return RedirectToAction("Table");
        }
        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult EditEssay(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Essay essay = db.Essays.Find(id);
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
            return RedirectToAction("Table");
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
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public void ReportWord()
        {
            if (Request.Form["Word"] != null)
            {
                string[] ids = this.Request.Form["ID"].Split(new char[] { ',' });
                StringBuilder sb = new StringBuilder();
                foreach (string id in ids)
                {
                    var essay = this.db.Essays.Find(int.Parse(id));
                    sb.Append(essay).Append("\n\n");
                }

                // Clear all the content from the current response
                // Очистить содержимое текущего запроса
                Response.ClearContent();
                Response.Buffer = true;
                // set the header
                // Установить заголовок
                Response.AddHeader("content-disposition", "attachment; filename = report.doc");
                Response.ContentType = "application/ms-word";
                Response.Charset = "";


                Response.Output.Write(sb.ToString());
                Response.Flush();
            }
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}