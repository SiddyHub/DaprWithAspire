# Dapr With Aspire Overview
This Sample contains a Dapr(ized) eShop App Solution with .Net 8 [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) template. [Dapr](https://dapr.io/) provides API for building Secure and Reliable Microservices, while Aspire assists in service discovery, telemetry, resilience, and health checks by default.

To get more information on building Apps with Dapr Components , please do checkout my [Dapr Series](https://github.com/SiddyHub/Dapr/tree/eshop_daprized) for more information on developing with Dapr Building Blocks using .Net from scratch.

This solution is refactored to use .Net 8 and Aspire template, along with Dapr version 1.12.

Also, do read this blog post for more information on [Introducing .NET Aspire: Simplifying Cloud-Native Development with .NET 8](https://devblogs.microsoft.com/dotnet/introducing-dotnet-aspire-simplifying-cloud-native-development-with-dotnet-8/) while building and running this Sample for more deep dive.


## Pre-requisites to Run the Application
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  or later, and [Visual Studio 2022 Preview (17.9 Preview 1)](https://visualstudio.microsoft.com/vs/preview/)
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)
- Access to an Azure subscription
- VS Code (optional)

## Architecture Overview

![architecture_overview](https://user-images.githubusercontent.com/84964657/191068659-62575c1a-9b42-4849-96d9-6d60c85db505.png)

1. This is a GloboTicket MVC Application which has a catalog service, which interacts with the shopping basket service, when user enters items in basket. 
2. The Shopping basket interacts with the Discount service, to check if any valid coupon code has been entered as part of the Checkout Process.
3. Once user checks out, the Shopping basket will place an event in queue (Azure Service Bus or Redis).
4. The Order Service picks up this event and create a new Order.
5. The Order service places a new event in queue for another service i.e. Payment.
6. The Payment Service talks to an External Payment Provider Service.
7. On Getting response from the external payment service, the Payment service, places another message on the queue, which will again be picked up by Order service.
8. The Marketing service will periodically keep checking for new events like User Basket changes etc., and add entry in database.

Overview with the Dapr sidecar running:

![service_invocation](https://user-images.githubusercontent.com/84964657/190984347-bc4d830b-5d20-4ebf-b0c3-08f563d0442f.png)

The Dapr Components covered, along with Azure Services in this sample are:

- State Management - using Azure Redis Cache
- Pub/Sub Messaging - using Azure Service Bus
- Service Invocation using HTTP and GRPC protocols - along with Azure Cosmos DB
- Distributed Tracing - using Zipkin and Aspire Dashboard
- Input/Output Bindings - using Cron Jobs and SMTP Email service
- Secrets Management - using local file (for local debugging) and Kubernetes Secrets (for Production)

## Running the Application

To download and run the sample, follow these steps:

1. Download and unzip the sample / Clone the Repo.
2. In Visual Studio (2022 or later):
    1. On the menu bar, choose **File** > **Open** > **Project/Solution**.
    2. Navigate to the folder that holds the unzipped sample code, and open the solution (.sln) file.
    3. Right click the _GloboTicket.AppHost_ project in the solution explore and choose it as the startup project.
    4. Choose the <kbd>F5</kbd> key to run with debugging, or <kbd>Ctrl</kbd>+<kbd>F5</kbd> keys to run the project without debugging.
3. From the command line:
   1. Navigate to the folder that holds the unzipped sample code.
   2. At the command line, type [`dotnet run`](https://docs.microsoft.com/dotnet/core/tools/dotnet-run).

To run the .NET Aspire app from command prompt, execute the following :

``` bash
dotnet run --project GloboTicket.AppHost
```

## Deploying the Application

Following are ways to deploy this Application to Azure:

### Deploy to Azure Kubernetes Service (AKS)

Dapr can be configured to run on any supported versions of Kubernetes. To achieve this, Dapr begins by deploying the following Kubernetes services, which provide first-class integration to make running applications with Dapr easy.

As part of this Solution, the Deployment Scripts are already provided with the solution files under **Deploy** folder.

To get more information on Deploying Dapr Apps on Kubernetes , please do checkout my [DaprAksDeploy](https://github.com/SiddyHub/DaprAksDeploy) repo, which provides step by step guidance on the following:

- Setting up AKS with Dapr
- Building Docker Images and Uploading to Azure Container Registry (ACR). This step is **optional**, as the solution file pulls images from my Docker Hub Account.
- Migrate On-Prem SQL Database to Azure SQL Database
- Deploying App Secrets to Kubernetes
- Deploying Ingress controller for exposing applications to external clients

### Deploy to Azure Container Apps

The final artifacts of a .NET Aspire application are .NET apps and configuration that can be deployed to your cloud environments. With the strong container-first mindset of .NET Aspire, the .NET SDK native container builds serve as a valuable tool to publish these apps to containers with ease.

The **AppHost** project, knows all about the application, it’s dependencies, configurations, and connections to each services. The application model can produce a manifest definition that describes all of these relationships and dependencies that tools can consume, augment, and build upon for deployment.

With this manifest, we’ve enabled getting your .NET Aspire application into Azure using Azure Container Apps in the simplest and fastest way possible.

Please refer [this link](https://learn.microsoft.com/en-us/dotnet/aspire/deployment/azure/aca-deployment?tabs=visual-studio%2Cinstall-az-windows%2Cpowershell&pivots=azure-azd) for more information on deploying .Net Aspire Apps to Azure Container Apps.

## Troubleshooting notes

- If using Azure Service Bus as a Pub Sub Message broker make sure to enter primary connection string value for **"servicebus"** key in `secrets.json` in individual service projects and client project under **"AzComponents"** folder.
- If using Azure Redis Cache as a State Store, make sure to enter **Redis Host and Key**  value in `secrets.json` in individual service projects and client project under **"AzComponents"** folder. Also make sure Non-SSL port is enabled.
- For Cosmos DB make sure to enter Endpoint and Key in `apsettings.json` file "CosmosDb" section in EventCatalog Service Project.
- If mail binding is not working, make sure `maildev` docker image is running locally. Refer [this link](https://github.com/maildev/maildev) for more info.
- For any more service issues, we can check Zipkin trace logs or Aspire Dashboard.