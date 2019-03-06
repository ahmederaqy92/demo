param(
[string]
  $containerId
)

docker inspect --format '{{ .NetworkSettings.Networks.nat.IPAddress }}' $containerId