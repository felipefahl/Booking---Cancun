## Acenario

### Overview

Implementation a booking API for the very last hotel in Cancun assuming the scenario of Post-Covid situation. People are now free to travel everywhere but because of the pandemic, a lot of hotels went bankrupt. Some former famous travel places are left with only one hotel. Taking in count the following requirements:
- API will be maintained by the hotel’s IT department.
- As it’s the very last hotel, the quality of service must be 99.99 to 100% => no downtime
- For the purpose of the test, we assume the hotel has only one room available
- To give a chance to everyone to book the room, the stay can’t be longer than 3 days and can’t be reserved more than 30 days in advance.
- All reservations start at least the next day of booking,
- To simplify the use case, a “DAY’ in the hotel room starts from 00:00 to 23:59:59.
- Every end-user can check the room availability, place a reservation, cancel it or modify it.
- To simplify the API is insecure.

---

### Docker
The implementation of this project was done in 5 separate Docker containers that holds:

- Net Core 6
- Zookeeper
- Microsoft SQL database
- Kafka
- Redis

---

### Run Steps

You’ll need to open a terminal and navigate to the solution root folder. Once you’re there, you just need to run the following command:

```
$ docker-compose up -d
```

When you're done using the app, you can shut it down by running the following command

```
$ docker-compose down
```

Once the project is up and running you can access the following URLs:

- [Booking Cancun API Documentation](https://localhost:80/index.html)

---

### Application Related Information

#### Email

To receive a notification, it is necessary to have a email provider or a email sandbox service like [Mailtrap](https://mailtrap.io/)

##### Email notification (mailtrap.io) for the situations below:
 - Booking Cancelled
 - Booking Booked
 - Booking Denied
 - Booking Update Denied
 - Booking Updated

#### Microsoft SQL database

- Host: *Server=localhost,1401*
- Database: *master*
- User: *sa*
- Password: *mssql1Ipw;*
