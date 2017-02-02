using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Mfr.Admin.Models.Country;
using Mfr.Core.Model;
using Mfr.Services.Repositories.Country;
using Mfr.Services.Repositories.State;
using PagedList;

namespace Mfr.Admin.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;

        public CountryController(ICountryRepository countryRepository, 
            IStateRepository stateRepository)
        {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
        }


        public ActionResult List()
        {
            return View();
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public ActionResult CountryList(DataSourceRequest command)
        {
            var countries = _countryRepository.GetAll();
            var countriesList = countries as IList<Country> ?? countries.ToList();
            var countryModel = new PagedList<Country>(countriesList.ToList(), 
                command.Page, command.PageSize);

            var countriesViewModel = countryModel.Select(item => new CountryViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Show = item.Show
            }).ToList();

            var dataSourceResult = new DataSourceResult()
            {
                Data = countriesViewModel,
                Total = countryModel.TotalItemCount
            };

            return Json(dataSourceResult);
        }

        public ActionResult CountryGetAll()
        {
            var countries = _countryRepository.GetAll();

            var countriesViewModel = countries.Select(item => new CountryViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Show = item.Show
            }).ToList();

            return Json(countriesViewModel);
        }

        [HttpPost]
        public ActionResult CountryInsert(CountryViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var countryModel = new Country()
            {
                Title = model.Title,
                Show = model.Show
            };

            _countryRepository.Add(countryModel);
            _countryRepository.Complete();

            return Json("");
        }

        [HttpPost]
        public ActionResult CountryUpdate(CountryViewModel country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            var countryModel = _countryRepository.Get(country.Id);

            if (countryModel == null)
                throw new ArgumentNullException();

            countryModel.Title = country.Title;
            countryModel.Show = country.Show;

            _countryRepository.Update(countryModel);
            _countryRepository.Complete();

            return Json("");
        }

        [HttpPost]
        public ActionResult CountryDelete(int id)
        {
            if (id == 0)
                throw new ArgumentException("bad request");

            var country = _countryRepository.Get(id);

            if (country == null)
                throw new ArgumentNullException();

            _countryRepository.Remove(country);
            _countryRepository.Complete();

            return Json("");
        }
        [HttpPost]
        public ActionResult GetCountryById(int id)
        {
            if (id == 0)
                throw new AggregateException("bad request");
            var country = _countryRepository.Get(id);

            if (country == null)
                throw new ArgumentNullException();
            return Json(country);
		}
        public ActionResult GetStatesByCountryId(int countryId)
        {
            if (countryId == 0)
                throw new ArgumentException("bad request");
            var country = _countryRepository.Get(countryId);
            var states = country != null ? _stateRepository.GetStateByCountryId(country.Id, showHidden: true).ToList() : new List<State>();
            var result = (from s in states
                          select new { id = s.Id, name = s.Title }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public SelectList ReturnAll()
        {
            var countries = _countryRepository.GetAll();
            if (countries == null)
                throw new AggregateException("bad request");
            SelectList list = new SelectList(countries.ToArray(), "Id", "Title");
            return list;
        }

    }
}