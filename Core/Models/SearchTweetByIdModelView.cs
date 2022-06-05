using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SearchTweetByIdModelView
    {
        public List<string> Tags { get; set; }
        public List<string> UserTagged { get; set; }
        public int TagCount { get; set; }
        public string TweetText { get; set; }
        public string UserId { get; set; }
        public int TweetViewCount { get; set; }
    }
}
