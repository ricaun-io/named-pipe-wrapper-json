using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tools.GitHub;
using ricaun.Nuke;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;

internal class Build : NukeBuild, IPublishPack, IShowGitVersion, ITest, IPrePack
{
    string ITest.TestProjectName => "UnitTests";
    bool ITest.TestBuildStopWhenFailed => false;
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}

public interface IShowGitVersion : IHazGitVersion, IHazChangelog, IHazGitRepository, IClean, IHazMainProject
{
    Target ShowGitVersion => _ => _
        .TriggeredBy(Clean)
        .Requires(() => GitRepository)
        .Executes(() =>
        {
            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();

            Serilog.Log.Information($"Repository: {gitHubName} Owner: {gitHubOwner}");
        });
}