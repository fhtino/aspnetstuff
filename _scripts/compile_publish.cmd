@echo off
@echo ##### SETUP VISUAL ENVIRONMENT using vswhere.exe (https://github.com/microsoft/vswhere) 
FOR /f "tokens=*" %%i in ('"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere" -products * -latest -property installationPath') do set MYVSPATH=%%i
ECHO MYVSPATH = %MYVSPATH%
CALL "%MYVSPATH%"\Common7\Tools\VsDevCmd.bat -no_ext
PAUSE

@echo ##### REMOVING TEMP FOLDER
RMDIR .\out_compile /s /q
PAUSE

@echo ##### NUGET - RESTORE
nuget restore ..\AspNetStuff.sln
PAUSE

@echo ##### MSBUILD.EXE
MSBUILD.EXE compile_publish.xml
PAUSE

@echo ##### Assembly INFO
powershell [System.Reflection.Assembly]::LoadFrom('out_compile\OwinAuthSimple\bin\OwinAuthSimple.dll').GetName().Version
PAUSE