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
    public class DogsController : Controller
    {
        private readonly DogRepository _dogRepo;
        private readonly OwnerRepository _ownerRepo;

        public DogsController(IConfiguration config)
        {
            _dogRepo = new DogRepository(config);
            _ownerRepo = new OwnerRepository(config);
        }
        // GET: DogController
        public ActionResult Index()
        {
            List<Dog> dogs = _dogRepo.GetAllDogs();

            return View(dogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            return View(dog);
        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();

            DogFormViewModel vm = new DogFormViewModel()
            {
                Owners = owners,
                Dog = new Dog()
                
            };

            return View(vm);
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DogFormViewModel vm)
        {
            try
            {
                _dogRepo.AddDog(vm.Dog);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            DogFormViewModel vm = new DogFormViewModel()
            {
                Dog = _dogRepo.GetDogById(id),
                Owners = _ownerRepo.GetAllOwners()
            };

            if(vm.Dog == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DogFormViewModel vm)
        {
            try
            {
                _dogRepo.UpdateDog(vm.Dog);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(vm);
            }
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }
    }
}
