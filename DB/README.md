# DB Schema
API 서버에서 사용하는 MySQL DB의 테이블 스키마를 소개합니다.

# Account DB

## Account 테이블
게임에서 생성 된 계정 정보들을 가지고 있는 테이블

```sql
CREATE TABLE AccountDB.`Account`
(
    Id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '계정번호',
    LoginId VARCHAR(50) NOT NULL UNIQUE COMMENT '계정',
    SaltValue VARCHAR(100) NOT NULL COMMENT  '암호화 값',
    HashedPassword VARCHAR(100) NOT NULL COMMENT '해싱된 비밀번호',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP  COMMENT '생성 날짜'
) COMMENT '계정 정보 테이블';
```   


# Master DB

## Version 테이블
마스터 데이터와 클라이언트 버전을 기록한 테이블   
한 행만 존재합니다.
```sql
CREATE TABLE MasterDB.`Version`
(
	Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '고유번호',
	Version VARCHAR(30) NOT NULL COMMENT '마스터데이터 버전',
	ClientVersion VARCHAR(30) NOT NULL COMMENT '클라이언트 버전'
) COMMENT '데이터 버전 테이블';
```

## Item 테이블
게임 아이템에 대한 정보를 가진 테이블
```sql
CREATE TABLE MasterDB.`Item`
(
	Code INT NOT NULL PRIMARY KEY COMMENT '아이템 번호',
	Name VARCHAR(50) NOT NULL UNIQUE COMMENT '아이템 이름',
	Attribute SMALLINT NOT NULL COMMENT  '특성번호',
	Sell INT NOT NULL COMMENT '판매 금액',
	Buy INT NOT NULL COMMENT '구매 금액',
	UseLevel INT NOT NULL COMMENT '사용가능 레벨',
	Attack INT NOT NULL COMMENT '공격력',
	Defence INT NOT NULL COMMENT '방어력',
	Magic INT NOT NULL COMMENT '마법력',
	MaxEnhanceCount INT NOT NULL COMMENT '최대 강화 가능 횟수',
	Consumable BOOL NOT NULL DEFAULT FALSE COMMENT '소비가능 여부'
) COMMENT '아이템 테이블';
```

## AttendanceReward 테이블
게임 출석보상에 대한 정보를 가진 테이블
```sql
CREATE TABLE MasterDB.`AttendanceReward`
(
	Day INT NOT NULL PRIMARY KEY COMMENT '날짜',
	ItemCode INT NOT NULL COMMENT '아이템 코드',
	ItemCount INT NOT NULL COMMENT  '아이템 개수'
) COMMENT '출석 보상 테이블';

```

## ShopItem 테이블
게임 인앱상점 아이템에 대한 정보를 가진 테이블
```sql
CREATE TABLE MasterDB.`ShopItem`
(
	UniqueNo INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT  '상품테이블 고유번호',
	Code INT NOT NULL COMMENT '상품번호',
	ItemCode INT NOT NULL COMMENT '아이템코드',
	ItemCount INT NOT NULL COMMENT '아이템 개수'
) COMMENT '인앱상품 테이블';
```

## StageItem 테이블
게임의 한 스테이지에서 얻을 수 있는 아이템 정보를 가진 테이블
```sql
CREATE TABLE MasterDB.`StageItem`
(
	UniqueNo INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '행 고유 번호',
	StageCode INT NOT NULL COMMENT '스테이지 번호',
	ItemCode INT NOT NULL COMMENT '아이템코드',
	MaxItemCount INT NOT NULL COMMENT '아이템 최대 개수'
) COMMENT '스테이지 아이템 테이블';
```

## StageNpc 테이블
게임의 한 스테이지에서 등장하는 NPC 정보를 가진 테이블
```sql
CREATE TABLE MasterDB.`StageNpc`
(
	Code INT NOT NULL PRIMARY KEY COMMENT '스테이지 NPC 번호',
	StageCode INT NOT NULL COMMENT '스테이지 번호',
	Count INT NOT NULL COMMENT 'NPC 개수',
	Exp INT NOT NULL COMMENT '1개당 경험치'
) COMMENT '스테이지 NPC 테이블';
```

## InitialPlayerItem 테이블
계정 생성 시 플레이어에게 부여할 아이템 정보를 가진 테이블
```sql
CREATE TABLE MasterDB.`InitialPlayerItem`
(
	Code INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '초기 아이템 행 공유 번호',
	ItemCode INT NOT NULL COMMENT '아이템 고유 번호',
	ItemCount INT NOT NULL COMMENT 'NPC 개수'
) COMMENT '초기 플레이어 아이템 테이블';
```


# Game DB

## Player 테이블
플레이어의 기본 데이터를 가진 테이블
```sql
CREATE TABLE GameDB.`Player`
(
	Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '플레이어 고유번호',
	Nickname VARCHAR(20) NOT NULL COMMENT '플레이어 이름',
	AccountId INT NOT NULL UNIQUE COMMENT '계정 고유번호',
	HP INT NOT NULL DEFAULT 20 COMMENT '체력',
	Attack INT NOT NULL DEFAULT 5 COMMENT '공격력',
	Defence INT NOT NULL DEFAULT 5 COMMENT '방어력',
	Magic INT NOT NULL DEFAULT 5 COMMENT '마법력',
	Exp INT NOT NULL DEFAULT 0 COMMENT '경험치',
	Level INT NOT NULL DEFAULT 1 COMMENT '경험치',
	Money INT NOT NULL DEFAULT 1000 COMMENT '소지금',
	HighestClearStageCode INT NOT NULL DEFAULT 0 COMMENT '클리어한 스테이지 중 가장 높은 코드'
) COMMENT '플레이어 기본 데이터 테이블';
```

## PlayerAttendance 테이블
플레이어의 출석정보를 가진 테이블
```sql
CREATE TABLE GameDB.`PlayerAttendance`
(
	PlayerId INT NOT NULL PRIMARY KEY COMMENT '출석부 소유 플레이어 고유번호',
	ContinuousAttendanceDays INT NOT NULL DEFAULT 0 COMMENT '연속출석일수',
	LastAttendanceDate DATE NOT NULL DEFAULT "1985-01-01" COMMENT '마지막출석일'
) COMMENT '플레이어 출석부 테이블';
```


## PlayerItem 테이블
플레이어 아이템 데이터를 가진 테이블
```sql
CREATE TABLE GameDB.`PlayerItem`
(
	Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '플레이어 아이템 고유번호',
	PlayerId INT NOT NULL COMMENT '아이템 소유 플레이어 고유번호',
	ItemCode INT NOT NULL COMMENT '아이템 코드',
	Attack INT NOT NULL COMMENT '아이템 공격력',
	Defence INT NOT NULL COMMENT '아이템 방어력',
	Magic INT NOT NULL COMMENT '아이템 마법력',
	EnhanceCount INT NOT NULL COMMENT '아이템 강화횟수',
	Count INT NOT NULL COMMENT '아이템 갯수'
) COMMENT '플레이어 아이템 테이블';
```

## Mail 테이블
플레이어의 메일 데이터를 가진 테이블
```sql
CREATE TABLE GameDB.`Mail`
(
	Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '메일 고유번호',
	PlayerId INT NOT NULL COMMENT '메일 소유 플레이어 고유번호',
	Name VARCHAR(50) NOT NULL COMMENT '메일 전송메일 이름',
	TransmissionDate DATETIME NOT NULL COMMENT '메일 전송일',
	ExpireDate DATETIME NOT NULL COMMENT '아이템 보관 기일',
	IsItemReceived BOOL NOT NULL DEFAULT FALSE COMMENT '아이템 수령 여부',
	Content VARCHAR(400) NOT NULL COMMENT '메일 내용',
    
	ItemCode1 INT NOT NULL DEFAULT -1 COMMENT '메일에 포함된 아이템1 코드',
	ItemCount1 INT NOT NULL DEFAULT 0 COMMENT '메일에 포함된 아이템1 개수',
	ItemCode2 INT NOT NULL DEFAULT -1 COMMENT '메일에 포함된 아이템2 코드',
	ItemCount2 INT NOT NULL DEFAULT 0 COMMENT '메일에 포함된 아이템2 개수',
	ItemCode3 INT NOT NULL DEFAULT -1 COMMENT '메일에 포함된 아이템3 코드',
	ItemCount3 INT NOT NULL DEFAULT 0 COMMENT '메일에 포함된 아이템3 개수',
	ItemCode4 INT NOT NULL DEFAULT -1 COMMENT '메일에 포함된 아이템4 코드',
	ItemCount4 INT NOT NULL DEFAULT 0 COMMENT '메일에 포함된 아이템4 개수'
) COMMENT '플레이어 우편함 테이블';
```


## Bill 테이블
플레이어의 결제 영수증 데이터를 가진 테이블
```sql
CREATE TABLE GameDB.`Bill`
(
	Token BIGINT NOT NULL PRIMARY KEY COMMENT '영수증 인증토큰',
	PlayerId INT NOT NULL COMMENT '결제한 플레이어 고유번호' 
) COMMENT '플레이어 영수증 테이블';
```

