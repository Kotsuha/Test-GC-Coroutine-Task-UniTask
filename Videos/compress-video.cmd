@pushd "%~dp0"
@set "input=%~nx1"
@set "output=%~n1.mp4"
@echo input: %input%
@echo output: %output%
@pause
ffmpeg -i "%input%" -vcodec libx265 -an -crf 30 "%output%"
