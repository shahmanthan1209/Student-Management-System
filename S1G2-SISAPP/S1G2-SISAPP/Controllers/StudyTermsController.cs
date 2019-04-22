using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using S1G2_SISAPP.Models;

namespace S1G2_SISAPP.Controllers
{
    public class StudyTermsController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: StudyTerms
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TermNameParm = String.IsNullOrEmpty(sortOrder) ? "Term_Name_desc" : "";
            ViewBag.TermSeasonParm = sortOrder == "Term_Season" ? "Term_Season_desc" : "Term_Season";
            ViewBag.TermDescriptionParm = sortOrder == "Term_Description" ? "Term_Description_desc" : "Term_Description";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var studyterms = from s in db.StudyTerms
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                studyterms = studyterms.Where(s => s.TermName.Contains(searchString)
                                       || s.TermSeason.Contains(searchString)
                                       || s.TermDescription.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Term_Name_desc":
                    studyterms = studyterms.OrderByDescending(s => s.TermName);
                    break;
                case "Term_Season":
                    studyterms = studyterms.OrderBy(s => s.TermSeason);
                    break;
                case "Term_Season_desc":
                    studyterms = studyterms.OrderByDescending(s => s.TermSeason);
                    break;
                case "Term_Description":
                    studyterms = studyterms.OrderBy(s => s.TermDescription);
                    break;
                case "Term_Description_desc":
                    studyterms = studyterms.OrderByDescending(s => s.TermDescription);
                    break;
                default:
                    studyterms = studyterms.OrderBy(s => s.TermName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(studyterms.ToPagedList(pageNumber, pageSize));
        }
        //public ActionResult Index()
        //{
        //    return View(db.StudyTerms.ToList());
        //}

        // GET: StudyTerms/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudyTerm studyTerm = db.StudyTerms.Find(id);
            if (studyTerm == null)
            {
                return HttpNotFound();
            }
            return View(studyTerm);
        }

        // GET: StudyTerms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudyTerms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TermID,TermName,TermSeason,TermDescription")] StudyTerm studyTerm)
        {
            if (ModelState.IsValid)
            {
                db.StudyTerms.Add(studyTerm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studyTerm);
        }

        // GET: StudyTerms/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudyTerm studyTerm = db.StudyTerms.Find(id);
            if (studyTerm == null)
            {
                return HttpNotFound();
            }
            return View(studyTerm);
        }

        // POST: StudyTerms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TermID,TermName,TermSeason,TermDescription")] StudyTerm studyTerm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studyTerm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studyTerm);
        }

        // GET: StudyTerms/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudyTerm studyTerm = db.StudyTerms.Find(id);
            if (studyTerm == null)
            {
                return HttpNotFound();
            }
            return View(studyTerm);
        }

        // POST: StudyTerms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            StudyTerm studyTerm = db.StudyTerms.Find(id);
            db.StudyTerms.Remove(studyTerm);
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
    }
}
