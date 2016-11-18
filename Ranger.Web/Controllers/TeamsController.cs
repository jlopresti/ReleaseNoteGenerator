 using System.IO;
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

        public ActionResult Edit(string id)
        {
            if (!_appService.TeamExists(id))
            {
                return HttpNotFound();
            }
            return View(new CreateTeamViewModel() { Name = id});
        }

        [HttpPost]
        public ActionResult Edit(string id, CreateTeamViewModel vm)
        {
            if (!_appService.TeamExists(id))
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (_appService.TeamExists(vm.Name))
            {
                ModelState.AddModelError(string.Empty, $"Directory {vm.Name} already exists");
                return View(vm);
            }

            _appService.EditTeam(id, vm.Name);

            return RedirectToAction("Index");
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