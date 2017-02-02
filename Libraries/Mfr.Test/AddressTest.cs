using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FizzWare.NBuilder;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using Mfr.Services.Repositories.Address;
using NUnit.Framework;

namespace Mfr.Test
{
    [TestFixture]
    class AddressTest:IGenericTest<Core.Model.Address>
    {
    
    public ApplicationDbContext Context { get; set; }

    public IRepository<Address> Target { get; set; }


#pragma warning disable 618
    [TestFixtureSetUp]
#pragma warning restore 618
    public void InitializeTest()
    {
           Context = new ApplicationDbContext();
           Target = new AddressRepository(Context);
    }

    [Test]
    public void GetAllTest()
        {
            var expectedCount = Context.Address.Count();
            var results = Target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedCount,results.Count());

        }
        
    [Test]
    public void GetByIdTest()
    {
        var firstRawId = Context.Address.FirstOrDefault().Id;
        var result = Target.Get(firstRawId);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Id == firstRawId);
    }



        [Test]
        public void RemoveSingleModelTest()
        {
            Address address = Context.Address.FirstOrDefault();
            int expected = Context.Address.Count() - 1;
            Target.Remove(address);
            Target.Complete();
            int actual = Context.Address.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void RemoveRangeTest()
        {
            var firstRawId = Context.Address.FirstOrDefault().Id;
            var result = Target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == firstRawId);
        }


    [Test]
    public void AddSingleModelTest()
        {
            //city, country,state Required
            int countryId = Context.Country.FirstOrDefault().Id,stateId=Context.State.FirstOrDefault().Id,cityId=Context.City.FirstOrDefault().Id;
            var expected = Context.Address.Count() + 1;
            var address = Builder<Address>.CreateNew().With(c => c.Description = Faker.Lorem.Paragraph())
                .With(c=>c.StateId=stateId)
                .With(c=>c.CityId=cityId)
                .With(c=>c.CountryId=countryId)
                .With(c=>c.UserApplicationId=Faker.RandomNumber.Next())
                .Build();
            
            //var address = new Address() { UserApplicationId = 1, Id = countryId, StateId = stateId, CityId = cityId, Description = Guid.NewGuid().ToString() };
            Target.Add(address);
            Target.Complete();
            var actual = Context.Address.Count();
            Assert.AreEqual(expected, actual);
        }

        [Test]
    public void AddRangeTest()
        {  //city, country,state Required
            int countryId = Context.Country.FirstOrDefault().Id, stateId = Context.State.FirstOrDefault().Id, cityId = Context.City.FirstOrDefault().Id;
            //List<Address> addresses=new List<Address>()
            //{
            //    new Address {UserApplicationId = 1, Id = countryId, StateId = stateId, CityId = cityId, Description = Guid.NewGuid().ToString()  },
            //    new Address {UserApplicationId = 1, Id = countryId, StateId = stateId, CityId = cityId, Description = Guid.NewGuid().ToString()  },
            //    new Address {UserApplicationId = 1, Id = countryId, StateId = stateId, CityId = cityId, Description = Guid.NewGuid().ToString()  }
            //};
            IList<Address> addresses = Builder<Address>.CreateListOfSize(100).All().With(c => c.Description = Faker.Lorem.Paragraph())
    .With(c => c.StateId = stateId)
    .With(c => c.CityId = cityId)
    .With(c => c.CountryId = countryId)
    .With(c => c.UserApplicationId = 0)
    .Build();
            var expected = Context.Address.Count() + addresses.Count;
            Target.AddRange(addresses);
            Target.Complete();
            var actual = Context.Address.Count();
            Assert.AreEqual(expected,actual);
          
        }
[Test]
    public void FindTest()
        {//Address Must NOT Be Empty
            var param = Context.Address.LastOrDefault();
            IEnumerable<Address> _foundItem= Target.Find(x => x.Id == param.Id);
            foreach (var item in _foundItem)
            {
                Assert.AreEqual(param.Id, item.Id);
            }
        }

    }
}
