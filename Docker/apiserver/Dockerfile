FROM ubuntu:22.10

COPY app/ /home/app

WORKDIR /home/app

EXPOSE 18989

RUN apt-get update 
RUN apt-get install -y aspnetcore-runtime-7.0

RUN chmod +x Com2usEduAPIServer

ENTRYPOINT ["./Com2usEduAPIServer"]