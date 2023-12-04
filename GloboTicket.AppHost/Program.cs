using Aspire.Hosting;
using Aspire.Hosting.Dapr;
using System.Collections.Immutable;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDapr();

builder.AddProject<Projects.GloboTicket_Services_Discount>("globoticket.services.discount")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "discountgrpc",
        AppProtocol = "grpc",
        ResourcesPaths = ImmutableHashSet.Create("./AzComponents"),
        Config = "./AzComponents/config.yaml",
        MetricsPort = 9097,        
        DaprHttpPort = 3507,
        DaprGrpcPort = 50007,
        AppPort = 5007
    });


builder.AddProject<Projects.GloboTicket_Services_Ordering>("globoticket.services.ordering")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "order",
        AppProtocol = "http",
        ResourcesPaths = ImmutableHashSet.Create("./AzComponents"),
        Config = "./AzComponents/config.yaml",
        MetricsPort = 9095,
        DaprHttpPort = 3505,
        AppPort = 5005
    });

builder.AddProject<Projects.GloboTicket_Services_EventCatalog>("globoticket.services.eventcatalog")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "catalog",
        AppProtocol = "http",
        ResourcesPaths = ImmutableHashSet.Create("./AzComponents"),
        Config = "./AzComponents/config.yaml",
        MetricsPort = 9091,
        DaprHttpPort = 3501,
        AppPort = 5001
    });

builder.AddProject<Projects.GloboTicket_Services_ShoppingBasket>("globoticket.services.shoppingbasket")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "shoppingbasket",
        AppProtocol = "http",
        ResourcesPaths = ImmutableHashSet.Create("./AzComponents"),
        Config = "./AzComponents/config.yaml",
        MetricsPort = 9092,
        DaprHttpPort = 3502,
        AppPort = 5002
    });

builder.AddProject<Projects.GloboTicket_Services_Marketing>("globoticket.services.marketing")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "marketing",
        AppProtocol = "http",
        ResourcesPaths = ImmutableHashSet.Create("./AzComponents"),
        Config = "./AzComponents/config.yaml",
        MetricsPort = 9099,
        DaprHttpPort = 3510,
        AppPort = 5010
    });

var external_payment = builder.AddProject<Projects.External_PaymentGateway>("external_paymentgateway");

builder.AddProject<Projects.GloboTicket_Services_Payment>("globoticket.services.payment")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "payment",
        AppProtocol = "http",
        ResourcesPaths = ImmutableHashSet.Create("./AzComponents"),
        Config = "./AzComponents/config.yaml",
        MetricsPort = 9096,
        DaprHttpPort = 3506,
        AppPort = 5006
    })
    .WithReference(external_payment);

builder.AddProject<Projects.GloboTicket_Web>("globoticket.web")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "frontend",
        AppProtocol = "http",
        ResourcesPaths = ImmutableHashSet.Create("./AzComponents"),
        MetricsPort = 9090,
        DaprHttpPort = 3500,
        Config = "./AzComponents/config.yaml",
        AppPort = 5000
    });

builder.Build().Run();
