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
    public class RegistrationsController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: Registrations
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CourseNameParm = String.IsNullOrEmpty(sortOrder) ? "Course_Name_desc" : "";
            ViewBag.StudentFirstNameParm = sortOrder == "Student_First_Name" ? "Student_First_Name_desc" : "Student_First_Name";
            ViewBag.TermNameParm = sortOrder == "Term_Name" ? "Term_Name_desc" : "Term_Name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var registrations = from r in db.Registrations.Include(r => r.Cours).Include(r => r.Student).Include(r => r.StudyTerm)
                                select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                registrations = registrations.Where(r => r.Cours.CourseName.Contains(searchString) ||
                                                         r.Student.StudentFirstName.Contains(searchString) ||
                                                         r.StudyTerm.TermName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Course_Name_desc":
                    registrations = registrations.OrderByDescending(r => r.Cours.CourseName);
                    break;
                case "Student_First_Name":
                    registrations = registrations.OrderBy(r => r.Student.StudentFirstName);
                    break;
                case "Student_First_Name_desc":
                    registrations = registrations.OrderByDescending(r => r.Student.StudentFirstName);
                    break;
                case "Term_Name":
                    registrations = registrations.OrderBy(r => r.StudyTerm.TermName);
                    break;
                case "Term_Name_desc":
                    registrations = registrations.OrderByDescending(r => r.StudyTerm.TermName);
                    break;
                default:
                    registrations = registrations.OrderBy(r => r.Cours.CourseName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(registrations.ToPagedList(pageNumber, pageSize));
        }
        //public ActionResult Index()
        //{
        //    var registrations = db.Registrations.Include(r => r.Cours).Include(r => r.Student).Include(r => r.StudyTerm);
        //    return View(registrations.ToList());
        //}

        // GET: Registrations/Details/5
        public ActionResult Details(string id, string id1, string id2)
        {
            if (id == null || id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id, id1, id2);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: Registrations/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.StudentID = new SelectList(db.Students1, "StudentID", "StudentFirstName");
            ViewBag.TermID = new SelectList(db.StudyTerms, "TermID", "TermName");
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,CourseID,TermID")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", registration.CourseID);
            ViewBag.StudentID = new SelectList(db.Students1, "StudentID", "StudentFirstName", registration.StudentID);
            ViewBag.TermID = new SelectList(db.StudyTerms, "TermID", "TermName", registration.TermID);
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public ActionResult Edit(string id, string id1, string id2)
        {
            if (id == null || id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id,id1,id2);
            if (registration == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", registration.CourseID);
            ViewBag.StudentID = new SelectList(db.Students1, "StudentID", "StudentFirstName", registration.StudentID);
            ViewBag.TermID = new SelectList(db.StudyTerms, "TermID", "TermName", registration.TermID);
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,CourseID,TermID")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", registration.CourseID);
            ViewBag.StudentID = new SelectList(db.Students1, "StudentID", "StudentFirstName", registration.StudentID);
            ViewBag.TermID = new SelectList(db.StudyTerms, "TermID", "TermName", registration.TermID);
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public ActionResult Delete(string id, string id1, string id2)
        {
            if (id == null || id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id, id1, id2);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string id1, string id2)
        {
            Registration registration = db.Registrations.Find(id, id1, id2);
            db.Registrations.Remove(registration);
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
