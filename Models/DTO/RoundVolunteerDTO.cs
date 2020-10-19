using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Volunteers.Models.DTO
{
    public class RoundVolunteerDTO
    {
        public long Round_Volunteer_ID { get; set; }
        public long Join_ID { get; set; }
        public long Volunteer_ID { get; set; }
        public string Fullname { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }

        public string Place { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Register { get; set; }
        public Nullable<int> Status { get; set; }
    }
}