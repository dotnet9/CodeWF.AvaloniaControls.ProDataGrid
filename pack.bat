@echo off
setlocal

pushd "%~dp0"

set "CONFIGURATION=Release"
set "ARTIFACTS_DIR=%CD%\artifacts"
set "PACKAGES_DIR=%ARTIFACTS_DIR%\packages"

if exist "%PACKAGES_DIR%" rmdir /s /q "%PACKAGES_DIR%"
mkdir "%PACKAGES_DIR%"

echo [1/4] Restoring solution...
dotnet restore CodeWF.AvaloniaControls.ProDataGrid.slnx
if errorlevel 1 goto :error

echo [2/4] Building solution...
dotnet build CodeWF.AvaloniaControls.ProDataGrid.slnx -c %CONFIGURATION% --no-restore
if errorlevel 1 goto :error

echo [3/4] Packing library...
dotnet pack "src\CodeWF.AvaloniaControls.ProDataGrid\CodeWF.AvaloniaControls.ProDataGrid.csproj" -c %CONFIGURATION% --no-build -o "%PACKAGES_DIR%"
if errorlevel 1 goto :error

echo [4/4] Packing theme library...
dotnet pack "src\CodeWF.AvaloniaControls.ProDataGrid.Themes\CodeWF.AvaloniaControls.ProDataGrid.Themes.csproj" -c %CONFIGURATION% --no-build -o "%PACKAGES_DIR%"
if errorlevel 1 goto :error

echo.
echo Packages are available in:
echo %PACKAGES_DIR%

popd
exit /b 0

:error
set "EXIT_CODE=%ERRORLEVEL%"
echo.
echo Pack failed with exit code %EXIT_CODE%.
popd
exit /b %EXIT_CODE%
