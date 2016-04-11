using System;
using System.IO;
using System.Linq;
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
            var vm = new ViewConfigurationsViewModel();
            vm.Teams = _appService.GetTeams();
            vm.SelectedTeam = string.IsNullOrEmpty(id) ? vm.Teams.FirstOrDefault() : vm.Teams.FirstOrDefault(x => x.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            vm.SelectedTeam = vm.SelectedTeam ?? string.Empty;
            vm.Configs = _appService.GetConfigs(vm.SelectedTeam);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Add(string id, CreateConfigViewModel config)
        {
            try
            {
                var conf = config.Configuration.Configuration.ToObject<Config>();
                Guard.IsValidConfig(() => conf);
                _appService.CreateConfig(id, config.Configuration.Name, config.Configuration.Configuration);
            }
            catch (JsonException ex)
            {

            }
            catch (ApplicationException ex)
            {

            }
            return RedirectToAction("Index", "Configurations", new { id = id});
        }

        public ActionResult Add(string id)
        {
            var vm = new CreateConfigViewModel();
            var sourceControlProviders = Utils.GetProviders<ISourceControl>();
            vm.SourceControlProviders = sourceControlProviders.Select(x => x.Name).ToList();
            var issueTrackerProviders = Utils.GetProviders<IIssueTracker>();
            vm.IssueTrackerProviders = issueTrackerProviders.Select(x => x.Name).ToList();
            var providerAttributes = Utils.GetProviders<ITemplate>();
            vm.TemplateProviders = providerAttributes.Select(x => x.Name).ToList();

            vm.Configuration = new ConfigViewModel();
            return View(vm);
        }
    }
}