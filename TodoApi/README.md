# Todo API Application

This is a sample application that demonstrates how to build a simple RESTful API that manages todo items using ASP.NET Core, EF (Entity Framework), and MySQL.

## How to Run the Application

1. Clone the repository: `git clone https://github.com/ajeetsingh02/dot-net-core-examples.git`
2. Navigate to the TodoApi folder: `cd dot-net-core-examples/TodoApi`
3. Open the appsettings.json file and enter your MySQL connection string.
4. Run the application: `dotnet run`
5. Make a HTTP GET request to the `/api/todoitems` endpoint to retrieve a list of all todo items.
6. Use a tool like Postman to test the other endpoints.

## Endpoints

The API contains the following endpoints:

1. `GET /api/todoitems` - Returns a list of all todo items in the database.
2. `GET /api/todoitems/{id}` - Returns a single todo item by its ID.
3. `POST /api/todoitems` - Creates a new todo item.
4. `PUT /api/todoitems/{id}` - Updates an existing todo item.
5. `DELETE /api/todoitems/{id}` - Deletes a todo item by its ID.

## Technical Details

The API is built using ASP.NET Core and Entity Framework. The `TodoItem` model is stored in a MySQL database using EF. The API endpoints are implemented using a `TodoItemsController` class that accepts HTTP requests and returns appropriate responses.

The `DataContext` class sets up the database context for the application, and the `appsettings.json` file stores the database configuration.

The API can be tested using a tool like Postman or by writing automated tests using a testing framework like NUnit or xUnit.

- Was it easy to complete the task using AI? -No
- How long did task take you to complete? 3 hours
- Was the code ready to run after generation? What did you have to change to make it usable? Maybe, yes. Nothing.
- Which challenges did you face during completion of the task? ChatGPT has a short memory.
- Which specific prompts you learned as a good practice to complete the task? -
