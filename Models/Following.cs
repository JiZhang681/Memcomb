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
    
    public partial class Following
    {
        public int User_Following { get; set; }
        public int User_Followed { get; set; }
        public string User_Followed_First_Name { get; set; }
        public string User_Followed_Last_Name { get; set; }

        public virtual User User { get; set; }
    }
}
