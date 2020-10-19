using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Volunteers.Models.DTO
{
    public class Materal_SupportDTO
    {
        public long ID { get; set; }
        public Nullable<long> Material_ID { get; set; }
        public Nullable<long> Round_Volunteer_ID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> TotalMoney { get; set; }

        public string Material_Name { get; set; }
        public string Round_Volunteer_Name { get; set; }
    }
}