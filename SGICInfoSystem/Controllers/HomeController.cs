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
using Microsoft.AspNet.Identity;

namespace SGICInfoSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //Создаем контекст БД
        GeologiyaContext db;
        List<DisplayFilterModel> listFilter = null;
        public HomeController()
        {
            db = new GeologiyaContext();
            listFilter = new List<DisplayFilterModel>();
            listFilter.Add(new DisplayFilterModel("Таблица").DefaultDisplayFilter());
            listFilter.Add(new DisplayFilterModel("Отчет").DefaultDisplayFilter());
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        //Таблица с записями
        [Authorize]
        [HttpGet]
        public ActionResult Table(int? page)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);

            ViewBag.Count = db.Essays.ToList().Count;
            return View(db.Essays.OrderByDescending(e => e.nomer).ToPagedList(pageNumber, pageSize));
        }
        [Authorize]
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
            db.Logs.Add(new Log {Action = "Search", DateTimeAction = DateTime.Now, User = User.Identity.GetUserName() });
            db.SaveChanges();
            return PartialView("Search", essays.ToList());
        }

        [HttpPost]
        public ActionResult EssaySearchUz()
        {
            SqlGenerationModel sqlModel = new SqlGenerationModel(Request.Form);

            string sqlQuery = sqlModel.GenerateSqlQuery("uzessays");
            Debug.WriteLine(sqlQuery);
            var essays = db.UzEssays.SqlQuery(sqlQuery, sqlModel.SqlParams.ToArray());
            db.Logs.Add(new Log { Action = "Search", DateTimeAction = DateTime.Now, User = User.Identity.GetUserName() });
            db.SaveChanges();
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
            //ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult AboutUz()
        {
            //ViewBag.Message = "Your application description page.";

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

                    List<DisplayFilterModel> listFilter = this.listFilter;
                    DisplayFilterModel filter = listFilter.Find(f => f.Display == "Отчет");
                    if (Session["DisplayFilter"] != null)
                    {
                        listFilter = (List<DisplayFilterModel>)Session["DisplayFilter"];
                        filter = ((List<DisplayFilterModel>)Session["DisplayFilter"]).Find(f => f.Display == "Отчет");
                    }

                    foreach (string id in ids)
                    {
                        var essay = this.db.Essays.Find(int.Parse(id));
                        if (filter.nomer)
                            sb.Append("\nНомер: ").Append(essay.nomer);
                        if (filter.tema1)
                            sb.Append("\nРубрика осн.: ").Append(essay.tema1);
                        if (filter.tema2)
                            sb.Append("\nРубрики вспом.: ").Append(essay.tema2);
                        if (filter.Aftor)
                            sb.Append("\nАвтор(ы): ").Append(essay.Aftor);
                        if (filter.Naz)
                            sb.Append("\nНазвание: ").Append(essay.Naz);
                        if (filter.Org)
                            sb.Append("\nОрганизация: ").Append(essay.Org);
                        if (filter.Gorod)
                            sb.Append("\nГород: ").Append(essay.Gorod);
                        if (filter.God)
                            sb.Append("\nГод: ").Append(essay.God);
                        if (filter.Str)
                            sb.Append("\nСтраницы: ").Append(essay.Str);
                        if (filter.Ilustr)
                            sb.Append("\nИллюстрации: ").Append(essay.Ilustr);
                        if (filter.Slova)
                            sb.Append("\nКлючевые слова: ").Append(essay.Slova);
                        if (filter.Referat)
                            sb.Append("\nРеферат: ").Append(essay.Referat);
                        if (filter.Mesto)
                            sb.Append("\nМесто хранения: ").Append(essay.Mesto);
                        sb.Append("\n\n");
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
                    db.Logs.Add(new Log { Action = "Report", DateTimeAction = DateTime.Now, User = User.Identity.GetUserName() });
                    db.SaveChanges();
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
            else if (this.Request.Form["Duplicate"] != null)
            {
                return RedirectToRoute(new { controller = "Duplicate", action = "Index" });
            }
            else if (this.Request.Form["Log"] != null)
            {
                return RedirectToAction("Log");
            }
                if (Request.Form["duplicate"] != null)
                return RedirectToRoute(new { controller = "Duplicate", action = "Index" });
            //if((string)Request.RequestContext.RouteData.Values["controller"] == "duplicate")
            //    return RedirectToRoute(new {controller = "Duplicate", action = "Index"});
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

                    List<DisplayFilterModel> listFilter = this.listFilter;
                    DisplayFilterModel filter = listFilter.Find(f => f.Display == "Отчет");
                    if (Session["DisplayFilter"] != null)
                    {
                        listFilter = (List<DisplayFilterModel>)Session["DisplayFilter"];
                        filter = ((List<DisplayFilterModel>)Session["DisplayFilter"]).Find(f => f.Display == "Отчет");
                    }

                    foreach (string id in ids)
                    {
                        var essay = this.db.UzEssays.Find(int.Parse(id));
                        if (filter.nomer)
                            sb.Append("\nРақам: ").Append(essay.nomer);
                        if (filter.tema1)
                            sb.Append("\nАсосий рукни: ").Append(essay.tema1);
                        if (filter.tema2)
                            sb.Append("\nҚўшимча рукни: ").Append(essay.tema2);
                        if (filter.Aftor)
                            sb.Append("\nМуаллиф: ").Append(essay.Aftor);
                        if (filter.Naz)
                            sb.Append("\nНоми: ").Append(essay.Naz);
                        if (filter.Org)
                            sb.Append("\nТашкилот: ").Append(essay.Org);
                        if (filter.Org)
                            sb.Append("\nШаҳар: ").Append(essay.Gorod);
                        if (filter.God)
                            sb.Append("\nЙил: ").Append(essay.God);
                        if (filter.Str)
                            sb.Append("\nБетлар: ").Append(essay.Str);
                        if (filter.Ilustr)
                            sb.Append("\nИллюстрациялар: ").Append(essay.Ilustr);
                        if (filter.Slova)
                            sb.Append("\nАсосий сўзлар: ").Append(essay.Slova);
                        if (filter.Referat)
                            sb.Append("\nРеферат: ").Append(essay.Referat);
                        if (filter.Mesto)
                            sb.Append("\nСақлаш жойи: ").Append(essay.Mesto);
                        sb.Append("\n\n");
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
                    db.Logs.Add(new Log { Action = "Report", DateTimeAction = DateTime.Now, User = User.Identity.GetUserName() });
                    db.SaveChanges();
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
            else if (this.Request.Form["Duplicate"] != null)
            {
                return RedirectToRoute(new { controller = "Duplicate", action = "IndexUz" });
            }
            else if (this.Request.Form["Log"] != null)
            {
                return RedirectToAction("LogUz");
            }
            if (Request.Form["duplicate"] != null)
                return RedirectToRoute(new { controller = "Duplicate", action = "IndexUz" });
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
            
            if (Session["DisplayFilter"] != null)
                listFilter = (List<DisplayFilterModel>)Session["DisplayFilter"];
           

            return View(listFilter);
        }
        public ActionResult DisplayFilterUz()
        {

            if (Session["DisplayFilter"] != null)
                listFilter = (List<DisplayFilterModel>)Session["DisplayFilter"];


            return View(listFilter);
        }

        public ActionResult EditDisplay(string display)
        {
            List<DisplayFilterModel> listFilter;
            DisplayFilterModel filter;
            if (display == null)
                return HttpNotFound();
            if (Session["DisplayFilter"] != null)
            {
                listFilter = (List<DisplayFilterModel>)Session["DisplayFilter"];
                filter = listFilter.Find(d => d.Display == display);
            }
            else
            {
                filter = this.listFilter.Find(d => d.Display == display);
            }
             
            if (filter != null)
                return View(filter);
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult EditDisplay(DisplayFilterModel filter)
        {
            List<DisplayFilterModel> listFilter = this.listFilter;
            if (Session["DisplayFilter"] != null)
            {
                listFilter = (List<DisplayFilterModel>)Session["DisplayFilter"];
            }
            listFilter.Remove(listFilter.Find(d => d.Display == filter.Display));
            if (filter.Display == "Таблица")
                listFilter.Insert(0, filter);
            else
                listFilter.Insert(1, filter);
            Session["DisplayFilter"] = listFilter;
            return RedirectToAction("DisplayFilter");
        }
        public ActionResult EditDisplayUz(string display)
        {
            List<DisplayFilterModel> listFilter;
            DisplayFilterModel filter;
            if (display == null)
                return HttpNotFound();
            if (Session["DisplayFilter"] != null)
            {
                listFilter = (List<DisplayFilterModel>)Session["DisplayFilter"];
                filter = listFilter.Find(d => d.Display == display);
            }
            else
            {
                filter = this.listFilter.Find(d => d.Display == display);
            }

            if (filter != null)
                return View(filter);
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult EditDisplayUz(DisplayFilterModel filter)
        {
            List<DisplayFilterModel> listFilter = this.listFilter;
            if (Session["DisplayFilter"] != null)
            {
                listFilter = (List<DisplayFilterModel>)Session["DisplayFilter"];
            }
            listFilter.Remove(listFilter.Find(d => d.Display == filter.Display));
            if (filter.Display == "Таблица")
                listFilter.Insert(0, filter);
            else
                listFilter.Insert(1, filter);
            Session["DisplayFilter"] = listFilter;
            return RedirectToAction("DisplayFilterUz");
        }

        public ActionResult Log(int? page)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);

            ViewBag.Count = db.Logs.ToList().Count;

            return View(db.Logs.OrderBy(e => e.DateTimeAction).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult LogUz(int? page)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);

            ViewBag.Count = db.Logs.ToList().Count;

            return View(db.Logs.OrderBy(e => e.DateTimeAction).ToPagedList(pageNumber, pageSize));
        }
        [Authorize (Users = "Admin")]
        public void FindRow()
        {
            int i = 0;
            // replace uz symbols
            //List<string> list = System.IO.File.ReadAllLines(@"c:\work\aips geolfond\converted.txt").ToList();

            //for (int e = 0; e < list.Count; e++)
            //{
            //    list[e] = list[e].Replace('Ї', 'Қ');
            //    list[e] = list[e].Replace('Є', 'Ғ');
            //    list[e] = list[e].Replace('°', 'Ҳ');

            //}
            //System.IO.File.WriteAllLines(@"c:\work\aips geolfond\converteduz.txt", list);
            // Парсинг в базу из конвертированного файла
            // Данные не соответствующие шаблону, помещаются отдельный файл
            //List<string> list = System.IO.File.ReadAllLines(@"c:\work\aips geolfond\converteduz.txt").ToList();

            //foreach (Essay essay in db.Essays.ToList())
            //{
            //    string concatStr = new StringBuilder()./*Append(essay.tema1).Append(essay.tema2).*/Append(essay.Aftor.Replace(", ", "")).Append(essay.Naz).Append(essay.Org).Append(essay.God)
            //        .Append(essay.Gorod).Append(essay.Str).ToString();
            //    string author = new StringBuilder(essay.Slova.Replace(", ", "//")).ToString();
            //    string[] result = new string[1];


            //    for (int e = 0; e < list.Count; e++)
            //    {
            //        if (list[e].Contains(concatStr))
            //        {
            //            string wordsUz = essay.Slova;
            //            result = list[e].Split(new[] { concatStr, author, essay.Mesto + essay.nomer }, StringSplitOptions.RemoveEmptyEntries);
            //            for(int j = 0; j < result.Length; j++)
            //            {
            //                if(result[j].StartsWith("//") && result[j].Length > 2)
            //                {
            //                    result[j] = result[j].Remove(0, 2);
            //                    result[j] = result[j].Replace("//", ", ");
            //                    result[j] = result[j].Remove(result[j].Length - 1, 1);
            //                    wordsUz = result[j];
            //                }
            //            }
            //            result[result.Length - 1] = result[result.Length - 1].Replace("%", ", ");
            //            string[] titleUzAr = result[result.Length - 1].Split(new[] { essay.Aftor }, StringSplitOptions.RemoveEmptyEntries);
            //            string titleUz = "";
            //            if (titleUzAr.Length > 0)
            //                titleUz = titleUzAr[0];
            //            /*result = result.Replace("//", ", ").Split(new[] { essay.Slova }, StringSplitOptions.RemoveEmptyEntries)[1];*/
            //            i++;

            //            UzEssay essayUz = new UzEssay {Aftor = essay.Aftor, God = essay.God, Gorod = essay.Gorod, Ilustr = essay.Ilustr,
            //            Mesto = essay.Mesto, Naz = titleUz, nomer = essay.nomer, Org = essay.Org, Referat = essay.Referat, Slova = wordsUz,
            //             Str = essay.Str, tema1 = essay.tema1, tema2 = essay.tema2};
            //            db.UzEssays.Add(essayUz);
            //            db.SaveChanges();

            //            list.RemoveAt(e);
            //            break;
            //        }
            //    }
            //}
            //Debug.WriteLine(i);

            //System.IO.FileStream fs = System.IO.File.Create(@"c:\work\aips geolfond\converteduzparsefirst.txt");
            //fs.Close();
            //System.IO.File.WriteAllLines(@"c:\work\aips geolfond\converteduzparsefirst.txt", list);
            //List<string> list = new List<string>(result);
            //return View(list);


            // Второй парсинг в базу из конвертированного файла
            // Данные не соответствующие шаблону, помещаются отдельный файл converteduzparse2
            List<string> list = System.IO.File.ReadAllLines(@"c:\work\aips geolfond\converteduzparsefirst.txt").ToList();
            //List<Essay> essays = db.Essays.Where(e => !db.UzEssays.Select(u => u.nomer).ToArray().Contains(e.nomer)).ToList();
            List<UzEssay> uzEssays = new List<UzEssay>();
            foreach (Essay essay in db.Essays.ToList())
            {
                string slova = new StringBuilder(essay.Slova.Replace(", ", "//")).ToString();
                string[] result = new string[1];


                for (int e = 0; e < list.Count; e++)
                {
                    if (list[e].Contains(slova) && list[e].Contains(essay.nomer.ToString()))
                    {
                        string wordsUz = essay.Slova;
                        result = list[e].Split(new[] {slova, essay.Mesto + essay.nomer }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < result.Length; j++)
                        {
                            if (result[j].StartsWith("//") && result[j].Length > 2)
                            {
                                result[j] = result[j].Remove(0, 2);
                                result[j] = result[j].Replace("//", ", ");
                                result[j] = result[j].Remove(result[j].Length - 1, 1);
                                wordsUz = result[j];
                                break;
                            }
                        }
                        result[result.Length - 1] = result[result.Length - 1].Replace("%", ", ");
                        string[] titleUzAr = result[result.Length - 1].Split(new[] { essay.Aftor }, StringSplitOptions.RemoveEmptyEntries);
                        string titleUz = "";
                        if (titleUzAr.Length > 0)
                            titleUz = titleUzAr[0];
                        /*result = result.Replace("//", ", ").Split(new[] { essay.Slova }, StringSplitOptions.RemoveEmptyEntries)[1];*/
                        i++;

                        //UzEssay essayUz = new UzEssay
                        //{
                        //    Aftor = essay.Aftor,
                        //    God = essay.God,
                        //    Gorod = essay.Gorod,
                        //    Ilustr = essay.Ilustr,
                        //    Mesto = essay.Mesto,
                        //    Naz = titleUz,
                        //    nomer = essay.nomer,
                        //    Org = essay.Org,
                        //    Referat = essay.Referat,
                        //    Slova = wordsUz,
                        //    Str = essay.Str,
                        //    tema1 = essay.tema1,
                        //    tema2 = essay.tema2
                        //};
                        //uzEssays.Add(essayUz);

                        list.RemoveAt(e);
                        break;
                    }
                }
            }
            //db.UzEssays.AddRange(uzEssays);
            //db.SaveChanges();
            Debug.WriteLine(i);
            //FileStream fs = System.IO.File.Create(@"c:\work\aips geolfond\converteduzparse2.txt");
            //fs.Close();
            System.IO.File.WriteAllLines(@"c:\work\aips geolfond\converteduzparse2.txt", list);

        }

    }
}