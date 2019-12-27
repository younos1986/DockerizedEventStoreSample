# DockerizedEventStoreSample

### how to use bridge driver network in docker to communicate between two containers.

resources : 

<a href='https://www.docker.com/blog/understanding-docker-networking-drivers-use-cases/'> Understanding Docker Networking Drivers and their use cases  </a>

<a href='https://docs.docker.com/compose/compose-file/#ipam'> ipam  </a>




```

version: '3.4'

services:
  eventstoresample: 
    image: eventstoresample
    build:
      context: .
      dockerfile: EventStoreSample/Dockerfile
    networks:
      clusternetwork:
        ipv4_address: 172.16.0.12

  eventstore: 
    image: eventstore/eventstore
    environment:
      - EVENTSTORE_INT_IP=172.16.0.13
      - EVENTSTORE_EXT_HTTP_PORT=2113
      - EVENTSTORE_EXT_TCP_PORT=1113
      - EVENTSTORE_EXT_HTTP_PREFIXES=http://*:2113/
    ports:
      - "1113:1113"
      - "2113:2113"
    networks:
      clusternetwork:
        ipv4_address: 172.16.0.13

networks:
  clusternetwork:
    driver: bridge
    ipam:
      driver: default
      config:
      - subnet: 172.16.0.0/24

```


This project is creating two containers

first: eventstoresample API which can communicate with event store container
second: event store 


How to test: 

In postman make a POST call with the url of https://localhost:44318/api/Customers and the body should be raw JSON as:

```
{
    "FirstName": "Younos",
    "LastName": "Baghaei Moghaddam"
}

```

then open EventStore dashboard on `http://127.0.0.1:2113/`. You should be able to see the event in the `Stream Browser`


<img src='https://raw.githubusercontent.com/younos1986/DockerizedEventStoreSample/master/eventStore.png' />  

