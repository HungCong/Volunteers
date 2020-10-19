using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.Entity;

namespace Volunteers.Controllers
{
    public class UserController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(User entity)
        {
            var res = db.Users.Where(x => x.Account == entity.Account && x.Password == entity.Password && x.Type == 1).Count();
            if(res > 0)
            {
                Session["user"] = db.Users.Single(x => x.Account == entity.Account && x.Password == entity.Password);
                return Redirect("/");
            }
            else
            {
                TempData["add_success"] = "Tài khoản hoặc mật khẩu không đúng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return Redirect("/User/Login");
        }
    }
}