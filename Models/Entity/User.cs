//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Volunteers.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public long ID { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Image { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
