using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tools.GitHub;
using ricaun.Nuke;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;

internal class Build : NukeBuild, IPublishPack, ITest, IShowGitVersion
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
            // GitVersion.BranchName
            Serilog.Log.Information(GitVersion.BranchName);

            // RootDirectory
            Serilog.Log.Information(RootDirectory);

            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();

            Serilog.Log.Information(gitHubName);
            Serilog.Log.Information(gitHubOwner);

            var version = MainProject.GetInformationalVersion();

            foreach (var repo in GitHubTasks.GitHubClient.Repository.GetAllPublic().Result)
            {
                Serilog.Log.Warning($"Repository: {repo.Name}");
            }

            if (GitHubExtension.CheckTags(gitHubOwner, gitHubName, version))
            {
                Serilog.Log.Warning($"The repository already contains a Release with the tag: {version}");
            }

            // GetReleaseNotes
            Serilog.Log.Information(GetReleaseNotes());
        });
}