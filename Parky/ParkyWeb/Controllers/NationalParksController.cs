using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRep;
        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRep = npRepo;

        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        public async Task <IActionResult> Upsert(int? id)
        {
            var obj = new NationalPark();
            if(id==null)
            {
                return View(obj);
            }
            obj = await _npRep.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault());
            if(obj==null)
            {
                return NotFound();
            }
            return View(obj);
        }
        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _npRep.GetAllAsync(SD.NationalParkAPIPath) });

        }
    }
}
