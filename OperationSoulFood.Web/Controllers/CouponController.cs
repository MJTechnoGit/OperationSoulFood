using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OperationSoulFood.Web.Models;
using OperationSoulFood.Web.Services.IServices;
using OperationSoulFood.Web.Utility;
using System.Collections.Generic;

namespace OperationSoulFood.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public  CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();

            ResponseDto? response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }



        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin)]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponsAsync(model);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);

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
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {           
            ResponseDto? response = await _couponService.DeleteCouponsAsync(couponDto.CouponId);

            if (response != null && response.IsSuccess)
            {   
                return RedirectToAction(nameof(CouponIndex));
            }  
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(couponDto);
        }
    }
}
