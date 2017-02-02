using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mfr.Core;
using Mfr.Core.Model;
using Mfr.Services;
using Moq;
using Mfr.Services.Repositories.State;

namespace Mfr.Test
{
    [TestClass]
    public class StateTest
    {
        [TestMethod]
        public void InsertUsingMoq()
        {
            var context = new ApplicationDbContext();
            Mock<IStateRepository> stateRep = new Mock<IStateRepository>();
            var target = new StateRepository(context);
            stateRep.Setup(x => x.Add(It.IsAny<State>()));
        }

        [TestMethod]
        public void AddSingleModelStateTest()
        {  
            //country table must NOT be Empty
            var context = new ApplicationDbContext();
            var expected = context.State.Count() + 1;
            var target = new StateRepository(context);
            var countryId = context.Country.FirstOrDefault().Id;//select first row from Country Table, For Country id
            var state = new State() {CountryId=countryId,Title="Rasht"};
            target.Add(state);
            target.Complete();
            var actual = context.State.Count();
            Assert.AreEqual(expected, actual);//if expected equals to actual then the test would be passed
        }

        [TestMethod]
        public void GetByIdStateTest()
        {
            //State Table Must Not be Empty
            var context = new ApplicationDbContext();
            var firstRawId = context.State.FirstOrDefault().Id;
            var target = new StateRepository(context);
            var result = target.Get(firstRawId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id== firstRawId);
        }

        [TestMethod]
        public void GetAllStateTest()
        {
            //State Table Must Not be Empty
            var context = new ApplicationDbContext();
            var expectedCount = context.State.Count();
            var target = new StateRepository(context);
            var results = target.GetAll().ToList();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void UpdateSingleModelStateTest()
        {//State Table Must Not be Empty
            var context = new ApplicationDbContext();
            var state = context.State.FirstOrDefault();
            var target = new StateRepository(context);
            state.Title = Guid.NewGuid().ToString();
            target.Update(state);
            target.Complete();
            var actual = target.Get(state.Id);
            Assert.AreEqual(state.Title, actual.Title);
            
        }

        [TestMethod]
        public void AddRangeStateTest()
        {
            //country table must NOT be Empty
            var context = new ApplicationDbContext();
            var target = new StateRepository(context);
            var countryId = context.Country.FirstOrDefault().Id;//select first row from Country Table, For Country id
            IList<State> stateList =new List<State>();
            var state1 = new State() { CountryId = countryId, Title = "Gilan" };
            var state2 = new State() {  CountryId = countryId, Title = "Tehran" };
            stateList.Add(state1);
            stateList.Add(state2);
            var expected = context.State.Count() + stateList.Count;
            target.AddRange(stateList);
            target.Complete();
            var actual = context.State.Count();
            Assert.AreEqual(expected, actual);//if expected equals to actual then the test would pass
         }

        [TestMethod]
        public void RemoveSingleModelStateTest()
        {//State Table Must Not be Empty
            var context = new ApplicationDbContext();
            State state = context.State.FirstOrDefault();
            int expected = context.State.Count() - 1;
            var target = new StateRepository(context);
            target.Remove(state);
            target.Complete();
            int actual = context.State.Count();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveRangeStateTest()
        {
            //State Table Must not Be Empty
            var context = new ApplicationDbContext();
            var target = new StateRepository(context);
            IList<State> stateList = target.GetAll().ToList();
            target.RemoveRange(stateList);
            target.Complete();
            var count = context.State.Count();
            Assert.AreEqual(count, 0);//if expected equals to actual then the test would pass
        }

        [TestMethod]
        public void FindStateTest()
        {//State Table Must not Be Empty
            var context = new ApplicationDbContext();
            var target = new StateRepository(context);
            var param= context.State.FirstOrDefault();
            IEnumerable<State> foundItem=  target.Find(x => x.Title == param.Title);
            foreach (var item in foundItem)
            {
                Assert.AreEqual(param.Title,item.Title);
            }
        }


    }
}
