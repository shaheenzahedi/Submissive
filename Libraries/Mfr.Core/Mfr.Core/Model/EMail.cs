using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfr.Core.Model
{
    public class EMail
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
