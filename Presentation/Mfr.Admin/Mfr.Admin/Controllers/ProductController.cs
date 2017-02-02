using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Mfr.Admin.Models.Product;
using Mfr.Admin.Models.ProductType;
using Mfr.Core.Model;
using Mfr.Services.Repositories.Product;
using Mfr.Services.Repositories.ProductType;
using Mfr.Services.Repositories.ProductPicture;
using PagedList;
using Microsoft.AspNet.Identity;

namespace Mfr.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductPictureRepository _productPictureRepository;

        public ProductController(IProductRepository productRepository, 
            IProductTypeRepository productTypeRepository, 
            IProductPictureRepository productPictureRepository)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _productPictureRepository = productPictureRepository;
        }

        //GET: Product
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProductList(DataSourceRequest command)
        {
            
            var products = _productRepository.GetAll();
            var productsList = products as IList<Product> ?? products.ToList();
            var productModel = new PagedList<Product>(productsList.ToList(),
                command.Page, command.PageSize);

            var productViewModel = productModel.Select(item => new ProductViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                ProductTypeName = item.ProductType.Title,
                ImageUrl = item.ProductPictures != null && item.ProductPictures.Count > 0 ?
                    Path.Combine("/App_Themes/Images", item.ProductPictures.ElementAt(0).PictureUrl ) : "/content/img/Bulb.png"

            }).ToList();

            var dataSourceResult = new DataSourceResult()
            {
                Data = productViewModel,
                Total = productModel.TotalItemCount
            };

            return Json(dataSourceResult);
        }
       


        public ActionResult GetImagesByProductId(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("bad request");
            }
            var images = _productPictureRepository.Find(m => m.ProductId == id);

            var productPictures = images as IList<ProductPicture> ?? images.ToList();
            var productPictureViewModel = productPictures.Select(item => new ProductPictureViewModel()
            {
                Id = item.Id,
                ImageUrl =  Path.Combine("/App_Themes/Images" , item.PictureUrl),
                OriginalFileName = item.OriginalFileName
            }).ToList();
            var dataSourceResult = new DataSourceResult()
            {
                Data = productPictureViewModel,
                Total = productPictures.Count()
            };
            return Json(dataSourceResult);
        }

        [HttpPost]
        public ActionResult ProductDelete(int id)
        {
            if (id == 0)
                throw new ArgumentException("bad request");

            var product = _productRepository.Get(id);

            if (product == null)
                throw new ArgumentNullException();
            var image = _productPictureRepository.Find(x=>x.ProductId==id);
            foreach (var item in image)
            {
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/App_Themes/Images"), item.PictureUrl));
                _productPictureRepository.Remove(item);

            }
            _productPictureRepository.Complete();
            _productRepository.Remove(product);
            _productRepository.Complete();

            return Json("");
        }

        [HttpPost]
        public ActionResult Create(ProductViewModel product)
        {
            if (product == null)
                throw new ArgumentException("bad request");

            if (ModelState.IsValid)
            {
                var productModel = new Product()
                {
                    Description = product.Description,
                    Title=product.Title,
                    ProductTypeId = product.ProductTypeId,
                    UserId = User.Identity.GetUserId<int>()
                };
                _productRepository.Add(productModel);
                _productRepository.Complete();
                product.IsSaved = true;
                product.Id = productModel.Id;

                return RedirectToAction("Update", new { id = product.Id});
            }

            ModelState.AddModelError(" ", "Problem");

            return View();
        }
        [HttpPost]
        public ActionResult Update(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                if (product == null)
                    throw new ArgumentException("bad request");
                var productModel = _productRepository.Get(product.Id);
                productModel.Title = product.Title;
                productModel.Description = product.Description;
                productModel.ProductTypeId = product.ProductTypeId;
                _productRepository.Update(productModel);
                _productRepository.Complete();

                return View("Update",product);
            }
            ModelState.AddModelError(" ", "Problem");
            return View("Update", product);
        }
        public ActionResult Create()
        {

            return View("Create", new ProductViewModel());
        }

        public ActionResult Update(int id)
        {
            if (id == 0)
                throw new ArgumentException("bad request");

            var product = _productRepository.Get(id);
            if (product == null)
                throw new ArgumentException("bad request");

            var productModel = new ProductViewModel()
            {
                Id = product.Id,
                Description = product.Description,
                Title = product.Title,
                ProductTypeId = product.ProductTypeId
            };
            return View(productModel);
        }

        public JsonResult GetProductTypeCombo()
        {
            var Types = _productTypeRepository.GetAll();

            return Json(Types.Select(c => new ProductTypeViewModel { Id = c.Id, Title = c.Title }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Submit(IEnumerable<HttpPostedFileBase> files, int productId)
		{
                if (files != null)
                {
                    TempData["UploadedFiles"] = GetFileInfo(files);
                    foreach (var file in files)
                    {


                        var fileName = Guid.NewGuid().ToString()+".jpg";
                        var pictureModel = new ProductPicture()
                        {
                            ProductId = productId,
                            PictureUrl = fileName,
                            OriginalFileName = file.FileName
                        };

                        _productPictureRepository.Add(pictureModel);


                        var path = Path.Combine(Server.MapPath("~/App_Themes/Images"), fileName);
                        file.SaveAs(path);
                    }
                    _productPictureRepository.Complete();
                }
                else
                {
                    throw new ArgumentException("bad request");
                }
          return Json("");
          }
 
        public JsonResult Remove(string[] fileNames)
        {
            if (fileNames != null)
            {
                foreach (var name in fileNames)
                {
                    var _res = _productPictureRepository.Find(m => m.OriginalFileName == name);
                    foreach (var item in _res)
                    {
                        System.IO.File.Delete(Path.Combine(Server.MapPath("~/App_Themes/Images"), item.PictureUrl ));
                        _productPictureRepository.Remove(item);
                     
                    }
                    _productPictureRepository.Complete();
                }

            }
            else
            {
                throw new ArgumentException("bad request");
            }
            return Json("");
        }

        private IEnumerable<string> GetFileInfo(IEnumerable<HttpPostedFileBase> files)
        {
            return
                from a in files
                where a != null
                select $"{Path.GetFileName(a.FileName)} ({a.ContentLength} bytes)";
        }

        public ActionResult RemoveImagesByProductId(int id)
        {
            if (id == 0)
                throw new ArgumentException("bad request");

            var image = _productPictureRepository.Get(id);

            if (image == null)
                throw new ArgumentException("bad request");
            
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/App_Themes/Images"), image.PictureUrl ));
                _productPictureRepository.Remove(image);

           
            _productPictureRepository.Complete();

            return Json("");
        }
    }
}