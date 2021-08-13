@echo off
pushd %~dp0
SET ThisScriptsDirectory=%~dp0

SET PowerShellScriptPath=%ThisScriptsDirectory%processCheck.ps1
for /f "usebackq delims=" %%a in (`powershell -NoProfile -ExecutionPolicy Bypass -Command "& '%PowerShellScriptPath%' -processID %1"`) do SET "value=%%a"


> temptext.txt ECHO %Value%