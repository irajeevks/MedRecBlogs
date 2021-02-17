using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedrecTechnologies.Blog.Models
{
    [Serializable]
    public class BlogsModel
    {
        [Required]
        [Display(Name = "Title"), MaxLength(100)]
        public string BlogTitle { get; set; }

        [Required]
        [Display(Name = "Short Description"), MaxLength(200)]
        public string BlogShortDescription { get; set; }

        [Required]
        [Display(Name = "Full Description"), MaxLength(200)]
        [AllowHtml]
        public string BlogFullDescription { get; set; }

        //[Required]
        [Display(Name = "Meta Title"), MaxLength(60)]
        public string BlogMetaTitle { get; set; }

        //[Required]
        [Display(Name = "Meta Keywords"), MaxLength(300)]
        public string BlogMetaKeywords { get; set; }

        //[Required]
        [Display(Name = "Meta Description"), MaxLength(200)]
        public string BlogMetaDescription { get; set; }

        //[Required]
        [Display(Name = "Banner Thumbnail")]
        public HttpPostedFileBase SmallFile { get; set; }

        //[Required]
        [Display(Name = "Full Banner")]
        public HttpPostedFileBase BigFile { get; set; }

        [Required]
        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

        //[Required]
        [Display(Name = "Publish Now")]
        public bool IsPublished { get; set; }
    }

    [Serializable]
    public class BlogsModelResponse
    {
        [Display(Name = "Status")]
        public int Status { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}