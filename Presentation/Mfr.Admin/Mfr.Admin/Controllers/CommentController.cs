using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Mfr.Admin.Models.City;
using Mfr.Admin.Models.Comment;
using Mfr.Core.Model;
using Mfr.Services.Repositories.Comment;
using Mfr.Services.Repositories.Product;
using Mfr.Services.Repositories.ProductPicture;
using PagedList;

namespace Mfr.Admin.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductPictureRepository _productPictureRepository;
        public CommentController(ICommentRepository commentRepository,IProductRepository productRepository,IProductPictureRepository productPictureRepository)
        {
            _commentRepository = commentRepository;
            _productRepository = productRepository;
            _productPictureRepository = productPictureRepository;
        }
        public ActionResult List()
        {
            return View();
        }
        // GET: Comment
       public ActionResult CommentList(DataSourceRequest command)
        {
            var comments = _commentRepository.GetAll();
            var commentsList = comments as IList<Comment> ?? comments.ToList();
            IList<CommentViewModel> commentViewModels = new List<CommentViewModel>();
            foreach (var item in commentsList)
            {
                var comment = new CommentViewModel();
                var productsList = _productRepository.Find(x => x.Id == item.ProductId);
                foreach (var product in productsList)
                {
                    comment.ProductName = product.Title;
                    if (_productPictureRepository.Find(x => x.ProductId == product.Id).Any())
                    {
                        comment.ProductPicture =
                         _productPictureRepository.Find(x => x.ProductId == product.Id).ElementAt(0).PictureUrl;
                    }

                }
                comment.Id = item.Id;
                comment.Text = item.Text;
                comment.Like = item.Like;
                comment.DisLike = item.DisLike;
                comment.AdminConfirm = item.AdminConfirm;
                commentViewModels.Add(comment);
            }

            return Json(commentViewModels.ToDataSourceResult(command),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CommentUpdate([DataSourceRequest] DataSourceRequest request, CommentViewModel comment)
        {
            if (comment == null)
                throw new ArgumentException("bad request");

            var commentModel = _commentRepository.Get(comment.Id);

            if (commentModel == null)
                throw new ArgumentNullException();

            commentModel.AdminConfirm = comment.AdminConfirm;

            _commentRepository.Update(commentModel);
            _commentRepository.Complete();

            return Json(new[] { comment }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult CommentDelete(int id)
        {
            if (id == 0)
                throw new ArgumentException("bad request");
            var result =_commentRepository.Get(id);
            if (result==null)
                throw new ArgumentException("bad request");
            _commentRepository.Remove(result);
            _commentRepository.Complete();
            return Json("");
        }
    }
}