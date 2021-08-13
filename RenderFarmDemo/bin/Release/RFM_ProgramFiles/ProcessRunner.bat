@echo off
pushd %~dp0
SET a=%1
SET p=%2
SET ThisScriptsDirectory=%~dp0
SET PowerShellScriptPath=%ThisScriptsDirectory%processAffinity.ps1
PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& '%PowerShellScriptPath%'" -adjustNum %a% -ProcessID %p% 
