docker container kill $(docker container ls -a -q)
docker container rm $(docker container ls -a -q)

docker run --rm -p 8081:8081 -d -e ASPNETCORE_ENVIRONMENT=Development benday.presidents.webui:latest