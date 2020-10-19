using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.DTO;
using Volunteers.Models.Entity;

namespace Volunteers.Areas.Admin.Controllers
{
    public class MaterialController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();
        public ActionResult Index()
        {
            var query = from mat in db.Materials
                        join vol in db.Volunteers on mat.Volunteers_ID equals vol.ID
                        select new MaterialDTO()
                        {
                            ID = mat.ID,
                            Volunteer_Name = vol.Fullname,
                            Form = mat.Form,
                            TotalMoney = mat.TotalMoney,
                            Material_Name = mat.Material_Name,
                            CreatedDate = mat.CreatedDate,
                            Place = mat.Place,
                            Quantity = mat.Quantity,
                            Status = mat.Status
                        };
            ViewBag.LstMaterial_Money = query.Where(x => x.TotalMoney != null).ToList();//Vật chất là tiền 
            ViewBag.LstMaterial = query.Where(x => x.TotalMoney == null).ToList();//Vật chất là hiện vật
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.lstVolunteer = db.Volunteers.Where(x => x.Type == 2).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddMaterial(Material entity)
        {
            try
            {
                var model = new Material();
                model.Volunteers_ID = entity.Volunteers_ID;
                model.Form = entity.Form;
                model.TotalMoney = entity.TotalMoney;
                model.Material_Name = entity.Material_Name;
                model.CreatedDate = DateTime.Now;
                model.Place = entity.Place;
                model.Quantity = entity.Quantity;
                model.Status = true;
                db.Materials.Add(model);
                db.SaveChanges();
                TempData["add_success"] = "Thêm vật chất tình nguyện thành công";
                return Redirect("/Admin/Material/Index");
            }
            catch
            {
                TempData["add_success"] = "Thêm vật chất tình nguyện KHÔNG thành công";
                return Redirect("/Admin/Material/Index");
            }
        }

        public ActionResult Edit(long ID)
        {
            ViewBag.Material = db.Materials.Find(ID);
            ViewBag.lstVolunteer = db.Volunteers.Where(x => x.Type == 2).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult EditMaterial(Material entity)
        {
            try
            {
                var model = db.Materials.Find(entity.ID);
                model.Volunteers_ID = entity.Volunteers_ID;
                model.Form = entity.Form;
                model.TotalMoney = entity.TotalMoney;
                model.Quantity = entity.Quantity;
                model.Material_Name = entity.Material_Name;
                model.Place = entity.Place;
                db.SaveChanges();
                TempData["add_success"] = "Sửa vật chất tình nguyện thành công";
                return Redirect("/Admin/Material/Index");
            }
            catch
            {
                TempData["add_success"] = "Sửa KHÔNG thành công";
                return Redirect("/Admin/Material/Index");
            }
        }

        public JsonResult Delete(long ID)
        {
            var mat = db.Materials.Find(ID);
            db.Materials.Remove(mat);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
    }
}