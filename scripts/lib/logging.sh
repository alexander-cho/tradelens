log_timestamp() {
  date +"%Y-%m-%d %H:%M:%S"
}

log_info() {
  echo "$(log_timestamp) [INFO] $*"
}