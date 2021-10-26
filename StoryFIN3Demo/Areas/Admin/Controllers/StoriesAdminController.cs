using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DemoFIN3.Core.Models;
using StoryFIN3Demo.Helper;

namespace StoryFIN3Demo.Areas.Admin.Controllers
{
    /**
     * ClassName
     * 
     * Version 1.0
     * 
     * Date: 19-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 19-09-2021     NghiaTNT         Create
     */
    [Authorize(Roles = "Admin")]
    public class StoriesAdminController : Controller
    {
        private DemoFIN3Context db = new DemoFIN3Context();

        // GET: Admin/StoriesAdmin
        public ActionResult Index()
        {
            var stories = db.Stories.Where(s => !s.isDelete).Include(s => s.Author).Include(s => s.Category).OrderByDescending(c => c.Id);
            return View(stories.ToList());
        }

        public ActionResult GetAllStory(string search, int pageSize, int page, string sortOrder)
        {
            search = search.Trim();
            try
            {
                var defaultStoriesList = (from c in db.Stories.Include(x => x.Author).Include(x => x.Category).Where(x => x.isDelete == false && (x.Title.Contains(search)
                                                                            || x.ShortDescription.Contains(search)))
                                          select new
                                          {
                                              Id = c.Id,
                                              Title = c.Title,
                                              ShortDescription = c.ShortDescription,
                                              PostedOn = c.PostedOn,
                                              AuthorName = c.Author.Name,
                                              CategoryName = c.Category.Name,
                                              StoryStatus = c.StoryStatus
                                          }).ToList();
                switch (sortOrder)
                {
                    case "id":
                        defaultStoriesList = defaultStoriesList.OrderBy(c => c.Id).ToList();
                        break;
                    case "name":
                        defaultStoriesList = defaultStoriesList.OrderBy(c => c.Title).ToList();
                        break;
                    case "name_desc":
                        defaultStoriesList = defaultStoriesList.OrderByDescending(c => c.Title).ToList();
                        break;
                    case "author":
                        defaultStoriesList = defaultStoriesList.OrderBy(c => c.AuthorName).ToList();
                        break;
                    case "author_desc":
                        defaultStoriesList = defaultStoriesList.OrderByDescending(c => c.AuthorName).ToList();
                        break;
                    case "category":
                        defaultStoriesList = defaultStoriesList.OrderBy(c => c.CategoryName).ToList();
                        break;
                    case "category_desc":
                        defaultStoriesList = defaultStoriesList.OrderByDescending(c => c.CategoryName).ToList();
                        break;
                    default:
                        defaultStoriesList = defaultStoriesList.OrderByDescending(c => c.Id).ToList();
                        break;
                }
                var categoriesList = defaultStoriesList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var countPage = defaultStoriesList.Count % pageSize == 0 ? defaultStoriesList.Count / pageSize : defaultStoriesList.Count / pageSize + 1;
                var pageIndex = page;
                return Json(new { code = 200, Data = categoriesList, pageIndex = pageIndex, pageSize = pageSize, totalRecords = defaultStoriesList.Count, countPage = countPage, msg = "Load data success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAllStoryDeleted(string search, int pageSize, int page, string sortOrder)
        {
            search = search.Trim();
            try
            {
                var defaultStoriesList = (from c in db.Stories.Include(x => x.Author).Include(x => x.Category).Where(x => x.isDelete == true && (x.Title.Contains(search)
                                                                            || x.ShortDescription.Contains(search)))
                                          select new
                                          {
                                              Id = c.Id,
                                              Title = c.Title,
                                              ShortDescription = c.ShortDescription,
                                              PostedOn = c.PostedOn,
                                              AuthorName = c.Author.Name,
                                              CategoryName = c.Category.Name,
                                              StoryStatus = c.StoryStatus
                                          }).ToList();
                switch (sortOrder)
                {
                    case "id_desc":
                        defaultStoriesList = defaultStoriesList.OrderByDescending(c => c.Id).ToList();
                        break;
                    case "name":
                        defaultStoriesList = defaultStoriesList.OrderBy(c => c.Title).ToList();
                        break;
                    case "name_desc":
                        defaultStoriesList = defaultStoriesList.OrderByDescending(c => c.Title).ToList();
                        break;
                    case "author":
                        defaultStoriesList = defaultStoriesList.OrderBy(c => c.AuthorName).ToList();
                        break;
                    case "author_desc":
                        defaultStoriesList = defaultStoriesList.OrderByDescending(c => c.AuthorName).ToList();
                        break;
                    case "category":
                        defaultStoriesList = defaultStoriesList.OrderBy(c => c.CategoryName).ToList();
                        break;
                    case "category_desc":
                        defaultStoriesList = defaultStoriesList.OrderByDescending(c => c.CategoryName).ToList();
                        break;
                    default:
                        defaultStoriesList = defaultStoriesList.OrderByDescending(c => c.Id).ToList();
                        break;
                }
                var categoriesList = defaultStoriesList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var countPage = defaultStoriesList.Count % pageSize == 0 ? defaultStoriesList.Count / pageSize : defaultStoriesList.Count / pageSize + 1;
                var pageIndex = page;
                return Json(new { code = 200, Data = categoriesList, pageIndex = pageIndex, pageSize = pageSize, totalRecords = defaultStoriesList.Count, countPage = countPage, msg = "Load data success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /**
         * Description
         * @param param
         * @response void
         */
        // GET: Admin/StoriesAdmin
        public ActionResult ListDeletedStory()
        {
            var stories = db.Stories.Where(s => s.isDelete).Include(s => s.Author).Include(s => s.Category);
            return View(stories.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Admin/StoriesAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Include(s => s.Author).Include(s => s.Category).Include(s => s.Source).Where(s => s.Id == id).FirstOrDefault();
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // GET: Admin/StoriesAdmin/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.SourceId = new SelectList(db.Sources, "Id", "Name");
            return View();
        }

        // POST: Admin/StoriesAdmin/Create
        /// <summary>
        /// Create a story
        /// </summary>
        /// <param name="story"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, AuthorId, CategoryId, SourceId, Title, ShortDescription, PublishDate, UrlSlug, ImageFile")] Story story)  
        {
            if (ModelState.IsValid)
            {
                story.PostedOn = DateTime.Now;
                story.UpdatedOn = DateTime.Now;
                story.RateCount = 0;
                story.ViewCount = 0;
                story.TotalRate = 0;
                story.UrlSlug = story.Title.SlugGenerate();
                string fileName = Path.GetFileNameWithoutExtension(story.ImageFile.FileName);
                string extension = Path.GetExtension(story.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                story.ImageUrl = "../../../Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                story.ImageFile.SaveAs(fileName);
                using (db)
                {
                    db.Stories.Add(story);
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "Name", story.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", story.CategoryId);
            ViewBag.SourceId = new SelectList(db.Sources, "Id", "Name", story.CategoryId);
            return View(story);
        }

        // GET: Admin/StoriesAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            Story story = db.Stories.Find(id);
            if (story == null || story.isDelete == true)
            {
                string message = "Can't edit this story!";
                return Json(new { code = 500, Message = message }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "Name", story.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", story.CategoryId);
            ViewBag.SourceId = new SelectList(db.Sources, "Id", "Name", story.SourceId);
            return View(story);
        }

        // POST: Admin/StoriesAdmin/Edit/5
        /// <summary>
        /// Edit story
        /// Upload image
        /// </summary>
        /// <param name="story"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id, AuthorId, CategoryId, SourceId, Title, ShortDescription, PublishDate, UrlSlug, ImageFile, StoryStatus")] Story story)
        {
            if (ModelState.IsValid)
            {
                var storyF = db.Stories.Find(story.Id);
                if (story.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(story.ImageFile.FileName);
                    string extension = Path.GetExtension(story.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    story.ImageUrl = "../../../Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    story.ImageFile.SaveAs(fileName);
                    storyF.ImageUrl = story.ImageUrl;
                }
                else
                {
                    storyF.ImageUrl = storyF.ImageUrl;
                }
                storyF.UpdatedOn = DateTime.Now;
                storyF.Title = story.Title;
                storyF.ShortDescription = story.ShortDescription;
                storyF.UrlSlug = story.UrlSlug;
                storyF.StoryStatus = story.StoryStatus;
                storyF.CategoryId = story.CategoryId;
                storyF.SourceId = story.SourceId;
                storyF.AuthorId = story.AuthorId;
                storyF.isDelete = story.isDelete;
                using (db)
                {
                    db.Entry(storyF).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ModelState.Clear();
                string message = "Edit success!";
                var codeS = 200;
                return Json(new { code = codeS, Message = message, JsonRequestBehavior.AllowGet });
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "Name", story.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", story.CategoryId);
            ViewBag.SourceId = new SelectList(db.Sources, "Id", "Name", story.SourceId);
            var code = 400;
            return Json(new { code = code, JsonRequestBehavior.AllowGet });
        }

        // GET: Admin/StoriesAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Include(s => s.Author).Include(s => s.Category).Where(s => s.Id == id).FirstOrDefault();
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // POST: Admin/StoriesAdmin/Delete/5
        /// <summary>
        /// Permanently Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            db.Stories.Remove(story);
            db.SaveChanges();
            string message = "Delete success!";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        public ActionResult DeleteLogic(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Include(s => s.Author).Include(s => s.Category).Where(s => s.Id == id).FirstOrDefault();
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        /// <summary>
        /// Delete logic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("DeleteLogic")]
        public ActionResult DeleteLogicConfirmed(int id)
        {
            Story storyF = db.Stories.Find(id);
            if (storyF == null || storyF.isDelete == true)
            {
                string messageNull = "Can't delete this story!";
                return Json(new { code = 500, Message = messageNull }, JsonRequestBehavior.AllowGet);
            }
            storyF.isDelete = true;
            db.Entry(storyF).State = EntityState.Modified;
            db.SaveChanges();
            string message = "Delete success!";
            return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Include(s => s.Author).Include(s => s.Category).Where(s => s.Id == id).FirstOrDefault();
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        /// <summary>
        /// Undo a story from Deleted List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Undo")]
        public ActionResult UndoConfirmed(int id)
        {
            Story storyF = db.Stories.Find(id);
            if (storyF == null)
            {
                string messageNull = "Can't undo this story!";
                return Json(new { code = 500, Message = messageNull }, JsonRequestBehavior.AllowGet);
            }
            storyF.isDelete = false;
            db.Entry(storyF).State = EntityState.Modified;
            db.SaveChanges();
            string message = "Undo success!";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        public ActionResult CheckExist(int id)
        {
            if (id > 0)
            {
                try
                {
                    var story = db.Stories.Find(id);
                    if (story != null && story.isDelete == false)
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
        public ActionResult DeleteLogicSelected(IEnumerable<int> ids)
        {
            if (ids != null)
            {
                try
                {
                    foreach (var id in ids)
                    {
                        var items = db.Stories.FirstOrDefault(c => c.Id == id);
                        if (items == null || items.isDelete == true)
                        {
                            return Json(new { code = 500, message = "Database has change, page will reload!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var id in ids)
                    {
                        var delStory = db.Stories.FirstOrDefault(c => c.Id == id);
                        delStory.isDelete = true;
                        db.SaveChanges();
                    }
                    return Json(new { code = 200, message = "Delete success!" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { code = 500, message = "Delete error " + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { code = 500, message = "Please select the records you want to delete ! " }, JsonRequestBehavior.AllowGet);
            }

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
                        var items = db.Stories.FirstOrDefault(c => c.Id == id);
                        if (items == null)
                        {
                            return Json(new { code = 500, message = "Database has change, page will reload!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var id in ids)
                    {
                        var delStory = db.Stories.FirstOrDefault(c => c.Id == id);
                        if (delStory != null)
                        {
                            db.Stories.Remove(delStory);
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

        [HttpPost]
        public ActionResult UndoSelected(IEnumerable<int> ids)
        {
            if (ids != null)
            {
                try
                {
                    foreach (var id in ids)
                    {
                        var items = db.Stories.FirstOrDefault(c => c.Id == id);
                        if (items == null)
                        {
                            return Json(new { code = 500, message = "Database has change, page will reload!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var id in ids)
                    {
                        var delStory = db.Stories.FirstOrDefault(c => c.Id == id);
                        delStory.isDelete = false;
                        db.SaveChanges();
                    }
                    return Json(new { code = 200, message = "Undo success!" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { code = 500, message = "Undo error " + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { code = 500, message = "Please select the records you want to undo ! " }, JsonRequestBehavior.AllowGet);
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
