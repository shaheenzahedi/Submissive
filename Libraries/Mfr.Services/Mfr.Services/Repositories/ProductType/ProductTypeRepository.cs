using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Services.Repositories.ProductType
{
    public class ProductTypeRepository : Repository<Core.Model.ProductType>, IProductTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
       
    }
}
