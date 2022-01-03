using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using MyBlog.Entities.Dtos;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Extensions;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.UI.Areas.Admin.Models;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace MyBlog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAll();

            return View(result.Data);
        }

        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto) //Artik bize Json formatinda(yani string olarak) bir categoryAddAjaxModel gelmis olacak. Bu sayede bunu gonderdigimiz her yerde Parse ederek kullanabiliriz...
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.Add(categoryAddDto, "Emre Tomruk");

                if (result.ResultStatus==ResultStatus.Success) //Yeni model icerisinde bunu kullaniciya(Front-End'e gonderiyoruz...)
                {
                    var categoryAddAjaxModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                    {
                        CategoryDto = result.Data,
                        CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto) //Eklenen categoryAddDto string'e cevirilir...
                    });
                    return Json(categoryAddAjaxModel);
                }
            }

            var categoryAddAjaxErrorModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
            {
                CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto) //Hata mesajlari vs. buradan verecegiz...
            });
            return Json(categoryAddAjaxErrorModel);          
        }

        public async Task<JsonResult> GetAllCategories()
        {
            var result = await _categoryService.GetAll();
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions //Objemiz icinde birbirini referans eden objeler oldugu icin bu yapiyi kullandik...
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }
    }
}
