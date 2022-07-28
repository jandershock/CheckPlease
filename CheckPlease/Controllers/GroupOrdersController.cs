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
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IGroupOrderRepository _groupOrderRepository;

        public GroupOrdersController(IRestaurantsRepository restaurantsRepository, IUserProfileRepository userProfileRepository, IGroupOrderRepository groupOrderRepository)
        {
            _restaurantsRepository = restaurantsRepository;
            _userProfileRepository = userProfileRepository;
            _groupOrderRepository = groupOrderRepository;
        }

        // GET: GroupOrdersController
        public ActionResult Index()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            GroupOrderIndexViewModel vm = new GroupOrderIndexViewModel()
            {
                GroupOrders = _userProfileRepository.GetGroupOrdersByUser(userId),
                UserId = userId
            };
            return View(vm);
        }

        // GET: GroupOrdersController/Details/5
        public ActionResult Details(int id)
        {
            GroupOrderDetailsViewModel vm = new GroupOrderDetailsViewModel()
            {
                CurrentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                GroupOrder = _groupOrderRepository.GetGroupOrderById(id)
            };
            return View();
        }

        // GET: GroupOrdersController/Create
        public ActionResult Create()
        {
            CreateGroupOrderViewModel vm = new CreateGroupOrderViewModel()
            {
                GroupOrder = new GroupOrder(),
                Restaurants = _restaurantsRepository.GetAll(),
                UserProfiles = _userProfileRepository.GetAll(),
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
                vm.GroupOrder.OwnerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _userProfileRepository.AddGroupOrder(vm.GroupOrder);
                foreach(int i in vm.SelectedUserIds)
                {
                    _userProfileRepository.CreateGroupOrderUserEntry(new GroupOrderUser()
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
                return View(vm);
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
