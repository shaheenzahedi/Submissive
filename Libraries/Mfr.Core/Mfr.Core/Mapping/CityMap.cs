using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Core.Mapping
{
    public class CityMap : EntityTypeConfiguration<City>
    {
        public CityMap()
        {
            ToTable("City");

            Property(p => p.Title)
                .HasMaxLength(250)
                .IsRequired();

            HasRequired(e => e.State)
                .WithMany(e => e.Cities)
                .HasForeignKey(e => e.StateId)
                .WillCascadeOnDelete(false);
        }
    }
}
