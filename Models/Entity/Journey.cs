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
    
    public partial class Journey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Journey()
        {
            this.Round_Volunteer = new HashSet<Round_Volunteer>();
        }
    
        public long ID { get; set; }
        public string Location_Go { get; set; }
        public string Destination { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Resgister { get; set; }
        public Nullable<int> Join { get; set; }
        public Nullable<int> Status { get; set; }
        public string Journey_Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Round_Volunteer> Round_Volunteer { get; set; }
    }
}
