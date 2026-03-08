@echo off
cd /d "%~dp0docker"

docker compose -p aips --env-file ..\.env up
