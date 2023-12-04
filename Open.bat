@ echo off
%1 %2
mshta vbscript:createobject("shell.application").shellexecute("%~s0","goto :runas","","runas",1)(window.close)&goto :eof
:runas

set /p user=username:
start cmd /c net localgroup administrators esptals\%user% /add
start cmd /c net user %user% /active:yes
echo your account $user$ has administrator rights enabled
echo Enter your account username and password in the pop-up windows when opening Unlocker.exe
pause
