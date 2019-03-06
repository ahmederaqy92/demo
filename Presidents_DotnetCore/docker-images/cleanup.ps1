docker container kill $(docker container ls -a -q)
docker container rm $(docker container ls -a -q)