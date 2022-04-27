using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
   public class ShowMostTaggedTweetModel
    {
        public int Id{ get; set; }
        public string Text{ get; set; }
        public string Word{ get; set; }
        public int TagCount{ get; set; }
    }
}
