REM Create a 'GeneratedReports' folder if it does not exist
if not exist "%~dp0GeneratedReports" mkdir "%~dp0GeneratedReports"
 
REM Remove any previous test execution files to prevent issues overwriting
IF EXIST "%~dp0PasswordVault.Services.trx" del "%~dp0PasswordVault.Services.trx%"
 
REM Remove any previously created test output directories
CD %~dp0
FOR /D /R %%X IN (%USERNAME%*) DO RD /S /Q "%%X"
 
REM Run the tests against the targeted output
call :RunOpenCoverUnitTestMetrics
 
REM Generate the report output based on the test results
if %errorlevel% equ 0 (
 call :RunReportGeneratorOutput
)
 
REM Launch the report
if %errorlevel% equ 0 (
 call :RunLaunchReport
)
exit /b %errorlevel%
 
:RunOpenCoverUnitTestMetrics
"%~dp0packages\OpenCover.4.7.922\tools\OpenCover.Console.exe" ^
-register:path32 ^
-target:"F:\Program Files\Microsoft Visual Studio\2019\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" ^
-targetargs:"%~dp0PasswordVault.ServicesTests\bin\Debug\PasswordVault.ServicesTests.dll /logger:trx;LogFileName=%~dp0PasswordVault.Services.trx" ^
-filter:"+[PasswordVault.Services*]* -[PasswordVault.ServicesTests]*" ^
-mergebyhash ^
-skipautoprops ^
-output:"%~dp0\GeneratedReports\PasswordVault.Services.xml"
exit /b %errorlevel%
 
:RunReportGeneratorOutput
"%~dp0\packages\ReportGenerator.4.3.0\tools\net47\ReportGenerator.exe" ^
-reports:"%~dp0\GeneratedReports\PasswordVault.Services.xml" ^
-targetdir:"%~dp0\GeneratedReports\ReportGenerator Output"
exit /b %errorlevel%
 
:RunLaunchReport
start "report" "%~dp0\GeneratedReports\ReportGenerator Output\index.htm"
exit /b %errorlevel%