using System;
using Mfr.Core.Model;
using Mfr.Services.Repositories.Address;
using Mfr.Services.Repositories.City;
using Mfr.Services.Repositories.Comment;
using Mfr.Services.Repositories.Country;
using Mfr.Services.Repositories.EMail;
using Mfr.Services.Repositories.PhoneNumber;
using Mfr.Services.Repositories.Product;
using Mfr.Services.Repositories.ProductPicture;
using Mfr.Services.Repositories.ProductType;
using Mfr.Services.Repositories.State;

namespace Mfr.Services.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            City = new CityRepository(_context);
            Product = new ProductRepository(_context);
            Address = new AddressRepository(_context);
            Comment = new CommentRepository(_context);
            Country = new CountryRepository(_context);
            Email = new EmailRepository(_context);
            PhoneNumber = new PhoneNumberRepository(_context);
            ProductPicture = new ProductPictureRepository(_context);
            ProductType = new ProductTypeRepository(_context);
            State = new StateRepository(_context);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ICityRepository City { get; private set; }
        public IProductRepository Product { get; private set; }
        public IAddressRepository Address { get; private set; }
        public ICommentRepository Comment { get; private set; }
        public ICountryRepository Country { get; private set; }
        public IEmailRepository Email { get; private set; }
        public IPhoneNumberRepository PhoneNumber { get; private set; }
        public IProductPictureRepository ProductPicture { get; private set; }
        public IProductTypeRepository ProductType { get; private set; }
        public IStateRepository State { get; private set; }
        void IUnitOfWork.Complete()
        {
            _context.SaveChanges();
        }

    }
}

