using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using Mfr.Services.Repositories.Product;
using NUnit.Framework;
namespace Mfr.Test
{[TestFixture]
    class ProductTest:IGenericTest<Core.Model.Product>
    {
        public ApplicationDbContext Context { get; set; }
        public IRepository<Product> Target { get; set; }
#pragma warning disable 618
        [TestFixtureSetUp]
#pragma warning restore 618
        public void InitializeTest()
        {
            Context = new ApplicationDbContext();
            Target = new ProductRepository(Context);
        }
        [Test]
        public void GetAllTest()
        {
            var expectedCount = Context.Product.Count();
            var results = Target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedCount, results.Count());
        }
        [Test]

        public void GetByIdTest()
        {
            var firstRawId = Context.Product.FirstOrDefault().Id;
            var result = Target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == firstRawId);
        }
        [Test]

        public void RemoveSingleModelTest()
        {
            Product product = Context.Product.FirstOrDefault();
            int expected = Context.Product.Count() - 1;
            Target.Remove(product);
            Target.Complete();
            int actual = Context.Product.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void RemoveRangeTest()
        {
            IList<Product> productList = Target.GetAll().ToList();
            Target.RemoveRange(productList);
            Target.Complete();
            var _count = Context.Product.Count();
            Assert.AreEqual(_count, 0);
        }
        [Test]

        public void AddSingleModelTest()
        {
            int productTypeId = Context.ProductType.FirstOrDefault().Id;
            var expected = Context.Product.Count() + 1;
            var product =
                Builder<Product>.CreateNew()
                    .With(c => c.ProductTypeId = productTypeId)
                    .With(c => c.UserApplicationId = Faker.RandomNumber.Next())
                    .With(c => c.Description = Faker.Lorem.Paragraph())
                    .With(c=>c.Title=Faker.Company.Name())
                    .Build();
                // new Product() {UserApplicationId=1,ProductTypeId=productTypeId,Title=Guid.NewGuid().ToString(),Description=Guid.NewGuid().ToString()};
            Target.Add(product);
            Target.Complete();
            var actual = Context.Product.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void AddRangeTest()
        {
            int productTypeId = Context.ProductType.FirstOrDefault().Id;

            IList<Product> products = Builder<Product>.CreateListOfSize(100).All()
                    .With(c => c.ProductTypeId = productTypeId)
                    .With(c => c.UserApplicationId = Faker.RandomNumber.Next())
                    .With(c => c.Description = Faker.Lorem.Paragraph())
                    .With(c => c.Title = Faker.Company.Name())
                    .Build();
            var expected = Context.Product.Count() + products.Count;
            Target.AddRange(products);
            Target.Complete();
            var actual = Context.Product.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void FindTest()
        {
            var param = Context.Product.FirstOrDefault();
            IEnumerable<Product> _foundItem = Target.Find(x => x.Id == param.Id);
            foreach (var item in _foundItem)
            {
                Assert.AreEqual(param.Id, item.Id);
            }
        }
    }
}
