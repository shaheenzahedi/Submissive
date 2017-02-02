using System;
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
    public interface IUnitOfWork : IDisposable
    {
        ICityRepository City { get;}
        IProductRepository Product { get;}
        IAddressRepository Address { get;}
        ICommentRepository Comment { get;}
        ICountryRepository Country { get;}
        IEmailRepository Email { get;}
        IPhoneNumberRepository PhoneNumber { get;}
        IProductPictureRepository ProductPicture { get;}
        IProductTypeRepository ProductType { get;}
        IStateRepository State { get;}
        void Complete();
    }
}
