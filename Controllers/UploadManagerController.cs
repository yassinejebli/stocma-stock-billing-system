using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DATA;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    //[SiteConfig]
    public class UploadManagerController : Controller
    {
        public string userfiles { get; set; }
        private MySaniSoftContext db = new MySaniSoftContext();
        //
        // GET: /Upload/
        public UploadManagerController()
        {
            userfiles = System.Configuration.ConfigurationManager.AppSettings["Filemanager_RootPath"];
            if (string.IsNullOrEmpty(userfiles))
                userfiles = "/UserFiles/";
            //="/UserFiles/";
            if (!userfiles.EndsWith("/"))
                userfiles += "/";
        }
        public ActionResult Index()
        {
            var model = new FileManagerModel()
            {
                CurrentDir = new DirectoryInfo(Server.MapPath("~" + userfiles)),
                Directories = Directory.GetDirectories(Server.MapPath("~" + userfiles)).Select(q => new DirectoryInfo(q)).ToList(),
                Files = Directory.GetFiles(Server.MapPath("~" + userfiles)).Select(q => new FileInfo(q)).ToList()
            };
            return View(model);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadImage(HttpPostedFileBase file, string dbtype = null, string id = null)
        {
            string url; // url to return
            //string message; // message to display (optional)

            string fileName = file.FileName;
            if (fileName.IndexOf("\\") > -1)
            {
                fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            }
            fileName = fileName.Replace("\\", "/");
            url = "~" + userfiles + fileName;
            var fii = new FileInfo(Server.MapPath(url));
            switch (dbtype)
            {
                case "Employee":
                    fileName = id.ToString() + fii.Extension;
                    url = "~" + userfiles + "/catalog/employee/" + fileName;
                    break;
                case "Article":
                    fileName = id.ToString() + fii.Extension;
                    url = "~" + userfiles + "/catalog/article/" + fileName;
                    break;
                case "Product":
                    fileName = id.ToString() + fii.Extension;
                    url = "~" + userfiles + "/catalog/product/" + fileName;
                    break;
                case "Category":
                    fileName = id.ToString() + fii.Extension;
                    url = "~" + userfiles + "/catalog/category/" + fileName;
                    break;
                case "Component":
                    fileName = id.ToString() + fii.Extension;
                    url = "~" + userfiles + "/catalog/component/" + fileName;
                    break;
                default:
                    break;
            }

            fileName = Server.MapPath(url);
            var fi = new FileInfo(fileName);
            if (!fi.Directory.Exists)
                fi.Directory.Create();
            if (fi.Exists)
                fi.Delete();
            file.SaveAs(fileName);
            //message = "Image was saved correctly";

            // since it is an ajax request it requires this string
            //string output = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction("+CKEditorFuncNum+", \""+url+"\", \""+message+"\");</script></body></html>";
            return Content(url.Substring(1));
        }

    }
}
