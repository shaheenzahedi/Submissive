using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Services.Repositories.ProductPicture
{
    public class ProductPictureRepository : Repository<Core.Model.ProductPicture>, IProductPictureRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductPictureRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
