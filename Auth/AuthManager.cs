using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using WebApplication1.DATA;
using WebApplication1.Models;

namespace WebApplication1.Auth
{
    public class AuthManager
    {
        private MySaniSoftContext context = new MySaniSoftContext();
    }
}