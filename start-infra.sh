#!/bin/bash
cd "$(dirname "$0")/docker"

docker compose -p aips --env-file ../.env up
