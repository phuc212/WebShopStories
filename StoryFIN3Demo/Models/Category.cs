using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DemoFIN3.Core.Models
{
    /**
     * Category
     * 
     * Version 1.0
     * 
     * Date: 14-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 14-09-2021     NghiaTNT      Category attribute
     */
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string ImageUrl { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public virtual IList<Story> Stories { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
