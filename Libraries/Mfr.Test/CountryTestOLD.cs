using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mfr.Core.Model;
using Mfr.Services.Repositories.Country;
namespace Mfr.Test
{[TestClass]
    public class CountryTest
    {
    [TestMethod]
    public void AddSingleModelCountryTest()
    {
        var context = new ApplicationDbContext();
        var expected = context.Country.Count()+1;
        var target = new CountryRepository(context);
        var country = new Country() {Title=Guid.NewGuid().ToString()};
        target.Add(country);
        target.Complete();
        var actual = context.Country.Count();
        Assert.AreEqual(expected,actual);
    }

    [TestMethod]
    public void GetByIdCountryTest()
    {
        var contetx = new ApplicationDbContext();
        var firstRawId = contetx.Country.FirstOrDefault().Id;
        var target = new CountryRepository(contetx);
        var result = target.Get(firstRawId);
        Assert.IsNotNull(result);
        Assert.IsTrue(firstRawId==result.Id);
        }

    [TestMethod]
    public void GetAllCountryTest()
    {
        var context = new ApplicationDbContext();
        var target = new CountryRepository(context);
        var result = target.GetAll().ToList();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());
    }

    [TestMethod]
    public void UpdateCountryTest()
    {
        var context = new ApplicationDbContext();
        var country = context.Country.FirstOrDefault();
        var target = new CountryRepository(context);
        
            country.Title = Guid.NewGuid().ToString();
            target.Update(country);
        target.Complete();
            var actual = target.Get(country.Id);
            Assert.AreEqual(actual.Title,country.Title);
    }

    [TestMethod]
    public void FindCountryTets()
    {
        var context = new ApplicationDbContext();
        var target = new CountryRepository(context);
        var param = context.Country.FirstOrDefault();
        IEnumerable<Country> foundItem = target.Find(x=>x.Title==param.Title);
        foreach (var item  in foundItem)
        {
            Assert.AreEqual(param.Title,item.Title);
        }
    }
        [TestMethod]
        public void RemoveSingleModelCountryTest()
        {
            var context = new ApplicationDbContext();
            var target = new CountryRepository(context);
            var dataToRemove = context.Country.FirstOrDefault();
            int expected = context.Country.Count() - 1;
            target.Remove(dataToRemove);
            target.Complete();
            int actual = context.Country.Count();
            Assert.AreEqual(actual,expected);
        }

    [TestMethod]
    public void RemoveRangeCountryTest()
    {
        var context = new ApplicationDbContext();
        var target = new CountryRepository(context);
        IList<Country> countries= target.GetAll().ToList();
        target.RemoveRange(countries);
        target.Complete();
            var count = context.Country.Count();
            Assert.AreEqual(count, 0);//if expected equals to actual then the test would pass
        }

    [TestMethod]
    public void AddRangeCountryTest()
    {

    }

    }
}
