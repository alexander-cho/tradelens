#!/bin/bash

source ./lib/docker-utils.sh
source ./lib/logging.sh

cd ../docker &&

while true; do
  echo "Docker menu:"
  echo "1: Run tradelens (compose up)"
  echo "2: Stop current (compose down)"
  echo "3: List containers"
  echo "4: Remove a container"
  echo "0: Exit"
  
  
  read -p "Enter your choice: " choice

  if [ "$choice" -eq 1 ]; then
    echo "Starting and running containers for tradelens..."
    docker_compose_up
        
  elif [ "$choice" -eq 2 ]; then
    echo "Shutting down all containers for tradelens and removing volumes/images..."
    docker_compose_down
    
  elif [ "$choice" -eq 3 ]; then
    echo "Containers:"
    log_info
    docker ps
      
  elif [ "$choice" -eq 4 ]; then
    read -p "Enter container name to remove: " container_name
    docker rm "$container_name"
    echo "Container '$container_name' deleted."
      
  elif [ "$choice" -eq 0 ]; then
    echo "Exiting..."
    break
    
  else
    echo "Invalid choice. Please select a valid option."
    
  fi
  
done
