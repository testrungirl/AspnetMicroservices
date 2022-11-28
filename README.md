# AspnetMicroservices

##Docker Challenges
 **I realized I encountered challenges when I ran the docker-compose.yml and docker-compose.override.yml files <br/>
 using  this command: <pre>docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d</pre>
 The service (Api Project) was not containerized with the set port<br/>
 when I tried to build the services(Basket.Api and Catalog.Api) using Docker on Visual Studio, Error response from daemon: <br/>
 driver failed programming external connectivity on endpoint basketdb (68a0d0071a97 Bind for 0.0.0.0:6379 failed:<br/>
 port is already allocated If the error persists, try restarting Docker Desktop.
 <p>Eventually fixed this issue by removing all the containers on local desktop using <pre>rm -f $(docker ps -aq)</pre>. I don't think this is the best way around the issue</p>

 