using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using Mfr.Services.Repositories.ProductPicture;
using NUnit.Framework;
namespace Mfr.Test
{[TestFixture]
    class ProductPictureTest:IGenericTest<Core.Model.ProductPicture>
    {
        public ApplicationDbContext Context { get; set; }
        public IRepository<ProductPicture> Target { get; set; }
#pragma warning disable 618
        [TestFixtureSetUp]
#pragma warning restore 618
        public void InitializeTest()
        {
            Context = new ApplicationDbContext();
            Target = new ProductPictureRepository(Context);
        }
        [Test]
        public void GetAllTest()
        {
            var expectedCount = Context.ProductPicture.Count();
            var results = Target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedCount, results.Count());
        }
        [Test]
        public void GetByIdTest()
        {
            var firstRawId = Context.ProductPicture.FirstOrDefault().Id;
            var result = Target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == firstRawId);
        }
        [Test]
        public void RemoveSingleModelTest()
        {
            ProductPicture productPicture = Context.ProductPicture.FirstOrDefault();
            int expected = Context.ProductPicture.Count() - 1;
            Target.Remove(productPicture);
            Target.Complete();
            int actual = Context.ProductPicture.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void RemoveRangeTest()
        {
            IList<ProductPicture> productPictureList = Target.GetAll().ToList();
            Target.RemoveRange(productPictureList);
            Target.Complete();
            var _count = Context.ProductPicture.Count();
            Assert.AreEqual(_count, 0);
        }
        [Test]
        public void AddSingleModelTest()
        {
            int productId = Context.Product.FirstOrDefault().Id;
            var expected = Context.ProductPicture.Count() + 1;
            var productPicture =
                Builder<ProductPicture>.CreateNew()
                    .With(c => c.PictureUrl = Faker.Lorem.Sentence())
                    .With(c => c.ProductId = productId)
                    .Build();
                //new ProductPicture() { ProductId=productId,PictureUrl=Guid.NewGuid().ToString()};
            Target.Add(productPicture);
            Target.Complete();
            var actual = Context.ProductPicture.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void AddRangeTest()
        {
            int productId = Context.Product.FirstOrDefault().Id;
          IList<ProductPicture> productPictures=  Builder<ProductPicture>.CreateListOfSize(100).All()
                .With(c => c.PictureUrl = Faker.Lorem.Sentence())
                .With(c => c.ProductId = productId)
                .Build();
            var expected = Context.ProductPicture.Count() + productPictures.Count;
            Target.AddRange(productPictures);
            Target.Complete();
            var actual = Context.ProductPicture.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindTest()
        {
            var param = Context.ProductPicture.FirstOrDefault();
            IEnumerable<ProductPicture> _foundItem = Target.Find(x => x.Id == param.Id);
            foreach (var item in _foundItem)
            {
                Assert.AreEqual(param.Id, item.Id);
            }
        }
    }
}
