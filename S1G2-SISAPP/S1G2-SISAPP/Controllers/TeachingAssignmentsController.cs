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
    public class TeachingAssignmentsController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: TeachingAssignments
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CourseNameParm = String.IsNullOrEmpty(sortOrder) ? "Course_Name_desc" : "";
            ViewBag.InstructorFirstNameParm = sortOrder == "Instructor_First_Name" ? "Instructor_First_Name_desc" : "Instructor_First_Name";
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

            var teaching = from t in db.TeachingAssignments1.Include(t => t.Cours).Include(t => t.Instructor).Include(t => t.StudyTerm)
                           select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                teaching = teaching.Where(t => t.Cours.CourseName.Contains(searchString) ||
                                                         t.Instructor.InstructorFirstName.Contains(searchString) ||
                                                         t.StudyTerm.TermName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Course_Name_desc":
                    teaching = teaching.OrderByDescending(t => t.Cours.CourseName);
                    break;
                case "Instructor_First_Name":
                    teaching = teaching.OrderBy(t => t.Instructor.InstructorFirstName);
                    break;
                case "Instructor_First_Name_desc":
                    teaching = teaching.OrderByDescending(t => t.Instructor.InstructorFirstName);
                    break;
                case "Term_Name":
                    teaching = teaching.OrderBy(t => t.StudyTerm.TermName);
                    break;
                case "Term_Name_desc":
                    teaching = teaching.OrderByDescending(t => t.StudyTerm.TermName);
                    break;
                default:
                    teaching = teaching.OrderBy(t => t.Cours.CourseName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(teaching.ToPagedList(pageNumber, pageSize));
        }
        //public ActionResult Index()
        //{
        //    var teachingAssignments1 = db.TeachingAssignments1.Include(t => t.Cours).Include(t => t.Instructor).Include(t => t.StudyTerm);
        //    return View(teachingAssignments1.ToList());
        //}

        // GET: TeachingAssignments/Details/5
        public ActionResult Details(string id, string id1, string id2)
        {
            if (id == null || id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingAssignments teachingAssignments = db.TeachingAssignments1.Find(id,id1,id2);
            if (teachingAssignments == null)
            {
                return HttpNotFound();
            }
            return View(teachingAssignments);
        }

        // GET: TeachingAssignments/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.InstructorID = new SelectList(db.Instructors1, "InstructorID", "InstructorFirstName");
            ViewBag.TermID = new SelectList(db.StudyTerms, "TermID", "TermName");
            return View();
        }

        // POST: TeachingAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstructorID,CourseID,TermID")] TeachingAssignments teachingAssignments)
        {
            if (ModelState.IsValid)
            {
                db.TeachingAssignments1.Add(teachingAssignments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", teachingAssignments.CourseID);
            ViewBag.InstructorID = new SelectList(db.Instructors1, "InstructorID", "InstructorFirstName", teachingAssignments.InstructorID);
            ViewBag.TermID = new SelectList(db.StudyTerms, "TermID", "TermName", teachingAssignments.TermID);
            return View(teachingAssignments);
        }

        // GET: TeachingAssignments/Edit/5
        public ActionResult Edit(string id, string id1, string id2)
        {
            if (id == null || id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingAssignments teachingAssignments = db.TeachingAssignments1.Find(id,id1,id2);
            if (teachingAssignments == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", teachingAssignments.CourseID);
            ViewBag.InstructorID = new SelectList(db.Instructors1, "InstructorID", "InstructorFirstName", teachingAssignments.InstructorID);
            ViewBag.TermID = new SelectList(db.StudyTerms, "TermID", "TermName", teachingAssignments.TermID);
            return View(teachingAssignments);
        }

        // POST: TeachingAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstructorID,CourseID,TermID")] TeachingAssignments teachingAssignments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teachingAssignments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", teachingAssignments.CourseID);
            ViewBag.InstructorID = new SelectList(db.Instructors1, "InstructorID", "InstructorFirstName", teachingAssignments.InstructorID);
            ViewBag.TermID = new SelectList(db.StudyTerms, "TermID", "TermName", teachingAssignments.TermID);
            return View(teachingAssignments);
        }

        // GET: TeachingAssignments/Delete/5
        public ActionResult Delete(string id, string id1 ,string id2)
        {
            if (id == null || id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingAssignments teachingAssignments = db.TeachingAssignments1.Find(id,id1,id2);
            if (teachingAssignments == null)
            {
                return HttpNotFound();
            }
            return View(teachingAssignments);
        }

        // POST: TeachingAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string id1, string id2)
        {
            TeachingAssignments teachingAssignments = db.TeachingAssignments1.Find(id, id1, id2);
            db.TeachingAssignments1.Remove(teachingAssignments);
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
