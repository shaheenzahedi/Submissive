using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Mfr.Admin.Models.City;
using Mfr.Core.Model;
using Mfr.Services.Repositories.City;
using Mfr.Services.Repositories.Country;
using Mfr.Services.Repositories.State;
using PagedList;

namespace Mfr.Admin.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;

        public CityController(ICityRepository cityRepository,
            ICountryRepository countryRepository,
            IStateRepository stateRepository)
        {
            this._cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
        }

        #region list / create / edit

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            if (id == 0)
                throw new ArgumentException("bad request");
            var city = _cityRepository.Get(id);
            if (city == null)
                throw new ArgumentException("bad request");

            var cityModel = new CityCreateOrUpdateViewModel()
            {
                Title = city.Title,

                StateId = city.StateId,
                CountryId = city.State.CountryId,
                Show = city.Show
            };
            return View("Edit", cityModel);
        }

        [HttpPost]
        public ActionResult Edit(CityCreateOrUpdateViewModel city)
        {
            if (ModelState.IsValid)
            {
                if (city == null)
                    throw new ArgumentException("bad request");
                var addressModel = _cityRepository.Get(city.Id);
                addressModel.Title = city.Title;
                addressModel.Show = city.Show;
                addressModel.StateId = city.StateId;

                _cityRepository.Update(addressModel);
                _cityRepository.Complete();

                return View("List");
            }
            ModelState.AddModelError(" ", "Problem");
            return View();
        }

        public ActionResult Create()
        {
            var cityViewModel = PrepareCreateCity();

            return View(cityViewModel);
        }

        [HttpPost]
        public ActionResult Create(CityCreateOrUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cityModel = new City()
                {
                    Title = model.Title,
                    Show = model.Show,
                    StateId = model.StateId
                };

                _cityRepository.Add(cityModel);
                _cityRepository.Complete();

                return RedirectToAction("List");
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult CityList(DataSourceRequest command)
        {
            var cities = _cityRepository.GetAll();
            var citiesList = cities as IList<City> ?? cities.ToList();
            var citiesModel = new PagedList<City>(citiesList.ToList(),
                command.Page, command.PageSize);

            var countriesViewModel = citiesModel.Select(item => new CityViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Show = item.Show,
                StateId = item.StateId,
                StateTitle = item.State.Title,
                CountryId = item.State.CountryId,
                CountryTitle = item.State.Country.Title
            }).ToList();

            var dataSourceResult = new DataSourceResult()
            {
                Data = countriesViewModel,
                Total = citiesModel.TotalItemCount
            };

            return Json(dataSourceResult);
        }

        [HttpPost]
        public ActionResult CityInsert(CityViewModel city)
        {

            if (city == null)
                throw new ArgumentNullException(nameof(city));

            var countryModel = new City()
            {
                Title = city.Title,
                Show = city.Show,
                StateId = city.StateId
            };

            _cityRepository.Add(countryModel);
            _cityRepository.Complete();

            return Json("");
        }

        [HttpPost]
        public ActionResult CityUpdate(CityViewModel city)
        {
            if (city == null)
                throw new ArgumentNullException(nameof(city));

            var cityModel = _cityRepository.Get(city.Id);

            if (cityModel == null)
                throw new ArgumentNullException();

            cityModel.Title = city.Title;
            cityModel.Show = city.Show;
            cityModel.StateId = city.StateId;

            _cityRepository.Update(cityModel);
            _cityRepository.Complete();

            return Json("");
        }

        [HttpPost]
        public ActionResult CityDelete(int id)
        {
            if (id == 0)
                throw new AggregateException("bad request");

            var country = _cityRepository.Get(id);

            if (country == null)
                throw new ArgumentNullException();

            _cityRepository.Remove(country);
            _cityRepository.Complete();

            return Json("");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CityGetAll()
        {
            var cities = _cityRepository.GetAll();

            var citiesViewModel = cities.Select(item => new CityViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Show = item.Show
            }).ToList();

            return Json(citiesViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Citylist(int id)
        {
            var city = from c in _cityRepository.GetAll()
                       where c.StateId == id
                       select c;
            return Json(new SelectList(city.ToArray(), "Id", "Title"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Ultimate

        private CityCreateOrUpdateViewModel PrepareCreateCity()
        {
            var cityViewModel = new CityCreateOrUpdateViewModel();

            // country
            var allCountries = _countryRepository.GetAll();

            cityViewModel.AvailableCountry.Add(new SelectListItem()
            {
                Text = "Please Select The Country:",
                Value = "0"
            });

            foreach (var country in allCountries)
            {
                var countrySelectListItem = new SelectListItem()
                {
                    Value = country.Id.ToString(),
                    Text = country.Title
                };
                cityViewModel.AvailableCountry.Add(countrySelectListItem);
            }

            return cityViewModel;
        }

        #endregion
    }
}