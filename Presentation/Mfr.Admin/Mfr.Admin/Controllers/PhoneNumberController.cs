using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Mfr.Admin.Models.PhoneNumber;
using Mfr.Core.Model;
using Mfr.Services.Repositories.PhoneNumber;
using Microsoft.AspNet.Identity;
using PagedList;
namespace Mfr.Admin.Controllers
{
    public class PhoneNumberController : Controller
    {
        private readonly IPhoneNumberRepository _phoneNumberRepository;


        public PhoneNumberController(IPhoneNumberRepository phoneNumberRepository)
        {
            _phoneNumberRepository = phoneNumberRepository;
        }
        // GET: PhoneNumber
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PhoneNumberList(DataSourceRequest command)
        {
            var phoneNumbers = _phoneNumberRepository.GetAll();
            var phoneNumbersList = phoneNumbers as IList<PhoneNumber> ?? phoneNumbers.ToList();
            var phoneNumberModel = new PagedList<PhoneNumber>(phoneNumbersList.ToList(),
                command.Page, command.PageSize);

            var phoneNumberViewModel = phoneNumberModel.Select(item => new PhoneNumberViewModel()
            {
                Id = item.Id,
                Number = item.Number,
                UserApplicationId = item.UserId,
                UserId=item.UserId
            }).ToList();

            var dataSourceResult = new DataSourceResult()
            {
                Data = phoneNumberViewModel,
                Total = phoneNumberModel.TotalItemCount
            };

            return Json(dataSourceResult);
        }
        [HttpPost]
        
            public ActionResult PhoneNumberInsert(PhoneNumberViewModel model)
        {
            if (model==null)throw new ArgumentNullException(nameof(model));
            var phoneNumberModel = new PhoneNumber()
            {
                Number =model.Number,
                 UserId=User.Identity.GetUserId<int>()
            };

            _phoneNumberRepository.Add(phoneNumberModel);
            _phoneNumberRepository.Complete();

            return Json("");
        }
        [HttpPost]
        public ActionResult PhonenumberUpdate(PhoneNumberViewModel phoneNumber)
        {
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            var phoneNumberModel = _phoneNumberRepository.Get(phoneNumber.Id);

            if (phoneNumberModel == null)
                throw new ArgumentNullException();

            phoneNumberModel.Number = phoneNumber.Number;
          //  phoneNumberModel.UserApplicationId = phoneNumber.UserApplicationId;

            _phoneNumberRepository.Update(phoneNumberModel);
            _phoneNumberRepository.Complete();

            return Json("");
        }
        [HttpPost]
        public ActionResult PhoneNumberDelete(int id)
        {
            if (id == 0)
                throw new AggregateException("bad request");

            var phoneNumber = _phoneNumberRepository.Get(id);

            if (phoneNumber == null)
                throw new ArgumentNullException();

            _phoneNumberRepository.Remove(phoneNumber);
            _phoneNumberRepository.Complete();

            return Json("");
        }


    }
}