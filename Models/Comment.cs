//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Memcomb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int Comment_ID { get; set; }
        public int Fragment_ID { get; set; }
        public int User_ID { get; set; }
        public Nullable<System.DateTime> Datetime_Posted { get; set; }
        public string Comment1 { get; set; }
    
        public virtual Fragment Fragment { get; set; }
    }
}