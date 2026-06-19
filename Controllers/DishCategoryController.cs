using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MID_BCS240027.Data;
using MID_BCS240027.Models;

namespace MID_BCS240027.Controllers
{
    public class DishCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public DishCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // INDEX

        public async Task<IActionResult> Index()
        {
            return View(await _context
                .DishCategories_BCS240027
                .ToListAsync());
        }

        // DETAIL

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context
                .DishCategories_BCS240027
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // CREATE

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            DishCategory_BCS240027 category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // EDIT

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category =
                await _context
                .DishCategories_BCS240027
                .FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            DishCategory_BCS240027 category)
        {
            if (id != category.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // DELETE

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context
                .DishCategories_BCS240027
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var category = await _context
                .DishCategories_BCS240027
                .Include(x => x.Dishes)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            if (category.Dishes != null &&
                category.Dishes.Any())
            {
                TempData["Error"] =
                    "Không thể xóa vì loại món ăn đang được sử dụng.";

                return RedirectToAction(nameof(Index));
            }

            _context.DishCategories_BCS240027
                .Remove(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}