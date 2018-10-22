string GetConfiguration()
{
	if(HasArgument("Configuration"))
	{
		return Argument<string>("Configuration");
	}

	return EnvironmentVariable("Configuration") != null ? 
		EnvironmentVariable("Configuration") : 
		"Release";
}

string GetNuGetVersion()
{
	var settings = new GitVersionSettings
	{
		OutputType = GitVersionOutput.Json
	};

	GitVersion versionInfo = GitVersion(settings);

	Information("GitVersion:");
	Information(versionInfo.Dump<GitVersion>());

	return versionInfo.NuGetVersion;
}
