using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using Mfr.Services.Repositories.EMail;
using NUnit.Framework;
using FizzWare.NBuilder;
namespace Mfr.Test
{[TestFixture]
    class EmailTest:IGenericTest<Core.Model.EMail>
    {
        public ApplicationDbContext Context { get; set; }
        public IRepository<EMail> Target { get; set; }
#pragma warning disable 618
        [TestFixtureSetUp]
#pragma warning restore 618
        public void InitializeTest()
        {
            Context = new ApplicationDbContext();
            Target = new EmailRepository(Context);
        }
        [Test]
        public void GetAllTest()
        {
            var expectedCount = Context.Email.Count();
            var results = Target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedCount, results.Count());
        }
        [Test]

        public void GetByIdTest()
        {
            var firstRawId = Context.Email.FirstOrDefault().Id;
            var result = Target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == firstRawId);
        }
        [Test]

        public void RemoveSingleModelTest()
        {
            EMail email = Context.Email.FirstOrDefault();
            int expected = Context.Email.Count() - 1;
            Target.Remove(email);
            Target.Complete();
            int actual = Context.Email.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void RemoveRangeTest()
        {
            IList<EMail> emailList = Target.GetAll().ToList();
            Target.RemoveRange(emailList);
            Target.Complete();
            var _count = Context.Email.Count();
            Assert.AreEqual(_count, 0);
        }
        [Test]

        public void AddSingleModelTest()
        {
            var expected = Context.Email.Count() + 1;
            var email = Builder<EMail>.CreateNew().With(c=>c.Email=Faker.Internet.Email()).Build();
            //new EMail() {Email=Guid.NewGuid().ToString()};
            Target.Add(email);
            Target.Complete();
            var actual = Context.Email.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void AddRangeTest()
        {
            IList < EMail > emails= Builder<EMail>.CreateListOfSize(100).All().With(c=>c.Email=Faker.Internet.Email()).Build();

            var expected = Context.Email.Count() + emails.Count;
            Target.AddRange(emails);
            Target.Complete();
            var actual = Context.Email.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void FindTest()
        {
            var param = Context.Email.FirstOrDefault();
            IEnumerable<EMail> _foundItem = Target.Find(x => x.Id == param.Id);
            foreach (var item in _foundItem)
            {
                Assert.AreEqual(param.Id, item.Id);
            }
        }

    }
}
