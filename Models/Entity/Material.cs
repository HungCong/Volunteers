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
    
    public partial class Material
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            this.Material_Support = new HashSet<Material_Support>();
        }
    
        public long ID { get; set; }
        public Nullable<long> Volunteers_ID { get; set; }
        public string Form { get; set; }
        public Nullable<decimal> TotalMoney { get; set; }
        public string Material_Name { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Place { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material_Support> Material_Support { get; set; }
        public virtual Volunteer Volunteer { get; set; }
    }
}