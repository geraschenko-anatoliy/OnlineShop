using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Concrete;
using System.Web.Routing;
using OnlineShop.Domain.Abstract;

namespace OnlineShop.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IUnitOfWork unitOfWork;
        private IRepository<Category> categoryRepository;
        private IRepository<Product> productRepository;

        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            categoryRepository = unitOfWork.CategoryRepository;
            productRepository = unitOfWork.ProductRepository;
        }

        public ActionResult Details(int id = 0)
        {
            Product product = productRepository.GetById(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        public ActionResult Create()
        {
            PopulateCategoriesDropDownList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, string selectedCategoryId)
        {
            if (ModelState.IsValid)
            {
                productRepository.Insert(product);
                unitOfWork.Save();
                string[] productCategories = {selectedCategoryId};
                UpdateProductCategories(productCategories, product);
                unitOfWork.Save();
                return RedirectToAction("List");
            }
            return View(product);
        }
        public ActionResult Edit(int id = 0)
        {
            Product product = productRepository.Get()
                .Include(i => i.Categories)
                .Where(i => i.ProductId == id)
                .Single();

            if (product == null)
            {
                return HttpNotFound();
            }

            PopulateAssignedProductCategories(product);

            return View(product);
        }

        private void PopulateCategoriesDropDownList()
        {
            List<SelectListItem> categories = new List<SelectListItem>();

            foreach (var item in categoryRepository.Get())
            {
                categories.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.CategoryID.ToString(),
                    Selected = false
                });
            }

            ViewBag.AllCategories = categories;
        }

        private void PopulateAssignedProductCategories(Product product)
        {
            List<SelectListItem> allCategories = new List<SelectListItem>();
            List<SelectListItem> selectedCategories = new List<SelectListItem>();

            foreach (var item in categoryRepository.Get())
            {
                allCategories.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.CategoryID.ToString(),
                    Selected = false
                });
            }

            foreach (var item in product.Categories.Select(c => c.CategoryID))
            {
                Category ct = categoryRepository.GetById(item);

                selectedCategories.Add(new SelectListItem
                {
                    Text = ct.Name,
                    Value = ct.CategoryID.ToString(),
                    Selected = false
                });
            }

            ViewBag.AllCategories = allCategories;
            ViewBag.SelectedCategories = selectedCategories;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string[] selectedCategories)
        {
            Product productToUpdate = productRepository.Get()
                                .Include(i => i.Categories)
                                .Where(i => i.ProductId == id)
                                .Single();

            if (TryUpdateModel(productToUpdate))
            {
                try
                {
                    UpdateProductCategories(selectedCategories, productToUpdate);
                    productRepository.Update(productToUpdate);
                    unitOfWork.Save();
                    return RedirectToAction("List");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            PopulateAssignedProductCategories(productToUpdate);
            
            return View(productToUpdate);
        }
        private void UpdateProductCategories(string[] selectedCategories, Product productToUpdate)
        {
            if (selectedCategories == null)
            {
                productToUpdate.Categories = new List<Category>();
                return;
            }
            
            if (productToUpdate.Categories == null)
            {
                productToUpdate.Categories = new List<Category>();
            }

            var selectedCategoryHS = new HashSet<string>(selectedCategories);

            HashSet<int> productCategories = new HashSet<int>();
            if (productToUpdate.Categories != null)
                productCategories = new HashSet<int>(productToUpdate.Categories.Select(c => c.CategoryID));

            foreach (var course in categoryRepository.Get())
            {
                if (selectedCategoryHS.Contains(course.CategoryID.ToString()))
                {
                    if (!productCategories.Contains(course.CategoryID))
                    {
                        productToUpdate.Categories.Add(course);
                    }
                }
                else
                {
                    if (productCategories.Contains(course.CategoryID))
                    {
                        productToUpdate.Categories.Remove(course);
                    }
                }
            }
        }
        public ActionResult Delete(int id = 0)
        {
            Product product = productRepository.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            productRepository.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("List");
        }
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
        public ViewResult Index(string sortOrder, string currentProductFilter, string searchProduct,  ICollection<int> selectedCategories, int? page, bool selectedAllCategories = false)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.selectedCategories = selectedCategories;

            if (searchProduct != null)
            {
                page = 1;      
            }
            else
            {
                searchProduct = currentProductFilter;
            }

            Session["currentSearchProduct"] = searchProduct;
            Session["currentSelectedCategories"] = selectedCategories;
            Session["currentSelectedAllCategories"] = selectedAllCategories;

            var products = from s in productRepository.Get()
                           select s;

            if (!selectedAllCategories)
                if (selectedCategories != null)
                {
                    products = (from s in productRepository.Get()
                                from v in s.Categories
                                from c in selectedCategories
                                where v.CategoryID == c
                                select s).Distinct();
                }

            if (!String.IsNullOrEmpty(searchProduct))
            {
                products = products.Where(s => s.Name.ToUpper().Contains(searchProduct.ToUpper()));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "Price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                case "Price":
                    products = products.OrderBy(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(products.ToPagedList(pageNumber, pageSize));
        }

        public ViewResult List()
        {
            return View(productRepository.Get());
        }
    }
}