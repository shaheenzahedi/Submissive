using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductPicture { get; set; }
        public bool AdminConfirm { get; set; }
        public string Text { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public int UserId { get; set; }

    }
}