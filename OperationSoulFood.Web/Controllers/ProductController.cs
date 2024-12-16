using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OperationSoulFood.Web.Models;
using OperationSoulFood.Web.Services;
using OperationSoulFood.Web.Services.IServices;
using OperationSoulFood.Web.Utility;
using System.Collections.Generic;

namespace OperationSoulFood.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? productList = new();

            ResponseDto? response = await _productService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productList);
        }

		[HttpGet]
		public async Task<IActionResult> ProductCreate()
		{
			return View();
		}


		[HttpPost]
		[Authorize(Roles = SD.RoleAdmin)]
		public async Task<IActionResult> ProductCreate(ProductDto model)
		{
			if (ModelState.IsValid)
			{
				ResponseDto? response = await _productService.CreateProductsAsync(model);

				if (response != null && response.IsSuccess)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response?.Message;
				}
			}

			return View(model);
		}


		[HttpGet]
		public async Task<IActionResult> ProductDelete(int productId)
		{
			if (ModelState.IsValid)
			{
				ResponseDto? response = await _productService.GetProductByIdAsync(productId);

				if (response != null && response.IsSuccess)
				{
					CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));

					return View(model);
				}
				else
				{
					TempData["error"] = response?.Message;
				}
			}

			return NotFound();
		}


		[HttpPost]
		[Authorize(Roles = SD.RoleAdmin)]
		public async Task<IActionResult> ProductDelete(ProductDto productDto)
		{
			ResponseDto? response = await _productService.DeleteProductsAsync(productDto.ProductId);

			if (response != null && response.IsSuccess)
			{
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(productDto);
		}


		[HttpGet]
		[Authorize(Roles = SD.RoleAdmin)]
		public async Task<IActionResult> ProductEdit(int productId)
		{
			ResponseDto? response = await _productService.GetProductByIdAsync(productId);

			if (response != null && response.IsSuccess)
			{
				ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
				return View(model);
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return NotFound();
		}


		[HttpPost]
		[Authorize(Roles = SD.RoleAdmin)]
		public async Task<IActionResult> ProductEdit(ProductDto productDto)
		{
			ResponseDto? response = await _productService.UpdateProductsAsync(productDto);

			if (response != null && response.IsSuccess)
			{
				TempData["success"] = "Product updated successfully";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(productDto);
		}


	}
}
