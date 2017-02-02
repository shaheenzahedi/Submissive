using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Mfr.Admin.Models.Address;
using Mfr.Admin.Models.Country;
using Mfr.Core.Model;
using Mfr.Services.Repositories.Address;
using Mfr.Services.Repositories.Country;
using Mfr.Services.Repositories.State;
using Mfr.Services.Repositories.City;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Mfr.Admin.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;

        public AddressController(IAddressRepository addressRepository,
            ICountryRepository countryRepository,IStateRepository stateRepository,
            ICityRepository cityRepository)
        {
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            var country = DependencyResolver.Current.GetService<CountryController>();
            ViewBag.CountryId = country.ReturnAll();
        }

        // GET: Address
        public ActionResult List(AddressViewModel view)
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddressList(DataSourceRequest command)
        {
            var addresses = _addressRepository.GetAll();
            var addressesList = addresses as IList<Address> ?? addresses.ToList();
            var addressModel = new PagedList<Address>(addressesList.ToList(),
                command.Page, command.PageSize);

            var addressViewModel = addressModel.Select(item => new CreateViewModel()
            {
                Id = item.Id,
                CityId = item.CityId,
                StateId = item.StateId,
                CountryId = item.CountryId,
                Description = item.Description,
                UserApplicationId = item.UserId,
                CityName = item.City.Title,
                StateName=item.State.Title,
                CountryName=item.Country.Title
            }).ToList();

            var dataSourceResult = new DataSourceResult()
            {
                Data = addressViewModel,
                Total = addressModel.TotalItemCount
            };

            return Json(dataSourceResult);
        }
        
       [HttpPost]
        public ActionResult AddressInsert(AddressViewModel address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            var addressModel = new Address()
            {
                Description = address.Description,
                CityId = address.CityId,
                StateId = address.StateId,
                CountryId = address.CountryId,
                UserId = User.Identity.GetUserId<int>()
            };

            _addressRepository.Add(addressModel);
            _addressRepository.Complete();

            return Json("");
        }

        [HttpPost]
        public ActionResult AddressUpdate(AddressViewModel address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            var addressModel = _addressRepository.Get(address.Id);

            if (addressModel == null)
                throw new ArgumentNullException();

            addressModel.CityId = address.CityId;
            addressModel.Description = address.Description;
            addressModel.StateId = address.StateId;
            addressModel.CountryId = address.CountryId;

            _addressRepository.Update(addressModel);
            _addressRepository.Complete();

            return Json("");
        }

        public ActionResult AddressDelete(int id)
        {
            if (id == 0)
                throw new ArgumentException("bad request");

            var address = _addressRepository.Get(id);

            if (address == null)
                throw new ArgumentNullException();

            _addressRepository.Remove(address);
            _addressRepository.Complete();

            return Json("");
        }

        // [AcceptVerbs(HttpVerbs.Get)]
        //Get:CreateOrUpdate
        //public ActionResult CreateOrUpdate()
        //{
        //    var addressViewModel = AddCountriesToModel();

        //    return View(addressViewModel);

        //}
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Update(int id)
        {
            if (id == 0)
                throw new ArgumentException("bad request");
            var address = _addressRepository.Get(id);
                if (address == null)
                throw new ArgumentException("bad request");

            var addressModel = new CreateViewModel()
            {
                Description = address.Description,
                CityId = address.CityId,
                StateId = address.StateId,
                CountryId = address.CountryId,
                UserApplicationId = User.Identity.GetUserId<int>()
            };
            return View(addressModel);
        }

        [HttpPost]
        public ActionResult Update(CreateViewModel address)
        {
            if (ModelState.IsValid)
            {
                if (address == null)
                    throw new ArgumentException("bad request");
                var addressModel = _addressRepository.Get(address.Id);
                addressModel.CityId = address.CityId;
                addressModel.Description = address.Description;
                addressModel.StateId = address.StateId;
                addressModel.CountryId = address.CountryId;

                _addressRepository.Update(addressModel);
                _addressRepository.Complete();

                return View("List");
            }
            ModelState.AddModelError(" ", "Problem");
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateViewModel address)
        {
            if (ModelState.IsValid)
            {
                if (address == null)
                    throw new ArgumentException("bad request");

                var addressModel = new Address()
                {
                    Description = address.Description,
                    CityId = address.CityId,
                    StateId = address.StateId,
                    CountryId = address.CountryId,
                    UserId = User.Identity.GetUserId<int>()
                };

                _addressRepository.Add(addressModel);
                _addressRepository.Complete();

                return View("List");
            }
            ModelState.AddModelError(" ", "Problem");
            return View();
        }

        public JsonResult GetCascadeCountries()
        {
            var countries = _countryRepository.GetAll();

            return Json(countries.Select(c => new CountryViewModel {  Id = c.Id, Title = c.Title }),
                            JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCascadeStates(int? state)
        {

            var states = _stateRepository.GetAll();

            if (state != null)
            {
                states = states.Where(p => p.CountryId == state);
            }
            return Json(states.Select(p => new { stateId = p.Id, stateTitle = p.Title }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCascadeCities(int? city)
        {
        
            var cities = _cityRepository.GetAll();

            if (city != null)
            {
                cities = cities.Where(o => o.StateId == city);
            }

            return Json(cities.Select(o => new { cityId = o.Id, cityTitle = o.Title }),
                JsonRequestBehavior.AllowGet);
        }
    
        //Get: CreateOrUpdate
        //public ActionResult CreateOrUpdate(int id)
        //{
        //    if (id == 0)
        //        throw new ArgumentException("bad request");
        //    var address = _addressRepository.Get(id);
        //    if (address == null)
        //        throw new ArgumentException("bad request");
        //    var addressViewModel = new AddressViewModel
        //    {
        //        Id = address.Id,
        //        CityId = address.CityId,
        //        StateId = address.StateId,
        //        CountryId = address.CountryId,
        //        Description = address.Description,
        //        UserApplicationId = address.UserApplicationId
        //    };
        //    return View(addressViewModel);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Post: Address/CreateOrUpdate
        public ActionResult CreateOrUpdate(AddressViewModel address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            var addressModel = new Address()
            {
                Description = address.Description,
                CityId = address.CityId,
                StateId = address.StateId,
                CountryId = address.CountryId,
                UserId = User.Identity.GetUserId<int>()
            };

            _addressRepository.Add(addressModel);
            _addressRepository.Complete();

            return Json("");

        }

    }
}