[Unit]  
Description=App.Finances  
  
[Service]  
ExecStart=/home/pi/dotnet/dotnet /home/pi/publishedAPIs/App.Finances/App.Finances.dll --runAsDebianService --port 5000  
WorkingDirectory=/home/pi/publishedAPIs/App.Finances/
User=pi
Group=pi
Restart=on-failure  
SyslogIdentifier=dotnet-app-finances  
PrivateTmp=true  
  
[Install]  
WantedBy=multi-user.target
