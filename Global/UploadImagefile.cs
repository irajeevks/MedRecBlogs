using MedrecTechnologies.Blog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MedrecTechnologies.Blog.Global
{
    public class UploadImagefile
    {
        public static ImageFileViewModel uploadfile(HttpPostedFileBase file, string ImagePath, string FolderName)
        {
            ImageFileViewModel objectfile = new ImageFileViewModel();

            string UniquefileName = null;
            string UploadFolder = Path.Combine(ImagePath);
            UniquefileName = Guid.NewGuid().ToString() + "-" + file.FileName;
            string filePath = Path.Combine(UploadFolder, UniquefileName);
            file.SaveAs(filePath);
            objectfile.ImagePath = FolderName + "\\" + UniquefileName;
            objectfile.ImageName = UniquefileName;
            return objectfile;
        }
    }
}