## Tech Academy of Programming

This is WebAPI application to manage users (Register, Login etc...) built with .Net 8.0

## Setting up the project
1. Open the app in **Visual Studio** or **Visual Studio Code**.
2. Run command **dotnet restore** to download and use all the packages needed to run the app.
3. Run command **dotnet ef database update** to create a database locally with the name **TAOP_DB**.
4. Run the app.
5. After first run of the app it will seed the database with some users data.
6. You can now use the API endpoints.

## Information about the app
- There are 10 records of user data and two records of user roles data (Admin, User) in the database.
- Each API endpoint is documented and have example of request bodies and responses.
- Dependency injection is used to implement the functionality of the APIs.
- Login user endpoint will generate json web token for the user which is valid for 1 day and can be used by clicking Authorize button at the top right corner of the app and pasting it in the box.
- Register endpoint will create a user and add it to user role by default.
- getUsers endpoint has pagination and can take two numbers (size for number of users to be returned, page for the page number).
- getProfile endpoint will take a Guid which represents user id and returns the specific user data.
