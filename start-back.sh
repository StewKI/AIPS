#!/bin/bash
cd "$(dirname "$0")/dotnet"

dotnet run --project AipsWebApi 2>&1 | sed "s/^/[WebApi]  /" &
dotnet run --project AipsRT 2>&1 | sed "s/^/[RT]     /" &
dotnet run --project AipsWorker 2>&1 | sed "s/^/[Worker] /" &

trap 'kill $(jobs -p) 2>/dev/null' EXIT
wait
