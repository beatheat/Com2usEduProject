

# Docker 환경구성 가이드


## 개요
MySQL 서버, Redis 서버, API 서버 3가지를 서로 다른 도커 컨테이너에 운영하여 배포합니다.

아래는 각 컨테이너의 이름입니다.

1. MySQL 서버 : com2us-edu-mysql
2. Redis 서버 : com2us-edu-redis
3. API 서버   : com2us-edu-apiserver


## Com2usEduProject Docker 환경 구조도
 
![](https://cdn.discordapp.com/attachments/987652135107850315/1110267817284079767/image.png)


## Docke-compose 
```docker-compose
version: "3"

services:
  apiserver:
    container_name: com2us-edu-apiserver
    build:
      context : apiserver
      dockerfile : Dockerfile
    ports:
      - 18989:18989
    networks:
      - com2us-edu-network


  db:   
    container_name: com2us-edu-mysql
    image: mysql

    restart: always

    environment:
      MYSQL_ROOT_PASSWORD: 1234
      TZ: Asia/Seoul

    volumes:
      - "./mysql/initSQL:/docker-entrypoint-initdb.d/"

    networks:
      - com2us-edu-network


  memorydb:
    container_name: com2us-edu-redis
    image: redis
    
    restart: always

    volumes:
      - "./redis/redis.conf:/usr/local/etc/redis/redis.conf"
    networks: 
      - com2us-edu-network
    command: "redis-server /usr/local/etc/redis/redis.conf"

networks:
  com2us-edu-network:
    driver: bridge
```
Docker compose를 통해 시스템을 구성할 수 있습니다. 시스템을 구성하기 전 작성해야하는 Configuration은 2가지가 있습니다.


1. MYSQL_ROOT_PASSWORD 작성
- docker-compose.yml에서 MySQL의 ROOT 비밀번호를 수정합니다. 
2. API Server의 appconfiguration.json 확인
- apiserver/app/appconfiguration.json에서 MySQL DB 연결정보를 수정합니다.



## 각 컨테이너의 구성방법

Docker-compose에서 행하는 각 구성요소의 의미를 파악하기 위해 각각의 컨테이너를 수동으로 설정하는 방법을 알아봅니다.


## Redis 설정
1. Redis 도커 컨테이너 생성
```sh
docker run --name com2us-edu-redis -d redis
```
2. IP 바인딩
```sh
vi /etc/redis.conf
```
```
bind com2us-edu-apiserver
```
com2us-edu-redis의 bash에 접속하여 레디스에 접속할 수 있는 IP주소(API Server의 주소)를 설정합니다.   
도커 네트워크를 활용하여 IP대신 컨테이너 이름을 사용합니다.

3. 공지사항 설정
```redis
redis-cli
set Notice "공지사항"
```
레디스에 접속하여 공지사항을 등록합니다.

## MySQL 설정
1. 도커 컨테이너 생성
```sh
docker run -d --name com2us-edu-mysql -e MYSQL_ROOT_PASSWORD=비밀번호 -e TZ=Asia/Seoul ubuntu/mysql --lower_case_table_names=1
```
MySQL 도커 컨테이너를 생성한다.
--lower_case_table_names=1의 의미는 MySQL의 Table과 DB이름으로 대소문자의 구분을 하지 않겠다는 것이다. 윈도에서는 기본셋팅으로 대소문자 구분을 하지 않고 리눅스에서는 대소문자 구분을 하기 때문에 대소문자 구분하지 않음으로 설정을 통일합니다.

2. IP 바인딩
```sh
vi /etc/mysql/conf.d/mysql.cnf
```
```
[mysqld] 
bind-address = com2us-edu-apiserver
```
com2us-edu-mysql의 bash에 접속하여 MySQL에 접속할 수 있는 IP주소(API Server의 주소)를 설정합니다.   

3. DB 스키마 입력   
[DB스키마 입력 SQL](https://github.com/beatheat/Com2usEduProject/tree/master/DB)   
위 링크의 sql 스크립트를 com2us-edu-mysql로 옮겨 mysql에서 실행합니다.


## API SERVER 설정
1. appsettings.json 설정
```json
  "DbConnectionConfig": {
    "Redis": "com2us-edu-redis",
    "AccountDb": "Server=com2us-edu-mysql;user=root;Password=비밀번호;Database=accountDb;Pooling=true;Min Pool Size=0;Max Pool Size=40;AllowUserVariables=True;",
    "GameDb": "Server=com2us-edu-mysql;user=root;Password=비밀번호;Database=gameDb;Pooling=true;Min Pool Size=0;Max Pool Size=40;AllowUserVariables=True;",
    "MasterDb": "Server=com2us-edu-mysql;user=root;Password=비밀번호;Database=masterDb;Pooling=true;Min Pool Size=0;Max Pool Size=40;AllowUserVariables=True;"
  }
```
appsettings에서 DB 연결 부분을 알맞게 수정합니다.


2. dockerfile을 통해 이미지 생성
```dockerfile
FROM ubuntu

WORKDIR /usr/src/app

COPY 'Com2usEduAPIServer빌드 폴더' /usr/src/app

RUN apt-get update && \
    apt-get install -y aspnetcore-runtime-7.0

EXPOSE 18989
ENTRYPOINT [./Com2usEduAPIServer]
```
우분투 서버기반에 .NET7.0 런타임을 설치한 이미지를 생성하는 dockerfile을 작성합니다.

```sh
docker build -t com2us-edu-apiserver:1.0.0 .
docker run -it -p 18989:18989 --name com2us-edu-apiserver com2us-edu-apiserver:1.0.0
```
dockerfile을 통해 이미지를 생성하고 컨테이너를 실행합니다.


## Docker Network 설정
```sh
docker network create com2us-edu-net
docker network connect com2us-edu-net com2us-edu-redis
docker network connect com2us-edu-net com2us-edu-mysql
docker network connect com2us-edu-net com2us-edu-apiserver
```
마지막으로 도커 네트워크를 생성하고 3컨테이너를 모두 연결시키면 도커를 실행하는 서버의
18989포트로 접근하여 API를 실행할 수 있습니다.
