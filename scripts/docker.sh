#!/bin/zsh

cd ../docker &&
#docker compose -p tradelens down --volumes --rmi all &&
docker compose -p tradelens up -d --force-recreate
