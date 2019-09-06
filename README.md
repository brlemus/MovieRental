# MovieRental
In order to test the API you can enter the url: https://movierentalbr.herokuapp.com/swagger/index.html where you will find the swagger documentation of each of the EndPoint for both the logging part, the logs and the requirements For the movies. There are two types of users to log in, which are:

1) Administrator Type:
user: "admin"
pass: "admin01"

2) User Type
user: "user"
pass: "user01"

With respect to the database it is automatically generated in memory or you can download the project and at the time of execution you can change the data source of the appsettings.Development.json file, in my case I use SQL Server SQLEXPRESS and my data source is as follows form "Data Source = localhost \\ SQLEXPRESS; Initial Catalog = MovieDb; Integrated Security = True" does not have to execute any scripts since, the project is configured to create the database and its respective tables automatically.

On the other hand, the API must be tested with Postman as indicated, then an example of the url of one of the endpoints to be consumed from this tool:

https://movierentalbr.herokuapp.com/Movies/AllMovie.
