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
    public class InstructorsController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: Instructors
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.InstructorFirstNameParm = String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            ViewBag.InstructorLastNameParm = sortOrder == "last_name" ? "last_name_desc" : "last_name";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var instructors = from i in db.Instructors1
                              select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                instructors = instructors.Where(i => i.InstructorFirstName.Contains(searchString)
                                       || i.InstructorLastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "first_name_desc":
                    instructors = instructors.OrderByDescending(i => i.InstructorFirstName);
                    break;
                case "last_name":
                    instructors = instructors.OrderBy(i => i.InstructorLastName);
                    break;
                case "last_name_desc":
                    instructors = instructors.OrderByDescending(i => i.InstructorLastName);
                    break;
                default:
                    instructors = instructors.OrderBy(i => i.InstructorFirstName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(instructors.ToPagedList(pageNumber, pageSize));
        }
        //public ActionResult Index()
        //{
        //    return View(db.Instructors1.ToList());
        //}

        // GET: Instructors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructors instructors = db.Instructors1.Find(id);
            if (instructors == null)
            {
                return HttpNotFound();
            }
            return View(instructors);
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstructorID,InstructorFirstName,InstructorLastName")] Instructors instructors)
        {
            if (ModelState.IsValid)
            {
                db.Instructors1.Add(instructors);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instructors);
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructors instructors = db.Instructors1.Find(id);
            if (instructors == null)
            {
                return HttpNotFound();
            }
            return View(instructors);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstructorID,InstructorFirstName,InstructorLastName")] Instructors instructors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructors).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructors);
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructors instructors = db.Instructors1.Find(id);
            if (instructors == null)
            {
                return HttpNotFound();
            }
            return View(instructors);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Instructors instructors = db.Instructors1.Find(id);
            db.Instructors1.Remove(instructors);
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
