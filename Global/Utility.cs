using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Web;

namespace MedrecTechnologies.Blog.Global
{
    public class Utility
    {
        private static string FTPServerUserId = "", FTPServerPassword = "";

        public static bool CreateDirectory(string path)
        {
            bool isSuccess = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    isSuccess = true;
                }
                catch (Exception exception)
                {
                    isSuccess = false;
                }
            }
            return isSuccess;
        }
        public static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }
        public static string AppendDateTimeKey(string key)
        {
            key = key.Replace(" ", "");
            return string.Format("{0}_{1}", DateTime.UtcNow.ToString("yyyy-MMM-dd-HH-mm-ss"), key);
        }
        public static string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }

        #region FTP Handling
        public static bool CreateFTPDirectory(string host, string path)
        {
            bool IsCreated = true;
            try
            {
                if (!DoesFtpDirectoryExist(Path.Combine(host, path)))
                {
                    WebRequest request = WebRequest.Create(Path.Combine(host, path));
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request.Credentials = new NetworkCredential(FTPServerUserId, FTPServerPassword);
                    using (var resp = (FtpWebResponse)request.GetResponse())
                    {
                        Console.WriteLine(resp.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                IsCreated = false;
            }
            return IsCreated;
        }
        public static bool DoesFtpDirectoryExist(string dirPath)
        {
            bool isexist = false;

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dirPath);
                request.Credentials = new NetworkCredential(FTPServerUserId, FTPServerPassword);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    isexist = true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }
                }
            }
            return isexist;
        }
        public static bool DoesFtpFileExist(string fullname)
        {
            bool isexist = false;
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(fullname);
                request.Credentials = new NetworkCredential(FTPServerUserId, FTPServerPassword);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                isexist = true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    isexist = false;
            }
            return isexist;
        }
        public static void DeleteFTPFolder(string Folderpath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Folderpath);
                request.Method = WebRequestMethods.Ftp.RemoveDirectory;
                request.Credentials = new System.Net.NetworkCredential(FTPServerUserId, FTPServerPassword);
                request.GetResponse().Close();
            }
            catch (Exception ex)
            {

            }
            GC.Collect();
        }
        public static void DeleteFTPFile(string fullname)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fullname);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(FTPServerUserId, FTPServerPassword);
                request.GetResponse().Close();
            }
            catch (Exception ex)
            {

            }
            GC.Collect();
        }
        public static bool UploadFileThroughFTP(String sourceFilePath, String targetFilePath)
        {
            bool isUploaded = true;
            try
            {
                if (DoesFtpFileExist(Path.Combine(targetFilePath)))
                {
                    DeleteFTPFile(targetFilePath);
                }

                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(FTPServerUserId, FTPServerPassword);
                    client.UploadFile(targetFilePath, WebRequestMethods.Ftp.UploadFile, sourceFilePath);
                }
            }
            catch (Exception ex)
            {
                isUploaded = false;
            }
            return isUploaded;
        }
        #endregion


        public static void SaveByteToFile(byte[] fileBytes, string folderName, string fileName, string fileExtension)
        {
            string fileApplicationPath = string.Empty;
            try
            {
                if (fileBytes != null)
                {
                    if (fileBytes.Length > 0)
                    {
                        fileApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory + "/" + folderName + "/";
                        CreateFolder(fileApplicationPath);
                        switch (fileExtension.ToLower().Replace(".", ""))
                        {
                            case "pdf":
                            case "doc":
                            case "docx":
                            case "xls":
                            case "xlsx":
                            case "txt":
                                File.WriteAllBytes(Path.Combine(fileApplicationPath, fileName), fileBytes);
                                break;
                            default:
                                using (Image image = Image.FromStream(new MemoryStream(fileBytes, 0, fileBytes.Length)))
                                {
                                    switch (fileExtension.ToLower().Replace(".", ""))
                                    {
                                        case "bmp":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Bmp);
                                            break;
                                        case "emf":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Emf);
                                            break;
                                        case "exif":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Exif);
                                            break;
                                        case "gif":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Gif);
                                            break;
                                        case "icon":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Icon);
                                            break;
                                        case "jpeg":
                                        case "jpg":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Jpeg);
                                            break;
                                        case "png":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Png);
                                            break;
                                        case "tiff":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Tiff);
                                            break;
                                        case "wmf":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Wmf);
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SaveByteToFile(byte[] fileBytes, string folderName, string fileName, string fileExtension, string BaseDirectoryPath)
        {
            string fileApplicationPath = string.Empty;
            try
            {
                if (fileBytes != null)
                {
                    if (fileBytes.Length > 0)
                    {
                        fileApplicationPath = BaseDirectoryPath + "/" + folderName + "/";
                        CreateFolder(fileApplicationPath);
                        switch (fileExtension.ToLower().Replace(".", ""))
                        {
                            case "pdf":
                            case "doc":
                            case "docx":
                            case "xls":
                            case "xlsx":
                            case "txt":
                                File.WriteAllBytes(Path.Combine(fileApplicationPath, fileName), fileBytes);
                                break;
                            default:
                                using (Image image = Image.FromStream(new MemoryStream(fileBytes, 0, fileBytes.Length)))
                                {
                                    switch (fileExtension.ToLower().Replace(".", ""))
                                    {
                                        case "bmp":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Bmp);
                                            break;
                                        case "emf":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Emf);
                                            break;
                                        case "exif":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Exif);
                                            break;
                                        case "gif":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Gif);
                                            break;
                                        case "icon":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Icon);
                                            break;
                                        case "jpeg":
                                        case "jpg":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Jpeg);
                                            break;
                                        case "png":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Png);
                                            break;
                                        case "tiff":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Tiff);
                                            break;
                                        case "wmf":
                                            image.Save(fileApplicationPath + fileName, ImageFormat.Wmf);
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SaveByteToFile(HttpPostedFileBase fileBytes, string folderName, string fileName, string fileExtension, string BaseDirectoryPath)
        {
            string fileApplicationPath = string.Empty;
            try
            {
                if (fileBytes != null)
                {
                    if (fileBytes.ContentLength > 0)
                    {
                        fileApplicationPath = BaseDirectoryPath + "/" + folderName + "/";
                        CreateFolder(fileApplicationPath);
                        fileBytes.SaveAs(Path.Combine(fileApplicationPath, fileName));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task SaveByteToFileAsync(IFormFile formFile, string folderName, string fileName, string fileExtension, string BaseDirectoryPath)
        {
            string fileApplicationPath = string.Empty;
            try
            {
                if (formFile != null)
                {
                    if (formFile.Length > 0)
                    {
                        fileApplicationPath = BaseDirectoryPath + "/" + folderName + "/";
                        CreateFolder(fileApplicationPath);
                        using (var stream = new FileStream(Path.Combine(fileApplicationPath, fileName), FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void CreateFolder(string FullPath)
        {
            try
            {
                DirectoryInfo dirTemp = new DirectoryInfo(FullPath);
                if (!dirTemp.Exists)
                {
                    dirTemp.Create();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool DeleteFile(string fileName)
        {
            bool deleted = false;
            FileInfo fileInfo = new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory + "/" + fileName);

            try
            {
                if (fileInfo.Exists)
                {
                    FileIOPermission filePermission = new FileIOPermission(PermissionState.Unrestricted);
                    filePermission.AddPathList(FileIOPermissionAccess.Write, fileInfo.DirectoryName);
                    File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "/" + fileName);
                    deleted = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fileInfo = null;
            }
            return deleted;
        }
        public static bool DeleteFile(string fileName, string BaseDirectoryPath)
        {
            bool deleted = false;
            FileInfo fileInfo = new FileInfo(BaseDirectoryPath + "/" + fileName);

            try
            {
                if (fileInfo.Exists)
                {
                    FileIOPermission filePermission = new FileIOPermission(PermissionState.Unrestricted);
                    filePermission.AddPathList(FileIOPermissionAccess.Write, fileInfo.DirectoryName);
                    File.Delete(BaseDirectoryPath + "/" + fileName);
                    deleted = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fileInfo = null;
            }
            return deleted;
        }
        public static double GetFileSizeInMB(string Base64String)
        {
            //var result = 4 * Math.Ceiling(((double)(Base64String.Length) / 3));
            //if (Base64String.EndsWith("=="))
            //{
            //    result = result - 2;
            //}
            //else if (Base64String.EndsWith("="))
            //{
            //    result = result - 1;
            //}
            //return Convert.ToDouble((result / 1048576));

            byte[] fileBytes = Convert.FromBase64String(Base64String);
            return Math.Round((fileBytes.Length / Convert.ToDouble(1048576)), 4);
        }

    }
}