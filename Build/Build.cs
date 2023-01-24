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
        .OnlyWhenStatic(() => GitHubToken.SkipEmpty())
        .Executes(() =>
        {
            // GitVersion.BranchName
            Serilog.Log.Information(GitVersion.BranchName);

            // RootDirectory
            Serilog.Log.Information(RootDirectory);

            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();
            var version = MainProject.GetInformationalVersion() ?? "1.0.0-dev";

            Serilog.Log.Information($"Repository: {gitHubName} Owner: {gitHubOwner} Version: {version}");

            Serilog.Log.Information($"Identifier: {GitRepository.Identifier}");
            Serilog.Log.Information($"RemoteName: {GitRepository.RemoteName} RemoteBranch: {GitRepository.RemoteBranch}");
            Serilog.Log.Information($"Branch: {GitRepository.Branch} Tags: {string.Join(" ", GitRepository.Tags)}");

            

            //Serilog.Log.Information($"CreatedDraft: {newRelease}");

            //GitHubTasks.GitHubClient.Credentials = new Octokit.Credentials(GitHubToken);

            //var gitHubTags = await GitHubTasks.GitHubClient.Repository.GetAllTags(gitHubOwner, gitHubName);

            //Serilog.Log.Information($"GetAllTags: {gitHubTags.Count}");

            ////foreach (var repo in GitHubTasks.GitHubClient.Repository.GetAllPublic().Result)
            ////{
            ////    Serilog.Log.Warning($"Repository: {repo.Name}");
            ////}
            try
            {
                GitHubTasks.GitHubClient = new Octokit.GitHubClient(new Octokit.ProductHeaderValue(Solution.Name))
                {
                    Credentials = new Octokit.Credentials(GitHubToken)
                };

                if (GitHubExtension.CheckTags(gitHubOwner, gitHubName, version))
                {
                    Serilog.Log.Warning($"The repository already contains a Release with the tag: {version}");
                }

                var newRelease = new Octokit.NewRelease(version)
                {
                    Name = version,
                    Body = GetReleaseNotes(),
                    Draft = true,
                    TargetCommitish = GitVersion.Sha
                };

                var draft = GitHubExtension.CreatedDraft(gitHubOwner, gitHubName, newRelease);
            }
            catch (System.Exception ex)
            {
                Serilog.Log.Error($"{ex}");
            }
            
            // GetReleaseNotes
            Serilog.Log.Information(GetReleaseNotes());
        });
}