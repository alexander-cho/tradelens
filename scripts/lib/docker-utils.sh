docker_compose_up() {
  docker compose -p tradelens up -d --force-recreate
}

docker_compose_down() {
  # need to check for any images currently in use 
  docker compose -p tradelens down --volumes --rmi all
}