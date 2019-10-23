﻿using System;
using System.Collections.Generic;

namespace BlogEngine.BusinessLogic.Models
{
    public partial class Post
    {
        public Post()
        {
            Comment = new HashSet<Comment>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string AuthorName { get; set; }
        public bool? PendingToApprove { get; set; }
        public string ApproverName { get; set; }
        public DateTime? ApprovalDateTime { get; set; }
        public bool? IsPublished { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
    }
}
