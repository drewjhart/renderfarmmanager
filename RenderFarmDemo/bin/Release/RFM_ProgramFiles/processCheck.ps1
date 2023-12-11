param (
[int]$processID = 1 
)
[int] $affinity = (Get-Process -Id $processID).processorAffinity
return $affinity



