﻿using SGICInfoSystem.Models;
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
        GeologiyaContext db;
        public HomeController()
        {
            db = new GeologiyaContext();
            
        }
        [Authorize(Users = "Admin, User")]
        public ActionResult Index()
        {
            return View();
        }
        //Таблица с записями
        [Authorize(Users = "Admin, User")]
        [HttpGet]
        public ActionResult Table(int? page)
        {           
            int pageSize = 100;
            int pageNumber = (page ?? 1);
                
            ViewBag.Count = db.Essays.ToList().Count;
            return View(db.Essays.OrderByDescending(e => e.nomer).ToPagedList(pageNumber, pageSize));            
        }
        [Authorize(Users = "Admin, User")]
        [HttpGet]
        public ActionResult TableUz(int? page)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);

            ViewBag.Count = db.UzEssays.ToList().Count;
            return View(db.UzEssays.OrderByDescending(e => e.nomer).ToPagedList(pageNumber, pageSize));           
        }
        
        [HttpPost, ActionName("Table")]
        public ActionResult Search()
        {
            SqlGenerationModel sqlModel = new SqlGenerationModel(Request.Form);

            string sqlQuery = sqlModel.GenerateSqlQuery("essays");
            Debug.WriteLine(sqlQuery);
            var essays = db.Essays.SqlQuery(sqlQuery, sqlModel.SqlParams.ToArray());
            return View(essays.ToList());
        }
        [HttpPost]
        public ActionResult EssaySearch()
        {
            SqlGenerationModel sqlModel = new SqlGenerationModel(Request.Form);

            string sqlQuery = sqlModel.GenerateSqlQuery("essays");
            Debug.WriteLine(sqlQuery);
            var essays = db.Essays.SqlQuery(sqlQuery, sqlModel.SqlParams.ToArray());
            return PartialView("Search", essays.ToList());
        }

        [HttpPost]
        public ActionResult EssaySearchUz()
        {
            SqlGenerationModel sqlModel = new SqlGenerationModel(Request.Form);

            string sqlQuery = sqlModel.GenerateSqlQuery("uzessays");
            Debug.WriteLine(sqlQuery);
            var essays = db.UzEssays.SqlQuery(sqlQuery, sqlModel.SqlParams.ToArray());
            return PartialView("SearchUz", essays.ToList());
        }
        

        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult CreateUz()
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
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult CreateUz(UzEssay essay)
        {
            db.UzEssays.Add(essay);
            db.SaveChanges();

            return RedirectToAction("TableUz");
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
        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult DeleteUz(int id)
        {
            UzEssay essay = db.UzEssays.Find(id);
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
        [HttpPost, ActionName("Delete")]
        [Authorize(Users = "Admin")]
        public ActionResult DeleteConfirmedUz(int id)
        {
            UzEssay essay = db.UzEssays.Find(id);
            if (essay == null)
                return HttpNotFound();
            db.UzEssays.Remove(essay);
            db.SaveChanges();
            return RedirectToAction("TableUz");
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
        [HttpGet]
        [Authorize(Users = "Admin")]
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
            return RedirectToAction("Table");
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult EditEssayUz(UzEssay essay)
        {
            db.Entry(essay).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("TableUz");
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
        public ActionResult ActionHandlers()
        {
            if (Request.Form["Word"] != null)
            {
                if (this.Request.Form["ID"] != null)
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
                    Response.Charset = "utf-8";

                    Response.Output.Write(sb.ToString());
                    Response.Flush();
                    return new EmptyResult();
                }
            }
            else if (this.Request.Form["DeleteChecked"] != null)
            {
                if (this.Request.Form["ID"] != null)
                {
                    string[] ids = this.Request.Form["ID"].Split(new char[] { ',' });

                    foreach (string id in ids)
                    {
                        var essay = this.db.Essays.Find(int.Parse(id));
                        this.db.Essays.Remove(essay);
                        this.db.SaveChanges();
                        Debug.WriteLine(essay.Naz);
                    }
                }
            }
            return RedirectToAction("Table");
        }
        public ActionResult ActionHandlersUz()
        {
            if (Request.Form["Word"] != null)
            {
                if (this.Request.Form["ID"] != null)
                {
                    string[] ids = this.Request.Form["ID"].Split(new char[] { ',' });
                    StringBuilder sb = new StringBuilder();
                    foreach (string id in ids)
                    {
                        var essay = this.db.UzEssays.Find(int.Parse(id));
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
                    Response.Charset = "utf-8";

                    Response.Output.Write(sb.ToString());
                    Response.Flush();
                    return new EmptyResult();
                }
            }
            else if (this.Request.Form["DeleteChecked"] != null)
            {
                if (this.Request.Form["ID"] != null)
                {
                    string[] ids = this.Request.Form["ID"].Split(new char[] { ',' });

                    foreach (string id in ids)
                    {
                        var essay = this.db.UzEssays.Find(int.Parse(id));
                        this.db.UzEssays.Remove(essay);
                        this.db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Table");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpGet]
        public ActionResult DisplayFilter()
        {
            List<DisplayFilterModel> display = null;
            if (Session["DisplayFilter"] != null)
                display = (List<DisplayFilterModel>)Session["DisplayFilter"];
            else
            {
                display = new List<DisplayFilterModel>();
                display.Add(new DisplayFilterModel("Таблица").DefaultDisplayFilter());
                display.Add(new DisplayFilterModel("Отчет").DefaultDisplayFilter());
            }
                
            return View(display);
        }
        
    }
}