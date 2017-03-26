#tool "nuget:?package=gitreleasemanager"
#tool "GitVersion.CommandLine"

var target = Argument("target", "Default");
var buildDir = "./build/";
var artifactDir = "./artifacts/";
var srcDir = "./src";
var testProject = "./src/Ranger.NetCore.Tests/Ranger.NetCore.Tests.csproj";
var consoleProject = "./src/Ranger.NetCore.Console/Ranger.NetCore.Console.csproj";
var settings = new DotNetCoreBuildSettings
              {
                  Configuration = "Release",
                  OutputDirectory = buildDir
              };
              
var login = EnvironmentVariable("GH_LOGIN");
var password = EnvironmentVariable("GH_PASSWORD");
var repository = "ReleaseNoteGenerator";

Task("Restore")
  .Does(() =>
{
  Information("Restoring all projects...");
  DotNetCoreRestore(srcDir);
});

Task("Build")
  .IsDependentOn("Restore")
  .Does(() =>
{
  Information("Cleaning build directory...");
  CleanDirectory(buildDir);
  Information("Building all projects...");
  DotNetCoreBuild(consoleProject, settings);
});

Task("Tests")
  .IsDependentOn("Build")
  .Does(() =>
{
  Information("Starting tests...");
  DotNetCoreTest(testProject);
});

Task("Prepare-Package")
  .IsDependentOn("Build")
  .Does(() =>
{
  Information("Create packages...");
  EnsureDirectoryExists(artifactDir);
  CleanDirectory(artifactDir);
  Zip(buildDir, artifactDir + "/ranger-console.zip");
});


Task("Publish")
  .IsDependentOn("Tests")
  .IsDependentOn("Prepare-Package")
  .Does(() =>
{
  Information("Create Github Release");
    var  version = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true
    });

   GitReleaseManagerCreate(login, password, login, repository, new GitReleaseManagerCreateSettings {
            Name              =  version.MajorMinorPatch,
            InputFilePath = "RELEASE.md",
            Prerelease        = true,
            TargetCommitish   = "develop",
  });
  GitReleaseManagerAddAssets(login, password, login, repository, version.MajorMinorPatch, artifactDir + File("ranger-console.zip"));  
  GitReleaseManagerClose(login, password, login, repository, version.MajorMinorPatch);
});


Task("Default")
  .IsDependentOn("Publish")
  .Does(() =>
{
});


RunTarget(target);