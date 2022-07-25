using CheckPlease.Models.ViewModels;
using CheckPlease.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckPlease.Controllers
{
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
            CreateGroupOrderViewModel vm = new CreateGroupOrderViewModel();
            vm.Restaurants = _restaurantsRepository.GetAll();
            vm.UserProfiles = _userProfileRespository.GetAll();
            return View();
        }

        // POST: GroupOrdersController/Create
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
