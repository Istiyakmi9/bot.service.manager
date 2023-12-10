#!/bin/bash
echo 'Script running'
env=$1
definedEnv="prod"

apiPath=""
basePath=""

echo "Environment selected: " $env

if [ "$env" = "$definedEnv" ]
then
    echo "Production environment"
    basePath=""
else
    echo "Development environment"
    basePath=""
fi

apiPath="./tracker-pod.yml"


echo '[Tracker] Installing Tracker API'
echo '[Kubernetes] step - 3: Deleting deployment'
microk8s kubectl delete -f $apiPath

echo '[Kubernetes] step - 4: Applying deployment'
microk8s kubectl apply -f $apiPath
