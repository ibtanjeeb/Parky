using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRep;
        private readonly ITrailRepository _trailRepo;
        public TrailsController(INationalParkRepository npRepo,ITrailRepository trailRepo)
        {

            _npRep = npRepo;
            _trailRepo = trailRepo;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        public async Task <IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _npRep.GetAllAsync(SD.NationalParkAPIPath);
            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };


            
            if(id==null)
            {
                return View(objVM);
            }
            objVM.Trail = await _trailRepo.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());
            if(objVM.Trail ==null)
            {
                return NotFound();
            }
            return View(objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM objVM)
        {
           if(ModelState.IsValid)
            { 

                if(objVM.Trail.Id==0)
                {
                     await _trailRepo.CreateAsync(SD.TrailAPIPath, objVM.Trail);
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailAPIPath+objVM.Trail.Id, objVM.Trail);

                }
                return RedirectToAction(nameof(Index));



            }
            else
            {
                return View(objVM);
            }
        }
       

        public async Task<IActionResult> GetAllTrails()
        {

            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailAPIPath) });

        }
         [HttpDelete]
        public async Task<IActionResult>Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TrailAPIPath, id);
            if(status)
            {
                return Json(new { success = true, message="Delete Sucessfully" });
            }
            return Json(new { success = false, message = "Delete Not Sucessfully" });

        }
    }
}
