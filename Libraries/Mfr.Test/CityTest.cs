using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using Mfr.Services.Repositories.City;
using NUnit.Framework;
namespace Mfr.Test
{[TestFixture]
    class CityTest:IGenericTest<Core.Model.City>
    {
        public ApplicationDbContext Context { get; set; }
        public IRepository<City> Target { get; set; }
        [TestFixtureSetUp]
        public void InitializeTest()
        {
            Context = new ApplicationDbContext();
            Target = new CityRepository(Context);
        }
        [Test]
        public void GetAllTest()
        {
            var expectedCount = Context.City.Count();
            var results = Target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedCount, results.Count());
        }
        [Test]

        public void GetByIdTest()
        {
            var firstRawId = Context.City.FirstOrDefault().Id;
            var result = Target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == firstRawId);
        }
        [Test]

        public void RemoveSingleModelTest()
        {
            City city = Context.City.FirstOrDefault();
            int expected = Context.City.Count() - 1;
            Target.Remove(city);
            Target.Complete();
            int actual = Context.City.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void RemoveRangeTest()
        {
            IList<City> cityList = Target.GetAll().ToList();
            Target.RemoveRange(cityList);
            Target.Complete();
            var _count = Context.City.Count();
            Assert.AreEqual(_count, 0);
        }
        [Test]

        public void AddSingleModelTest()
        {
            //State Required
            int stateId = Context.State.FirstOrDefault().Id;
            var expected = Context.City.Count() + 1;
            //var city = new City() { StateId=stateId,Title=Guid.NewGuid().ToString()};
            var city = Builder<City>.CreateNew().With(c => c.Title = Faker.Address.City())
                .With(c => c.StateId = stateId)
                .Build();
            Target.Add(city);
            Target.Complete();
            var actual = Context.City.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void AddRangeTest()
        {
            //State Required
            int stateId = Context.State.FirstOrDefault().Id;
            //List<City> cities = new List<City>()
            //{
            //    new City {StateId=stateId,Title=Guid.NewGuid().ToString()},
            //    new City {StateId=stateId,Title=Guid.NewGuid().ToString()},
            //    new City {StateId=stateId,Title=Guid.NewGuid().ToString() }
            //};
            IList<City> cities= Builder<City>.CreateListOfSize(100).All()
                .With(c => c.Title = Faker.Address.City())
                .With(c => c.StateId = stateId)
    .Build();
            var expected = Context.City.Count() + cities.Count;
            Target.AddRange(cities);
            Target.Complete();
            var actual = Context.City.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void FindTest()
        {
            var param = Context.City.LastOrDefault();
            IEnumerable<City> _foundItem = Target.Find(x => x.Id == param.Id);
            foreach (var item in _foundItem)
            {
                Assert.AreEqual(param.Id, item.Id);
            }
        }
    }
}
