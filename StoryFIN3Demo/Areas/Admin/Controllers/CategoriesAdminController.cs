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
     * CategoriesAdminController
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
     * 20-09-2021     NghiaTNT        CategoriesAdminController
     */
    [Authorize(Roles ="Admin")]
    public class CategoriesAdminController : Controller
    {
        private DemoFIN3Context db = new DemoFIN3Context();

        // GET: Admin/CategoriesAdmin
        public ActionResult Index()
        {
            return View(db.Categories.OrderByDescending(c => c.Id).ToList());
        }

        // GET: Admin/CategoriesAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/CategoriesAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/CategoriesAdmin/Create
        /// <summary>
        /// Create a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, Name, Description, ImageFile")] Category category)
        {
            if (ModelState.IsValid)
            {
                var cateCheck = db.Categories.Where(c => c.Name == category.Name).FirstOrDefault();
                if (cateCheck != null)
                {
                    ModelState.AddModelError("Name", "Category is exist in DB!");
                    return View(category);
                }
                string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                string extension = Path.GetExtension(category.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                category.ImageUrl = "../../../Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                category.ImageFile.SaveAs(fileName);
                using (db)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("Index", "CategoriesUpdate");
            }
            return View(category);
        }

        // GET: Admin/CategoriesAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/CategoriesAdmin/Edit/5
        /// <summary>
        /// Edit category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, Description, ImageFile")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (db.Categories.Any(a => a.Name.Equals(category.Name) && a.Id != category.Id))
                {
                    ModelState.AddModelError("Name", "Category name has been duplicated!");
                    return View(category);
                }
                var cateF = db.Categories.Find(category.Id);
                if (category.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                    string extension = Path.GetExtension(category.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    category.ImageUrl = "../../../Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    category.ImageFile.SaveAs(fileName);
                    cateF.ImageUrl = category.ImageUrl;
                }
                else
                {
                    cateF.ImageUrl = cateF.ImageUrl;
                }
                cateF.Name = category.Name;
                cateF.Description = category.Description;
                using (db)
                {
                    db.Entry(cateF).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("Index", "CategoriesUpdate");
            }
            return View(category);
        }

        // GET: Admin/CategoriesAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/CategoriesAdmin/Delete/5
        /// <summary>
        /// Delete a category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                string messageNull = "Can't delete this category!";
                return Json(new { code = 500, Message = messageNull }, JsonRequestBehavior.AllowGet);
            }
            string msgCant = "Can't delete this category!";
            if (category.Stories.Count > 0)
            {
                return Json(new { code = 1, Message = msgCant },  JsonRequestBehavior.AllowGet);
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            string message = "Delete success!";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
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
                        var items = db.Categories.FirstOrDefault(c => c.Id == id);
                        if (items == null)
                        {
                            return Json(new { code = 500, message = "Database has change, page will reload!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var id in ids)
                    {
                        var delCategory = db.Categories.FirstOrDefault(c => c.Id == id);
                        if (delCategory != null)
                        {
                            db.Categories.Remove(delCategory);
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

        public ActionResult CheckExist(int id)
        {
            if (id > 0)
            {
                try
                {
                    var category = db.Categories.Find(id);
                    if (category != null)
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
