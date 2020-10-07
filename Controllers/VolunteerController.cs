using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.Entity;

namespace Volunteers.Controllers
{
    public class VolunteerController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();
        // GET: Volunteer
        public ActionResult Index()
        {
            ViewBag.lstVolunteers = db.Volunteers.Where(x => x.Type == 1).ToList();
            return View();
        }
        

        //Thêm tình nguyện viên
        public ActionResult AddVolunteer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddVolunteer(Volunteer entity)
        {
            try
            {
                var model = new Volunteer();
                model.Fullname = entity.Fullname;
                model.Birthday = entity.Birthday;
                model.Phone = entity.Phone;
                model.Email = entity.Email;
                model.Address = entity.Address;
                model.Type = 1;
                model.RoleName = entity.RoleName;
                db.Volunteers.Add(model);
                db.SaveChanges();
                TempData["add_success"] = "Thêm tình nguyện viên thành công";
                return Redirect("/Volunteer/Index");
            }
            catch
            {
                TempData["add_success"] = "Thêm tình nguyện viên KHÔNG thành công";
                return Redirect("/Volunteer/Index");
            }
        }

        //Sửa tình nguyện viên
        public ActionResult EditVolunteer(long ID)
        {
            ViewBag.volunteer = db.Volunteers.Find(ID);
            return View();
        }

        [HttpPost]
        public ActionResult EditVolunteer(Volunteer entity)
        {
            try
            {
                var model = db.Volunteers.Find(entity.ID);
                model.Fullname = entity.Fullname;
                model.Birthday = entity.Birthday;
                model.Phone = entity.Phone;
                model.Email = entity.Email;
                model.Address = entity.Address;
                model.RoleName = entity.RoleName;
                db.SaveChanges();
                TempData["add_success"] = "Sửa tình nguyện viên thành công";
                return Redirect("/Volunteer/Index");
            }
            catch
            {
                TempData["add_success"] = "Sửa tình nguyện viên KHÔNG thành công";
                return Redirect("/Volunteer/Index");
            }
        }

        //Xóa tình nguyện viên
        public JsonResult DeleteVolunteer(long ID)
        {
            var vol = db.Volunteers.Find(ID);
            db.Volunteers.Remove(vol);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        //Mạnh thường quân
        public ActionResult Person()
        {
            ViewBag.lstVolunteers = db.Volunteers.Where(x => x.Type == 2).ToList();
            return View();
        }

        //Thêm mạnh thường quân
        public ActionResult AddPerson()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPerson(Volunteer entity)
        {
            try
            {
                var model = new Volunteer();
                model.Fullname = entity.Fullname;
                model.Birthday = entity.Birthday;
                model.Phone = entity.Phone;
                model.Email = entity.Email;
                model.Address = entity.Address;
                model.Type = 2;
                db.Volunteers.Add(model);
                db.SaveChanges();
                TempData["add_success"] = "Thêm mạnh thường quân thành công";
                return Redirect("/Volunteer/Person");
            }
            catch
            {
                TempData["add_success"] = "Thêm mạnh thường quân KHÔNG thành công";
                return Redirect("/Volunteer/Person");
            }
        }

        //Sửa mạnh thường quân
        public ActionResult EditPerson(long ID)
        {
            ViewBag.volunteer = db.Volunteers.Find(ID);
            return View();
        }

        [HttpPost]
        public ActionResult EditPerson(Volunteer entity)
        {
            try
            {
                var model = db.Volunteers.Find(entity.ID);
                model.Fullname = entity.Fullname;
                model.Birthday = entity.Birthday;
                model.Phone = entity.Phone;
                model.Email = entity.Email;
                model.Address = entity.Address;
                db.SaveChanges();
                TempData["add_success"] = "Sửa mạnh thường quân thành công";
                return Redirect("/Volunteer/Person");
            }
            catch
            {
                TempData["add_success"] = "Sửa mạnh thường quân KHÔNG thành công";
                return Redirect("/Volunteer/Person");
            }
        }

        //Xóa mạnh thường quân
        public JsonResult DeletePerson(long ID)
        {
            var vol = db.Volunteers.Find(ID);
            db.Volunteers.Remove(vol);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
    }
}