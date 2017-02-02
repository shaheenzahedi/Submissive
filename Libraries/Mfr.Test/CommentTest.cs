using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using Mfr.Services.Repositories.Comment;
using NUnit.Framework;
namespace Mfr.Test
{[TestFixture]
    class CommentTest:IGenericTest<Core.Model.Comment>
    {
        public ApplicationDbContext Context { get; set; }
        public IRepository<Comment> Target { get; set; }
#pragma warning disable 618
        [TestFixtureSetUp]
#pragma warning restore 618
        public void InitializeTest()
        {
            Context = new ApplicationDbContext();
            Target = new CommentRepository(Context);
        }
        [Test]
        public void GetAllTest()
        {
            var expectedCount = Context.Comment.Count();
            var results = Target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.AreEqual(expectedCount, results.Count());
        }
        [Test]

        public void GetByIdTest()
        {
            var firstRawId = Context.Comment.FirstOrDefault().Id;
            var result = Target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == firstRawId);
        }
        [Test]

        public void RemoveSingleModelTest()
        {
            Comment comment = Context.Comment.FirstOrDefault();
            int expected = Context.Comment.Count() - 1;
            Target.Remove(comment);
            Target.Complete();
            int actual = Context.Comment.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void RemoveRangeTest()
        {
            IList<Comment> commentList = Target.GetAll().ToList();
            Target.RemoveRange(commentList);
            Target.Complete();
            var _count = Context.Comment.Count();
            Assert.AreEqual(_count, 0);
        }
        [Test]

        public void AddSingleModelTest()
        {
            //Product Required
            int productId = Context.Product.FirstOrDefault().Id;
            var expected = Context.Comment.Count() + 1;
            // var comment = new Comment() { ProductId = productId,UserApplicationId=1, Text = Guid.NewGuid().ToString(),Like=10,DisLike=20 };
            var comment = Builder<Comment>.CreateNew().With(c => c.Like = Faker.RandomNumber.Next())
                .With(c => c.ProductId = productId)
                .With(c => c.UserApplicationId = Faker.RandomNumber.Next())
                .With(c => c.Text = Faker.Lorem.Paragraph())
                .With(c=>c.DisLike=Faker.RandomNumber.Next())
                .Build();
                
            Target.Add(comment);
            Target.Complete();
            var actual = Context.Comment.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void AddRangeTest()
        {
            //Product Required
            int productId = Context.Product.FirstOrDefault().Id;
            List<Comment> comments = new List<Comment>()
            {
                new Comment {ProductId = productId,UserApplicationId=1, Text = Guid.NewGuid().ToString(),Like=10,DisLike=20 },
                new Comment {ProductId = productId,UserApplicationId=1, Text = Guid.NewGuid().ToString(),Like=10,DisLike=20 },
                new Comment {ProductId = productId,UserApplicationId=1, Text = Guid.NewGuid().ToString(),Like=10,DisLike=20  }
            };
            //IList<Comment> comments = Builder<Comment>.CreateListOfSize(100).All().With(c => c.Like = 1)
            //    .With(c => c.ProductId = productId)
            //    .With(c => c.UserApplicationId = 1)
            //    .With(c => c.Text = Faker.Lorem.Paragraph())
            //    .With(c => c.DisLike = 1)
            //    .Build();
            var expected = Context.Comment.Count() + comments.Count;
            Target.AddRange(comments);
            Target.Complete();
            var actual = Context.Comment.Count();
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public void FindTest()
        {
            var param = Context.Comment.FirstOrDefault();
            IEnumerable<Comment> _foundItem = Target.Find(x => x.Id == param.Id);
            foreach (var item in _foundItem)
            {
                Assert.AreEqual(param.Id, item.Id);
            }
        }
    }
}
