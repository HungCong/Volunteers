using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Volunteers.Models.DTO
{
    public class PurchaseDTO
    {
        public long ID { get; set; }
        public Nullable<long> Volunteers_ID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public Nullable<long> Material_ID { get; set; }

        public string Volunteer_Name { get; set; }
        public string Material_Name { get; set; }
        public Nullable<decimal> TotalMoney { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}