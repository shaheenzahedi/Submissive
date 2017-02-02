using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Core.Mapping
{
    public class StateMap : EntityTypeConfiguration<State>
    {
        public StateMap()
        {
            ToTable("State");

            Property(p => p.Title)
                .HasMaxLength(250)
                .IsRequired();

            HasRequired(e => e.Country)
                .WithMany(e => e.States)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete(false);

        }
    }
}
