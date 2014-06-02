using OnlineShop.Domain.Abstract;
using OnlineShop.Domain.Concrete;
using OnlineShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IRepository<Category> categoryRepository;
        private IUnitOfWork unitOfWork;
        public NavController(IUnitOfWork unitOfwork)
        {
            this.unitOfWork = unitOfwork;
            categoryRepository = unitOfWork.CategoryRepository;
        }


        public PartialViewResult Menu()
        {
            ViewBag.searchProduct = Session["currentSearchProduct"] as string;

            if (Session["currentSelectedAllCategories"] != null)
                ViewBag.selectedAllCategories = Session["currentSelectedAllCategories"];
            else
                ViewBag.selectedAllCategories = false;

            if (Session["currentSelectedCategories"] != null)
                ViewBag.selectedCategories = Session["currentSelectedCategories"];
            else
            {
                HashSet<int> emptyList = new HashSet<int>();
                emptyList.Add(-1);
                ViewBag.selectedCategories = emptyList;
            }

            return PartialView(categoryRepository.Get());
        }
    }
}
