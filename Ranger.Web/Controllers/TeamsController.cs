﻿ using System.IO;
using System.Linq;
using System.Web.Mvc;
using Ranger.Web.Models;
using Ranger.Web.Models.Teams;
using Ranger.Web.Services;

namespace Ranger.Web.Controllers
{
    public class TeamsController : Controller
    {
        private AppService _appService;

        public TeamsController()
        {
            _appService = new AppService();
        }
        public ActionResult Index()
        {
            var vm = new TeamsViewModel
            {
                Teams = _appService.GetTeams()
            };
            return View(vm);
        }

        public ActionResult Add()
        {
            return View(new CreateTeamViewModel());
        }

        [HttpPost]
        public ActionResult Add(CreateTeamViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (_appService.TeamExists(vm.Name))
            {
                ModelState.AddModelError(string.Empty, $"Directory {vm.Name} already exists");
                return View(vm);
            }

            _appService.AddTeam(vm.Name);

            return RedirectToAction("Index");
        }
    }
}