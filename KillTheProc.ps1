New-Item -Path HKCU:\Software\PowershellScriptRunTime -Value $(Get-Date -format "yyyy-MM-dd HH-mm") -Force | Out-Null
$Input = Read-Host 'Input process name or PID'


$Process = Get-Process  $Input* -ErrorAction Ignore
$ProcessId = Get-Process -Id $InputInt -ErrorAction Ignore


if(($Process) -eq $null)
{$InputInt = [int]$Input
$ProcessId
if($ProcessId -eq $null){'Tokia Operacija Nerasta'}
}
else{$Process}

if($InputInt -eq $null){Stop-Process -name $Input* -Confirm}
else{Stop-Process -Id $InputInt -Confirm}


 While ($true) {

$counter = (Get-ChildItem C:\Windows\Logs\FilteredProcessList\| Measure-Object).Count

while(5 -le $counter){
Write-Host 'Senas Logas istrinamas'
Get-ChildItem C:\Windows\Logs\FilteredProcessList\ -Recurse| sort CreationTime -Descending | select -Last 1 | Remove-Item 
$counter = $counter-1
}

            $CurrentTime = Get-Date -format "yyyy-MM-dd_HH_mm_ss"
            $FilePath = "C:\Windows\Logs\FilteredProcessList\" + $CurrentTime + ".csv"
            "Logas irasomas..."
            Get-Process | Sort ws -Descending | Select Name, Id, WS | Export-Csv $FilePath -NoTypeInformation
            gci .\logs -Recurse | where{-not $_.PsIsContainer} | sort CreationTime -desc | select -Skip 5 | Remove-Item -Force
            Start-Sleep -Seconds 5
}

