

# Docker 환경구성 가이드


## 개요
MySQL 서버, Redis 서버, API 서버 3가지를 서로 다른 도커 컨테이너에 운영하여
분리된 환경에서 시스템이 정상적으로 운영되는 지 확인한다. 각 서버는 아래와 같이 컨테이너 이름을 짓는다.

1. MySQL 서버 : com2us-edu-mysql
2. Redis 서버 : com2us-edu-redis
3. API 서버   : com2us-edu-apiserver


## Com2usEduProject Docker 환경 구조도
 
![](https://cdn.discordapp.com/attachments/987652135107850315/1110267817284079767/image.png)


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
com2us-edu-redis의 bash에 접속하여 레디스에 접속할 수 있는 IP주소(API Server의 주소)를 설정한다.   
도커 네트워크를 활용하여 IP대신 컨테이너 이름을 사용한다.

3. 공지사항 설정
```redis
redis-cli
set Notice "공지사항"
```
레디스에 접속하여 공지사항을 등록한다.

## MySQL 설정
1. 도커 컨테이너 생성
```sh
docker run -d --name com2us-edu-mysql -e MYSQL_ROOT_PASSWORD=비밀번호 -e TZ=Asia/Seoul ubuntu/mysql --lower_case_table_names=1
```
MySQL 도커 컨테이너를 생성한다.
--lower_case_table_names=1의 의미는 MySQL의 Table과 DB이름으로 대소문자의 구분을 하지 않겠다는 것이다. 윈도에서는 기본셋팅으로 대소문자 구분을 하지 않고 리눅스에서는 대소문자 구분을 하기 때문에 대소문자 구분하지 않음으로 설정을 통일한다.

2. IP 바인딩
```sh
vi /etc/mysql/conf.d/mysql.cnf
```
```
[mysqld] 
bind-address = com2us-edu-apiserver
```
com2us-edu-mysql의 bash에 접속하여 MySQL에 접속할 수 있는 IP주소(API Server의 주소)를 설정한다.   

3. DB 스키마 입력   
[DB스키마 입력 SQL](https://github.com/beatheat/Com2usEduProject/tree/master/DB)   
위 링크의 sql 스크립트를 com2us-edu-mysql로 옮겨 mysql에서 실행한다.


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
appsettings에서 DB 연결 부분을 알맞게 수정한다.


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
우분투 서버기반에 .NET7.0 런타임을 설치한 이미지를 생성하는 dockerfile을 작성한다.

```sh
docker build -t com2us-edu-apiserver:1.0.0 .
docker run -it -p 18989:18989 --name com2us-edu-apiserver com2us-edu-apiserver:1.0.0
```
dockerfile을 통해 이미지를 생성하고 컨테이너를 실행한다.


## Docker Network 설정
```sh
docker network create com2us-edu-net
docker network connect com2us-edu-net com2us-edu-redis
docker network connect com2us-edu-net com2us-edu-mysql
docker network connect com2us-edu-net com2us-edu-apiserver
```
마지막으로 도커 네트워크를 생성하고 3컨테이너를 모두 연결시키면 도커를 실행하는 서버의
18989포트로 접근하여 API를 실행할 수 있다.
