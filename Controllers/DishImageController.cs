using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MID_BCS240027.Data;
using MID_BCS240027.Models;

namespace MID_BCS240027.Controllers
{
    public class DishImageController : Controller
    {
        private readonly AppDbContext _context;

        public DishImageController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var images = _context.DishImages_BCS240027
                .Include(x => x.Dish);

            return View(await images.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Dishes = new SelectList(
                _context.Dishes_BCS240027,
                "Id",
                "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            DishImage_BCS240027 image)
        {
            if (!_context.Dishes_BCS240027
                .Any(x => x.Id == image.DishId))
            {
                ModelState.AddModelError(
                    "",
                    "Món ăn không tồn tại");
            }

            if (image.IsThumbnail)
            {
                var oldThumbs = _context
                    .DishImages_BCS240027
                    .Where(x =>
                        x.DishId == image.DishId);

                foreach (var item in oldThumbs)
                {
                    item.IsThumbnail = false;
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(image);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Dishes = new SelectList(
                _context.Dishes_BCS240027,
                "Id",
                "Name");

            return View(image);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var image =
                await _context.DishImages_BCS240027
                .FindAsync(id);

            if (image == null)
                return NotFound();

            ViewBag.Dishes = new SelectList(
                _context.Dishes_BCS240027,
                "Id",
                "Name",
                image.DishId);

            return View(image);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            DishImage_BCS240027 image)
        {
            if (id != image.Id)
                return NotFound();

            if (image.IsThumbnail)
            {
                var oldThumbs = _context
                    .DishImages_BCS240027
                    .Where(x =>
                        x.DishId == image.DishId &&
                        x.Id != image.Id);

                foreach (var item in oldThumbs)
                {
                    item.IsThumbnail = false;
                }
            }

            if (ModelState.IsValid)
            {
                _context.Update(image);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(image);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var image = await _context
                .DishImages_BCS240027
                .Include(x => x.Dish)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
                return NotFound();

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var image =
                await _context.DishImages_BCS240027
                .FindAsync(id);

            if (image == null)
                return NotFound();

            _context.DishImages_BCS240027
                .Remove(image);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}