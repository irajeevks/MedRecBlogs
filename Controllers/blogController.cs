using MedrecTechnologies.Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedrecTechnologies.Blog.Controllers
{
    public class blogController : Controller
    {
        // GET: KPO BPO
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }
        [ActionName("create")]
        public ActionResult create()
        {
            return View();
        }
        [ActionName("save")]
        [HttpPost, ValidateInput(false)]
        public ActionResult save(BlogsModel blogsModel, FormCollection formCollection)
        {
            bool flag = true;
            Int32 executeFlag = 1;
            DateTime currentDateTime = DateTime.Now;
            String fileExtension = String.Empty;
            BlogsModelResponse responseModel = new BlogsModelResponse();
            responseModel.Status = 0;
            responseModel.Message = "";

            #region Validation
            if (Request.Files.Count > 0)
            {
                blogsModel.SmallFile = Request.Files["fupSmallBanner"];
                blogsModel.BigFile = Request.Files["fupFullBanner"];
            }

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        responseModel.Message += error.ErrorMessage + "<br>";
                    }
                }
                flag = false;
            }
            if (flag)
            {
                if (blogsModel.SmallFile == null)
                {
                    responseModel.Message += "The Banner Thumbnail field is required.<br>";
                    flag = false;
                }
                if (blogsModel.BigFile == null)
                {
                    responseModel.Message += "The Full Banner field is required.<br>";
                    flag = false;
                }

            }

            if (flag)
            {
                fileExtension = Path.GetExtension(blogsModel.SmallFile.FileName);
                if (Array.IndexOf(FileValidExtensions(), fileExtension) < 0)
                {
                    responseModel.Message += String.Join(",", FileValidExtensions()) + " are supported only for Banner Thumbnail" + "<br>";
                }

                fileExtension = Path.GetExtension(blogsModel.BigFile.FileName);
                if (Array.IndexOf(FileValidExtensions(), fileExtension) < 0)
                {
                    responseModel.Message += String.Join(",", FileValidExtensions()) + " are supported only for Full Banner" + "<br>";
                }
            }
            #endregion

            #region Save
            if (flag)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(AppValidation.ConnectionString))
                    {
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.CommandText = "spiu_blog";
                        sqlCommand.Connection = con;
                        sqlCommand.Parameters.Add(new SqlParameter("@exectype", executeFlag));
                        sqlCommand.Parameters.Add(new SqlParameter("@blogid", new Guid().ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@blogtitle", blogsModel.BlogTitle));
                        sqlCommand.Parameters.Add(new SqlParameter("@shortdesc", blogsModel.BlogShortDescription));
                        sqlCommand.Parameters.Add(new SqlParameter("@fulldesc", blogsModel.BlogFullDescription));
                        sqlCommand.Parameters.Add(new SqlParameter("@smallbanner", "Some path1"));
                        sqlCommand.Parameters.Add(new SqlParameter("@fullbanner", "Some Path 2"));
                        sqlCommand.Parameters.Add(new SqlParameter("@datecreated", currentDateTime));
                        sqlCommand.Parameters.Add(new SqlParameter("@dateupdated", currentDateTime));
                        sqlCommand.Parameters.Add(new SqlParameter("@publishdate", blogsModel.PublishDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@ispublished", blogsModel.IsPublished));
                        sqlCommand.Parameters.Add(new SqlParameter("metatitle", blogsModel.BlogMetaTitle));
                        sqlCommand.Parameters.Add(new SqlParameter("metakeywords", blogsModel.BlogMetaKeywords));
                        sqlCommand.Parameters.Add(new SqlParameter("metadescription", blogsModel.BlogMetaDescription));

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch(Exception ex)
                {
                    responseModel.Message += "There is come technical problem in saving blog. Please contact support.";
                }
            }
            #endregion

            return Json(responseModel);
        }
        [ActionName("filemanager")]
        public ActionResult filemanager()
        {
            return View();
        }

        protected string[] FileValidExtensions()
        {
            string[] fileExtensions = new string[3];
            fileExtensions[0] = ".jpg";
            fileExtensions[1] = ".jpeg";
            fileExtensions[2] = ".png";

            return fileExtensions;
        }
    }
}