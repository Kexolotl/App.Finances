New-WebAppPool -Name UnmanagedPool -ErrorAction Ignore
Set-ItemProperty IIS:\AppPools\UnmanagedPool managedRuntimeVersion ""
Remove-Website -Name "MyFinances" -ErrorAction Ignore
New-Website -Name "MyFinances" -Port 28000 -PhysicalPath (Resolve-Path ..\..\PublishedServices\MyFinances\Spa) -ApplicationPool "UnmanagedPool" -Force