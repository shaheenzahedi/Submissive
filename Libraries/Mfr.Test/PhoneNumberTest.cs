using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using Mfr.Services.Repositories.PhoneNumber;
using NUnit.Framework;
using FizzWare.NBuilder;
namespace Mfr.Test
{[TestFixture]
    class PhoneNumberTest:IGenericTest<Core.Model.PhoneNumber>
    {
        public ApplicationDbContext Context { get; set; }
        public IRepository<PhoneNumber> Target { get; set; }
#pragma warning disable 618
        [TestFixtureSetUp]
#pragma warning restore 618
        public void InitializeTest()
        {
            Context = new ApplicationDbContext();
            Target = new PhoneNumberRepository(Context);
        }
        [Test]
        public void GetAllTest()
        {
            var expectedCount = Context.PhoneNumber.Count();
            var results = Target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedCount, results.Count());
        }
        [Test]
        public void GetByIdTest()
        {
            var firstRawId = Context.PhoneNumber.FirstOrDefault().Id;
            var result = Target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == firstRawId);
        }
        [Test]
        public void RemoveSingleModelTest()
        {
            PhoneNumber phoneNumber = Context.PhoneNumber.FirstOrDefault();
            int expected = Context.PhoneNumber.Count() - 1;
            Target.Remove(phoneNumber);
            Target.Complete();
            int actual = Context.PhoneNumber.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void RemoveRangeTest()
        {
            IList<PhoneNumber> phoneNumberList = Target.GetAll().ToList();
            Target.RemoveRange(phoneNumberList);
            Target.Complete();
            var _count = Context.PhoneNumber.Count();
            Assert.AreEqual(_count, 0);
        }
        [Test]
        public void AddSingleModelTest()
        {
            var expected = Context.PhoneNumber.Count() + 1;
            var phoneNumber =
                Builder<PhoneNumber>.CreateNew().With(c => c.Number = Faker.RandomNumber.Next().ToString()).Build(); //new PhoneNumber() { PhoneNnumber =new Random().Next()};
            
            Target.Add(phoneNumber);
            Target.Complete();
            var actual = Context.PhoneNumber.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void AddRangeTest()
        {
            //List<PhoneNumber> phoneNumbers = new List<PhoneNumber>()
            //{
            //    new PhoneNumber {PhoneNnumber =new Random().Next()},
            //    new PhoneNumber {PhoneNnumber =new Random().Next()},
            //    new PhoneNumber {PhoneNnumber =new Random().Next() }
            //};
            IList<PhoneNumber> phoneNumbers = Builder<PhoneNumber>.CreateListOfSize(100).All().With(c => c.Number = Faker.RandomNumber.Next().ToString()).Build();
            var expected = Context.PhoneNumber.Count() + phoneNumbers.Count;
            Target.AddRange(phoneNumbers);
            Target.Complete();
            var actual = Context.PhoneNumber.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindTest()
        {
            var param = Context.Email.FirstOrDefault();
            IEnumerable<PhoneNumber> _foundItem = Target.Find(x => x.Id == param.Id);
            foreach (var item in _foundItem)
            {
                Assert.AreEqual(param.Id, item.Id);
            }
        }
    }
}
