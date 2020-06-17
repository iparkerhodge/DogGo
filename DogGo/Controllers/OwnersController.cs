using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly OwnerRepository _ownerRepo;
        private readonly DogRepository _dogRepo;
        private readonly NeighborhoodRepository _neighborhoodRepo;
        private readonly WalkerRepository _walkerRepo;


        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnersController(IConfiguration config)
        {
            _ownerRepo = new OwnerRepository(config);
            _dogRepo = new DogRepository(config);
            _neighborhoodRepo = new NeighborhoodRepository(config);
            _walkerRepo = new WalkerRepository(config);
        }

        // GET: OwnerController
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();

            return View(owners);
        }

        // GET: OwnerController/Details/5
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            if(owner == null)
            {
                return NotFound();
            }

            List<Dog> dogs = _dogRepo.GetDogsByOwner(owner.Id);
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);

            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers
            };

            return View(vm);
        }

        // GET: OwnerController/Create
        public ActionResult Create()
        {
            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = _neighborhoodRepo.GetAll()
            };

            return View(vm);
        }

        // POST: OwnerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OwnerFormViewModel vm)
        {
            try
            {
                _ownerRepo.AddOwner(vm.Owner);

                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                return View(vm);
            }
        }

        // GET: OwnerController/Edit/5
        public ActionResult Edit(int id)
        {
            OwnerFormViewModel vm = new OwnerFormViewModel
            {
                Owner = _ownerRepo.GetOwnerById(id),
                Neighborhoods = _neighborhoodRepo.GetAll()
            };

            if(vm.Owner == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: OwnerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OwnerFormViewModel vm)
        {
            try
            {
                _ownerRepo.UpdateOwner(vm.Owner);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: Owners/Delete/5
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }
    }
}
