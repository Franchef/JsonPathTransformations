using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Docker.DockerTasks;

using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.ReportGenerator;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    readonly AbsolutePath OutputDirectory = RootDirectory / "output";
    readonly AbsolutePath TestsDirectory = RootDirectory / "tests";
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean(c => c.SetProject(Solution));
            //RootDirectory.GlobDirectories("**/bin", "**/obj").ForEach(folder => folder.DeleteDirectory());
            OutputDirectory.CreateOrCleanDirectory();
            TestsDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
               .SetConfiguration(Configuration)
               //.SetVersion(GitVersion.NuGetVersionV2)
               //.SetAssemblyVersion(GitVersion.AssemblySemVer)
               //.SetInformationalVersion(GitVersion.InformationalVersion)
               //.SetFileVersion(GitVersion.AssemblySemFileVer)
               .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(RootDirectory / "test" / "JPTTests" / "JPTTests.csproj")
                .SetConfiguration(Configuration)
                .EnableCollectCoverage()
                .SetCoverletOutputFormat(CoverletOutputFormat.opencover)
                .SetProcessArgumentConfigurator(a => a.Add("--collect:\"XPlat Code Coverage\"").Add("/p:CollectCoverage=true"))
                .SetResultsDirectory(TestsDirectory)
                .SetCoverletOutput("StremsEngine.DomainTests.coverage.cobertura.xml")
                .SetSettingsFile(RootDirectory /  "test" / "coverlet.runsettings")
                //.SetOutput(TestsDirectory)
                //.SetVersion(GitVersion.NuGetVersionV2)
                //.SetAssemblyVersion(GitVersion.AssemblySemVer)
                //.SetInformationalVersion(GitVersion.InformationalVersion)
                //.SetFileVersion(GitVersion.AssemblySemFileVer)
                .EnableNoRestore()
                .EnableNoBuild()
            );

            ReportGeneratorTasks.ReportGenerator(s => s
                .SetTargetDirectory(TestsDirectory)
                .AddReports(TestsDirectory / "**/**.xml")
                .AddReportTypes(
                    ReportTypes.lcov,
                    ReportTypes.HtmlInline_AzurePipelines_Dark
                )
            );
        });

}
