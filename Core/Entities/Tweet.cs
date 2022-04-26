using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Entities
{
    public class Tweet
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public ICollection<TweetTags> Tags  { get; set; }
        public int TagCount { get; set; } = 0;
        public int TweetViewCount { get; set; } = 1;
        //[NotMapped]
        //public IFormFile Photo { get; set; }
        public int Likes { get; set; } = 0;
    }
}
