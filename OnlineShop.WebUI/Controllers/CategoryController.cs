using System;
using System.Collections.Generic;
using System.Data;
using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Concrete;
using OnlineShop.Domain.Abstract;

namespace OnlineShop.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private IRepository<Category> categoryRepository;
        private IRepository<Product> productRepository;
        private IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            categoryRepository = unitOfWork.CategoryRepository;
            productRepository = unitOfWork.ProductRepository;
        }

        public ActionResult Index()
        {
            return View(unitOfWork.CategoryRepository.Get());
        }

        public ActionResult Details(int id = 0)
        {
            Category category = categoryRepository.GetById(id);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        private void PopulateAssignedCategoryProducts(Category category)
        {
            List<SelectListItem> allProducts = new List<SelectListItem>();
            List<SelectListItem> selectedProducts = new List<SelectListItem>();

            foreach (var item in productRepository.Get())
            {
                allProducts.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ProductId.ToString(),
                    Selected = false
                });
            }

            foreach (var item in category.Products.Select(c=> c.ProductId))
            {
                Product pr = productRepository.GetById(item);

                selectedProducts.Add(new SelectListItem
                {
                    Text = pr.Name,
                    Value = pr.ProductId.ToString(),
                    Selected = false
                });
            }

            ViewBag.AllProducts = allProducts;
            ViewBag.SelectedProducts = selectedProducts;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category, string[] selectedProducts)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Insert(category);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int id = 0)
        {
            Category category = categoryRepository.Get()
                .Include(i => i.Products)
                .Where(i => i.CategoryID == id)
                .Single();

            PopulateAssignedCategoryProducts(category);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string[] selectedProducts)
        {
            Category categoryToUpdate = categoryRepository.Get()
                                .Include(i => i.Products)
                                .Where(i => i.CategoryID == id)
                                .Single();

            if (TryUpdateModel(categoryToUpdate))
            {
                try
                {
                    UpdateCategoryProducts(selectedProducts, categoryToUpdate);
                    unitOfWork.Save();;
                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            PopulateAssignedCategoryProducts(categoryToUpdate);
            return View(categoryToUpdate);
        }

        private void UpdateCategoryProducts(string[] selectedProducts, Category categoryToUpdate)
        {
            if (selectedProducts == null)
            {
                categoryToUpdate.Products = new List<Product>();
                return;
            }
            
            if (categoryToUpdate.Products == null)
            {
                categoryToUpdate.Products = new List<Product>();
            }

            var selectedProductHS = new HashSet<string>(selectedProducts);

            HashSet<int> categoryProducts = new HashSet<int>();
            categoryProducts = new HashSet<int>(categoryToUpdate.Products.Select(c => c.ProductId));

            foreach (var product in productRepository.Get())
            {
                if (selectedProductHS.Contains(product.ProductId.ToString()))
                {
                    if (!categoryProducts.Contains(product.ProductId))
                    {
                        categoryToUpdate.Products.Add(product);
                    }
                }
                else
                {
                    if (categoryProducts.Contains(product.ProductId))
                    {
                        categoryToUpdate.Products.Remove(product);
                    }
                }
            }
        }

        public ActionResult Delete(int id = 0)
        {
            Category category = categoryRepository.GetById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            categoryRepository.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}