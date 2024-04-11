

Architecture explain:


I Have 4 layers based on DDD:
</br>
WebAPI -> Layer with Api swagger, controller, configurations.</br>
Applicaton - > Bussiness Logic Layer.</br>
Core -> Models, Dto, Entities and Interfaces of Business, Services, WebServices and Repositorys</br>
Infraestructure -> Persistence, Services and Connected Services.</br>


TODO, Unit Test Layer with NUnit


The application is ready to be deployed on Kubernetes. Follow these steps:

Dockerize the application using a Dockerfile.
Publish the Docker image on Kubernetes.
Apply the Kubernetes manifests.
In minutes, the application will be ready and running on Kubernetes.
