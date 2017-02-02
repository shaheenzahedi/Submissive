using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using NUnit.Framework;
using Mfr.Services.Repositories.State;
namespace Mfr.Test
{
    [TestFixture]
    class StateTest:IGenericTest<Core.Model.State>
    {
        public ApplicationDbContext Context { get; set; }
        public IRepository<State> Target { get; set; }
#pragma warning disable 618
       [TestFixtureSetUp]
#pragma warning restore 618
        public void InitializeTest()
        {
            Context = new ApplicationDbContext();
            Target = new StateRepository(Context);
        }
[Test]
        public void GetAllTest()
        {
            var expectedCount = Context.State.Count();
            var results = Target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedCount, results.Count());
        }
        [Test]
        public void GetByIdTest()
        {
            var firstRawId = Context.State.FirstOrDefault().Id;
            var result = Target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == firstRawId);
        }
        [Test]
        public void RemoveSingleModelTest()
        {
            State state = Context.State.FirstOrDefault();
            int expected = Context.State.Count() - 1;
            Target.Remove(state);
            Target.Complete();
            int actual = Context.State.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void RemoveRangeTest()
        {
            IList<State> stateList = Target.GetAll().ToList();
            Target.RemoveRange(stateList);
            Target.Complete();
            var _count = Context.State.Count();
            Assert.AreEqual(_count, 0);
        }
        [Test]
        public void AddSingleModelTest()
        {
            int countryId = Context.Country.FirstOrDefault().Id;
            var expected = Context.State.Count() + 1;
            var state =
                Builder<State>.CreateNew()
                    .With(c => c.CountryId = countryId)
                    .With(c => c.Title = Faker.Address.UsState())
                    .Build();
            Target.Add(state);
            Target.Complete();
            var actual = Context.State.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void AddRangeTest()
        {
            int countryId = Context.Country.FirstOrDefault().Id;
            IList<State> states=Builder<State>.CreateListOfSize(100).All().With(c => c.CountryId = countryId)
                    .With(c => c.Title = Faker.Address.UsState())
                    .Build();

            var expected = Context.State.Count() + states.Count;
            Target.AddRange(states);
            Target.Complete();
            var actual = Context.State.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindTest()
        {
            var param = Context.State.FirstOrDefault();
            IEnumerable<State> _foundItem = Target.Find(x => x.Id == param.Id);
            foreach (var item in _foundItem)
            {
                Assert.AreEqual(param.Id, item.Id);
            }
        }
    }
}
