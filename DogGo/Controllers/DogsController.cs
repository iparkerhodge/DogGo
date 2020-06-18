using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwner(ownerId);

            return View(dogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            return View(dog);
        }

        // GET: DogController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Dog dog)
        {
            try
            {
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(dog);
            }
        }

        // GET: DogController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if(dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.UpdateDog(dog);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
