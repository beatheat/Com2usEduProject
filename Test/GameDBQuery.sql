DROP DATABASE IF EXISTS GameDB;
CREATE DATABASE IF NOT EXISTS GameDB;

USE GameDB;

DROP TABLE IF EXISTS GameDB.`Player`;
CREATE TABLE IF NOT EXISTS GameDB.`Player`
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
	Money INT NOT NULL DEFAULT 1000 COMMENT '소지금'
) COMMENT '플레이어 기본 데이터 테이블';

DROP TABLE IF EXISTS GameDB.`PlayerAttendance`;
CREATE TABLE IF NOT EXISTS GameDB.`PlayerAttendance`
(
    PlayerId INT NOT NULL PRIMARY KEY COMMENT '출석부 소유 플레이어 고유번호',
	ContinuousAttendanceDays INT NOT NULL DEFAULT 0 COMMENT '연속출석일수',
    LastAttendanceDate DATE NOT NULL DEFAULT "1985-01-01" COMMENT '마지막출석일'
) COMMENT '플레이어 출석부 테이블';

/*비선형적인 스테이지일 경우를 고려해서 완료한 스테이지 테이블을 분리함*/
DROP TABLE IF EXISTS GameDB.`PlayerCompletedStage`;
CREATE TABLE IF NOT EXISTS GameDB.`PlayerCompletedStage`
(
    PlayerId INT NOT NULL COMMENT '플레이어 고유번호',
    StageCode INT NOT NULL COMMENT '완료한 스테이지 코드',
    CONSTRAINT PlayerCompletedStagePK PRIMARY KEY(PlayerId,StageCode)
) COMMENT '플레이어가 완료한 스테이지 테이블';


DROP TABLE IF EXISTS GameDB.`PlayerItem`;
CREATE TABLE IF NOT EXISTS GameDB.`PlayerItem`
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

DROP TABLE IF EXISTS GameDB.`Mail`;
CREATE TABLE IF NOT EXISTS GameDB.`Mail`
(
    Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '메일 고유번호',
    PlayerId INT NOT NULL COMMENT '메일 소유 플레이어 고유번호',
    Name VARCHAR(50) NOT NULL COMMENT '메일 전송메일 이름',
    TransmissionDate DATETIME NOT NULL COMMENT '메일 전송일',
    ExpireDate DATETIME NOT NULL COMMENT '아이템 보관 기일',
    IsItemReceived BOOL NOT NULL DEFAULT FALSE COMMENT '아이템 수령 여부',
	Content VARCHAR(400) NOT NULL COMMENT '메일 내용',
    
    ItemCode1 INT NOT NULL DEFAULT -1 COMMENT '메일에 포함된 아이템 코드',
    ItemCount1 INT NOT NULL DEFAULT 0 COMMENT '메일에 포함된 아이템 개수',
	ItemCode2 INT NOT NULL DEFAULT -1 COMMENT '메일에 포함된 아이템 코드',
    ItemCount2 INT NOT NULL DEFAULT 0 COMMENT '메일에 포함된 아이템 개수',
	ItemCode3 INT NOT NULL DEFAULT -1 COMMENT '메일에 포함된 아이템 코드',
    ItemCount3 INT NOT NULL DEFAULT 0 COMMENT '메일에 포함된 아이템 개수',
	ItemCode4 INT NOT NULL DEFAULT -1 COMMENT '메일에 포함된 아이템 코드',
    ItemCount4 INT NOT NULL DEFAULT 0 COMMENT '메일에 포함된 아이템 개수'
) COMMENT '플레이어 우편함 테이블';


DROP TABLE IF EXISTS GameDB.`Bill`;
CREATE TABLE IF NOT EXISTS GameDB.`Bill`
(
	Token BIGINT NOT NULL PRIMARY KEY COMMENT '영수증 인증토큰',
    PlayerId INT NOT NULL COMMENT '결제한 플레이어 고유번호' 
) COMMENT '플레이어 영수증 테이블';



