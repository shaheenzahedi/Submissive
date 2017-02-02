using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Core.Mapping
{
    public class PhoneNumberMap : EntityTypeConfiguration<PhoneNumber>
    {
        public PhoneNumberMap()
        {
            ToTable("PhoneNumber");

            Property(p => p.Number)
                .IsRequired();
         
        }
    }
}
