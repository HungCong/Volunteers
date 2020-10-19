using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Volunteers.Models.DTO
{
    public class MaterialDTO
    {
        public long ID { get; set; }
        public Nullable<long> Volunteers_ID { get; set; }
        public string Form { get; set; }
        public Nullable<decimal> TotalMoney { get; set; }
        public string Material_Name { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Place { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Volunteer_Name { get; set; }
    }
}