# deploy Dapr components
kubectl apply -f .\AzComponents\pubsub.yaml
kubectl apply -f .\AzComponents\statestore.yaml
kubectl apply -f .\AzComponents\email.yaml
kubectl create deployment maildev --image maildev/maildev
kubectl expose deployment maildev --type ClusterIP --port 1025,1080
# kubectl apply -f .\AzComponents\cron.yaml
kubectl create deployment zipkin --image openzipkin/zipkin
kubectl expose deployment zipkin --type ClusterIP --port 9411
kubectl apply -f .\AzComponents\config.yaml

# deploy Dapr application, using containers kept in container registry / docker hub
kubectl apply -f .\Deploy\k8s\frontend.yaml
kubectl apply -f .\Deploy\k8s\catalog.yaml
kubectl apply -f .\Deploy\k8s\order.yaml
kubectl apply -f .\Deploy\k8s\shoppingbasket.yaml
kubectl apply -f .\Deploy\k8s\discount.yaml
# kubectl apply -f .\Deploy\k8s\marketing.yaml
kubectl apply -f .\Deploy\k8s\payment.yaml