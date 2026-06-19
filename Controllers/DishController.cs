using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MID_BCS240027.Data;
using MID_BCS240027.Models;

namespace MID_BCS240027.Controllers
{
    public class DishController : Controller
    {
        private readonly AppDbContext _context;

        public DishController(AppDbContext context)
        {
            _context = context;
        }

        // ================= INDEX =================

        public async Task<IActionResult> Index(
            string? keyword,
            int? categoryId,
            bool? isAvailable,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortOrder)
        {
            var dishes = _context.Dishes_BCS240027
                .Include(x => x.DishCategory)
                .AsQueryable();

            // Search tên món
            if (!string.IsNullOrEmpty(keyword))
            {
                dishes = dishes.Where(x =>
                    x.Name.Contains(keyword));
            }

            // Lọc loại món
            if (categoryId.HasValue)
            {
                dishes = dishes.Where(x =>
                    x.DishCategoryId == categoryId);
            }

            // Lọc trạng thái
            if (isAvailable.HasValue)
            {
                dishes = dishes.Where(x =>
                    x.IsAvailable == isAvailable);
            }

            // Kiểm tra khoảng giá
            if (minPrice.HasValue && maxPrice.HasValue
                && minPrice > maxPrice)
            {
                ViewBag.PriceError =
                    "Khoảng giá không hợp lệ";
            }
            else
            {
                if (minPrice.HasValue)
                    dishes = dishes.Where(x =>
                        x.Price >= minPrice);

                if (maxPrice.HasValue)
                    dishes = dishes.Where(x =>
                        x.Price <= maxPrice);
            }

            // Sắp xếp

            switch (sortOrder)
            {
                case "priceAsc":
                    dishes = dishes.OrderBy(x => x.Price);
                    break;

                case "priceDesc":
                    dishes = dishes.OrderByDescending(x => x.Price);
                    break;

                case "timeAsc":
                    dishes = dishes.OrderBy(x => x.PreparationTime);
                    break;

                default:
                    dishes = dishes.OrderBy(x => x.Id);
                    break;
            }

            ViewBag.Categories =
                new SelectList(
                    _context.DishCategories_BCS240027,
                    "Id",
                    "Name");

            return View(await dishes.ToListAsync());
        }

        // ================= DETAIL =================

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var dish = await _context.Dishes_BCS240027
                .Include(x => x.DishCategory)
                .Include(x => x.DishImages)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dish == null)
                return NotFound();

            return View(dish);
        }

        // ================= CREATE =================

        public IActionResult Create()
        {
            ViewBag.Categories =
                new SelectList(
                    _context.DishCategories_BCS240027,
                    "Id",
                    "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            Dish_BCS240027 dish)
        {
            if (!_context.DishCategories_BCS240027
                .Any(x => x.Id == dish.DishCategoryId))
            {
                ModelState.AddModelError(
                    "",
                    "Loại món ăn không tồn tại");
            }

            bool exists = _context.Dishes_BCS240027.Any(x =>
                x.Name == dish.Name &&
                x.DishCategoryId == dish.DishCategoryId);

            if (exists)
            {
                ModelState.AddModelError(
                    "Name",
                    "Tên món ăn đã tồn tại trong loại này");
            }

            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories =
                new SelectList(
                    _context.DishCategories_BCS240027,
                    "Id",
                    "Name");

            return View(dish);
        }

        // ================= EDIT =================

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dish = await _context.Dishes_BCS240027
                .FindAsync(id);

            if (dish == null)
                return NotFound();

            ViewBag.Categories =
                new SelectList(
                    _context.DishCategories_BCS240027,
                    "Id",
                    "Name",
                    dish.DishCategoryId);

            return View(dish);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            Dish_BCS240027 dish)
        {
            if (id != dish.Id)
                return NotFound();

            bool exists = _context.Dishes_BCS240027.Any(x =>
                x.Id != dish.Id &&
                x.Name == dish.Name &&
                x.DishCategoryId == dish.DishCategoryId);

            if (exists)
            {
                ModelState.AddModelError(
                    "Name",
                    "Tên món ăn đã tồn tại");
            }

            if (ModelState.IsValid)
            {
                _context.Update(dish);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories =
                new SelectList(
                    _context.DishCategories_BCS240027,
                    "Id",
                    "Name",
                    dish.DishCategoryId);

            return View(dish);
        }

        // ================= DELETE =================

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dish = await _context.Dishes_BCS240027
                .Include(x => x.DishCategory)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dish == null)
                return NotFound();

            return View(dish);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes_BCS240027
                .FindAsync(id);

            if (dish == null)
                return NotFound();

            _context.Dishes_BCS240027.Remove(dish);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}