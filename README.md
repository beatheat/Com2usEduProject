# 컴투스 서버 캠퍼스 1기 본과제

## 개요

컴투스 서버캠퍼스 1기

<img src = "https://cdn.discordapp.com/attachments/987651625776709654/1110421511279493240/image.png" width = 800></img>

컴투스 서버캠퍼스 1기에서 본과제로 진행한 게임 서버 개발 학습 프로젝트입니다.
API 서버 개발을 타겟으로 하고, 컴투스의 서머너즈워 게임의 컨텐츠를 주로 모작하고 있습니다.

구현할 기능은 
계정
- 계정 생성, 로그인
출석
- 출석과 출석보상
아이템
- 아이템 강화
메일함
- 메일 읽기

인앱 아이템 구매
- 아이템 구매 

스테이지
- 스테이지 진입
- 스테이지 파밍
- 스테이지 클리어 및 보상


## 시스템

계정생성 및 인증



## DB Schema

[스키마](https://github.com/beatheat/Com2usEduProject/blob/master/DB/README.md)

## API

### Account

- CreateAccount

- LoginController

### Attendance

- LoadAttendanceInfo

- ReceiveAttendanceReward

### Chat

- EnterChatLobby

- ExitChatLobby

- ReadChat

- WriteChat

### Enforce

- EnforcePlayerItem

### Mail

- LoadMail

- LoadMailList

- ReceiveMailItem

### Player

- LoadPlayer

- LoadPlayerItem

### Shop

- ReceiveInAppPurchaseItem

### Stage

- CompleteStage

- EnterStage

- FarmStageItem

- FarmStageNpc

- LoadPlayerStageInfo



## 