# DistributedTracing
Distributed Tracing .Net Core with OpenTelemetry

make sure you had docker desktop
then by, you could use these commands to activate your jaeger/zipkin backend processes

docker pull jaegertracing/all-in-one
docker run -d --name jaeger -e COLLECTOR_ZIPKIN_HTTP_PORT=9411 -p 5775:5775/udp -p 6831:6831/udp -p 6832:6832/udp -p 5778:5778 -p 16686:16686 -p 14268:14268 -p 14250:14250 -p 9411:9411 jaegertracing/all-in-one:latest

docker pull openzipkin/zipkin
docker run --name ZIPKINIM -d -p 9411:9411 openzipkin/zipkin

some of projects need rabbitmq , then by you could use these commands with docker

docker pull rabbitmq:3-management
docker run -d --hostname my-rabbit --name my-rabbit -p 15672:15672 -p  5672:5672 rabbitmq:3-management

jaeger-ui
http://localhost:16686/search

zipkin-ui
http://localhost:9411/zipkin/
