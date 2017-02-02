using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mfr.Services.Repositories.State;
using Mfr.Core.Model;
using Kendo.Mvc.UI;
using Mfr.Admin.Models.State;
using PagedList;

namespace Mfr.Admin.Controllers
{

    public class StateController : Controller
    {

        private readonly IStateRepository _stateRepository;

        public StateController(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public ActionResult List()
        {
            return View();
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public ActionResult StateList(DataSourceRequest command)
        {
            var states = _stateRepository.GetAll();
            var statesList = states as IList<State> ?? states.ToList();
            var statesModel = new PagedList<State>(statesList.ToList(),
                command.Page, command.PageSize);

            var countriesViewModel = statesModel.Select(item => new StateViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Show = item.Show,
                CountryId = item.CountryId,
                CountryName = item.Country.Title
            }).ToList();

            var dataSourceResult = new DataSourceResult()
            {
                Data = countriesViewModel,
                Total = statesModel.TotalItemCount
            };

            return Json(dataSourceResult);
        }

        [HttpPost]
        public ActionResult StateInsert(StateViewModel state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            var countryModel = new State()
            {
                Title = state.Title,
                Show = state.Show,
                CountryId = state.CountryId
            };

            _stateRepository.Add(countryModel);
            _stateRepository.Complete();

            return Json("");
        }

        [HttpPost]
        public ActionResult StateUpdate(StateViewModel state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            var stateModel = _stateRepository.Get(state.Id);

            if (stateModel == null)
                throw new ArgumentNullException();

            stateModel.Title = state.Title;
            stateModel.Show = state.Show;
            stateModel.CountryId = state.CountryId;

            _stateRepository.Update(stateModel);
            _stateRepository.Complete();

            return Json("");
        }

        [HttpPost]
        public ActionResult StateDelete(int id)
        {
            if (id == 0)
                throw new AggregateException("bad request");

            var country = _stateRepository.Get(id);

            if (country == null)
                throw new ArgumentNullException();

            _stateRepository.Remove(country);
            _stateRepository.Complete();

            return Json("");
        }

        public JsonResult StateName(int id)
        {
            var state = _stateRepository.Get(id);
            return state==null ? Json(0) : Json(state.Title);
        }

        [HttpPost]
        public ActionResult GetStatesByCountryId(int id)
        {
            var state = _stateRepository.GetByCountryId(id);

            return Json(new SelectList(state.ToArray(), "Id", "Title"),
                JsonRequestBehavior.AllowGet);
        }

        public IList<State> ReturnFunctional(int countryId)
        {
            return _stateRepository.GetAll().Where(m => m.CountryId == countryId).ToList();
        }
    }
}