using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Mfr.Admin.Controllers;
using Mfr.Core.Model;
using Mfr.Services.Repositories.Address;
using Mfr.Services.Repositories.City;
using Mfr.Services.Repositories.Comment;
using Mfr.Services.Repositories.Country;
using Mfr.Services.Repositories.PhoneNumber;
using Mfr.Services.Repositories.Product;
using Mfr.Services.Repositories.ProductType;
using Mfr.Services.Repositories.ProductPicture;

using Mfr.Services.Repositories.State;
namespace Mfr.Admin
{
    public static class AutofacConfig
    {
        public static void RegisterComponent()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CountryRepository>().As<ICountryRepository>();
            builder.RegisterType<CityRepository>().As<ICityRepository>();

            builder.RegisterType<CountryController>();
            builder.RegisterType<CityController>();


            builder.RegisterType<ProductTypeRepository>().As<IProductTypeRepository>();
            builder.RegisterType<ProductTypeController>();
            
            builder.RegisterType<CityRepository>().As<ICityRepository>();
            builder.RegisterType<CityController>();

            builder.RegisterType<StateRepository>().As<IStateRepository>();
            builder.RegisterType<StateController>();

            builder.RegisterType<PhoneNumberRepository>().As<IPhoneNumberRepository>();
            builder.RegisterType<PhoneNumberController>();

            builder.RegisterType<AddressRepository>().As<IAddressRepository>();
            builder.RegisterType<AddressController>();

            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<ProductController>();

            builder.RegisterType<CommentRepository>().As<ICommentRepository>();
            builder.RegisterType<CommentController>();

            builder.RegisterType<ProductPictureRepository>().As<IProductPictureRepository>();
            
            builder.RegisterType<ApplicationDbContext>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}