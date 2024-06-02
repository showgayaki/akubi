@echo off

@REM �Ǘ��҂Ŏ��s����
@REM https://qiita.com/kokosan60/items/a135f0e59e2790db36a3
net session >NUL 2>nul
if %errorlevel% neq 0 (
    @powershell start-process %~0 -verb runas
    exit
)

set SERVICE_NAME=akubi

@REM �����f�B���N�g���Ɏ��s�t�@�C�������邩�`�F�b�N
@REM ����F�{�Ԋ�
@REM �Ȃ��F�J����
@REM ��z��
cd /d %~dp0
if exist %SERVICE_NAME%.exe (
    set EXE_DIR=%cd%
) else (
    set EXE_DIR=%cd%\akubi\bin\Debug
)

@REM ���łɃC���X�g�[������Ă��邩�`�F�b�N
sc query %SERVICE_NAME% | findstr %SERVICE_NAME%

if %ERRORLEVEL%==0 (
    echo %SERVICE_NAME%�͂��łɃC���X�g�[������Ă��܂�
    echo %SERVICE_NAME%���A���C���X�g�[�����܂�

    sc stop %SERVICE_NAME%
    sc delete %SERVICE_NAME%
    call:install
) else (
    echo %SERVICE_NAME%�̓C���X�g�[������Ă��܂���
    call:install
)

pause


:install
echo %SERVICE_NAME%���C���X�g�[�����܂�

sc create %SERVICE_NAME% binPath=%EXE_DIR%\%SERVICE_NAME%.exe start=auto
exit /b

