using DemoFIN3.Core.Enum;
using StoryFIN3Demo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DemoFIN3.Core.Models
{
    /**
     * Story
     * 
     * Version 1.0
     * 
     * Date: 12-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 12-09-2021     NghiaTNT      Story attribute, update view and rate
     */
    public class Story
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter story title!")]
        [StringLength(255)]
        [Display(Name = "Story Title")]
        public string Title { get; set; }

        [Column("Short Description")]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [StringLength(255)]
        [Display(Name = "Url")]
        public string UrlSlug { get; set; }

        [Column("Image Url")]
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Column("Publish Date")]
        [Display(Name = "Publish Date")]
        [Required(ErrorMessage = "Please enter publish date!")]
        public DateTime PublishDate { get; set; }

        [Column("Posted On")]
        [Display(Name = "Posted On")]
        [Required(ErrorMessage = "Please enter posted date!")]
        public DateTime PostedOn { get; set; }

        [Column("Updated On")]
        [Display(Name = "Updated On")]
        [Required]
        public DateTime UpdatedOn { get; set; }

        [Display(Name = "Delete Status")]
        public bool isDelete { get; set; }

        [Display(Name = "Story Status")]
        public StoryStatus StoryStatus { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int SourceId { get; set; }
        public Source Source { get; set; }

        public int? ViewCount { get; set; }
        public int? RateCount { get; set; }
        public int? TotalRate { get; set; }

        [NotMapped]
        public decimal Rate
        {
            get
            {
                if (RateCount == null || RateCount == 0)
                {
                    return 0;
                }
                return Math.Round((decimal)TotalRate.Value / RateCount.Value, 1);
            }
        }

        public virtual IList<Comment> Comments { get; set; }
        public virtual IList<Chapter> Chapters { get; set; }
        public virtual IList<Account> Accounts { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
