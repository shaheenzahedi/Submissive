using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Core.Mapping
{
    public class ProductPictureMap : EntityTypeConfiguration<ProductPicture>
    {
        public ProductPictureMap()
        {
            ToTable("ProductPicture");

            Property(p => p.PictureUrl)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}
