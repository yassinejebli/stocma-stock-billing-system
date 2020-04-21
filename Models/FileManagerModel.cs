using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FileManagerModel
    {
        public DirectoryInfo CurrentDir { get; set; }
        public List<DirectoryInfo> Directories{ get; set; }
        public List<FileInfo> Files { get; set; }

    }
  
}