# delete and then create the secrets for the Dapr components and Sql database
kubectl delete secret cosmosdb
kubectl create secret generic cosmosdb --from-literal=Endpoint='azure cosmosdb endpoint' --from-literal=Key='azure cosmosdb key' --from-literal=DatabaseName='EventCatalog'
kubectl delete secret servicebus-secret
kubectl create secret generic servicebus-secret --from-literal=connectionString='azure servicebus connectionstring'
kubectl delete secret rediscache-secret
kubectl create secret generic rediscache-secret --from-literal=redisPassword='azure rediscache secret'

kubectl delete secret shoppingbasket-db
kubectl create secret generic shoppingbasket-db --from-literal=connectionstring="Data Source=tcp:globoticket.database.windows.net,1433;Initial Catalog=GloboTicketShoppingbasketDB;Integrated Security=False;User ID=globoticket-user;Password=****;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;"
kubectl delete secret ordering-db
kubectl create secret generic ordering-db --from-literal=connectionstring="Data Source=tcp:globoticket.database.windows.net,1433;Initial Catalog=GloboTicketOrderDb;Integrated Security=False;User ID=globoticket-user;Password=****;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;"
kubectl delete secret marketing-db
kubectl create secret generic marketing-db --from-literal=connectionstring="Data Source=tcp:globoticket.database.windows.net,1433;Initial Catalog=GloboTicketMarketingDb;Integrated Security=False;User ID=globoticket-user;Password=****;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;"
kubectl delete secret discount-db
kubectl create secret generic discount-db --from-literal=connectionstring="Server=tcp:globoticket.database.windows.net,1433;Initial Catalog=GloboTicketDiscountDB;;Integrated Security=False;User ID=globoticket-user;Password=****;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;"