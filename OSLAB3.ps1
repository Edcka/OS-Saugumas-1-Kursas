$cpuLoad = (Get-WmiObject win32_processor | Measure-Object -property LoadPercentage -Average | Select Average ).Average
$compInfo = Get-WmiObject -Class win32_operatingsystem -ErrorAction Stop
$totalMem = $compInfo.TotalVisibleMemorySize/1000
$freeMem = $compInfo.FreePhysicalMemory/1000
$compName= $compInfo |select -ExpandProperty __Server
$compDisk=Get-WmiObject -Class Win32_logicaldisk | Format-List -Property DeviceID,FreeSpace,Size,VolumeName
$currTime = Get-Date -format "yyyy-MM-dd_HH_mm_ss"

$params= @{
PcName = "$compName"
pcTotalMem = "$totalMem MB" 
pcFreeMem = "$freeMem MB"
pcCpuLoad = "$cpuLoad% "
}
$pcParams = New-Object psobject -Property $params
$pcParams,$compDisk | Out-File -FilePath C:\Users\Dede\Desktop\$CurrTime.txt

Get-Service | Where-Object {$_.Status -eq "Running"} | Out-File -FilePath C:\Users\Dede\Desktop\ServiceRunning_at_$CurrTime.txt
Get-Service | Where-Object {$_.Status -eq "Stopped"} | Out-File -FilePath C:\Users\Dede\Desktop\ServiceStopped_at_$CurrTime.txt

function FindServiceStatus{
Get-Service  $args[0] | Where-Object {$_.Status}
}

FindServiceStatus (Read-Host 'Enter Service name : ')

Get-EventLog * -Newest 10

function AppEventLog{
param([string]$appName,[int]$ammountOfLogs )
Get-EventLog $appName -Newest $ammountOfLogs | Out-File -FilePath C:\Users\Dede\Desktop\$appName'_'$currTime.txt
}

AppEventLog -appName (Read-Host 'Enter app name: ') -ammountOfLogs (Read-Host 'Enter the amount of Logs: ')


C:\Users\Dede\Downloads\RAMMap\RAMMap.exe | Out-File -FilePath C:\Users\Dede\Desktop\RAMMAP.RMP

$action = New-ScheduledTaskAction -Execute 'C:\Users\Dede\Desktop\OSLAB3.ps1' 
$trgger = New-ScheduledTaskTrigger -Daily -At 10:30pm

Register-ScheduledTask -Action $action -Trigger $trgger -TaskName "OSLAB3"
