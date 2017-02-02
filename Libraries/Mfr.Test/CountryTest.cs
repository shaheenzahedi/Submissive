using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using Mfr.Services.Repositories.Country;
using NUnit.Framework;
namespace Mfr.Test
{[TestFixture]
    class CountryTest:IGenericTest<Core.Model.Country>
    {
        public ApplicationDbContext Context { get; set; }
        public IRepository<Country> Target { get; set; }
#pragma warning disable 618
        [TestFixtureSetUp]
#pragma warning restore 618
        public void InitializeTest()
        {
            Context = new ApplicationDbContext();
            Target = new CountryRepository(Context);
        }
        [Test]
        public void GetAllTest()
        {
            var expectedCount = Context.Country.Count();
            var results = Target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedCount, results.Count());
        }
        [Test]

        public void GetByIdTest()
        {
            var firstRawId = Context.Country.FirstOrDefault().Id;
            var result = Target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == firstRawId);
        }
        [Test]

        public void RemoveSingleModelTest()
        {
            Country country = Context.Country.FirstOrDefault();
            int expected = Context.Country.Count() - 1;
            Target.Remove(country);
            Target.Complete();
            int actual = Context.Country.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void RemoveRangeTest()
        {
            IList<Country> countryList = Target.GetAll().ToList();
            Target.RemoveRange(countryList);
            Target.Complete();
            var _count = Context.Country.Count();
            Assert.AreEqual(_count, 0);
        }
        [Test]

        public void AddSingleModelTest()
        {
            var expected = Context.Country.Count() + 1;
            //var country = new Country() {Title=Guid.NewGuid().ToString()};
            var country = Builder<Country>.CreateNew().With(c => c.Title = Faker.Address.Country()).Build();
            Target.Add(country);
            Target.Complete();
            var actual = Context.Country.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void AddRangeTest()
        {
            var countries = Builder<Country>.CreateListOfSize(100).All().With(c => c.Title = Faker.Address.Country()).Build();
            var expected = Context.Country.Count() + countries.Count;
            Target.AddRange(countries);
            Target.Complete();
            var actual = Context.Country.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void FindTest()
        {
            var param = Context.Country.FirstOrDefault();
            IEnumerable<Country> _foundItem = Target.Find(x => x.Id == param.Id);
            foreach (var item in _foundItem)
            {
                Assert.AreEqual(param.Id, item.Id);
            }
        }
    }
}
