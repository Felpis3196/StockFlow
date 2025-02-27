using Microsoft.AspNetCore.Mvc;
using StockFlow.Models;
using StockFlow.Services;

public class ItemController : Controller
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _itemService.GetAllAsync();
        return View(items);
    }
    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Delete()
    {
        return View();
    }

    public IActionResult Edit()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return Json(new
        {
            id = item.Id,
            name = item.Name,
            amount = item.Amount,
            minAmount = item.MinAmount,
            supplier = item.Supplier,
            category = item.Category,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(Item item)
    {
        try
        {
            await _itemService.AddAsync(item);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(item);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update(Item item)
    {
        if (item == null || item.Id == Guid.Empty)
        {
            return Json(new { success = false, message = "Dados inválidos" });
        }

        try
        {
            await _itemService.UpdateAsync(item);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }


    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _itemService.DeleteAsync(id);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

}
