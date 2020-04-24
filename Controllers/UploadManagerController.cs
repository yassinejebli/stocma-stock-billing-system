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
        public string files { get; set; }
        private MySaniSoftContext db = new MySaniSoftContext();
        //
        // GET: /Upload/
        public UploadManagerController()
        {
            files = System.Configuration.ConfigurationManager.AppSettings["Filemanager_RootPath"];
            if (string.IsNullOrEmpty(files))
                files = "/UserFiles/";
            if (!files.EndsWith("/"))
                files += "/";
        }
        public ActionResult Index()
        {
            var model = new FileManagerModel()
            {
                CurrentDir = new DirectoryInfo(Server.MapPath("~" + files)),
                Directories = Directory.GetDirectories(Server.MapPath("~" + files)).Select(q => new DirectoryInfo(q)).ToList(),
                Files = Directory.GetFiles(Server.MapPath("~" + files)).Select(q => new FileInfo(q)).ToList()
            };
            return View(model);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadImage(HttpPostedFileBase file, string type = null, string id = null)
        {
            string url;

            string fileName = file.FileName;
            if (fileName.IndexOf("\\") > -1)
            {
                fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            }
            fileName = fileName.Replace("\\", "/");
            url = "~" + files + fileName;
            var fii = new FileInfo(Server.MapPath(url));
            switch (type)
            {
                case "Articles":
                    fileName = id.ToString() + fii.Extension;
                    url = "~" + files + "/images/articles/" + fileName;
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

            return Content(url.Substring(1));
        }

    }
}
