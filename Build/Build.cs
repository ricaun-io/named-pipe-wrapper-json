using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tools.GitHub;
using ricaun.Nuke;
using ricaun.Nuke.Components;

internal class Build : NukeBuild, IPublishPack, ITest, IShowGitVersion
{
    string ITest.TestProjectName => "UnitTests";
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}

public interface IShowGitVersion : IHazGitVersion, IHazChangelog, IHazGitRepository, IClean
{
    Target ShowGitVersion => _ => _
        .TriggeredBy(Clean)
        .Requires(() => GitRepository)
        .Executes(() =>
        {
            // GitVersion.BranchName
            Serilog.Log.Information(GitVersion.BranchName);

            // RootDirectory
            Serilog.Log.Information(RootDirectory);

            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();

            Serilog.Log.Information(gitHubName);
            Serilog.Log.Information(gitHubOwner);

            // GetReleaseNotes
            Serilog.Log.Information(GetReleaseNotes());
        });
}