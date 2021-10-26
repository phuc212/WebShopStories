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

namespace StoryFIN3Demo.Controllers
{
    /**
     * CategoriesController
     * 
     * Version 1.0
     * 
     * Date: 22-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 22-09-2021     NghiaTNT        CategoriesController
     */
    public class CategoriesController : Controller
    {
        private CategoryRepository categoryRepository;
        public CategoriesController()
        {
            categoryRepository = new CategoryRepository();
        }

        /// <summary>
        /// List categories
        /// </summary>
        /// <returns></returns>
        public ActionResult Categories()
        {
            var categoryList = categoryRepository.GetAllCategories();
            return PartialView("_Categories", categoryList);
        }

        public ActionResult CarouselCategories()
        {
            var categoryList = categoryRepository.GetAllCategories();
            return PartialView("_CarouselCategories", categoryList);
        }
    }
}
