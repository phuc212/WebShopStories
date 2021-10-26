using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/**
     * Admin
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
     * 21-09-2021     NghiaTNT      Admin view models
     */
namespace StoryFIN3Demo.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}