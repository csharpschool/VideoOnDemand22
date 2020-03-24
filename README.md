# VideoOnDemand22
This repository contains the code to the book ASP.NET Core 2.2 MVC, Razor Pages, API, JSON Web Tokens &amp; HttpClient

There have been some issues with the package references in the Common and Database projects that are fixed.

To run the downloaded or cloned code, do this:
1. Update to Visual studio 2019 (16.5.0), or install it here: https://visualstudio.microsoft.com/downloads/
2. Install the latest SDK for ASP.NET Core 2.2 (2.2.8) here: https://dotnet.microsoft.com/download/dotnet-core/2.2
3. When the installation is complete, download or clone the code in this repository.
4. Open the solution in VS 2019.
5. Run the update-database command in the Package Manager Console to create an empty database.
6. Open the Startup.cs file in the VOD.UI project.
7. Uncomment the following line of code in the Configure method: DbInitializer.Initialize(db);
8. Save all changes.
9. Close Visul Studio 2019 and reopen it (in some cases there might be an issue with IIS if you don't restart.)
10. Start the applicaiton. (This should add data to the database because you uncommented the code in the Configure method)
11. Stop the application and comment out the code you uncommented in the Configure method (step 7).
12. Start the application again and log in as the Admin user.

How to create a new admin user after the database has been seeded with data.
1. Register a new user with the site.
2. Open the SQL Server Explorer in Visual Studio 2019.
3. View the data in the AspNetUsers table.
4. Copy the user id from the user you created.
5. Open the AspNetUserRoles table.
6. Add a USER-ROLE combination for the id 1 (which is the admin role in the AspNetRoles table) and the user id you copied earlier.
7. Logout any user and restart the application if the browser is running the applicaiton.
8. You can now login as the new user as an admin.
