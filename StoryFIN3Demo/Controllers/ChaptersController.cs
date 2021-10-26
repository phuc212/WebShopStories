using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoFIN3.Core.Models;
using DemoFIN3.Core.Repositories;
using PagedList;

namespace StoryFIN3Demo.Controllers
{
    /**
     * ChaptersController
     * 
     * Version 1.0
     * 
     * Date: 25-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 25-09-2021     NghiaTNT      ChaptersController
     */
    public class ChaptersController : Controller
    {
        private ChapterRepository chapterRepository;
        public ChaptersController()
        {
            chapterRepository = new ChapterRepository();
        }

        private DemoFIN3Context db = new DemoFIN3Context();

        /// <summary>
        /// List chapter
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var chapters = db.Chapters.Include(c => c.Story);
            return View(chapters.ToList());
        }


        /// <summary>
        /// List chapters for story
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="title"></param>
        /// <returns>_ChaptersForStory</returns>
        [ChildActionOnly]
        public ActionResult ChaptersForStory(string sortOrder, string currentFilter, string searchString, int? page, int storyId, string Url)
        {
            ViewBag.UrlPage = Url.Replace("Stories/", "");
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var chaptersForStory = from c in db.Chapters.Where(c => c.StoryId == storyId).ToList()
                select c;
            if (!string.IsNullOrEmpty(searchString))
            {
                chaptersForStory = chaptersForStory.Where(c => c.ChapterHeader.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    chaptersForStory = chaptersForStory.OrderByDescending(s => s.ChapterHeader);
                    break;
                default:  // Name ascending 
                    chaptersForStory = chaptersForStory.OrderBy(s => s.ChapterHeader);
                    break;
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return PartialView("_ChaptersForStory", chaptersForStory.ToPagedList(pageNumber, pageSize));
        }

        // GET: Chapters/Details/5
        /// <summary>
        /// Chapter details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Include(s => s.Story).Where(s => s.Id == id).FirstOrDefault();
            if (chapter == null)
            {
                return HttpNotFound();
            }
            chapter.isReading = true;
            db.SaveChanges();
            return View(chapter);
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
