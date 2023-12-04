# Install Helm3 (if not installed)
brew install helm

# Create a namespace for your ingress resources
kubectl create namespace ingress-basic

# Add the ingress-nginx repository
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

# Use Helm to deploy an NGINX ingress controller
helm install nginx-ingress ingress-nginx/ingress-nginx `
    --namespace default `
    -f .\Deploy\nginx-dapr-annotations.yaml `
    --set controller.replicaCount=1 `
    --set controller.service.externalTrafficPolicy=Local ` # this can help to restrict access to IC https://github.com/kubernetes/ingress-nginx/blob/main/docs/user-guide/nginx-configuration/configmap.md#whitelist-source-range
    --set controller.service.annotations."service\.beta\.kubernetes\.io/azure-dns-label-name"="dapringress" `
    --set controller.service.loadBalancerIP="<your Static IP Address>"

# verify
kubectl --namespace default get services -o wide -w nginx-ingress-ingress-nginx-controller

# create ingress 
kubectl apply -f .\Deploy\ingress-nginx-dapr.yaml