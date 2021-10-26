using DemoFIN3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoryFIN3Demo.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesUpdateController : Controller
    {
        private DemoFIN3Context db = new DemoFIN3Context();
        // GET: Admin/CategoriesUpdate
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllCategory(string search, int pageSize, int page, string sortOrder)
        {
            search = search.Trim();
            try
            {
                var defaultCategoriesList = (from c in db.Categories.Where(x => x.Name.Contains(search)
                                                                            || x.Description.Contains(search))
                                             select new
                                             {
                                                 Id = c.Id,
                                                 CategoryName = c.Name,
                                                 Description = c.Description
                                             }).ToList();
                switch (sortOrder)
                {
                    case "id":
                        defaultCategoriesList = defaultCategoriesList.OrderBy(c => c.Id).ToList();
                        break;
                    case "name":
                        defaultCategoriesList = defaultCategoriesList.OrderBy(c => c.CategoryName).ToList();
                        break;
                    case "name_desc":
                        defaultCategoriesList = defaultCategoriesList.OrderByDescending(c => c.CategoryName).ToList();
                        break;
                    default:
                        defaultCategoriesList = defaultCategoriesList.OrderByDescending(c => c.Id).ToList();
                        break;
                }
                var categoriesList = defaultCategoriesList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var countPage = defaultCategoriesList.Count % pageSize == 0 ? defaultCategoriesList.Count / pageSize : defaultCategoriesList.Count / pageSize + 1;
                var pageIndex = page;
                return Json(new { code = 200, Data = categoriesList, pageIndex = pageIndex, pageSize = pageSize, totalRecords = defaultCategoriesList.Count, countPage = countPage, msg = "Load data success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}