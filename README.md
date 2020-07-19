# Payment Gateway

This Payment Gateway is a core domain of the Stark Bank and contains the Payments bounded context. It allows Merchants to submit a payment request with the card number, expiry date, amount, currency and CCV provided by their shoppers and then later retrieve payment details using the unique payment id.

# Getting Started

## Prerequisites

To run this project locally you will need:

* .NET Core 3.1 ([available here](https://dotnet.microsoft.com/download/dotnet-core/3.1))
* Azure Cosmos DB Emulator ([available here](https://aka.ms/cosmosdb-emulator))
* To compile/run the Go SDK and sample application, [Go](https://golang.org/) will be required

## Project Layout

* `/lib` - Contains class libraries which would be shared over many projects. Normally these would be made available via a NuGet feed.
* `/sample` - Contains a sample .NET console application and sample Go application, showing how to use the .NET SDK to connect to the Payment Gateway. 
* `/src`
  * `/src/Stark.PaymentGateway` - Contains the ASPNET Core host, providing HTTP API endpoints.
  * `/src/Stark.PaymentGateway.Application` - Contains the application layer for the Payment Gateway, such as the commands and command handlers.
  * `/src/Stark.PaymentGateway.Domain` - Contains the domain layer for the Payment Gateway, encapsulating any business logic and interfaces required to persist the domain model.
  * `/src/Stark.PaymentGateway.Infrastructure` - Contains the infrastructure layer for the Payment Gateway, including the implementation of any repositories.
  * `/src/Stark.PaymentGateway.Sdk.Go` - Contains a client library for connecting to the Gateway API using Go.
  * `/src/Stark.PaymentGateway.Sdk.Net` - Contains a client library for other applications to easily connect to the Gateway API.
* `/test` - Contains any automated tests projects

## Automated Tests

### Load Tests

To run the automated K6 load tests, open a new terminal, navigate to the `/test/Stark.PaymentGateway.LoadTests` directory and run the command `k6 run process-payment-load-test.js`.

# Troubleshooting

## System.Net.Http.HttpRequestException

When starting the Payment Gateway service, if there is a `System.Net.Http.HttpRequestException` thrown, with the message "No connection could be made because the target machine actively refused it", it is likely caused because the connection to Cosmos could not be established. Check the connection string is correct and that the instance/emulator is running.

## System.IO.IOException

When starting the Payment Gateway service, if there is a `System.IO.IOException` thrown, with the message "Failed to bind to address ...", it is likely another instance of the service is already running or there is another service on the machine using the same port number. First check whether there are any consoles with dotnet running, or another instance of Visual Studio actively debugging. If that isn't the case, then open a console window and use the command `netstat -na` to confirm if something is already bound to this port. If that is the case, open the file `launchSettings.json` and change the `applicationUrl` property to use another port.

## Debuging Go Applications

It is possible to debug the Go libary and sample application using Visual Studio Code. 

1. Ensure the Go extension for Visual Studio Code (`golang.go`) is installed.
1. Open the command palette (control + shift + P) and run the command `Debug: Open launch.json`. This will create a new launch.json if one does not already exist.
1. Ensure the `program` property is set to the sample application. This must be a absolute uri (eg. `C:/dev/Stark/sample/Stark.PaymentGateway.SampleApplication.Go/PaymentGatewaySample.go`).
1. Add breakpoints to the code. Then go to the Run window and press the Start Debugging button.

*When debugging the sample go application, both the PaymentGateway and Auth services will already need to be running. To start these services it may be easist to open two terminals, navigate to the directory for each service and use the command `dotnet run`*