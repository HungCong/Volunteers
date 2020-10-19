using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.Business;
using Volunteers.Models.DTO;
using Volunteers.Models.Entity;

namespace Volunteers.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        //đợt tn chưa duyệt
        public ActionResult Round_Volunteer_NotTest()
        {
            ViewBag.LstVolunteer = db.Round_Volunteer.Where(x => x.Status == 0).OrderByDescending(x => x.CreatedDate).ToList();
            return View();
        }

        //Chi tiết đợt tình nguyện
        public ActionResult Detail_Round_Volunteer(long ID)
        {
            ViewBag.Round_Volunteer = db.Round_Volunteer.Find(ID);
            return View();
        }

        //Lý do không duyệt
        [HttpPost]
        public ActionResult Round_VolunteerCancer(Round_Volunteer entity)
        {
            try
            {
                var round = db.Round_Volunteer.Find(entity.ID);
                round.ReasonNotTest = entity.ReasonNotTest;
                round.Status = 1;
                db.SaveChanges();
                TempData["add_success"] = "Cập nhật trạng thái đợt tình nguyện thành công";
                return RedirectToAction("Round_Volunteer_NotTest");
            }
            catch
            {
                TempData["add_success"] = "Cập nhật trạng thái đợt tình nguyện KHÔNG thành công";
                return RedirectToAction("Round_Volunteer_NotTest");
            }
        }

        //Duyệt đợt tn
        public JsonResult ChangeStatus(long ID)
        {
            var round = db.Round_Volunteer.Find(ID);
            if (round.Dealine_Register > DateTime.Now)
                round.Status = 2;
            else if (round.Dealine_Register < DateTime.Now || round.StartDate > DateTime.Now)
                round.Status = 3;
            else if (round.StartDate < DateTime.Now || DateTime.Now < round.EndDate)
                round.Status = 4;
            else if (DateTime.Now > round.EndDate)
                round.Status = 5;
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        //đợt tn đã duyệt
        public ActionResult Round_Volunteer_Tested()
        {
            var model = db.Round_Volunteer.Where(x => x.Status == 4 || x.Status == 2 || x.Status == 3 || x.Status == 5).OrderByDescending(x => x.CreatedDate).ToList();
            
            foreach(var item in model)
            {
                new Round_VolunteerBusiness().UpdateStatus(item.ID);
            }

            ViewBag.LstVolunteer = model;
            return View();
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


        //đợt tn Không duyệt
        public ActionResult Round_Volunteer_Cancer()
        {
            ViewBag.LstVolunteer = db.Round_Volunteer.Where(x => x.Status == 1 || x.Status == -1).OrderByDescending(x => x.CreatedDate).ToList();
            return View();
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
            ViewBag.lstSelectMaterial = db.Materials.ToList();
            return View();
        }

        public JsonResult getMaterialByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var mat = db.Materials.Find(ID);
            return Json(mat, JsonRequestBehavior.AllowGet);
        }

        //Xóa vật chất hỗ trợ
        public JsonResult DeleteMaterial(long ID)
        {
            //Xóa
            var mat = db.Material_Support.Find(ID);
            db.Material_Support.Remove(mat);

            //Cập nhật lại số lượng/tổng tiền
            var material = db.Materials.Find(mat.Material_ID);
            if(mat.Quantity != null)
            {
                material.Quantity += mat.Quantity;
            }else if(mat.TotalMoney != null)
            {
                material.TotalMoney += mat.TotalMoney;
            }
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public ActionResult AddMaterialSupport(Material_Support entity)
        {
            var mat = new Material_Support();
            mat.Material_ID = entity.Material_ID;
            mat.Round_Volunteer_ID = entity.Round_Volunteer_ID;
            if(entity.Quantity != null)
            {
                mat.Quantity = entity.Quantity;
                new Round_VolunteerBusiness().UpdateQuantity((long)entity.Material_ID, (int)entity.Quantity, true);
            }
            
            if(entity.TotalMoney != null)
            {
                mat.TotalMoney = entity.TotalMoney;
                new Round_VolunteerBusiness().UpdateTotalMoney((long)entity.Material_ID, (decimal)entity.TotalMoney, true);
            }
            db.Material_Support.Add(mat);
            db.SaveChanges();
            TempData["add_success"] = "Thêm vật chất thành công";
            return Redirect("/Admin/Home/ListMaterial_Support/" + entity.Round_Volunteer_ID);
        }

        [HttpPost]
        public ActionResult EditMaterialSupport(Material_Support entity, int? Ex_Quantity, decimal? Ex_TotalMoney)
        {
            var mat = db.Material_Support.Find(entity.ID);

            mat.Material_ID = entity.Material_ID;
            if (entity.Quantity != null)
            {
                mat.Quantity = entity.Quantity;
                var res = Ex_Quantity - mat.Quantity;
                if (res > 0)
                    new Round_VolunteerBusiness().UpdateQuantity((long)entity.Material_ID, (int)res, false);
                else
                {
                    res = res * (-1);
                    new Round_VolunteerBusiness().UpdateQuantity((long)entity.Material_ID, (int)res, true);
                }
            }

            if (entity.TotalMoney != null)
            {
                mat.TotalMoney = entity.TotalMoney;
                var res = Ex_TotalMoney - mat.TotalMoney;
                if (res > 0)
                    new Round_VolunteerBusiness().UpdateTotalMoney((long)entity.Material_ID, (decimal)res, false);
                else
                {
                    res = res * (-1);
                    new Round_VolunteerBusiness().UpdateTotalMoney((long)entity.Material_ID, (decimal)res, true);
                }
            }

            db.SaveChanges();
            TempData["add_success"] = "Sửa vật chất thành công";
            return Redirect("/Admin/Home/ListMaterial_Support/" + entity.Round_Volunteer_ID);
        }

        //Tìm vật chất hỗ trợ
        public JsonResult FindMaterialSupport(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var mat = db.Material_Support.Find(ID);
            return Json(mat, JsonRequestBehavior.AllowGet);
        }
    }
}