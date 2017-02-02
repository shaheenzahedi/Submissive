using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using Mfr.Services.Repositories.ProductType;
using NUnit.Framework;
namespace Mfr.Test
{[TestFixture]
    class ProductTypeTest:IGenericTest<Core.Model.ProductType>
    {
        public ApplicationDbContext Context { get; set; }
        public IRepository<ProductType> Target { get; set; }
#pragma warning disable 618
        [TestFixtureSetUp]
#pragma warning restore 618
        public void InitializeTest()
        {
            Context = new ApplicationDbContext();
            Target = new ProductTypeRepository(Context);
        }
        [Test]
        public void GetAllTest()
        {
            var expectedCount = Context.ProductType.Count();
            var results = Target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedCount, results.Count());
        }
        [Test]
        public void GetByIdTest()
        {
            var firstRawId = Context.ProductType.FirstOrDefault().Id;
            var result = Target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == firstRawId);
        }
        [Test]
        public void RemoveSingleModelTest()
        {
            ProductType productType = Context.ProductType.FirstOrDefault();
            int expected = Context.ProductType.Count() - 1;
            Target.Remove(productType);
            Target.Complete();
            int actual = Context.ProductType.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void RemoveRangeTest()
        {
            IList<ProductType> productTypeList = Target.GetAll().ToList();
            Target.RemoveRange(productTypeList);
            Target.Complete();
            var _count = Context.ProductType.Count();
            Assert.AreEqual(_count, 0);
        }
        [Test]
        public void AddSingleModelTest()
        {
            var expected = Context.ProductType.Count() + 1;
            var productType = Builder<ProductType>.CreateNew().With(c => c.Title = Faker.Company.Name()).Build();
                //new ProductType() {Title=Guid.NewGuid().ToString()};
            Target.Add(productType);
            Target.Complete();
            var actual = Context.ProductType.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void AddRangeTest()
        {
   IList<ProductType> products = Builder<ProductType>.CreateListOfSize(100).All().With(c => c.Title = Faker.Company.Name()).Build();

            var expected = Context.ProductType.Count() + products.Count;
            Target.AddRange(products);
            Target.Complete();
            var actual = Context.ProductType.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindTest()
        {
            var param = Context.ProductType.FirstOrDefault();
            IEnumerable<ProductType> _foundItem = Target.Find(x => x.Id == param.Id);
            foreach (var item in _foundItem)
            {
                Assert.AreEqual(param.Id, item.Id);
            }
        }
    }
}
