# DockerizedEventStoreSample

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

