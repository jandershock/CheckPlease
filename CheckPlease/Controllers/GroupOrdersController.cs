using CheckPlease.Models.ViewModels;
using CheckPlease.Models;
using CheckPlease.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CheckPlease.Controllers
{
    [Authorize]
    public class GroupOrdersController : Controller
    {
        private readonly IRestaurantsRepository _restaurantsRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IGroupOrderRepository _groupOrderRepository;
        private readonly IFoodItemsRepository _foodItemsRepository;

        public GroupOrdersController(IRestaurantsRepository restaurantsRepository, IUserProfileRepository userProfileRepository, IGroupOrderRepository groupOrderRepository, IFoodItemsRepository foodItemsRepository)
        {
            _restaurantsRepository = restaurantsRepository;
            _userProfileRepository = userProfileRepository;
            _groupOrderRepository = groupOrderRepository;
            _foodItemsRepository = foodItemsRepository;
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
        [Route("GroupOrders/{id}/Details")]
        public ActionResult Details(int id)
        {
            GroupOrderDetailsViewModel vm = new GroupOrderDetailsViewModel()
            {
                CurrentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                GroupOrder = _groupOrderRepository.GetGroupOrderById(id)
            };
            return View(vm);
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
                foreach (int i in vm.SelectedUserIds)
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

        [HttpGet]
        [Route("GroupOrders/{id}/AddOrder")]
        public ActionResult AddOrder(int id)
        {
            AddOrderViewModel vm = new AddOrderViewModel();
            GroupOrder go = _groupOrderRepository.GetGroupOrderById(id);
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            vm.Menu = _foodItemsRepository.GetMenuByRestaurantId(go.RestaurantId);
            vm.Gou = go.GroupMembers.Where(gm => gm.UserId == userId).FirstOrDefault();
            return View(vm);
        }

        [HttpPost("GroupOrders/{id}/AddOrder")]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrder(AddOrderViewModel vm)
        {
            throw new NotImplementedException();
        }
    }
}
