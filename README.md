Compile:
SET DOTNET_PATH=c:\Windows\Microsoft.NET\Framework\v4.0.30319\
%DOTNET_PATH%csc.exe /target:winexe /optimize /out:nosleep.exe /win32icon:icon.ico *.cs

Right click on tray icon to exit.