DROP DATABASE IF EXISTS GameDB;
CREATE DATABASE IF NOT EXISTS GameDB;

USE GameDB;

DROP TABLE IF EXISTS GameDB.`Player`;
CREATE TABLE IF NOT EXISTS GameDB.`Player`
(
	Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '플레이어 고유번호',
    AccountId INT NOT NULL UNIQUE COMMENT '계정 고유번호',
    ContinuousAttendanceDays INT NOT NULL DEFAULT 0 COMMENT '연속출석일수',
    LastAttendanceDate DATE DEFAULT NULL COMMENT '마지막출석일'
) COMMENT '플레이어 기본 데이터 테이블';

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
	Content VARCHAR(400) NOT NULL COMMENT '메일 내용'
    
) COMMENT '플레이어 우편함 테이블';

DROP TABLE IF EXISTS GameDB.`MailItem`;
CREATE TABLE IF NOT EXISTS GameDB.`MailItem`
(
	Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '메일 아이템 고유번호',
	MailId INT NOT NULL COMMENT '메일 고유번호',
    ItemCode INT NOT NULL COMMENT '메일에 포함된 아이템 코드',
    ItemCount INT NOT NULL COMMENT '메일에 포함된 아이템 개수'
);


DROP TABLE IF EXISTS GameDB.`Bill`;
CREATE TABLE IF NOT EXISTS GameDB.`Bill`
(
	Id BIGINT NOT NULL PRIMARY KEY COMMENT '영수증 고유번호',
	Token BIGINT NOT NULL COMMENT '영수증 인증토큰',
    PlayerId INT NOT NULL COMMENT '결제한 플레이어 고유번호' 
) COMMENT '플레이어 영수증 테이블';



