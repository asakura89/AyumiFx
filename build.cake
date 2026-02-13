#tool nuget:?package=Newtonsoft.Json&version=10.0.3

#r newtonsoft.json.10.0.3/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll

using System;
using System.Globalization;
using Newtonsoft.Json;

Boolean dumpVersion = Argument("DumpVersion", false);

const String SlnPath = "./AppSea.sln";
const String BuildConfig = "Release";
const String ProjectStarted = "01012018";
const Int32 VMajor = 1;
const Int32 VMinor = 0;
const String DefaultTaskTarget = "Default";

const String Copyright = "Copyright © ";
String FixCopyright(String copy) {
    String[] splitted = copy.Split(' ');
    Int32 year;
    if (!Int32.TryParse(splitted[splitted.Length-1], out year))
        year = DateTime.Now.Year;
    return $"{Copyright} {year.ToString()}";
}

String GetBuildVersion(Int32 major, Int32 minor, DateTime projectStarted) {
    var date = DateTime.Now;
    var buildYearMo = new DateTime(date.Year, date.Month, 1);

    Int32 build = buildYearMo.Year;
    String revision1 = (Convert.ToInt32(buildYearMo.Subtract(projectStarted).TotalDays/30) + 1).ToString().PadLeft(2, '0');
    String revision2 = date.Day.ToString().PadLeft(2, '0');
    return $"{major}.{minor}.{build}.{revision1}{revision2}";
}

internal class AssemblyVersioningInfo {
    internal FilePath Path { get; set; }
    internal AssemblyInfoParseResult Info { get; set; }
}

internal void DumpVersion(ConvertableFilePath sln, String configuration, DateTime projectStarted, Int32 major, Int32 minor) {
    String version = GetBuildVersion(major, minor, projectStarted);
    SolutionParserResult slnInfo = ParseSolution(sln);
    var vInfos = slnInfo
        .Projects
        .Where(p => p is SolutionProject)
        .Select(p => p.Path.GetDirectory().Combine("properties").CombineWithFilePath("AssemblyInfo.cs"))
        .Where(p => System.IO.File.Exists(p.FullPath))
        .Select(p => new AssemblyVersioningInfo { Path = p, Info = ParseAssemblyInfo(p) })
        .ToList();

    vInfos
        .ForEach(asm => {
            CreateAssemblyInfo(asm.Path, new AssemblyInfoSettings {
                CLSCompliant = asm.Info.ClsCompliant,
                Company = asm.Info.Company,
                ComVisible = asm.Info.ComVisible,
                Configuration = configuration,
                Copyright = FixCopyright(asm.Info.Copyright),
                Description = asm.Info.Description,
                FileVersion = version,
                Guid = asm.Info.Guid,
                InformationalVersion = version,
                Product = asm.Info.Product,
                Title = asm.Info.Title,
                Trademark = asm.Info.Trademark,
                Version = version
            });
        });
}

Task("Clean")
    .Does(() => {
        CleanDirectories($"./**/obj/{BuildConfig}");
        CleanDirectories($"./**/bin/{BuildConfig}");
    })
    .ReportError(ex => Error(ex.Message));

Task("Restore")
    .Does(() => {
        GetFiles(SlnPath)
            .ToList()
            .ForEach(sln => NuGetRestore(sln));
    })
    .ReportError(ex => Error(ex.Message));

var buildTask = Task("Build");
if (dumpVersion) {
    Task("DumpVersion")
        .Does(() => DumpVersion(File(SlnPath), BuildConfig, DateTime.ParseExact(ProjectStarted, "ddMMyyyy", CultureInfo.InvariantCulture), VMajor, VMinor))
        .ReportError(ex => Error(ex.Message));

    buildTask.IsDependentOn("DumpVersion");
}

var cwd = Context.Environment.WorkingDirectory;
var buildInfo = new List<(String, String)>{
    ("v3.5", cwd.Combine("build\\net35").MakeAbsolute(Context.Environment).FullPath),
    ("v4.0", cwd.Combine("build\\net40").MakeAbsolute(Context.Environment).FullPath),
    ("v4.5", cwd.Combine("build\\net45").MakeAbsolute(Context.Environment).FullPath)
};

buildTask
    .Does(() =>
        buildInfo
            .ForEach(tup =>
                MSBuild(File(SlnPath), settings =>
                    settings
                        .SetConfiguration(BuildConfig)
                        .UseToolVersion(MSBuildToolVersion.VS2017)
                        .WithProperty("AllowUnsafeBlocks", "true")
                        .WithProperty("CLSCompliant", "false")
                        .WithProperty("Platform", "Any Cpu")
                        .WithProperty("TargetFrameworkVersion", tup.Item1)
                        .WithProperty("OutputPath", tup.Item2)
                )
            )
    )
    .ReportError(ex => Error(ex.Message));

Task("UnitTest")
    .Does(() => MSTest($"./**/bin/{BuildConfig}/*Test.dll"))
    .ReportError(ex => Error(ex.Message));

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTest")
    .ReportError(ex => Error(ex.Message));

RunTarget(DefaultTaskTarget);
