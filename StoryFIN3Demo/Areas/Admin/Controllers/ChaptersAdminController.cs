using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoFIN3.Core.Models;

namespace StoryFIN3Demo.Areas.Admin.Controllers
{
    /**
     * ChaptersAdminController
     * 
     * Version 1.0
     * 
     * Date: 21-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 21-09-2021     NghiaTNT         ChaptersAdminController
     */
    [Authorize(Roles ="Admin")]
    public class ChaptersAdminController : Controller
    {
        private DemoFIN3Context db = new DemoFIN3Context();

        // GET: Admin/ChaptersAdmin
        /// <summary>
        /// Show list chapters
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var chapters = db.Chapters.Include(c => c.Story).OrderByDescending(c => c.Id);
            return View(chapters.ToList());
        }

        public ActionResult GetAllChapter(string search, int pageSize, int page, string sortOrder)
        {
            search = search.Trim();
            try
            {
                var defaultChaptersList = (from c in db.Chapters.Where(x => x.ChapterHeader.Contains(search)
                                                                            || x.Story.Title.Contains(search)
                                                                            || x.ChapterNumber.ToString().Contains(search))
                                          select new
                                          {
                                              Id = c.Id,
                                              ChapterHeader = c.ChapterHeader,
                                              StoryTitle = c.Story.Title,
                                              ChapterNumber = c.ChapterNumber,
                                              ChapterContent = c.ChapterContent
                                          }).ToList();
                switch (sortOrder)
                {
                    case "id_desc":
                        defaultChaptersList = defaultChaptersList.OrderByDescending(c => c.Id).ToList();
                        break;
                    case "name":
                        defaultChaptersList = defaultChaptersList.OrderBy(c => c.ChapterHeader).ToList();
                        break;
                    case "name_desc":
                        defaultChaptersList = defaultChaptersList.OrderByDescending(c => c.ChapterHeader).ToList();
                        break;
                    case "story":
                        defaultChaptersList = defaultChaptersList.OrderBy(c => c.StoryTitle).ToList();
                        break;
                    case "story_desc":
                        defaultChaptersList = defaultChaptersList.OrderByDescending(c => c.StoryTitle).ToList();
                        break;
                    case "number":
                        defaultChaptersList = defaultChaptersList.OrderBy(c => c.ChapterNumber).ToList();
                        break;
                    case "number_desc":
                        defaultChaptersList = defaultChaptersList.OrderByDescending(c => c.ChapterNumber).ToList();
                        break;
                    default:
                        defaultChaptersList = defaultChaptersList.OrderByDescending(c => c.Id).ToList();
                        break;
                }
                var categoriesList = defaultChaptersList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var countPage = defaultChaptersList.Count % pageSize == 0 ? defaultChaptersList.Count / pageSize : defaultChaptersList.Count / pageSize + 1;
                var pageIndex = page;
                return Json(new { code = 200, Data = categoriesList, pageIndex = pageIndex, pageSize = pageSize, totalRecords = defaultChaptersList.Count, countPage = countPage, msg = "Load data success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/ChaptersAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Include(c => c.Story).Where(c => c.Id == id).FirstOrDefault();
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // GET: Admin/ChaptersAdmin/Create
        public ActionResult Create()
        {
            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Title");
            return View();
        }

        public ActionResult CreateForStory(int storyId)
        {
            ViewBag.StoryId = storyId;
            ViewBag.StoryTitle = db.Stories.Find(storyId).Title;
            return View();
        }

        // POST: Admin/ChaptersAdmin/CreateForStory
        /// <summary>
        /// Create a chapter for a story
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForStory([Bind(Include = "Id,ChapterNumber,ChapterHeader,ChapterContent")] Chapter chapter, int storyId)
        {
            if (ModelState.IsValid)
            {
                chapter.StoryId = storyId;
                var story = db.Stories.Find(storyId);
                story.UpdatedOn = DateTime.Now;
                db.Chapters.Add(chapter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Title", chapter.StoryId);
            return View(chapter);
        }

        // POST: Admin/ChaptersAdmin/Create
        /// <summary>
        /// Create a chapter
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StoryId,ChapterNumber,ChapterHeader,ChapterContent")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Chapters.Add(chapter);
                var story = db.Stories.Find(chapter.StoryId);
                story.UpdatedOn = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Title", chapter.StoryId);
            return View(chapter);
        }

        // GET: Admin/ChaptersAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Title", chapter.StoryId);
            return View(chapter);
        }

        // POST: Admin/ChaptersAdmin/Edit/5
        /// <summary>
        /// Edit chapter
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StoryId,ChapterNumber,ChapterHeader,ChapterContent")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chapter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Title", chapter.StoryId);
            return View(chapter);
        }

        // GET: Admin/ChaptersAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // POST: Admin/ChaptersAdmin/Delete/5
        /// <summary>
        /// Delete a chapter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                string messageNull = "Can't delete this chapter!";
                return Json(new { code = 500, Message = messageNull }, JsonRequestBehavior.AllowGet);
            }

            /*if (chapter.isReading)
            {
                string messageRead = "This chapter is read by user. Do you want to continue deleting?";
                return Json(new { code = 404, Message = messageRead }, JsonRequestBehavior.AllowGet);
            }*/
            try
            {
                db.Chapters.Remove(chapter);
                db.SaveChanges();
                string message = "Delete success!";
                return Json(new { Message = message, JsonRequestBehavior.AllowGet });
            }
            catch (Exception e)
            {
                return Json(new { code = 500, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult CheckExist(int id)
        {
            if (id > 0)
            {
                try
                {
                    var chapter = db.Chapters.Find(id);
                    if (chapter != null)
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
                        var items = db.Chapters.FirstOrDefault(c => c.Id == id);
                        if (items == null)
                        {
                            return Json(new { code = 500, message = "Database has change, page will reload!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var id in ids)
                    {
                        var delChapter = db.Chapters.FirstOrDefault(c => c.Id == id);
                        if (delChapter != null)
                        {
                            db.Chapters.Remove(delChapter);
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
