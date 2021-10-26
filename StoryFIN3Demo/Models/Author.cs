using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DemoFIN3.Core.Models
{
    /**
     * Author
     * 
     * Version 1.0
     * 
     * Date: 15-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 15-09-2021     NghiaTNT      Author attribute
     */
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public virtual IList<Story> Stories { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
