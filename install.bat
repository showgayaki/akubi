@echo off

@REM 管理者で実行する
@REM https://qiita.com/kokosan60/items/a135f0e59e2790db36a3
net session >NUL 2>nul
if %errorlevel% neq 0 (
    @powershell start-process %~0 -verb runas
    exit
)

set SERVICE_NAME=akubi

@REM 同じディレクトリに実行ファイルがあるかチェック
@REM ある：本番環境
@REM ない：開発環境
@REM を想定
cd /d %~dp0
if exist %SERVICE_NAME%.exe (
    set EXE_DIR=%cd%
) else (
    set EXE_DIR=%cd%\akubi\bin\Debug
)

@REM すでにインストールされているかチェック
sc query %SERVICE_NAME% | findstr %SERVICE_NAME%

if %ERRORLEVEL%==0 (
    echo %SERVICE_NAME%はすでにインストールされています
    echo %SERVICE_NAME%をアンインストールします

    sc stop %SERVICE_NAME%
    sc delete %SERVICE_NAME%
    call:install
) else (
    echo %SERVICE_NAME%はインストールされていません
    call:install
)

pause


:install
echo %SERVICE_NAME%をインストールします

sc create %SERVICE_NAME% binPath=%EXE_DIR%\%SERVICE_NAME%.exe start=auto
exit /b

