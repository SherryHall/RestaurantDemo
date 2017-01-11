using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ResturantDemo.Models;

namespace ResturantDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var menu = HttpContext.Cache["menu"] as IEnumerable<Category>;
            if (menu == null)
            {
                var db = new ApplicationDbContext();
                menu = db.Categories.Include("Menu").OrderBy(o => o.Name).ToList();
                HttpContext.Cache.Add(
                    "menu",
                    menu,
                    null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    new TimeSpan(0, 5, 0),
                    System.Web.Caching.CacheItemPriority.Default,
                    null
                    );
            }

            return View(menu);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Create()
        {
            return PartialView();
        }


        [HttpPost]
        public ActionResult Create(Category category)
        {
            var db = new ApplicationDbContext();
            HttpContext.Cache.Remove("menu");
            db.Categories.Add(category);
            db.SaveChanges();
            var menu = db.Categories.Include("Menu").OrderBy(o => o.Name).ToList();
            HttpContext.Cache.Add(
                "menu",
                menu,
                null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                new TimeSpan(0, 5, 0),
                System.Web.Caching.CacheItemPriority.Default,
                null
                );
            return RedirectToAction("Index");
        }
	

		public ActionResult CreateItem(int categoryId)
		{
			var newItem = new MenuItem();
			newItem.CategoryId = categoryId;
			return PartialView(newItem);
		}


		[HttpPost]
		public ActionResult CreateItem(MenuItem menuitem)
		{
			var db = new ApplicationDbContext();
			HttpContext.Cache.Remove("menu");
			db.MenuItems.Add(menuitem);
			db.SaveChanges();
			var menu = db.Categories.Include("Menu").OrderBy(o => o.Name).ToList();
			HttpContext.Cache.Add(
				"menu",
				menu,
				null,
				System.Web.Caching.Cache.NoAbsoluteExpiration,
				new TimeSpan(0, 5, 0),
				System.Web.Caching.CacheItemPriority.Default,
				null
				);
			return RedirectToAction("Index");
		}
	}
}