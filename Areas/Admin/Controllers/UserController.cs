using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.Entity;

namespace Volunteers.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();
        // GET: Admin/User
        public ActionResult Index()
        {
            ViewBag.lstUser = db.Users.Where(x => x.Type == 1).OrderByDescending(x => x.Fullname).ToList();
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(User entity)
        {
            var res = db.Users.Where(x => x.Account == entity.Account && x.Password == entity.Password && x.Type == 0).Count();
            if (res > 0)
            {
                Session["admin"] = db.Users.Single(x => x.Account == entity.Account && x.Password == entity.Password);
                return Redirect("/Admin/Home/Round_Volunteer_Tested");
            }
            else
            {
                TempData["add_success"] = "Tài khoản hoặc mật khẩu không đúng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login");
            }
        }

        //Thêm tài khoản đăng nhập
        public ActionResult Add()
        {
            ViewBag.lstVolunteer = db.Volunteers.Where(x => x.Type == 1).OrderByDescending(x => x.Fullname).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(User entity, HttpPostedFileBase Image)
        {
            //try
            //{
                var user = new User();
                user.Account = entity.Account;
                user.Password = entity.Password;
                user.Fullname = entity.Fullname;
                user.Type = 1;
                user.Volunteers_ID = entity.Volunteers_ID;
                //Thêm hình ảnh
                var path = Path.Combine(Server.MapPath("~/Assets/dist/img/"), Image.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName = Path.GetExtension(Image.FileName);
                    string filename = Image.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                    path = Path.Combine(Server.MapPath("~/Assets/dist/img/"), filename);
                    Image.SaveAs(path);
                    user.Image = filename;
                }
                else
                {
                    Image.SaveAs(path);
                    user.Image = Image.FileName;
                }
                db.Users.Add(user);
                db.SaveChanges();
                TempData["add_success"] = "Thêm tài khoản đăng nhập hệ thống thành công.";
                return Redirect("/Admin/User");
            //}
            //catch
            //{
            //    return Redirect("/Admin/User/Add");
            //}
        }

        public JsonResult Delete(long ID)
        {
            var user = db.Users.Find(ID);
            db.Users.Remove(user);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        public ActionResult Logout()
        {
            Session["admin"] = null;
            return Redirect("/Admin/User/Login");
        }

        public JsonResult FindFullnameByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var vol = db.Volunteers.Find(ID);
            return Json(vol, JsonRequestBehavior.AllowGet);
        }
    }
}