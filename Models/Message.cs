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
    
    public partial class Message
    {
        public int Message_ID { get; set; }
        public int Convo_ID { get; set; }
        public int Message_Sender_ID { get; set; }
        public string Message_Text { get; set; }
        public System.DateTime DATETIME_SENT { get; set; }
    
        public virtual Convo Convo { get; set; }
    }
}
