If you are using the Nginx Ingress Controller in a Kubernetes cluster to manage your web traffic, you can still obtain and configure SSL/TLS certificates for your services using Let's Encrypt. Here are the steps to achieve this:

Step 1: Prerequisites

Ensure you have a Kubernetes cluster set up.
Install and configure the Nginx Ingress Controller in your cluster. You can follow the official documentation or use Helm to deploy it.
Step 2: Install Cert-Manager

Cert-Manager is a Kubernetes add-on that simplifies the management of SSL/TLS certificates, including integration with Let's Encrypt.

bash
--------------------
kubectl apply -f https://github.com/jetstack/cert-manager/releases/download/v1.5.3/cert-manager.yaml


This YAML file installs Cert-Manager components in your cluster.

Step 3: Create a ClusterIssuer or Issuer

You need to define a ClusterIssuer or Issuer resource in your Kubernetes cluster to specify how Cert-Manager should obtain certificates from Let's Encrypt. A ClusterIssuer is cluster-wide, while an Issuer is namespace-specific. Here's an example of a ClusterIssuer for Let's Encrypt:

yaml
--------------------

apiVersion: cert-manager.io/v1
kind: ClusterIssuer
metadata:
  name: letsencrypt-prod
spec:
  acme:
    server: https://acme-v02.api.letsencrypt.org/directory
    email: your-email@example.com
    privateKeySecretRef:
      name: letsencrypt-prod
    solvers:
    - http01:
        ingress:
          class: nginx
Make sure to replace your-email@example.com with your email address.

Apply this configuration to create the ClusterIssuer:

bash
------------------------
kubectl apply -f your-clusterissuer.yaml

Step 4: Create an Ingress Resource

Create an Ingress resource for your application, specifying the host (your domain name) and the backend service. Make sure to add annotations to enable Cert-Manager and use the ClusterIssuer or Issuer you defined earlier.

Here's an example Ingress resource:

yaml
--------------------------------

apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: your-ingress
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-prod
    nginx.ingress.kubernetes.io/rewrite-target: /
spec:
  tls:
    - hosts:
        - yourdomain.com
      secretName: your-tls-secret
  rules:
    - host: yourdomain.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: your-service
                port:
                  number: 80
This example specifies that the Ingress resource should use the Let's Encrypt ClusterIssuer letsencrypt-prod and configure TLS for the domain yourdomain.com. Replace your-service with the name of your Kubernetes service.

Apply the Ingress resource to your cluster:

bash
Copy code
kubectl apply -f your-ingress.yaml
Step 5: Verify

Cert-Manager will automatically request and manage the SSL/TLS certificate for your Ingress resource. You can monitor the certificate's status:

bash
Copy code
kubectl describe certificate your-tls-secret
Ensure that the certificate is successfully issued and that the Ingress resource is updated to use the certificate.

That's it! Your Nginx Ingress Controller is now configured to handle HTTPS traffic with a Let's Encrypt certificate for your Kubernetes services. Make sure to configure your DNS to point to the cluster's external IP or load balancer to access your services securely via HTTPS.