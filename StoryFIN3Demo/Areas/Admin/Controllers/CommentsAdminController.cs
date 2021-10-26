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

namespace StoryFIN3Demo.Areas.Admin.Controllers
{
    /**
     * CommentsAdminController
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
     * 23-09-2021     NghiaTNT         CommentsAdminController
     */
    [Authorize(Roles = "Admin")]
    public class CommentsAdminController : Controller
    {
        private DemoFIN3Context db = new DemoFIN3Context();

        // GET: Admin/CommentsAdmin
        /// <summary>
        /// Show list approved comments 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Account).Include(c => c.Story).Where(c => c.CommentStatus == CommentStatus.Approved).OrderByDescending(c => c.Id);
            return View(comments.ToList());
        }

        /// <summary>
        /// Show list pending comments
        /// </summary>
        /// <returns></returns>
        public ActionResult Pending()
        {
            var comments = db.Comments.Include(c => c.Account).Include(c => c.Story).Where(c => c.CommentStatus == CommentStatus.Pending).OrderByDescending(c => c.CommentTime);
            return View(comments.ToList());
        }

        public ActionResult GetAllCommentPending(string search, int pageSize, int page, string sortOrder)
        {
            search = search.Trim();
            try
            {
                var defaultCommentsList = (from c in db.Comments.Include(x => x.Account).Include(x => x.Story).Where(x => x.CommentStatus == CommentStatus.Pending && (x.CommentHeader.Contains(search)
                                                                            || x.CommentText.Contains(search)))
                                           select new
                                           {
                                               Id = c.Id,
                                               CommentHeader = c.CommentHeader,
                                               Account = c.Account.Name,
                                               Rate = c.Rating,
                                               Story = c.Story.Title,
                                               CommentTime = c.CommentTime,
                                               CommentText = c.CommentText
                                           }).ToList();
                switch (sortOrder)
                {
                    case "id_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Id).ToList();
                        break;
                    case "name":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.CommentHeader).ToList();
                        break;
                    case "name_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.CommentHeader).ToList();
                        break;
                    case "time":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.CommentTime).ToList();
                        break;
                    case "time_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.CommentTime).ToList();
                        break;
                    case "account":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.Account).ToList();
                        break;
                    case "account_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Account).ToList();
                        break;
                    case "story":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.Story).ToList();
                        break;
                    case "story_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Story).ToList();
                        break;
                    default:
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Id).ToList();
                        break;
                }
                var categoriesList = defaultCommentsList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var countPage = defaultCommentsList.Count % pageSize == 0 ? defaultCommentsList.Count / pageSize : defaultCommentsList.Count / pageSize + 1;
                var pageIndex = page;
                return Json(new { code = 200, Data = categoriesList, pageIndex = pageIndex, pageSize = pageSize, totalRecords = defaultCommentsList.Count, countPage = countPage, msg = "Load data success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAllCommentApproved(string search, int pageSize, int page, string sortOrder)
        {
            search = search.Trim();
            try
            {
                var defaultCommentsList = (from c in db.Comments.Include(x => x.Account).Include(x => x.Story).Where(x => x.CommentStatus == CommentStatus.Approved && (x.CommentHeader.Contains(search)
                                                                            || x.CommentText.Contains(search)))
                                           select new
                                           {
                                               Id = c.Id,
                                               CommentHeader = c.CommentHeader,
                                               Account = c.Account.Name,
                                               Story = c.Story.Title,
                                               CommentTime = c.CommentTime,
                                               Rate = c.Rating,
                                               CommentText = c.CommentText
                                           }).ToList();
                switch (sortOrder)
                {
                    case "id_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Id).ToList();
                        break;
                    case "name":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.CommentHeader).ToList();
                        break;
                    case "name_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.CommentHeader).ToList();
                        break;
                    case "time":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.CommentTime).ToList();
                        break;
                    case "time_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.CommentTime).ToList();
                        break;
                    case "account":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.Account).ToList();
                        break;
                    case "account_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Account).ToList();
                        break;
                    case "story":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.Story).ToList();
                        break;
                    case "story_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Story).ToList();
                        break;
                    default:
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Id).ToList();
                        break;
                }
                var categoriesList = defaultCommentsList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var countPage = defaultCommentsList.Count % pageSize == 0 ? defaultCommentsList.Count / pageSize : defaultCommentsList.Count / pageSize + 1;
                var pageIndex = page;
                return Json(new { code = 200, Data = categoriesList, pageIndex = pageIndex, pageSize = pageSize, totalRecords = defaultCommentsList.Count, countPage = countPage, msg = "Load data success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAllCommentDenied(string search, int pageSize, int page, string sortOrder)
        {
            search = search.Trim();
            try
            {
                var defaultCommentsList = (from c in db.Comments.Include(x => x.Account).Include(x => x.Story).Where(x => x.CommentStatus == CommentStatus.Denied && (x.CommentHeader.Contains(search)
                                                                            || x.CommentText.Contains(search)))
                                           select new
                                           {
                                               Id = c.Id,
                                               CommentHeader = c.CommentHeader,
                                               Account = c.Account.Name,
                                               CommentTime = c.CommentTime,
                                               Story = c.Story.Title,
                                               Rate = c.Rating,
                                               CommentText = c.CommentText
                                           }).ToList();
                switch (sortOrder)
                {
                    case "id_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Id).ToList();
                        break;
                    case "name":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.CommentHeader).ToList();
                        break;
                    case "name_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.CommentHeader).ToList();
                        break;
                    case "time":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.CommentTime).ToList();
                        break;
                    case "time_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.CommentTime).ToList();
                        break;
                    case "account":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.Account).ToList();
                        break;
                    case "account_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Account).ToList();
                        break;
                    case "story":
                        defaultCommentsList = defaultCommentsList.OrderBy(c => c.Story).ToList();
                        break;
                    case "story_desc":
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Story).ToList();
                        break;
                    default:
                        defaultCommentsList = defaultCommentsList.OrderByDescending(c => c.Id).ToList();
                        break;
                }
                var categoriesList = defaultCommentsList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var countPage = defaultCommentsList.Count % pageSize == 0 ? defaultCommentsList.Count / pageSize : defaultCommentsList.Count / pageSize + 1;
                var pageIndex = page;
                return Json(new { code = 200, Data = categoriesList, pageIndex = pageIndex, pageSize = pageSize, totalRecords = defaultCommentsList.Count, countPage = countPage, msg = "Load data success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Show list denied comments
        /// </summary>
        /// <returns></returns>
        public ActionResult Denied()
        {
            var comments = db.Comments.Include(c => c.Account).Include(c => c.Story).Where(c => c.CommentStatus == CommentStatus.Denied);
            return View(comments.ToList());
        }

        // GET: Admin/CommentsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Include(c => c.Account).Where(c => c.Id == id).FirstOrDefault();
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Admin/CommentsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.Users, "Id", "Name");
            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Title");
            return View();
        }

        // POST: Admin/CommentsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StoryId,AccountId,CommentHeader,CommentText,Rating,CommentStatus,CommentTime")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Users, "Id", "Name", comment.AccountId);
            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Title", comment.StoryId);
            return View(comment);
        }

        // GET: Admin/CommentsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Users, "Id", "Name", comment.AccountId);
            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Title", comment.StoryId);
            return View(comment);
        }

        // POST: Admin/CommentsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StoryId,AccountId,CommentHeader,CommentText,Rating,CommentStatus,CommentTime")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Users, "Id", "Name", comment.AccountId);
            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Title", comment.StoryId);
            return View(comment);
        }

        // GET: Admin/CommentsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Admin/CommentsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                string messageFail = "Data has been changed. Please check your data!";
                return Json(new { Message = messageFail, JsonRequestBehavior.AllowGet });
            }
            db.Comments.Remove(comment);
            db.SaveChanges();
            string message = "Delete success!";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult Accept(int id)
        {
            Comment comment = db.Comments.Find(id);
            comment.CommentStatus = CommentStatus.Approved;
            var story = db.Stories.Find(comment.StoryId);
            story.RateCount++;
            story.TotalRate += comment.Rating;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            string message = "Accept success!";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult Undo(int id)
        {
            Comment comment = db.Comments.Find(id);
            comment.CommentStatus = CommentStatus.Pending;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            string message = "Comment => Pending success!";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult Denied(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null || comment.CommentStatus == CommentStatus.Denied)
            {
                string messageFail = "Data has been changed. Please check your data!";
                return Json(new { Message = messageFail, JsonRequestBehavior.AllowGet });
            }
            comment.CommentStatus = CommentStatus.Denied;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            string message = "Deny success!";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult DeniedF(int id)
        {
            Comment comment = db.Comments.Find(id);
            comment.CommentStatus = CommentStatus.Denied;
            var story = db.Stories.Find(comment.StoryId);
            story.RateCount--;
            story.TotalRate -= comment.Rating;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            string message = "Deny success!";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult DenyDataSelected(IEnumerable<int> ids)
        {
            if (ids != null)
            {
                try
                {
                    foreach (var id in ids)
                    {
                        var items = db.Comments.FirstOrDefault(c => c.Id == id);
                        if (items == null || items.CommentStatus == CommentStatus.Denied)
                        {
                            return Json(new { code = 500, message = "Database has change, page will reload!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var id in ids)
                    {
                        var delStory = db.Comments.FirstOrDefault(c => c.Id == id);
                        delStory.CommentStatus = CommentStatus.Denied;
                        db.SaveChanges();
                    }
                    return Json(new { code = 200, message = "Deny success!" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { code = 500, message = "Deny error " + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { code = 500, message = "Please select the records you want to deny ! " }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult UndoDataSelected(IEnumerable<int> ids)
        {
            if (ids != null)
            {
                try
                {
                    foreach (var id in ids)
                    {
                        var items = db.Comments.FirstOrDefault(c => c.Id == id);
                        if (items == null)
                        {
                            return Json(new { code = 500, message = "Database has change, page will reload!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var id in ids)
                    {
                        var delStory = db.Comments.FirstOrDefault(c => c.Id == id);
                        delStory.CommentStatus = CommentStatus.Pending;
                        db.SaveChanges();
                    }
                    return Json(new { code = 200, message = "Undo success! => Pending" }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult AcceptDataSelected(IEnumerable<int> ids)
        {
            if (ids != null)
            {
                try
                {
                    foreach (var id in ids)
                    {
                        var items = db.Comments.FirstOrDefault(c => c.Id == id);
                        if (items == null || items.CommentStatus == CommentStatus.Denied)
                        {
                            return Json(new { code = 500, message = "Database has change, page will reload!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var id in ids)
                    {
                        var delStory = db.Comments.FirstOrDefault(c => c.Id == id);
                        delStory.CommentStatus = CommentStatus.Approved;
                        db.SaveChanges();
                    }
                    return Json(new { code = 200, message = "Accept success!" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { code = 500, message = "Accept error " + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { code = 500, message = "Please select the records you want to accept ! " }, JsonRequestBehavior.AllowGet);
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
                        var items = db.Comments.FirstOrDefault(c => c.Id == id);
                        if (items == null)
                        {
                            return Json(new { code = 500, message = "Database has change, page will reload!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var id in ids)
                    {
                        var delAuthor = db.Comments.FirstOrDefault(c => c.Id == id);
                        if (delAuthor != null)
                        {
                            db.Comments.Remove(delAuthor);
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
