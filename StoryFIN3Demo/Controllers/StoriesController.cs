using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoFIN3.Core.Enum;
using DemoFIN3.Core.Models;
using DemoFIN3.Core.Repositories;
using Microsoft.AspNet.Identity;
using PagedList;

namespace StoryFIN3Demo.Controllers
{
    /**
     * StoriesController
     * 
     * Version 1.0
     * 
     * Date: 16-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 16-09-2021     NghiaTNT      StoriesController
     */
    public class StoriesController : Controller
    {
        private StoryRepository storyRepository;
        private DemoFIN3Context db;
        public StoriesController()
        {
            db = new DemoFIN3Context();
            storyRepository = new StoryRepository(db);
        }

        

        /// <summary>
        /// Search story page
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
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

            var stories = from s in db.Stories.Where(s => !s.isDelete).Include(s => s.Author).Include(s => s.Category)
                          select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                stories = stories.Where(s => s.Author.Name.Contains(searchString)
                || s.Title.Contains(searchString)
                || s.Category.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    stories = stories.OrderByDescending(s => s.Title);
                    break;
                default:  // Name ascending 
                    stories = stories.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(stories.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Hot story
        /// </summary>
        /// <returns>Partial View</returns>
        [ChildActionOnly]
        public ActionResult HotStory()
        {
            var stories = storyRepository.GetMostViewedStory(8);
            return PartialView("_ListHotStory", stories);
        }

        /// <summary>
        /// New story update
        /// </summary>
        /// <returns>Partial View</returns>
        [ChildActionOnly]
        public ActionResult NewStoryUpdate()
        {
            var stories = storyRepository.GetLatestStory(10);
            return PartialView("_NewStoryUpdate", stories);
        }

        /// <summary>
        /// Finished Story
        /// </summary>
        /// <returns>Partial View</returns>
        [ChildActionOnly]
        public ActionResult FinishedStory()
        {
            var stories = db.Stories.Include(s => s.Author).Where(s => s.StoryStatus == StoryStatus.Full && s.isDelete == false).OrderByDescending(s => s.UpdatedOn).Take(9);
            return PartialView("_FinishedStory", stories);
        }

        /// <summary>
        /// List stories by category
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult StoriesByCategory(string name)
        {
            var storiesByCategory = storyRepository.GetStoriesByCategory(name);
            ViewBag.Name = name;
            return View(storiesByCategory);
        }

        /// <summary>
        /// List stories by author
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult StoriesByAuthor(string name)
        {
            var storiesByAuthor = storyRepository.GetStoriesByAuthor(name);
            ViewBag.Name = name;
            return View(storiesByAuthor);
        }

        /// <summary>
        /// Story detail, find story by ...
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="title"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int year, int month, string title, int id)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserInfo = db.Users.Find(userId);
            ViewBag.Url = Request.Url;
            ViewBag.Path = Request.Url.LocalPath;
            var story = storyRepository.FindStory(year, month, title, id);
            if (story == null)
                return HttpNotFound();
            return View(story);
        }

        /// <summary>
        /// Follow a story
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Follow(int storyId, string Url)
        {
            var story = storyRepository.FindStory(storyId);
            if (story == null)
                return HttpNotFound();
            string userId = User.Identity.GetUserId();
            var userInfo = db.Users.Find(userId);
            if (userInfo == null)
                return HttpNotFound();
            userInfo.Stories.Add(story);
            db.SaveChanges();
            return Redirect(Url);
        }

        [Authorize]
        public ActionResult UnFollow(int storyId, string Url)
        {
            var story = storyRepository.FindStory(storyId);
            if (story == null)
                return HttpNotFound();
            string userId = User.Identity.GetUserId();
            var userInfo = db.Users.Find(userId);
            if (userInfo == null)
                return HttpNotFound();
            userInfo.Stories.Remove(story);
            db.SaveChanges();
            return Redirect(Url);
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
