@echo off
echo Debug 5.3 AnyCPU
start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug 5.3|AnyCPU" /out log.txt

echo Debug 5.3 x86
start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug 5.3|x86" /out log.txt

echo Debug 5.3 x64
start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug 5.3|x64" /out log.txt

echo Debug 5.4 AnyCPU
start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug 5.4|AnyCPU" /out log.txt

echo Debug 5.4 x86
start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug 5.4|x86" /out log.txt

echo Debug 5.4 x64
start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug 5.4|x64" /out log.txt

echo Debug 5.6 AnyCPU
start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug 5.6|AnyCPU" /out log.txt

echo Debug 5.6 x86
start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug 5.6|x86" /out log.txt

echo Debug 5.6 x64
start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug 5.6|x64" /out log.txt

REM echo Debug x64
REM start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug|x64" /out log.txt

REM echo Debug x86
REM start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Debug|x86" /out log.txt

REM echo Release AnyCPU
REM start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Release|AnyCPU" /out log.txt

REM echo Release x64
REM start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Release|x64" /out log.txt

REM echo Release x86
REM start /wait "" "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "K2Spy.sln" /rebuild "Release|x86" /out log.txt