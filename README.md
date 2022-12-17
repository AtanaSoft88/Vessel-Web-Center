# Vessel Web Center
That's my final project based on SoftUni ASP.NET CORE Web course requirements.

# Description & Project Introduction 
The Project follows the best practices for Object Oriented design and high-quality code for the Web application.
Application is designed for education only and any coincidence of names, mathematical measurements or anything in connection
with the real world objects shall be deemed as formal and not on purpose.
This project shows small amount of the following:
- Choose your favourite Vessels and make a voyage around the globe.
- Get the Vessels from their starting position to another ( limited positions at this time) , by getting very close and sharp mathematical
calculations. Thus you have idea of what distance and how much time will the voyage take to be done.
-All Vessels have their own Crew and capability of collecting or removing such.
-The limits are imposed of minimum 15 Crew members and maximum 25 in order to get any Vessel on her intended voyage.
-There are statistics of the current Port congestion and traffic of vessels inside the Port and the Top 10 of it.
-There are statistics of the most visited Ports by Vessels, as that can be easily changed dynamically by doing voyages with your favourite vessels.
-A Crew member can be Add to or removed from any vessel, as well as saving it in database for late assignment.
-Destinations are also  limited and pure distance can be obtained between two different ports.
-Application keeps track of the distances of the any vessel and also the total distance sailed by all vessels part of a given company.

# Built using the following:
-ASP.NET Core 6 MVC
-MSSQL Server
-Entity Framework Core
-AJAX
-jQuery
-Moq
-NUnit tests
-Bootstrap
-DataTables
-High Charts - Diagram Implementation
-Toast messages

Getting started...
*You have to register a User. By default after log/in/out you are given full control
and permission of this Application users,roles and actions.
*Most of the Entities are seeded and some of the Vessels already have a Crew capable of doing their first international Voyage!
*If you want more vessels enabled for sailing you have to hire more crew members on board of these vessels.
*All subsequent users registered need to be given a role or they will be without such.
*The following roles can be used : Administrator, User-Owner and Ordinary-User.There is a menu where this can be done.
*Only first 2 roles can have the big interaction and test the full application potential.The third one only can observe partial
information and can not be part of the big deal:)
*Not logged in users have almost no point of using this application ( don't be one of them) :)

# Data Base Diagram
![title](Image/diagram.png)

