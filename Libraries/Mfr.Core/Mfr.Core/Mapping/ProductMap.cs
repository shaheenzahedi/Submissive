using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Core.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Product");

            Property(p => p.Title)
                .HasMaxLength(250)
                .IsRequired();

            Property(p => p.Description)
                .HasMaxLength(500)
                .IsOptional();

            HasRequired(p => p.User)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
