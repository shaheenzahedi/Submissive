using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Core.Mapping
{
    public class CommentMap : EntityTypeConfiguration<Comment>
    {
        public CommentMap()
        {
            ToTable("Comment");

            Property(p => p.Text)
                .HasMaxLength(250)
                .IsRequired();

            //HasRequired(e => e.User)
            //    .WithMany(e => e.Comments)
            //    .HasForeignKey(e => e.UserId)
            //    .WillCascadeOnDelete(false);
        }
    }
}
