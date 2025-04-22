# Order Management API

A lightweight system for managing products and orders, supporting discounts and accessible through a REST API.

---

Order Management API is a simple and extensible backend solution for retailers that allows:

- Product creation with optional discount rules
- Order placement with product quantities
- Full CRUD operations
- Dockerized deployment
- Automated schema migrations

---

## Built With

- [.NET 8](https://dotnet.microsoft.com/en-us/download)
- [Dapper](https://github.com/DapperLib/Dapper)
- [PostgreSQL](https://www.postgresql.org/)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [Docker & Docker Compose](https://docs.docker.com/)
- [xUnit](https://xunit.net/)
- [Swagger/OpenAPI](https://swagger.io/)

---

## Getting Started

### Prerequisites

Before running the project, make sure you have:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Installation

1. Clone the repo:

   ```bash
   git clone https://github.com/<your-username>/order-management.git
   cd order-management
   ```

2. Run the app with Docker:

   ```bash
   docker compose up --build
   ```

   The API will be available at [http://localhost:5000](http://localhost:5000)

---

## Usage

Use Swagger to explore and test available endpoints.

When running locally in `Development` mode, access it at:

```
http://localhost:5000/swagger
```

Example use cases:

- **Create product**
- **Apply product discounts**
- **Place order**
- **Get all orders**
- **Get order invoice data**
- **Get order discounted products**

---

## Tests

Run unit tests with:

```bash
dotnet test
```

---

## Roadmap

- [x] Product creation
- [x] Discount logic
- [x] Orders and quantities
- [x] Input validation
- [x] API documentation
- [x] Docker-based deployment
- [x] CI/CD (GitHub Actions)

---

## Room for improvements

```text

- Since project was made using web application init setup, should consider removing all front-end related usages.
- Add pagination support for endpoints like GetOrders and GetFilteredProducts.
- Add validation tests to verify rules such as preventing negative prices.
- Return total item count alongside paged responses for frontend UI pagination.
- Add integration tests using TestServer and a real or in-memory database.
- Add production-hardening features like logging, security headers.
```


## Contact

**Mantas Jarašūnas**  
jarasunas29@gmail.com 
[GitHub Profile](https://github.com/mantasjarasunas)

---
