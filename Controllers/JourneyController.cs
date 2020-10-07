using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.Entity;

namespace Volunteers.Controllers
{
    public class JourneyController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();
        // GET: Journey
        public ActionResult Index()
        {
            ViewBag.lstJourney = db.Journeys.ToList();
            return View();
        }

        //Thêm hành trình
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddJourney(Journey entity)
        {
            try
            {
                var model = new Journey();
                model.Location_Go = entity.Location_Go;
                model.Destination = entity.Destination;
                model.StartDate = entity.StartDate;
                model.EndDate = entity.EndDate;
                model.Resgister = entity.Resgister;
                if (entity.StartDate > DateTime.Now)
                    model.Status = 0;
                else if (entity.StartDate <= DateTime.Now && DateTime.Now <= entity.EndDate)
                    model.Status = 1;
                else if (DateTime.Now > entity.EndDate)
                    model.Status = 2;
                model.Join = 0;
                db.Journeys.Add(model);
                db.SaveChanges();
                TempData["add_success"] = "Thêm hành trình tình nguyện thành công";
                return Redirect("/Journey/Index");
            }
            catch
            {
                TempData["add_success"] = "Thêm hành trình tình nguyện KHÔNG thành công";
                return Redirect("/Journey/Index");
            }
        }

        //Sửa tình nguyện viên
        public ActionResult Edit(long ID)
        {
            ViewBag.journey = db.Journeys.Find(ID);
            return View();
        }

        [HttpPost]
        public ActionResult EditJourney(Journey entity)
        {
            try
            {
                var model = db.Journeys.Find(entity.ID);
                model.Location_Go = entity.Location_Go;
                model.Destination = entity.Destination;
                model.StartDate = entity.StartDate;
                model.EndDate = entity.EndDate;
                model.Resgister = entity.Resgister;
                if (entity.StartDate > DateTime.Now)
                    model.Status = 0;
                else if (entity.StartDate <= DateTime.Now && DateTime.Now <= entity.EndDate)
                    model.Status = 1;
                else if (DateTime.Now > entity.EndDate)
                    model.Status = 2;
                db.SaveChanges();
                TempData["add_success"] = "Sửa hành trình tình nguyện thành công";
                return Redirect("/Journey/Index");
            }
            catch
            {
                TempData["add_success"] = "Sửa hành trình tình nguyện KHÔNG thành công";
                return Redirect("/Journey/Index");
            }
        }

        //Xóa hành trình
        public JsonResult Delete(long ID)
        {
            var jou = db.Journeys.Find(ID);
            db.Journeys.Remove(jou);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
    }
}