Param(
[int]$adjustNum,
[int]$ProcessID
)
Write-Output $adjustNum
Write-Output $ProcessID
$thisProcess = Get-Process -Id $ProcessID
Write-Output $thisProcess
$thisProcess.ProcessorAffinity=$adjustNum 