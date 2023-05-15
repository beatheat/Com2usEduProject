DROP DATABASE IF EXISTS MasterDB;
CREATE DATABASE IF NOT EXISTS MasterDB;

USE MasterDB;

DROP TABLE IF EXISTS MasterDB.`Version`;
CREATE TABLE IF NOT EXISTS MasterDB.`Version`
(
	Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '고유번호',
	Version VARCHAR(30) NOT NULL COMMENT '마스터데이터 버전',
    ClientVersion VARCHAR(30) NOT NULL COMMENT '클라이언트 버전'
) COMMENT '데이터 버전 테이블';


DROP TABLE IF EXISTS MasterDB.`Item`;
CREATE TABLE IF NOT EXISTS MasterDB.`Item`
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

DROP TABLE IF EXISTS MasterDB.`AttendanceReward`;
CREATE TABLE IF NOT EXISTS MasterDB.`AttendanceReward`
(
    Day INT NOT NULL PRIMARY KEY COMMENT '날짜',
    ItemCode INT NOT NULL COMMENT '아이템 코드',
    ItemCount INT NOT NULL COMMENT  '아이템 개수'
) COMMENT '출석 보상 테이블';

DROP TABLE IF EXISTS MasterDB.`ShopItem`;
CREATE TABLE IF NOT EXISTS MasterDB.`ShopItem`
(
	UniqueNo INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT  '상품테이블 고유번호',
	Code INT NOT NULL COMMENT '상품번호',
	ItemCode INT NOT NULL COMMENT '아이템코드',
    ItemCount INT NOT NULL COMMENT '아이템 개수'
) COMMENT '인앱상품 테이블';

DROP TABLE IF EXISTS MasterDB.`StageItem`;
CREATE TABLE IF NOT EXISTS MasterDB.`StageItem`
(
	UniqueNo INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '행 고유 번호',
	StageCode INT NOT NULL COMMENT '스테이지 번호',
	ItemCode INT NOT NULL COMMENT '아이템코드',
    MaxItemCount INT NOT NULL COMMENT '아이템 최대 개수'
) COMMENT '스테이지 아이템 테이블';

DROP TABLE IF EXISTS MasterDB.`StageNpc`;
CREATE TABLE IF NOT EXISTS MasterDB.`StageNpc`
(

	Code INT NOT NULL PRIMARY KEY COMMENT '스테이지 NPC 번호',
	StageCode INT NOT NULL COMMENT '스테이지 번호',
	Count INT NOT NULL COMMENT 'NPC 개수',
	Exp INT NOT NULL COMMENT '1개당 경험치'

) COMMENT '스테이지 NPC 테이블';


DROP TABLE IF EXISTS MasterDB.`InitialPlayerItem`;
CREATE TABLE IF NOT EXISTS MasterDB.`InitialPlayerItem`
(
	Code INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT '초기 아이템 행 공유 번호',
	ItemCode INT NOT NULL COMMENT '아이템 고유 번호',
	ItemCount INT NOT NULL COMMENT 'NPC 개수'
) COMMENT '초기 플레이어 아이템 테이블';


INSERT INTO MasterDB.`Version`(Version, ClientVersion) VALUES('1.0.0', '1.0.0');

INSERT INTO MasterDB.`InitialPlayerItem`(ItemCode, ItemCount) VALUES(1,300);
INSERT INTO MasterDB.`InitialPlayerItem`(ItemCode, ItemCount) VALUES(2,1);
INSERT INTO MasterDB.`InitialPlayerItem`(ItemCode, ItemCount) VALUES(6,3);


INSERT INTO MasterDB.`Item` VALUES(1,"돈", 5,0,0,0,0,0,0,0,false);
INSERT INTO MasterDB.`Item` VALUES(2,"작은 칼", 1,10,20,1,10,5,1,10,false);
INSERT INTO MasterDB.`Item` VALUES(3,"도금 칼", 1,100,200,5,29,12,10,10,false);
INSERT INTO MasterDB.`Item` VALUES(4,"나무 방패", 2,7,15,1,3,10,1,10,false);
INSERT INTO MasterDB.`Item` VALUES(5,"보통 모자", 3,5,8,1,1,1,1,10,false);
INSERT INTO MasterDB.`Item` VALUES(6,"포션", 4,3,6,1,0,0,0,0, true);


INSERT INTO MasterDB.`AttendanceReward` VALUES(1,1,100);
INSERT INTO MasterDB.`AttendanceReward` VALUES(2,1,100);
INSERT INTO MasterDB.`AttendanceReward` VALUES(3,1,100);
INSERT INTO MasterDB.`AttendanceReward` VALUES(4,1,200);
INSERT INTO MasterDB.`AttendanceReward` VALUES(5,1,200);
INSERT INTO MasterDB.`AttendanceReward` VALUES(6,1,200);
INSERT INTO MasterDB.`AttendanceReward` VALUES(7,2,1);
INSERT INTO MasterDB.`AttendanceReward` VALUES(8,1,100);
INSERT INTO MasterDB.`AttendanceReward` VALUES(9,1,100);
INSERT INTO MasterDB.`AttendanceReward` VALUES(10,1,100);
INSERT INTO MasterDB.`AttendanceReward` VALUES(11,6,5);
INSERT INTO MasterDB.`AttendanceReward` VALUES(12,1,150);
INSERT INTO MasterDB.`AttendanceReward` VALUES(13,1,150);
INSERT INTO MasterDB.`AttendanceReward` VALUES(14,1,150);
INSERT INTO MasterDB.`AttendanceReward` VALUES(15,1,150);
INSERT INTO MasterDB.`AttendanceReward` VALUES(16,1,150);
INSERT INTO MasterDB.`AttendanceReward` VALUES(17,1,150);
INSERT INTO MasterDB.`AttendanceReward` VALUES(18,4,1);
INSERT INTO MasterDB.`AttendanceReward` VALUES(19,1,200);
INSERT INTO MasterDB.`AttendanceReward` VALUES(20,1,200);
INSERT INTO MasterDB.`AttendanceReward` VALUES(21,1,200);
INSERT INTO MasterDB.`AttendanceReward` VALUES(22,1,200);
INSERT INTO MasterDB.`AttendanceReward` VALUES(23,1,200);
INSERT INTO MasterDB.`AttendanceReward` VALUES(24,5,1);
INSERT INTO MasterDB.`AttendanceReward` VALUES(25,1,250);
INSERT INTO MasterDB.`AttendanceReward` VALUES(26,1,250);
INSERT INTO MasterDB.`AttendanceReward` VALUES(27,1,250);
INSERT INTO MasterDB.`AttendanceReward` VALUES(28,1,250);
INSERT INTO MasterDB.`AttendanceReward` VALUES(29,1,250);
INSERT INTO MasterDB.`AttendanceReward` VALUES(30,3,1);


INSERT INTO MasterDB.`ShopItem`(Code,ItemCode,ItemCount) VALUES(1,1,1000);
INSERT INTO MasterDB.`ShopItem`(Code,ItemCode,ItemCount) VALUES(1,2,1);
INSERT INTO MasterDB.`ShopItem`(Code,ItemCode,ItemCount) VALUES(1,3,1);
INSERT INTO MasterDB.`ShopItem`(Code,ItemCode,ItemCount) VALUES(2,4,1);
INSERT INTO MasterDB.`ShopItem`(Code,ItemCode,ItemCount) VALUES(2,5,1);
INSERT INTO MasterDB.`ShopItem`(Code,ItemCode,ItemCount) VALUES(2,6,10);
INSERT INTO MasterDB.`ShopItem`(Code,ItemCode,ItemCount) VALUES(3,1,2000);
INSERT INTO MasterDB.`ShopItem`(Code,ItemCode,ItemCount) VALUES(3,2,1);
INSERT INTO MasterDB.`ShopItem`(Code,ItemCode,ItemCount) VALUES(3,3,1);
INSERT INTO MasterDB.`ShopItem`(Code,ItemCode,ItemCount) VALUES(3,5,1);


INSERT INTO MasterDB.`StageItem`(StageCode,ItemCode,MaxItemCount) VALUES(1,1,3);
INSERT INTO MasterDB.`StageItem`(StageCode,ItemCode,MaxItemCount) VALUES(1,2,3);
INSERT INTO MasterDB.`StageItem`(StageCode,ItemCode,MaxItemCount) VALUES(2,2,2);
INSERT INTO MasterDB.`StageItem`(StageCode,ItemCode,MaxItemCount) VALUES(2,3,2);
INSERT INTO MasterDB.`StageItem`(StageCode,ItemCode,MaxItemCount) VALUES(3,4,2);
INSERT INTO MasterDB.`StageItem`(StageCode,ItemCode,MaxItemCount) VALUES(3,6,5);
INSERT INTO MasterDB.`StageItem`(StageCode,ItemCode,MaxItemCount) VALUES(4,5,2);
INSERT INTO MasterDB.`StageItem`(StageCode,ItemCode,MaxItemCount) VALUES(4,6,5);


INSERT INTO MasterDB.`StageNpc` VALUES(101,1,10,10);
INSERT INTO MasterDB.`StageNpc` VALUES(110,1,12,15);
INSERT INTO MasterDB.`StageNpc` VALUES(201,2,40,20);
INSERT INTO MasterDB.`StageNpc` VALUES(211,2,20,36);
INSERT INTO MasterDB.`StageNpc` VALUES(221,2,1,50);

INSERT INTO MasterDB.`StageNpc` VALUES(301,3,10,25);
INSERT INTO MasterDB.`StageNpc` VALUES(311,3,12,40);
INSERT INTO MasterDB.`StageNpc` VALUES(401,4,40,35);
INSERT INTO MasterDB.`StageNpc` VALUES(411,4,20,45);
INSERT INTO MasterDB.`StageNpc` VALUES(421,4,1,80);
