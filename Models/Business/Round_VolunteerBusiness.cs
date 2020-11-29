using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Volunteers.Models.Entity;

namespace Volunteers.Models.Business
{
    public class Round_VolunteerBusiness
    {
        private VolunteerEntities db = new VolunteerEntities();

        //Cập nhật số lượng vật chất
        public void UpdateQuantity(long Material_ID, int Quantity, bool sub)
        {
            var material = db.Materials.Find(Material_ID);
            if (sub)
            {
                material.Quantity -= Quantity;
            }
            else
            {
                material.Quantity += Quantity;
            }
            if (material.Quantity == 0)
                material.Status = false;
            db.SaveChanges();
        }

        //Cập nhật số tiền
        public void UpdateTotalMoney(long Material_ID, decimal TotalMoney, bool sub)
        {
            var material = db.Materials.Find(Material_ID);
            if (sub)
            {
                material.TotalMoney -= TotalMoney;
            }
            else
            {
                material.TotalMoney += TotalMoney;
            }
            if (material.TotalMoney == 0)
                material.Status = false;
            db.SaveChanges();
        }

        //Cập nhật số lượng tnv
        public void UpdateRegister(long ID)
        {
            var round = db.Round_Volunteer.Find(ID);
            round.Register += 1;
            db.SaveChanges();
        }

        //Thêm trưởng đoàn
        public void AddCoach(long Volunteers_ID, long Round_Volunteer_ID)
        {
            var joi = new Join();
            joi.Volunteers_ID = Volunteers_ID;
            joi.Round_Volunteer_ID = Round_Volunteer_ID;
            joi.Rolename = "Trưởng đoàn";
            db.Joins.Add(joi);
            db.SaveChanges();
        }

        //Cập nhật trạng thái đợt tn
        public void UpdateStatus(long ID)
        {
            var round = db.Round_Volunteer.Find(ID);
            if(round.Status != 0)
            {
                if (round.Dealine_Register > DateTime.Now)
                    round.Status = 2;
                else if (round.Dealine_Register < DateTime.Now && round.StartDate > DateTime.Now)
                    round.Status = 3;
                else if (round.StartDate < DateTime.Now && DateTime.Now < round.EndDate)
                    round.Status = 4;
                else if (DateTime.Now > round.EndDate)
                    round.Status = 5;
                db.SaveChanges();
            }
        }
    }
}