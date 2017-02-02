using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mfr.Core.Model;

namespace Mfr.Services.Repositories.Product
{
    public class ProductRepository : Repository<Core.Model.Product>, IProductRepository
    {

        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
