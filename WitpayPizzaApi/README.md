# Witpay Pizza API

A RESTful ASP.NET Core 8 Web API for managing pizzas and toppings.

The API supports CRUD operations for toppings and pizzas, including a many-to-many relationship between pizzas and toppings. It also validates input and prevents duplicate pizza and topping names.

## Features

### Toppings

* List all toppings
* Add a new topping
* Update an existing topping
* Delete a topping
* Prevent duplicate topping names

### Pizzas

* List all pizzas with their toppings
* Add a new pizza with toppings
* Update pizza details
* Update toppings on an existing pizza
* Delete a pizza
* Prevent duplicate pizza names

## Quick Start

```bash
git clone <your-github-repository-url>
cd WitpayPizzaApi
dotnet restore
dotnet ef database update
dotnet run
```

After starting the API, open Swagger:

```text
https://localhost:<port>/swagger
```

Use Swagger to test all available endpoints.

> **Prerequisites:** .NET 8 SDK and the `dotnet-ef` CLI tool must be installed.

## Tech Stack

* **ASP.NET Core 8 Web API**
* **Entity Framework Core 8**
* **SQLite**
* **Swagger / OpenAPI**
* **C#**
* **EF Core Migrations**

## Project Structure

```text
WitpayPizzaApi/
├── Controllers/
│   ├── PizzasController.cs
│   └── ToppingsController.cs
├── Data/
│   └── WitpayDbContext.cs
├── DTOs/
├── Models/
│   ├── Pizza.cs
│   └── Topping.cs
├── Migrations/
├── Program.cs
├── WitpayPizzaApi.csproj
└── witpay.db
```

## Database Design

The application uses **SQLite** for a simple, local database that does not require a separate database server.

Pizzas and toppings have a **many-to-many relationship**:

```text
Pizza
  │
  ├──── PizzaTopping ──── Topping
  │
  └──── PizzaTopping ──── Topping
```

Entity Framework Core creates and manages the join table used for the relationship.

Unique indexes are also configured for pizza and topping names to help prevent duplicate values at the database level.

## Prerequisites

Make sure the following are installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Git
* EF Core CLI

Verify .NET:

```bash
dotnet --version
```

Verify EF Core CLI:

```bash
dotnet ef --version
```

If `dotnet ef` is not installed:

```bash
dotnet tool install --global dotnet-ef
```

## Getting Started

### 1. Clone the repository

```bash
git clone <your-github-repository-url>
```

### 2. Navigate to the project

```bash
cd WitpayPizzaApi
```

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Apply database migrations

The project uses EF Core migrations to create and update the SQLite database.

Run:

```bash
dotnet ef database update
```

## Running the API

Start the application with:

```bash
dotnet run
```

The terminal will display the URL where the API is running.

Open Swagger using:

```text
https://localhost:<port>/swagger
```

Swagger provides an interactive interface for testing all API endpoints.

## API Endpoints

### Toppings

| Method   | Endpoint                    | Description      |
| -------- | --------------------------- | ---------------- |
| `GET`    | `/api/Toppings`             | Get all toppings |
| `POST`   | `/api/Toppings`             | Create a topping |
| `PUT`    | `/api/Toppings/{toppingId}` | Update a topping |
| `DELETE` | `/api/Toppings/{toppingId}` | Delete a topping |

### Pizzas

| Method   | Endpoint                         | Description                     |
| -------- | -------------------------------- | ------------------------------- |
| `GET`    | `/api/Pizzas`                    | Get all pizzas with toppings    |
| `POST`   | `/api/Pizzas`                    | Create a pizza with toppings    |
| `PUT`    | `/api/Pizzas/{pizzaId}`          | Update pizza details            |
| `PUT`    | `/api/Pizzas/{pizzaId}/toppings` | Replace the toppings of a pizza |
| `DELETE` | `/api/Pizzas/{pizzaId}`          | Delete a pizza                  |

## Example Requests

### Create a Topping

**POST**

```http
/api/Toppings
```

Request body:

```json
{
  "name": "Cheese"
}
```

Expected response:

```json
{
  "id": "generated-guid",
  "name": "Cheese"
}
```

Status:

```text
201 Created
```

### Update a Topping

**PUT**

```http
/api/Toppings/{toppingId}
```

Request body:

```json
{
  "name": "Mozzarella"
}
```

Status:

```text
200 OK
```

### Delete a Topping

**DELETE**

```http
/api/Toppings/{toppingId}
```

Successful response:

```text
204 No Content
```

### Create a Pizza with Toppings

First create the toppings you want to use and copy their IDs.

**POST**

```http
/api/Pizzas
```

Request body:

```json
{
  "name": "Pepperoni Pizza",
  "toppingIds": [
    "topping-guid-1",
    "topping-guid-2"
  ]
}
```

Example response:

```json
{
  "id": "generated-pizza-guid",
  "name": "Pepperoni Pizza",
  "toppings": [
    {
      "id": "topping-guid-1",
      "name": "Cheese"
    },
    {
      "id": "topping-guid-2",
      "name": "Pepperoni"
    }
  ]
}
```

Status:

```text
201 Created
```

### Get All Pizzas with Toppings

**GET**

```http
/api/Pizzas
```

Example response:

```json
[
  {
    "id": "pizza-guid",
    "name": "Pepperoni Pizza",
    "toppings": [
      {
        "id": "topping-guid-1",
        "name": "Cheese"
      },
      {
        "id": "topping-guid-2",
        "name": "Pepperoni"
      }
    ]
  }
]
```

Status:

```text
200 OK
```

### Update Pizza Details

This endpoint updates the pizza details only, such as its name.

**PUT**

```http
/api/Pizzas/{pizzaId}
```

Request body:

```json
{
  "name": "Hawaiian Pizza"
}
```

Status:

```text
200 OK
```

### Update Pizza Toppings

This endpoint replaces the existing toppings assigned to a pizza.

**PUT**

```http
/api/Pizzas/{pizzaId}/toppings
```

Request body:

```json
{
  "toppingIds": [
    "topping-guid-1",
    "topping-guid-3"
  ]
}
```

Status:

```text
200 OK
```

### Delete a Pizza

**DELETE**

```http
/api/Pizzas/{pizzaId}
```

Successful response:

```text
204 No Content
```

## Validation and Error Handling

The API validates request data and returns meaningful HTTP status codes.

### `400 Bad Request`

Returned when the request contains invalid data, such as one or more topping IDs that do not exist.

Example:

```text
One or more toppings were not found.
```

### `404 Not Found`

Returned when the requested pizza or topping does not exist.

Examples:

```text
Pizza not found.
```

```text
Topping not found.
```

### `409 Conflict`

Returned when attempting to create or update a pizza/topping with a name that already exists.

Examples:

```text
Pizza already exists.
```

```text
Pizza name already exists.
```

```text
Topping name already exists.
```

## Testing the API

The easiest way to test the API is through Swagger.

After running the application:

```bash
dotnet run
```

open:

```text
https://localhost:<port>/swagger
```

### Recommended test order

1. Create several toppings using `POST /api/Toppings`.
2. Use the generated topping IDs when creating a pizza.
3. Create a pizza using `POST /api/Pizzas`.
4. Retrieve pizzas using `GET /api/Pizzas`.
5. Update pizza details using `PUT /api/Pizzas/{pizzaId}`.
6. Update pizza toppings using `PUT /api/Pizzas/{pizzaId}/toppings`.
7. Delete a pizza using `DELETE /api/Pizzas/{pizzaId}`.
8. Test duplicate names and invalid IDs to verify validation and error handling.

## HTTP Status Codes

| Status Code       | Usage                                     |
| ----------------- | ----------------------------------------- |
| `200 OK`          | Successful GET or update operation        |
| `201 Created`     | Successful creation of a pizza or topping |
| `204 No Content`  | Successful deletion                       |
| `400 Bad Request` | Invalid request data                      |
| `404 Not Found`   | Resource does not exist                   |
| `409 Conflict`    | Duplicate pizza or topping name           |

## Design Notes

### DTOs

Request and response DTOs are used instead of exposing EF Core entities directly. This keeps the API contract separate from the persistence models and allows the API to return only the required data.

### Many-to-Many Relationship

A pizza can have multiple toppings, and a topping can belong to multiple pizzas. Entity Framework Core manages this relationship through a join table.

### Duplicate Prevention

Duplicate pizza and topping names are checked in the application before saving. Unique database indexes are also configured for the `Name` fields.

### Separate Pizza Updates

Pizza details and pizza toppings are updated through separate endpoints:

```text
PUT /api/Pizzas/{pizzaId}
PUT /api/Pizzas/{pizzaId}/toppings
```

This keeps updating the pizza's own data separate from modifying its relationships.

## Notes

* No frontend application is included because the exercise requires a backend API only.
* SQLite is used to keep setup simple and avoid requiring a separate database server.
* Swagger is included for API exploration and testing.
