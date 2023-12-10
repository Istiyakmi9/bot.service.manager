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

apiPath="./tracker.yml"


echo 'Installing Tracker API'
echo 'Cerate docker image after git update'
# git -C ./tracker/ pull


echo '[Tracker] Step - 1: Create api docker image'
#sudo docker build -t istiyaqmi9/bot-tracker -f ./tracker/Dockerfile ./tracker/
echo

echo '[Tracker] Step - 2: Pushing api image to docker hub'
#sudo docker push istiyaqmi9/bot-tracker

echo '[Tracker] Image pushed successfully'



echo '[Tracker] Installing Tracker API'
echo '[Kubernetes] step - 3: Deleting deployment'
microk8s kubectl delete -f $apiPath

echo '[Kubernetes] step - 4: Applying deployment'
microk8s kubectl apply -f $apiPath
