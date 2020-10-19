using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.DTO;
using Volunteers.Models.Entity;

namespace Volunteers.Areas.Admin.Controllers
{
    public class PurchaseController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();
        // GET: Purchase
        public ActionResult Index()
        {
            var query = from pur in db.Purchases
                        join mat in db.Materials on pur.Material_ID equals mat.ID
                        join vol in db.Volunteers on pur.Volunteers_ID equals vol.ID
                        select new PurchaseDTO()
                        {
                            ID = pur.ID,
                            Volunteer_Name = vol.Fullname,
                            Material_Name = mat.Material_Name,
                            CreatedDate = pur.CreatedDate,
                            Place = pur.Place,
                            Description = pur.Description,
                            TotalMoney = mat.TotalMoney,
                            Quantity = mat.Quantity
                        };
            ViewBag.LstPurchase = query.ToList();
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.lstVolunteer = db.Volunteers.Where(x => x.Type == 1).ToList();
            ViewBag.lstMaterial = db.Materials.OrderByDescending(x => x.Material_Name).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddPurchase(Purchase entity)
        {
            try
            {
                var model = new Purchase();
                model.Volunteers_ID = entity.Volunteers_ID;
                model.Place = entity.Place;
                model.Material_ID = entity.Material_ID;
                model.Description = entity.Description;
                model.CreatedDate = DateTime.Now;

                db.Purchases.Add(model);
                db.SaveChanges();
                TempData["add_success"] = "Thêm thu chi thành công";
                return Redirect("/Purchase/Index");
            }
            catch
            {
                TempData["add_success"] = "Thêm thu chi KHÔNG thành công";
                return Redirect("/Purchase/Index");
            }
        }

        public ActionResult Edit(long ID)
        {
            ViewBag.Purchase = db.Purchases.Find(ID);
            ViewBag.lstVolunteer = db.Volunteers.Where(x => x.Type == 1).ToList();
            ViewBag.lstMaterial = db.Materials.OrderByDescending(x => x.Material_Name).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult EditPurchase(Purchase entity)
        {
            try
            {
                var model = db.Purchases.Find(entity.ID);
                model.Volunteers_ID = entity.Volunteers_ID;
                model.Place = entity.Place;
                model.Material_ID = entity.Material_ID;
                model.Description = entity.Description;
                model.CreatedDate = DateTime.Now;
                db.SaveChanges();
                TempData["add_success"] = "Sửa thu chi thành công";
                return Redirect("/Purchase/Index");
            }
            catch
            {
                TempData["add_success"] = "Sửa KHÔNG thành công";
                return Redirect("/Purchase/Index");
            }
        }

        public JsonResult Delete(long ID)
        {
            var pur = db.Purchases.Find(ID);
            db.Purchases.Remove(pur);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
    }
}