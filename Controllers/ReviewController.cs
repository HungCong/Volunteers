using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models.DTO;
using Volunteers.Models.Entity;

namespace Volunteers.Controllers
{
    public class ReviewController : Controller
    {
        private VolunteerEntities db = new VolunteerEntities();

        // GET: Review
        public ActionResult Index()
        {
            var user = Session["user"] as User;
            var query = from round in db.Round_Volunteer
                        join joi in db.Joins on round.ID equals joi.Round_Volunteer_ID
                        where joi.Volunteers_ID == user.Volunteers_ID && round.Status == 5
                        select new RoundVolunteerDTO()
                        {
                            Round_Volunteer_ID = round.ID,
                            Place = round.Place,
                            StartDate = round.StartDate,
                            EndDate = round.EndDate,
                            Register = round.Register,
                            CreatedDate = round.CreatedDate
                        };
            ViewBag.LstVolunteer = query.OrderByDescending(x => x.CreatedDate).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddReview(Review entity)
        {
            var review = new Review();
            review.Round_Volunteer_ID = entity.Round_Volunteer_ID;
            review.Standard_1 = entity.Standard_1;
            review.Standard_2 = entity.Standard_2;
            review.Standard_3 = entity.Standard_3;
            review.Point = entity.Point;
            review.CreatedDate = DateTime.Now;
            db.Reviews.Add(review);
            db.SaveChanges();
            TempData["add_success"] = "Đánh giá đợt tình nguyện thành công";
            return Redirect("/Review");
        }
    }
}