using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {

        private readonly WalkerRepository _walkerRepo;
        private readonly WalkRepository _walkRepo;
        private readonly OwnerRepository _ownerRepo;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkersController(IConfiguration config)
        {
            _walkerRepo = new WalkerRepository(config);
            _walkRepo = new WalkRepository(config);
            _ownerRepo = new OwnerRepository(config);
        }

        // GET: WalkersController
        public ActionResult Index()
        {
            int ownerId = 0;
            ownerId = GetCurrentUserId();

            if(ownerId == 0)
            {
                List<Walker> walkers = _walkerRepo.GetAllWalkers();
                return View(walkers);
            }
            else
            {
                Owner Owner = _ownerRepo.GetOwnerById(ownerId);
                List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(Owner.NeighborhoodId);
                return View(walkers);
            }

        }

        // GET: WalkersController/Details/5
        public ActionResult Details(int id)
        {
            WalkerProfileViewModel vm = new WalkerProfileViewModel
            {
                Walker = _walkerRepo.GetWalkerById(id),
                Walks = _walkRepo.GetWalksByWalker(id),
                TotalDuration = _walkRepo.TotalDurationByWalker(id)
            };
            
            if (vm.Walker == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(id != null)
            {
                return int.Parse(id);
            }

            return 0;
        }
    }
}
