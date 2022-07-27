using CheckPlease.Models.ViewModels;
using CheckPlease.Models;
using CheckPlease.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System;

namespace CheckPlease.Controllers
{
    [Authorize]
    public class GroupOrdersController : Controller
    {
        private readonly IRestaurantsRepository _restaurantsRepository;
        private readonly IUserProfileRepository _userProfileRespository;

        public GroupOrdersController(IRestaurantsRepository restaurantsRepository, IUserProfileRepository userProfileRespository)
        {
            _restaurantsRepository = restaurantsRepository;
            _userProfileRespository = userProfileRespository;
        }

        // GET: GroupOrdersController
        public ActionResult Index()
        {
            return View();
        }

        // GET: GroupOrdersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GroupOrdersController/Create
        public ActionResult Create()
        {
            CreateGroupOrderViewModel vm = new CreateGroupOrderViewModel()
            {
                GroupOrder = new GroupOrder(),
                Restaurants = _restaurantsRepository.GetAll(),
                UserProfiles = _userProfileRespository.GetAll(),
                SelectedUserIds = new List<int>()
            };
            return View(vm);
        }

        // POST: GroupOrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateGroupOrderViewModel vm)
        {
            try
            {
                //if(GetCurrentUserProfile().Id != vm.GroupOrder.OwnerId)
                //{
                //    return Unauthorized();
                //}
                
                vm.GroupOrder.OwnerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _userProfileRespository.AddGroupOrder(vm.GroupOrder);
                foreach(int i in vm.SelectedUserIds)
                {
                    _userProfileRespository.CreateGroupOrderUserEntry(new GroupOrderUser()
                    {
                        UserId = i,
                        GroupOrderId = vm.GroupOrder.Id,
                        HasOrdered = false
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: GroupOrdersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GroupOrdersController/Edit/5
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

        // GET: GroupOrdersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GroupOrdersController/Delete/5
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
        
    }
}
