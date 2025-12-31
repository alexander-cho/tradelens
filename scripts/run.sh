#!/bin/zsh

cd ../docker &&
docker compose -p tradelens up -d --force-recreate
