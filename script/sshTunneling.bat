@echo off
set /p openshiftssh= Inserta la direccion ssh de Openshift:
echo %openshiftssh%
set /p user= Inserta el usuario del servidor de Ramon:
echo %user%
start tunneling.bat %openshiftssh% %user%
set /p userOS= Inserta el usuario del openShift:

git clone ssh://%userOS%@localhost:2121/~/git/app.git/

pause