using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfr.Core.Model
{
    public class Comment
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        public string Text { get; set; }

        public int Like { get; set; }

        public int DisLike { get; set; }

        public bool AdminConfirm { get; set; }

        public virtual Product Product { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}
