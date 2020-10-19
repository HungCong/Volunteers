using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.Business;
using Volunteers.Models.DTO;
using Volunteers.Models.Entity;

namespace Volunteers.Controllers
{
    public class HomeController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();

        //Đợt tình nguyện đã tạo
        public ActionResult Index()
        {
            var user = Session["user"] as User;
            if (user == null)
                return Redirect("/dang-nhap");
            var model = db.Round_Volunteer.Where(x => x.Volunteers_ID == user.Volunteers_ID).OrderByDescending(x => x.CreatedDate).ToList();
            foreach (var item in model)
            {
                new Round_VolunteerBusiness().UpdateStatus(item.ID);
            }
            ViewBag.LstVolunteer = model;
            return View();
        }

        //Tham gia đợt tn
        public ActionResult JoinVolunteer()
        {
            var user = Session["user"] as User;
            if (user == null)
                return Redirect("/dang-nhap");
            var query = from joi in db.Joins
                        join round in db.Round_Volunteer on joi.Round_Volunteer_ID equals round.ID
                        where round.Volunteers_ID != user.Volunteers_ID && joi.Volunteers_ID != user.Volunteers_ID
                        select new RoundVolunteerDTO()
                        {
                            Round_Volunteer_ID = round.ID,
                            Place = round.Place,
                            StartDate = round.StartDate,
                            EndDate = round.EndDate,
                            Register = round.Register,
                            Status = round.Status,
                            CreatedDate = round.CreatedDate,
                            Volunteer_ID = (long)joi.Volunteers_ID
                        };
            var model = query.Where(x => x.Status == 4 || x.Status == 2 || x.Status == 3 || x.Status == 5).OrderByDescending(x => x.CreatedDate).ToList();
            
            //Cập nhật trạng thái đợt tn
            foreach (var item in model)
            {
                new Round_VolunteerBusiness().UpdateStatus(item.Round_Volunteer_ID);
            }

            ViewBag.listJoin = db.Joins.Where(x => x.Volunteers_ID == user.Volunteers_ID && x.Rolename == "Tình nguyện viên").ToList();
            ViewBag.LstVolunteer = model;
            return View();
        }

        //Đợt TN đang tham gia
        public ActionResult Volunteer_Joining()
        {
            var user = Session["user"] as User;
            var query = from round in db.Round_Volunteer
                        join joi in db.Joins on round.ID equals joi.Round_Volunteer_ID
                        where joi.Volunteers_ID == user.Volunteers_ID && round.Volunteers_ID != user.Volunteers_ID
                        select new RoundVolunteerDTO()
                        {
                            Round_Volunteer_ID = round.ID,
                            Place = round.Place,
                            StartDate = round.StartDate,
                            EndDate = round.EndDate,
                            Register = round.Register,
                            Status = round.Status,
                            CreatedDate = round.CreatedDate
                        };
            var model = query.Where(x => x.Status == 4 || x.Status == 2 || x.Status == 3 || x.Status == 5).OrderByDescending(x => x.CreatedDate).ToList();
            foreach (var item in model)
            {
                new Round_VolunteerBusiness().UpdateStatus(item.Round_Volunteer_ID);
            }
            ViewBag.LstVolunteer = model;
            return View();
        }

        public ActionResult AddVolunteer()
        {
            ViewBag.lstJourney = db.Journeys.Where(x => x.StartDate <= DateTime.Now || x.EndDate >= DateTime.Now).ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Round_Volunteer entity)
        {
            var user = Session["user"] as User;
            try
            {
                var model = new Round_Volunteer();
                model.CreatedDate = DateTime.Now;
                model.Place = entity.Place;
                model.StartDate = entity.StartDate;
                model.EndDate = entity.EndDate;
                model.Register = 0;
                model.Description = entity.Description;
                model.MinPeople = entity.MinPeople;
                model.MaxPeople = entity.MaxPeople;
                model.Dealine_Register = entity.Dealine_Register;
                model.Journey_ID = entity.Journey_ID;
                model.Volunteers_ID = user.Volunteers_ID;
                model.Status = 0;
                db.Round_Volunteer.Add(model);
                db.SaveChanges();

                //Thêm trưởng đoàn zô luôn
                new Round_VolunteerBusiness().AddCoach((long)user.Volunteers_ID, db.Round_Volunteer.Max(x => x.ID));
                //Thêm số lượng tnv tham gia
                new Round_VolunteerBusiness().UpdateRegister(db.Round_Volunteer.Max(x => x.ID));
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
            ViewBag.lstJourney = db.Journeys.ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Round_Volunteer entity)
        {
            try
            {
                var model = db.Round_Volunteer.Find(entity.ID);
                model.Place = entity.Place;
                model.StartDate = entity.StartDate;
                model.EndDate = entity.EndDate;
                model.Journey_ID = entity.Journey_ID;
                model.Description = entity.Description;
                model.MinPeople = entity.MinPeople;
                model.MaxPeople = entity.MaxPeople;
                model.Dealine_Register = entity.Dealine_Register;
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

        //danh sách tnv tham gia
        public ActionResult ListVolunteer(long ID)//ID là Round_Volunteer_ID
        {
            var query = from Jo in db.Joins
                        join round in db.Round_Volunteer on Jo.Round_Volunteer_ID equals round.ID
                        join vol in db.Volunteers on Jo.Volunteers_ID equals vol.ID
                        where Jo.Round_Volunteer_ID == ID
                        select new RoundVolunteerDTO()
                        {
                            Join_ID = Jo.ID,
                            Volunteer_ID = vol.ID,
                            Fullname = vol.Fullname,
                            Birthday = vol.Birthday,
                            Phone = vol.Phone,
                            RoleName = vol.RoleName
                        };
            ViewBag.Round_Volunteer = db.Round_Volunteer.Find(ID);
            ViewBag.lstVolunteer = query.OrderByDescending(x => x.Fullname).ToList();
            return View();
        }

        //Thêm tnv tham gia
        public JsonResult Join(long ID, long Volunteers_ID)
        {
            var joi = new Join();
            joi.Round_Volunteer_ID = ID;
            joi.Volunteers_ID = Volunteers_ID;
            joi.Rolename = "Tình nguyện viên";
            db.Joins.Add(joi);
            db.SaveChanges();

            new Round_VolunteerBusiness().UpdateRegister(ID);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public ActionResult AddCancer(Round_Volunteer entity)
        {
            var round = db.Round_Volunteer.Find(entity.ID);
            round.ReasonCancer = entity.ReasonCancer;
            round.Status = -1;
            db.SaveChanges();
            TempData["add_success"] = "Hủy đợt tình nguyện thành công";
            return Redirect("/");
        }

        //Vật chất đợt tn
        public ActionResult ListMaterial_Support(long ID)
        {
            var query = from sup in db.Material_Support
                        join mat in db.Materials on sup.Material_ID equals mat.ID
                        where sup.Round_Volunteer_ID == ID
                        select new Materal_SupportDTO()
                        {
                            ID = sup.ID,
                            Material_Name = mat.Material_Name,
                            Quantity = sup.Quantity,
                            TotalMoney = sup.TotalMoney
                        };
            ViewBag.lstTotalMoney = query.Where(x => x.TotalMoney != null).ToList();
            ViewBag.LstMaterial = query.Where(x => x.Quantity != null).ToList();
            ViewBag.Round_Volunteer = db.Round_Volunteer.Find(ID);
            return View();
        }
    }
}