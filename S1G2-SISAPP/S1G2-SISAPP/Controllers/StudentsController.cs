using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using S1G2_SISAPP.Models;

namespace S1G2_SISAPP.Controllers
{
    public class StudentsController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: Students
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameParm = String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            ViewBag.LastNameParm = sortOrder == "last_name" ? "last_name_desc" : "last_name";
            ViewBag.PhoneSortParm = sortOrder == "phone_no" ? "phone_no_desc" : "phone_no";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var students = from s in db.Students1
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.StudentFirstName.Contains(searchString)
                                       || s.StudentLastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "first_name_desc":
                    students = students.OrderByDescending(s => s.StudentFirstName);
                    break;
                case "last_name":
                    students = students.OrderBy(s => s.StudentLastName);
                    break;
                case "last_name_desc":
                    students = students.OrderByDescending(s => s.StudentLastName);
                    break;
                case "phone_no":
                    students = students.OrderBy(s => s.StudentPhoneNo);
                    break;
                case "phone_no_desc":
                    students = students.OrderByDescending(s => s.StudentPhoneNo);
                    break;
                default:
                    students = students.OrderBy(s => s.StudentFirstName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }
        //public ActionResult Index()
        //{
        //    return View(db.Students1.ToList());
        //}

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students1.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,StudentFirstName,StudentLastName,StudentPhoneNo")] Students students)
        {
            if (ModelState.IsValid)
            {
                db.Students1.Add(students);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(students);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students1.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,StudentFirstName,StudentLastName,StudentPhoneNo")] Students students)
        {
            if (ModelState.IsValid)
            {
                db.Entry(students).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(students);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students1.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Students students = db.Students1.Find(id);
            db.Students1.Remove(students);
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
