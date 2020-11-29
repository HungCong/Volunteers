using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Volunteers.Models.DTO
{
    public class ReviewDTO
    {
        public long ID { get; set; }
        public Nullable<long> Round_Volunteer_ID { get; set; }
        public string Standard_1 { get; set; }
        public string Standard_2 { get; set; }
        public string Standard_3 { get; set; }
        public Nullable<int> Point { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }

        public string Round_Volunteer_Name { get; set; }
    }
}