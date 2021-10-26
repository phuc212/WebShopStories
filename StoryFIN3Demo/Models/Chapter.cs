using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoFIN3.Core.Models
{
    /**
     * Chapter
     * 
     * Version 1.0
     * 
     * Date: 18-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 18-09-2021     NghiaTNT       Chapter attribute
     */
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        public Story Story { get; set; }
        public int StoryId { get; set; }

        [Required(ErrorMessage = "Please enter chapter number!")]
        [Display(Name = "Chapter Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Chapter number must be numeric")]
        public int ChapterNumber { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Chapter title is required.")]
        [Display(Name = "Title")]
        public string ChapterHeader { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Content")]
        public string ChapterContent { get; set; }

        public bool isReading { get; set; }
    }
}
