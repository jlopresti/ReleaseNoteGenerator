using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Ranger.Core.Helpers;
using Ranger.Core.IssueTracker;
using Ranger.Core.Models;
using Ranger.Core.SourceControl;
using Ranger.Core.TemplateProvider;
using Ranger.Web.Models;
using Ranger.Web.Models.Configurations;
using Ranger.Web.Services;

namespace Ranger.Web.Controllers
{
    public class ConfigurationsController : Controller
    {
        private AppService _appService;

        public ConfigurationsController()
        {
            _appService = new AppService();
        }        

        public ActionResult Index(string id)
        {
            var cookie = Request.Cookies["team"];
            var teamId = cookie != null ? cookie.Value : id;
            var vm = new ViewConfigurationsViewModel();
            vm.Teams = _appService.GetTeams();
            vm.SelectedTeam = string.IsNullOrEmpty(teamId) ? vm.Teams.FirstOrDefault() : vm.Teams.FirstOrDefault(x => x.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            vm.SelectedTeam = vm.SelectedTeam ?? string.Empty;
            vm.Config = _appService.GetConfig(vm.SelectedTeam) ?? string.Empty;

            Response.Cookies.Set(new HttpCookie("team", vm.SelectedTeam));
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(string id, string config)
        {
            try
            {
                var conf = config.ToObject<Config>();
                Guard.IsValidConfig(() => conf);
                _appService.CreateConfig(id, config);
            }
            catch (JsonException ex)
            {

            }
            catch (ApplicationException ex)
            {

            }
            return RedirectToAction("Index", "Configurations", new { id = id});
        }
    }
}