using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Core.Mapping
{
    public class EMailMap : EntityTypeConfiguration<EMail>
    {
        public EMailMap()
        {
            ToTable("EMail");

            Property(p => p.Email)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}
