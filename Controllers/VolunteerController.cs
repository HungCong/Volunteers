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
                model.Sex = entity.Sex;
                model.Type = 1;
                model.RoleName = entity.RoleName;
                model.Status = false;
                db.Volunteers.Add(model);
                db.SaveChanges();
                TempData["add_success"] = "Thêm tình nguyện viên thành công. Bạn vui lòng chờ quản trị viên phê duyệt";
                return Redirect("/Home/Index");
            }
            catch
            {
                TempData["add_success"] = "Thêm tình nguyện viên KHÔNG thành công";
                return Redirect("/Home/Index");
            }
        }

    }
}