using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.Entity;

namespace Volunteers.Areas.Admin.Controllers
{
    public class VolunteerController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();
        // GET: Volunteer
        public ActionResult Index()
        {
            ViewBag.lstVolunteers = db.Volunteers.Where(x => x.Type == 1 && x.Status == true).ToList();
            return View();
        }

        //danh sách tnv chưa duyệt
        public ActionResult VolunteerNotTest()
        {
            ViewBag.lstVolunteers = db.Volunteers.Where(x => x.Type == 1 && x.Status == false).ToList();
            return View();
        }

        //Duyệt tnv
        public ActionResult ChangeStatus(long ID)
        {
            var vol = db.Volunteers.Find(ID);
            vol.Status = true;
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
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
                return Redirect("/Admin/Volunteer/Person");
            }
            catch
            {
                TempData["add_success"] = "Thêm mạnh thường quân KHÔNG thành công";
                return Redirect("/Admin/Volunteer/Person");
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
                return Redirect("/Admin/Volunteer/Person");
            }
            catch
            {
                TempData["add_success"] = "Sửa mạnh thường quân KHÔNG thành công";
                return Redirect("/Admin/Volunteer/Person");
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