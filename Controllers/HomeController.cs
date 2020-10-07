using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.Entity;

namespace Volunteers.Controllers
{
    public class HomeController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();
        public ActionResult Index()
        {
            ViewBag.LstVolunteer = db.Round_Volunteer.ToList();
            return View();
        }

        public ActionResult AddVolunteer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Round_Volunteer entity)
        {
            try
            {
                var model = new Round_Volunteer();
                model.CreatedDate = DateTime.Now;
                model.Place = entity.Place;
                model.StartDate = entity.StartDate;
                model.EndDate = entity.EndDate;
                model.Register = entity.Register;
                model.Status = true;
                db.Round_Volunteer.Add(model);
                db.SaveChanges();
                TempData["add_success"] = "Thêm đợt tình nguyện thành công";
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("AddVolunteer");
            }
        }

        public ActionResult EditVolunteer(long ID)
        {
            ViewBag.Round_Volunteer = db.Round_Volunteer.Find(ID);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Round_Volunteer entity)
        {
            try
            {
                var model = db.Round_Volunteer.Find(entity.ID);
                model.Place = entity.Place;
                model.StartDate = entity.StartDate;
                model.EndDate = entity.EndDate;
                model.Register = entity.Register;
                db.SaveChanges();
                TempData["add_success"] = "Sửa đợt tình nguyện thành công";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["add_success"] = "Sửa KHÔNG thành công";
                return RedirectToAction("Index");
            }
        }

        public JsonResult Delete(long ID)
        {
            var rv = db.Round_Volunteer.Find(ID);
            db.Round_Volunteer.Remove(rv);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
    }
}