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

namespace StoryFIN3Demo.Controllers
{
    /**
     * CommentsController
     * 
     * Version 1.0
     * 
     * Date: 23-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 23-09-2021     NghiaTNT      CommentsController
     */
    public class CommentsController : Controller
    {
        private CommentRepository commentRepository;
        public CommentsController()
        {
            commentRepository = new CommentRepository();
        }

        private DemoFIN3Context db = new DemoFIN3Context();


        /// <summary>
        /// Create comment
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id, StoryId, AccountId, CommentHeader, CommentText, Rating, Url")] Comment comment, string Url)
        {
            if (ModelState.IsValid)
            {
                comment.CommentTime = DateTime.Now;
                comment.CommentStatus = CommentStatus.Pending;
                var story = db.Stories.Find(comment.StoryId);
                var userId = User.Identity.GetUserId();
                var account = db.Users.Where(u => u.Id == userId).FirstOrDefault();
                comment.AccountId = account.Id;
                account.Comments.Add(comment);
                comment.Story = story;
                story.Comments.Add(comment);
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect(Url);
            }
            return Redirect(Url);
        }

        /// <summary>
        /// List comments for story, find by storyId
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns>PartialView</returns>
        [ChildActionOnly]
        public ActionResult GetCommentsForStory(int storyId)
        {
            var commentsForStory = commentRepository.GetCommentsForStory(storyId);
            return PartialView("_CommentsForStory", commentsForStory);
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
