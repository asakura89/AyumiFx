@echo off

set appname=Maaya
set cwd=%CD%
set outputdir=%cwd%\build
set commonflags=/p:Configuration=Release;AllowUnsafeBlocks=False /p:CLSCompliant=True /p:Platform="Any Cpu" /p:langversion=6

set nugetversion=latest
set cachednuget=%LocalAppData%\NuGet\nuget.%nugetversion%.exe
set msbuild="C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
goto build

:build-error
echo Failed to compile.
goto exit

:build
echo ---------------------------------------------------------------------
echo Building AnyCpu release...
%msbuild% %appname%.sln %commonflags% /p:TargetFrameworkVersion=v4.5 /p:OutputPath="%outputdir%\net45"
if errorlevel 1 goto build-error

:done
echo.
echo ---------------------------------------------------------------------
echo Compile finished.
echo.

if exist %cachednuget% goto copynuget
echo Downloading latest version of NuGet.exe...
if not exist %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/%nugetversion%/nuget.exe' -OutFile '%cachednuget%'"
goto createpackage

:copynuget
if exist .nuget\nuget.exe goto createpackage
md .nuget
copy %cachednuget% .nuget\nuget.exe > nul

:createpackage
echo Creating nuget package...
.nuget\nuget.exe pack %appname%.nuspec -OutputDirectory "%outputdir%"
cd %cwd%
goto exit

:exit