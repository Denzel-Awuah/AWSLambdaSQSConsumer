## .NET 8 Web API AWS System with EKS, EC2, SQS, Lambda and S3
A .NET 8 AWS Lambda function that recieves events from an AWS SQS queue. The lambda then processes the messages received from the queue and stores updates into an S3 bucket.

## Deployment 
This lambda function is a service within a larger system deployed on AWS. See full deployment strategy and details below.

A .NET Web API application was containerized using Docker, then the image is pushed to a repository in Elastic Container Registry. The image was then pulled from ECR and deployed to Amazon Elastic Kubernetes Services (EKS).
The image pulled from ECR was then deployed to a managed nodegroup within the EKS cluster using AWS EC2 Instances. A Load Balancer was also deployed to balance the traffic coming into the cluster.


## Deployment Strategy
![Application](/AWS-EKS-DotNet-System-Infrastructure.jpg)

