using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorViewCrude.Data;
using RazorViewCrude.Models;
using RazorViewCrude.ViewModel;

namespace RazorViewCrude.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _db.Products;
            return View(objProductList);
        }
        //GEt
        public IActionResult Create()
        {
            ProductViewModel model = new ProductViewModel();
            var categoryData = _db.Categories;
            foreach (var data in categoryData)
            {
                model.CategoryList.Add(new SelectListItem
                {
                    Value = data.CategoryId.ToString(),
                    Text = data.Name
                });
            }
            return View(model);
        }
        //POST
        [HttpPost]
        public IActionResult Create(ProductViewModel model)
        {
            if (model.ProductName == model.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                Product Product = new Product()
                {
                    CategoryId = Convert.ToInt32(model.CategoryId),
                    ProductName = model.ProductName,
                    ProductId = model.ProductId,
                    UnitPrice = model.UnitPrice,
                    DisplayOrder = model.DisplayOrder
                };
                {
                    _db.Products.Add(Product);
                    _db.SaveChanges();
                }
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            return View(model);
        }
        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ProductFromDb = _db.Products.Find(id);

            if (ProductFromDb != null)
            {
                ProductViewModel model = new ProductViewModel();
                var categoryData = _db.Categories;
                foreach (var data in categoryData)
                {
                    model.CategoryList.Add(new SelectListItem
                    {
                        Value = data.CategoryId.ToString(),
                        Text = data.Name
                    });
                }
                model.CategoryId = ProductFromDb.CategoryId;
                model.ProductId = ProductFromDb.ProductId;
                model.ProductName = ProductFromDb.ProductName;
                model.UnitPrice = ProductFromDb.UnitPrice;
                model.DisplayOrder = ProductFromDb.DisplayOrder;

                return View(model);
            }
            else
            {
                return NotFound();
            }

        }
        //POST
        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            var ProductDetails = _db.Products.Find(model.ProductId);
            if (ProductDetails != null)
            {

                if (model.ProductName == model.DisplayOrder.ToString())
                {
                    ModelState.AddModelError("Name", "The display order cannot exactly match the name");
                }
                if (ModelState.IsValid)
                {
                    //Product.CategoryId = Convert.ToInt32(model.CategoryId);
                    ProductDetails.CategoryId = model.CategoryId;
                    ProductDetails.ProductName = model.ProductName;
                    ProductDetails.ProductId = Convert.ToInt32(model.ProductId);
                    ProductDetails.UnitPrice = model.UnitPrice;
                    ProductDetails.DisplayOrder = model.DisplayOrder;

                    _db.Products.Update(ProductDetails);
                    _db.SaveChanges();
                    TempData["success"] = "Product Edited Successfully";
                    return RedirectToAction("Index");
                }
                return View(ProductDetails);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            Product pr = _db.Products.Where(x => x.ProductId == id).FirstOrDefault();
            if (pr != null)
            {
                _db.Products.Remove(pr);
                _db.SaveChanges();
                TempData["success"] = "Product Deleted Successfully";
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
