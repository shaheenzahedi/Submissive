using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Mfr.Admin.Models.ProductType;
using Mfr.Core.Model;
using Mfr.Services.Repositories.ProductType;
using PagedList;
namespace Mfr.Admin.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeRepository _productTypeRepository;


        public ProductTypeController(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }
        // GET: ProductType
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ProductTypeList(DataSourceRequest command)
        {
            var productTypes = _productTypeRepository.GetAll();
            var productTypeList = productTypes as IList<ProductType> ?? productTypes.ToList();
            var productTypeModel = new PagedList<ProductType>(productTypeList.ToList(), command.Page, command.PageSize);

            var productTypeViewModel = productTypeModel.Select(item => new ProductTypeViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Show = item.Show
            }).ToList();

            var dataSourceResult = new DataSourceResult()
            {
                Data = productTypeViewModel,
                Total = productTypeModel.TotalItemCount
            };

            return Json(dataSourceResult);
        }
        [HttpPost]
        public ActionResult ProductTypeInsert(ProductTypeViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException("productType");

            var productTypeModel = new ProductType()
            {
                Title = model.Title,
                Show = model.Show
            };

            _productTypeRepository.Add(productTypeModel);
            _productTypeRepository.Complete();

            return Json("");
        }
        [HttpPost]
        public ActionResult ProductTypeUpdate(ProductTypeViewModel productType)
        {
            if (productType == null)
                throw new ArgumentNullException("productType");

            var productTypeModel = _productTypeRepository.Get(productType.Id);

            if (productTypeModel == null)
                throw new ArgumentNullException();

            productTypeModel.Title = productType.Title;
            productTypeModel.Show = productType.Show;
            _productTypeRepository.Update(productTypeModel);
            _productTypeRepository.Complete();

            return Json("");
        }
        [HttpPost]
        public ActionResult ProductTypeDelete(int id)
        {
            if (id == 0)
                throw new AggregateException("bad request");

            var productType = _productTypeRepository.Get(id);

            if (productType == null)
                throw new ArgumentNullException();

            _productTypeRepository.Remove(productType);
            _productTypeRepository.Complete();

            return Json("");
        }

    }
}