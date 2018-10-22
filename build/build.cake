#addin nuget:?package=Cake.Incubator&version=3.0.0
#tool "nuget:?package=NUnit.Runners&version=2.6.4"
#load "ByteDev.Io.cake"

var nugetSources = new[] {"https://api.nuget.org/v3/index.json"};

var target = Argument("target", "Default");

var solutionFilePath = "../src/ByteDev.Io.sln";

var artifactsDirectory = Directory("../artifacts");
var nugetDirectory = artifactsDirectory + Directory("NuGet");
	
var configuration = GetConfiguration();
	
Information("Configurtion: " + configuration);


Task("Clean")
    .Does(() =>
{
    CleanDirectory(artifactsDirectory);
	
	var binDir = GetDirectories("../src/**/bin");
	var objDir = GetDirectories("../src/**/obj");

	CleanDirectories(binDir);
	CleanDirectories(objDir);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
		var settings = new NuGetRestoreSettings
		{
			Source = nugetSources
		};

		NuGetRestore(solutionFilePath, settings);
    });

Task("Build")
	.IsDependentOn("Restore")
    .Does(() =>
	{	
        DotNetCoreBuild(
            solutionFilePath,
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration
            });
	});

Task("UnitTests")
    .IsDependentOn("Build")
    .Does(() =>
	{
		var projects = GetFiles("../src/*UnitTests/**/*.csproj");
		
		foreach(var project in projects)
		{
			DotNetCoreTest(
				project.FullPath,
				new DotNetCoreTestSettings()
				{
					Configuration = configuration,
					NoBuild = true
				});
		}
	});
	
Task("IntegrationTests")
    .IsDependentOn("UnitTests")
    .Does(() =>
	{
		var projects = GetFiles("../src/*IntTests/**/*.csproj");
		
		foreach(var project in projects)
		{
			DotNetCoreTest(
				project.FullPath,
				new DotNetCoreTestSettings()
				{
					Configuration = configuration,
					NoBuild = true
				});
		}
	});
	
Task("CreateNuGetPackages")
    .IsDependentOn("IntegrationTests")
    .Does(() =>
    {
		var nugetVersion = GetNuGetVersion();

        var settings = new DotNetCorePackSettings()
		{
			ArgumentCustomization = args => args.Append("/p:Version=" + nugetVersion),
			Configuration = configuration,
			OutputDirectory = nugetDirectory
		};
                
		DotNetCorePack("../src/ByteDev.Io/ByteDev.Io.csproj", settings);
    });

   
Task("Default")
    .IsDependentOn("CreateNuGetPackages");

RunTarget(target);
