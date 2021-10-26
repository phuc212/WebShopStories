using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoFIN3.Core.Models;

namespace StoryFIN3Demo.Areas.Admin.Controllers
{
    /**
     * AuthorsAdminController
     * 
     * Version 1.0
     * 
     * Date: 20-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 20-09-2021     NghiaTNT        AuthorsAdminController
     */
    [Authorize(Roles = "Admin")]
    public class AuthorsAdminController : Controller
    {
        private DemoFIN3Context db = new DemoFIN3Context();

        // GET: Admin/AuthorsAdmin
        public ActionResult Index()
        {
            return View(db.Authors.OrderByDescending(c => c.Id).ToList());
        }

        public ActionResult GetAllAuthor(string search, int pageSize, int page, string sortOrder)
        {
            search = search.Trim();
            try
            {
                var defaultAuthorsList = (from c in db.Authors.Where(x => x.Name.Contains(search)
                                                                            || x.Description.Contains(search))
                                             select new
                                             {
                                                 Id = c.Id,
                                                 AuthorName = c.Name,
                                                 Description = c.Description
                                             }).ToList();
                switch (sortOrder)
                {
                    case "id":
                        defaultAuthorsList = defaultAuthorsList.OrderBy(c => c.Id).ToList();
                        break;
                    case "name":
                        defaultAuthorsList = defaultAuthorsList.OrderBy(c => c.AuthorName).ToList();
                        break;
                    case "name_desc":
                        defaultAuthorsList = defaultAuthorsList.OrderByDescending(c => c.AuthorName).ToList();
                        break;
                    default:
                        defaultAuthorsList = defaultAuthorsList.OrderByDescending(c => c.Id).ToList();
                        break;
                }
                var categoriesList = defaultAuthorsList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var countPage = defaultAuthorsList.Count % pageSize == 0 ? defaultAuthorsList.Count / pageSize : defaultAuthorsList.Count / pageSize + 1;
                var pageIndex = page;
                return Json(new { code = 200, Data = categoriesList, pageIndex = pageIndex, pageSize = pageSize, totalRecords = defaultAuthorsList.Count, countPage = countPage, msg = "Load data success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/AuthorsAdmin/Details/5
        /// <summary>
        /// Author details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Admin/AuthorsAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AuthorsAdmin/Create
        /// <summary>
        /// Create an author with name, des and image
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, Name, Description, ImageFile")] Author author)
        {
            if (ModelState.IsValid)
            {
                var autCheck = db.Authors.Where(c => c.Name == author.Name).FirstOrDefault();
                if (autCheck != null)
                {
                    ModelState.AddModelError("Name", "Author is exist in DB!");
                    return View(author);
                }
                string fileName = Path.GetFileNameWithoutExtension(author.ImageFile.FileName);
                string extension = Path.GetExtension(author.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                author.ImageUrl = "../../../Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                author.ImageFile.SaveAs(fileName);
                using (db)
                {
                    db.Authors.Add(author);
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Admin/AuthorsAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                string message = "Can't edit this author!";
                return Json(new { code = 500, Message = message }, JsonRequestBehavior.AllowGet );
            }
            return View(author);
        }

        /// <summary>
        /// Edit author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, Description, ImageFile")] Author author)
        {
            if (ModelState.IsValid)
            {
                if (db.Authors.Any(a => a.Name.Equals(author.Name) && a.Id != author.Id))
                {
                    author.ImageUrl = author.ImageUrl;
                    ModelState.AddModelError("Name", "Author name has been duplicated!");
                    return View(author);
                }
                var authorF = db.Authors.Find(author.Id);
                if (author.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(author.ImageFile.FileName);
                    string extension = Path.GetExtension(author.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    author.ImageUrl = "../../../Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    author.ImageFile.SaveAs(fileName);
                    authorF.ImageUrl = author.ImageUrl;
                }
                else
                {
                    authorF.ImageUrl = authorF.ImageUrl;
                }
                authorF.Name = author.Name;
                authorF.Description = author.Description;
                using (db)
                {
                    db.Entry(authorF).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Admin/AuthorsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Admin/AuthorsAdmin/Delete/5
        /// <summary>
        /// Delete author
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                string messageNull = "Can't delete this author!";
                return Json(new { code = 500, Message = messageNull }, JsonRequestBehavior.AllowGet);
            }
            if (author.Stories.Count > 0)
            {
                string msgCant = "Can't delete this author!";
                return Json(new { code = 1, Message = msgCant, JsonRequestBehavior.AllowGet });
            }
            db.Authors.Remove(author);
            db.SaveChanges();
            string message = "Delete success!";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        public ActionResult CheckExist(int id)
        {
            if (id > 0)
            {
                try
                {
                    var author = db.Authors.Find(id);
                    if (author != null)
                    {
                        return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDataSelected(IEnumerable<int> ids)
        {
            if (ids != null)
            {
                try
                {
                    foreach (var id in ids)
                    {
                        var items = db.Authors.FirstOrDefault(c => c.Id == id);
                        if (items == null)
                        {
                            return Json(new { code = 500, message = "Database has change, page will reload!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var id in ids)
                    {
                        var delAuthor = db.Authors.FirstOrDefault(c => c.Id == id);
                        if (delAuthor != null)
                        {
                            db.Authors.Remove(delAuthor);
                            db.SaveChanges();
                        }
                    }
                    return Json(new { code = 200, message = "Delete selected data success!" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { code = 500, message = "Delete selected data error " + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { code = 500, message = "Please select the records you want to delete ! " }, JsonRequestBehavior.AllowGet);
            }

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
