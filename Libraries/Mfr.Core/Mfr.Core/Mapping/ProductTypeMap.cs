using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Core.Mapping
{
    public class ProductTypeMap : EntityTypeConfiguration<ProductType>
    {
        public ProductTypeMap()
        {
            ToTable("ProductType");

            Property(p => p.Title)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}
