@echo off

cd packages
for /R %%s in (*) do ( 
..\.nuget\nuget push %%s -s http://nuget.lunz.cn/ Lunz
) 

pause 