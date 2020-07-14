# Payment Gateway

This Payment Gateway is a core domain of the Stark Bank and contains the Payments bounded context. It allows Merchants to submit a payment request with the card number, expiry date, amount, currency and CCV provided by their shoppers and then later retrieve payment details using the unique payment id.

# TODO 

- [x] Payment Request
- [ ] Payment Details
- [ ] Authentication
- [ ] Client Library and Sample
- [ ] Healthcheck and monitoring

# Getting Started

## Prerequisites

To run this project locally you will need:

* .NET Core 3.1 ([available here](https://dotnet.microsoft.com/download/dotnet-core/3.1))
* Azure Cosmos DB Emulator ([available here](https://aka.ms/cosmosdb-emulator))

## Project Layout

* `/lib` - Contains class libraries which would be shared over many domains. Normally these would be made available via a NuGet feed.
* `/src`
  * `/src/Stark.PaymentGateway` - Contains the ASPNET Core host, providing HTTP API endpoints.
  * `/src/Stark.PaymentGateway.Application` - Contains the application layer for the Payment Gateway, such as the commands and command handlers.
  * `/src/Stark.PaymentGateway.Domain` - Contains the domain layer for the Payment Gateway, encapsulating any business logic and interfaces required to persist the domain model.
  * `/src/Stark.PaymentGateway.Infrastructure` - Contains the infrastructure layer for the Payment Gateway, including the implementation of any repositories.
* `/test` - Contains any automated tests projects
