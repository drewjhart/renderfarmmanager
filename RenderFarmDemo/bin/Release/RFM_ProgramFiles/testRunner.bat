@echo off
for /f %%i in ('powershell -File C:\IBI\affinityAdjust\processTest.ps1') do echo %%i
pause