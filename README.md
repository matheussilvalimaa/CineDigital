# 🎥 CineDigital

**CineDigital** is a name that i gave for that robust cinema ticket reservation system that i developed using ASP.NET Core and Entity Framework Core, with PostgreSQL as the database. The application offers comprehensive functionalities for managing users, movies, showtimes, theaters, reservations, and payments integrated with Stripe API.

## ✨ Features

- **Auhentication and Autorization:**
    - User registration and login with JWT and roles (*User* and *Admin*).
- **Movie Management:**
    - Complete CRUD operations for movies, including search with filters.
- **Theater Management:**
    - CRUD operations for theaters and seat management.
- **Showtime Management:**
    - CRUD operations for movie showtimes.
- **Seat Reservations:**
    - Reserve available seats for specific showtimes with payment processing.
- **Containeirization:**
    - Easy deployment and scalability.

## 🚀 Technologies

- **Backend**
    - ASP.NET Core 8.0.
- **ORM**
    - Entity Framework Core.
- **Database**
    - PostgreSQL.
- **Authentication**
    - JWT (JSON Web Tokens).
- **Payment processing**
    - Stripe.
- **Containerization**
    - Docker, Docker-Compose.

## 🔧 Prerequisites

Before you begin, ensure you have met the following requirements:
- [Docker](https://www.docker.com/get-started) (includes Docker-Compose)
- [Git](https://git-scm.com/downloads)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) *(for local development without Docker)*

## 🛠️ Installation

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/matheussilvalima/CineDigital.git
   cd CineDigital

2. **Set Up Environment Variables:**

Duplicate the .env.example file and rename it to .env, and then configure the necessary variables:

```bash
  cp .env.example .env

  //.env
  POSTGRES_USER=cineUser
  POSTGRES_PASSWORD=cinePass
  POSTGRES_DB=cinedb
  CONNECTION_STRING="Host=localhost;Database=cinedb;Username=cineUser;Password=cinePass"
 
  STRIPE_SECRET_KEY=sk_test_...
  STRIPE_PUBLISHABLE_KEY=pk_test_...
  JWT_SECRET_KEY=SuaChaveSecretaParaJWT
```
## 🏃 Running the Application

1. **Running with Docker**

With Docker and Docker Compose installed, follow these steps to run the application:

- *Build and Start Containers*
```bash
  docker-compose up -d --build
```

- *Verify Running Containers*
```bash
  docker-compose ps
```

- *Monitor Logs*
```bash
  docker-compose logs -f
```

- *Access the Application*
    - Swagger UI: http://localhost:5032/swagger

- *Stopping the Services*
```bash
  docker-compose down
```

2. **Running Locally without Docker**

You can run locally without Docker:

- *Install [PostgreSQL](https://www.postgresql.org/download/) and execute it*

- *Copy the .env.example to .env:*
```bash
  cp .env.example .env
```

- *Edit the .env file with your configurations*

- *Apply your migrations*
```bash
  dotnet tool install --global dotnet-ef
  dotnet ef migrations add InitialCreate
  dotnet ef database update
```

- *Start the application*
```bash
  dotnet run
```
  - It will be available at http://localhost://5146.

## 📝 Usage

1. **Accessing Swagger UI:**

- *Open your Browser:*
    - Navigate to http://localhost:5146/swagger to access the API test environment.

- *Explore the Endpoints*
    - Browse through the available controllers and endpoints. You can test each endpoint directly from the Swagger UI by providing the necessary parameters and request bodies.

2. **Authentication**

- *Register a New User:*
    - Use the endpoint POST /api/user/register.
    - Provide Name, Email and Password in the request body.

- *Login*
    - Use the endpoint POST /api/user/login
    - Provide the Email and Password that you had created to receive a JWT token.

- *Authorize in Swagger*
    - Click on the "Authorize" button in the Swagger UI.
    - Enter the JWT token in the format: Bearer {your_jwt_token}.
    - Click "Authorize" and then "Close".

- *Access Protected Endpoints*
    - With the token authorized, you can now access endpoints that require authentication or specific roles.

3. **Testing the API**

- *Select an Endpoint*
    - Expand the desired controller.
    - Click on the endpoint you wish to test.

- *Provide Required Parameters*
    - For POST requests, fill in the required JSON body with appropriate data.
    - For endpoints with path parameters, provide the necessary ID.

- *Execute the Request*
    - Click the "Try it out" button.
    - After filling in the parameters, click the "Execute" button.
    - Observe the response returned by the API in the "Responses" section below.

## 🤝 Contributing

Contributions are welcome! Feel free to fork the project and contribute.







