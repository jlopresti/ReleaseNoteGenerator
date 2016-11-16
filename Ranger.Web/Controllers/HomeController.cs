using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ninject;
using Ranger.Core.Helpers;
using Ranger.Core.IssueTracker;
using Ranger.Core.Linker;
using Ranger.Core.Models;
using Ranger.Core.SourceControl;
using Ranger.Core.TemplateProvider;
using Ranger.Web.Models;
using Ranger.Web.Models.Home;
using Ranger.Web.Services;

namespace Ranger.Web.Controllers
{
    public class HomeController : Controller
    {
        private AppService _appservice;

        public HomeController()
        {
            _appservice = new AppService();
        }

        public ActionResult Index(string id)
        {
            var teams = _appservice.GetTeams().ToList();
            var cookie = Request.Cookies["team"];
            var teamId = cookie != null ? cookie.Value : id;
            var team = string.IsNullOrEmpty(teamId) ? teams.FirstOrDefault() : teams.FirstOrDefault(x => x.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            team = team ?? string.Empty;
            var vm = new CreateReleaseNoteViewModel();
            var components = _appservice.GetComponents(team);
            vm.Components = components.Select(x => new Compo() {Label = x}).ToList();
            vm.Teams = teams;
            vm.Team = team;
            Response.Cookies.Set(new HttpCookie("team", vm.Team));
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(string id, CreateReleaseNoteViewModel vm)
        {
            return RedirectToAction("ReleaseNote", new { id = id, release = vm.Release, components = Request.Form["components"] });
        }

        public async Task<ActionResult> Release(string id, string release, string components)
        {
            string path = HttpContext.Server.MapPath("~/App_Data/configs/"+id);
            var configPath = Directory.EnumerateFiles(path).FirstOrDefault(x => Path.GetFileName(x) == "config.json");
            var cfg = new ReleaseNoteConfiguration(configPath);
            var cmp = cfg.Config.SourceControl["projectConfigs"] as JArray;
            var l = cmp.Where(x => components.Contains(x["project"].Value<string>())).ToList();
            cfg.Config.SourceControl["projectConfigs"] = new JArray(l);
            var kernel = new StandardKernel();
            kernel.Bind<ReleaseNoteConfiguration>().ToMethod(x => cfg);
            kernel.Load<ReleaseNoteModule>();
            var issueTracker = kernel.Get<IIssueTracker>();
            var sourceControl = kernel.Get<ISourceControl>();
            var releaseNoteLinker = kernel.Get<IReleaseNoteLinker>();
            var template = kernel.Get<ITemplate>();
            var issues = await issueTracker.GetIssues(release);
            var commits = await sourceControl.GetCommits(release);
            var releaseNoteModel = releaseNoteLinker.Link(commits, issues);
            var output = template.Build(release, releaseNoteModel);

            return View(new ReleaseNoteOutputViewModel() { Html = output});
        }

        public ActionResult ReleaseNote(string id, string release, string components)
        {
            ViewBag.Url = Url.Action("Release", new {id = id,release = release, components = components });
            return View();
        }
    }
}