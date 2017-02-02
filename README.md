#  Submissive
## MVC ASP.NET Open Source Project
This project is an open source system for crud based admins,
You can use this project for instance cases which I described below:

* Responsive admin panel for crud base Systems
* Fully support of any language, specially RTL ones(You can use LTR on LTR branch though )
* Send & receive e-mail verification for sign up and forgot password feature.
* Dynamically role based system
* Great identity control with ASP.NET Identity 2.0

## Idea
Simply It's for managing the Crud operations of a database, and beside of that, you can put you're specific product within database with all details like the creator, time stamp and all, also on the Front-End side(Which is not implemented yet) 
you can see the products and do the advanced search also, so basically it's an intermediary between creator of  and the investors all around the world. below you can see the database structure
![Db Structure](http://3.1m.yt/VVm_Fn7.png)



# Technical Descriptions
## Models and the Database
I've done the whole model and view model with TDD approach, which you can see under the unit-test part. 
also in this project we made a use out of Repository pattern and unit of work, to separate the data access logic and business logic.
### Technologies for Database:
**Database Server**: SQL Server 2012 

**Database Operation Patterns and Systems**: Entity Framework, Repository, Unit of work, Linq

**Mapping**: FLUIT API,Some Asp Annotations
## Controllers and Procedures
### Membership
to solve site membership requirements which involved Forms Authentication, and a database for user names, passwords, and profile data.

beside, for handling External Logins, we used OWIN authentication .
### Roles
For controlling the data displayed and the functionality offered by a page, based on the user visiting we created a role-based system as you see in the image below also you can feel that it is more maintainable approach than using rules on a user-by-user basis:
![Roles](http://4.1m.yt/mOmgE.jpg)
### Dependency-Injection
We use dependency-injection as it is mainly suggested by MVC architecture, and also this purposes
* Reduce class coupling

* Increase code reusing

* Improving code maintainability

* Improving application testing

## Technologies for controllers and procedures

**For Authentication**: Asp.net Identity 2.0, OWIN authentication 

**For Roles**: Asp.net Role system

**For Dependency injection**: Autofac 
## Views
For the view part we used Flatlab, the fully responsive admin dashboard template built with Bootstrap 3.3.6 Framework, modern web technology HTML5 and CSS3.

And for the Components we used the set of great HTML 5 library of the telerik(Kendo UI) with jquery.

## Technologies for View

**For View**: Kendo-UI, HTML5, JQuery, CSS3, java script
## Unit-Testing

After finishing up each layer we done an integration testing, also All the repositories had been tested. the framework for unit-testing is an open source framework for all .NET languages "nUnit".
Besides we test all the database with some fake informations with faker library.
## Technologies for Unit Tests
**For Unit testing**: Nunit, Faker, FizzWare

# Setting up the project
### Adding default Role
With this seed code, we added a default role for Highest level of management with the code below, you can change it tho ;)

         `if (!context.Roles.Any(r => r.Name == "SuperAdmin")){ 
 
                var store = new RoleStore<ApplicationRole,int,ApplicationUserRole>(context); 

                var manager = new RoleManager<ApplicationRole,int>(store); 

                manager.Create(new ApplicationRole { Name = "SuperAdmin" ,BaseCoding=true,IsActive=true,Show=true}); 

            }`
### Adding default member and assign it to Role
We must have at least one member to use, below you can see the seed code for adding this member:

         `if (!context.Roles.Any(r => r.Name == "Member")){ 
 
                var store = new RoleStore<ApplicationRole, int, ApplicationUserRole>(context);

                var manager = new RoleManager<ApplicationRole, int>(store);

                manager.Create(ApplicationRole { Name = "Member", BaseCoding = false, IsActive = false, Show = true });
            }`

You can see all of this codes in "Configuration.cs"
At the end all you have to do is to change the "connection string" with changing the properties of the variable in the root web.config:

      `<connectionStrings> 
            <add name="[Name Here]" connectionString="Data Source=[Address HERE];Initial Catalog=[Your Catalog     HERE];Integrated Security=true" providerName="System.Data.SqlClient" /> 
       </connectionStrings>`

## Licence
The content of this project is Open-Source, and the underlying source code used to format and display the content is licensed under the [MIT](http://opensource.org/licenses/mit-license.php) license.



