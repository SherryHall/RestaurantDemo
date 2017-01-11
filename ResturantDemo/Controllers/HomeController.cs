using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ResturantDemo.Models;
using System.Net;
using System.Data.Entity;

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

		[Authorize]
		public ActionResult Create()
		{
			return PartialView();
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Category category)
		{
			var db = new ApplicationDbContext();
			HttpContext.Cache.Remove("menu");
			db.Categories.Add(category);
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		[Authorize]
		public ActionResult CreateItem(int categoryId)
		{
			var newItem = new MenuItem();
			newItem.CategoryId = categoryId;
			return PartialView(newItem);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateItem(MenuItem menuitem)
		{
			var db = new ApplicationDbContext();
			HttpContext.Cache.Remove("menu");
			db.MenuItems.Add(menuitem);
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		[Authorize]
		public ActionResult Edit(int categoryId)
		{
			//if (categoryId == null)
			//{
			//	return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			//}
			var db = new ApplicationDbContext();
			var currCategory = db.Categories.Find(categoryId);
			if (currCategory == null)
			{
				return HttpNotFound();
			}
			return PartialView(currCategory);
		}

		// POST: Categories/Edit/5
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Name")] Category category)
		{
			var db = new ApplicationDbContext();
			if (ModelState.IsValid)
			{
				HttpContext.Cache.Remove("menu");
				//db.Entry(category).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(category);
		}

		[Authorize]
		public ActionResult EditItem(int menuId)
		{
			//if (menuId == null)
			//{
			//	return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			//}
			var db = new ApplicationDbContext();
			var currMenuItem = db.MenuItems.Find(menuId);
			if (currMenuItem == null)
			{
				return HttpNotFound();
			}
			return PartialView(currMenuItem);
		}

		// POST: MenuItems/Edit/5
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditItem([Bind(Include = "Name, Description, Price, CategoryId")] MenuItem menuitem)
		{
			var db = new ApplicationDbContext();
			if (ModelState.IsValid)
			{
				HttpContext.Cache.Remove("menu");
				//db.Entry(menuitem).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(menuitem);
		}

		// GET: Categories/Delete/5
		[Authorize]
		public ActionResult Delete(int? categoryId)
		{
			if (categoryId == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var db = new ApplicationDbContext();
			var currCategory = db.Categories.Find(categoryId);
			if (currCategory == null)
			{
				return HttpNotFound();
			}
			return View(currCategory);
		}

		// POST: Categories/Delete/5
		[Authorize]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int categoryId)
		{
			var db = new ApplicationDbContext();
			HttpContext.Cache.Remove("menu");
			var currCategory = db.Categories.Find(categoryId);
			db.Categories.Remove(currCategory);
			db.SaveChanges();

			return RedirectToAction("Index");
		}
		// GET: MenuItems/Delete/5
		[Authorize]
		public ActionResult DeleteItem(int? menuId)
		{
			if (menuId == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var db = new ApplicationDbContext();
			var currMenuItem = db.MenuItems.Find(menuId);
			if (currMenuItem == null)
			{
				return HttpNotFound();
			}
			return View(currMenuItem);
		}

		// POST: MenuItem/Delete/5
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteItem(int menuId)
		{
			var db = new ApplicationDbContext();
			HttpContext.Cache.Remove("menu");
			var currMenuItem = db.MenuItems.Find(menuId);
			db.MenuItems.Remove(currMenuItem);
			db.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}