
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memcomb.Models
{
    public class ProfilePageModel
    {
        public Following following { get; set; }
        public User user { get; set; }
    }
}