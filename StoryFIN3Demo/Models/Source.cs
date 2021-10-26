using DemoFIN3.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoryFIN3Demo.Models
{
    public class Source
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter source name!")]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual IList<Story> Stories { get; set; }

    }
}