# Microservice Architecture Roadmap

## 1. Project Goals

1.1. Understand what Microservice Architecture is like  
1.2. Use different tools to create the ecosystem  
1.3. Learn service communication methods  
1.4. Promote diversity in service implementations  
1.5. Complete a full ecosystem with all its parts  

---

## 2. What We Do

2.1. Forget what you know, change your perspective  
2.2. Treat everything as a separate project  
2.3. Write the same code for each project (challenge DRY)  
2.4. Keep it simple but elegant  
2.5. Develop different kinds of applications  
2.6. Keep some parts incomplete and return to complete them later  
2.7. Use many references from previous videos  

---

## 3. What We Don't Do

3.1. Don't be lazy  
3.2. Try not to be perfect  
3.3. Avoid getting lost in details  
3.4. Don't get stuck on UI design – it’s not our focus  

---

## 4. Reference Project

- **eShopOnContainers** reference application  
  GitHub: [https://github.com/dotnet/eShop](https://github.com/dotnet/eShop)

---

## 5. Why We Use This Reference

5.1. It is well known  
5.2. It is proven in production environments  
5.3. It is easy to access  
5.4. It has extensive documentation  

---

## 6. Our Project Structure

- **6.1. Web UI** – Blazor (Blazor WASM)  
- **6.2. API Gateway** – Aggregator  
- **6.3. Identity Service** – API  
- **6.4. Basket Service** – API  
- **6.5. Order Service** – API  
- **6.6. Catalog Service** – API  
- **6.7. Notification Service** – Console App  
- **6.8. Payment Service** – API  
- **6.9. Event Bus** – Pub/Sub Channel  

---

## 7. Tools

- RabbitMQ  
- Azure Service Bus  
- Redis  
- SQL Server  
- Blazor WASM  
- .NET 8 / .NET 9  
- Entity Framework  
- Graylog  

---

## 8. Patterns

- Domain-Driven Design (DDD)  
- Command Query Responsibility Segregation (CQRS)  
- Mediator Pattern (MediatR)  
- HTTP Aggregation  
- JSON Web Token (JWT)  
- Health Check  

---

## ✅ Finally

- **Architecture & Design**  
  - DDD, Onion Architecture, CQRS, MediatR

- **Frontend**  
  - Blazor Series (Authentication, Form Post)

- **Backend Best Practices**  
  - .NET Core Best Practices, Microservice Patterns

- **Logging**  
  - Serilog, Graylog

- **API Gateway**  
  - Ocelot

- **Messaging**  
  - RabbitMQ / Azure Service Bus

- **Deployment & Containers**  
  - Docker Series

- **Data Access**  
  - Entity Framework Series + Blazor EF