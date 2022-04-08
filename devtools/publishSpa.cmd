cd ..\frontend
call yarn install
call npm run build
call xcopy /E /Y .\dist\*.* ..\..\PublishedServices\MyFinances\Spa\
cd ..\devtools