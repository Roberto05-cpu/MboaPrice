@echo off
REM Lance le backend (http://0.0.0.0:5080, Swagger sur http://localhost:5080/swagger)
cd /d "%~dp0MboaPrice.Api"
dotnet run
