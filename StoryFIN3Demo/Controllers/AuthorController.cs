using DemoFIN3.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoryFIN3Demo.Controllers
{
    /**
     * AuthorController
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
     * 22-09-2021     NghiaTNT        AuthorController
     */
    public class AuthorController : Controller
    {
        private AuthorRepository authorRepository;
        public AuthorController()
        {
            authorRepository = new AuthorRepository();
        }
        // GET: author
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List authors
        /// </summary>
        /// <returns></returns>
        public ActionResult Authors()
        {
            var authorList = authorRepository.GetAllAuthors();
            return PartialView("_Authors", authorList);
        }
    }
}