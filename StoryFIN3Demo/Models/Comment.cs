using DemoFIN3.Core.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace DemoFIN3.Core.Models
{
    /**
     * Comment
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
     * 15-09-2021     NghiaTNT      Comment attribute, update rating, status
     */
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public Story Story { get; set; }
        public int StoryId { get; set; }
        public Account Account { get; set; }
        public string AccountId { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Please enter comment header!")]
        [Display(Name = "Title")]
        public string CommentHeader { get; set; }

        [StringLength(1024)]
        [Display(Name = "Comment")]
        public string CommentText { get; set; }

        public int Rating { get; set; }

        public CommentStatus CommentStatus { get; set; }
        public DateTime CommentTime { get; set; }
    }
}
