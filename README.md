# 컴투스 서버 캠퍼스 1기 본과제

## 개요

본 프로젝트는 컴투스 서버캠퍼스 1기에서 본과제로 진행한 게임 서버 개발 학습 프로젝트입니다.
일반적인 수집형 RPG게임의 기본적인 기능들의 구현을 목표로 하고, 컴투스의 서머너즈워 게임의 컨텐츠를 주로 모작하고 있습니다.

API서버로는 C# ASP.NET Core 7.0을 사용하고 있고 DB로는 MySQL과 MemoryDB인 Redis를 사용하고 있습니다.

## Docker 배포

[도커 배포](https://github.com/beatheat/Com2usEduProject/blob/master/Docker/README.md)

## 구현

### 목차
1. [클라이언트 및 게임 요구사항](#클라이언트-및-게임-요구사항)
	- [계정](#1-계정)
	- [우편함](#2-우편함)
	- [플레이어 정보](#3-플레이어-정보)
	- [출석부](#4-출석부)
	- [상점](#5-상점)
	- [강화](#6-강화)
	- [채팅](#7-채팅)
	- [스테이지](#8-스테이지)

2. [서버 구현](#서버-구현)
	- [DB](#db)
	- [미들웨어](#미들웨어)
	- [API](#api)
		- [계정](#계정)
		- [플레이어](#플레이어)
		- [메일함](#메일함)
		- [출석](#출석)
		- [인앱 아이템 상점](#인앱-아이템-상점)
		- [강화](#강화)
		- [스테이지](#스테이지)
		- [채팅](#채팅)

### 클라이언트 및 게임 요구사항

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111866344988745759/image.png" width = 700></img>

1. 빨간 상자
 - 로그인 이후 호출할 모든 API에 보낼 데이터를 표시합니다. 
2. 파란 상자
 - 버튼을 통해 게임서버의 동작이 올바른지 확인합니다. 
3. 노란 상자
 - 파란 상자에서 호출한 요청에 담긴 Json 데이터와 응답으로 돌아온 Json 데이터를 표시합니다.


#### 1. 계정

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111868868772765806/image.png" width = 700></img>

계정 탭에서는 아이디와 비밀번호를 통해 계정 생성 그리고 로그인을 할 수 있습니다.
로그인 시 인증 토큰과 고유번호, 공지사항을 서버로 부터 받습니다.

#### 2. 우편함

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111869402397294672/image.png" width = 700></img>

우편함 탭에서는 사용자가 받은 메일을 확인할 수 있습니다. 좌측 리스트박스에 사용자의 메일리스트가 20개씩 표시되고 <,> 버튼을 통해 다른 페이지의 메일을 모두 확인할 수 있습니다. 메일리스트에서 메일을 더블클릭하면 우편상세에 메일의 내용과 메일에 포함된 아이템이 표시됩니다. 아이템 수령 버튼을 클리겨하면 메일에 포함된 아이템을 획득할 수 있습니다.

#### 3. 플레이어 정보

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111870177592741969/image.png" width = 700></img>

플레이어 정보 탭에서는 플레이어의 닉네임 소지금, 레벨 등 플레이어의 상세 정보와 플레이어가 소지하고 있는 아이템을 확인할 수 있습니다.

#### 4. 출석부


<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111871616746524682/image.png" width = 700></img>

출석부 탭에서는 리스트박스에서 출석보상을 확인할 수 있고 현재 플레이어가 받을 수 있는 보상을 표시합니다. 출석하기 버튼을 클릭하여 메일을 통해 출석보상 아이템을 받을 수 있습니다.


#### 5. 상점

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111871616746524682/image.png" width = 700></img>

상점 탭에서는 구매버튼을 이용해 원하는 상품을 구매할 수 있습니다. 상점 탭은 게임머니가 아닌 외부 시스템을 이용해 결제하는 인앱결제 시스템을 가정한 것입니다. 구매한 아이템은 메일을 통해 플레이어에게 전달됩니다.

#### 6. 강화

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111872975466156084/image.png" width = 700></img>

강화 탭에서는 아이템을 강화할 수 있습니다. 좌측의 리스트박스에서 플레이어가 소지한 아이템 중 강화가능한 아이템이 표시됩니다. 리스트박스에서 아이템을 더블클릭하면 강화 대상에 아이템의 상세정보가 표시됩니다. 강화버튼을 클릭 시 강화가 진행되고 성공 시 강화결과가 표시됩니다. 강화 실패 시 실패했다는 메시지 박스가 나오면서 아이템이 삭제됩니다.


#### 7. 채팅

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111873503663231066/image.png" width = 700></img>

채팅 탭에서는 플레이어 간 채팅을 할 수 있습니다. 중심의 텍스트박스에서 채팅이 표시되고 그 바로 아래 활성화된 텍스트박스에 사용자가 채팅을 입력합니다. 엔터를 누르거나 ▶버튼을 클릭하면 채팅을 보낼 수 있습니다. 우측 하단에 콤보박스에서는 채팅 채널을 표시합니다. 콤보박스를 변경함으로 다른 채팅채널로 이동할 수 있습니다.

#### 8. 스테이지

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111874233316933652/image.png" width = 700></img>

스테이지 탭에서는 원하는 스테이지에 입장하여 파밍을 할 수 있습니다. 좌측에 스테이지 리스트박스에서는 입장가능한 스테이지가 표시됩니다. 입장이 불가능한 스테이지에는 자물쇠가 표시됩니다. 입장가능한 스테이지를 더블클릭하면 스테이지 진행 로그에 스테이지 이름이 표시됩니다. 그 후 자동사냥 시작을 클릭하면 NPC를 처치하거나 아이템 파밍을 자동으로 진행하며 로그를 표시합니다. 스테이지 클리어에 성공하면 성공했다는 메시지박스와 획득한 아이템과 경험치가 표시되고 실패했다면 실패했다는 메시지박스가 표시됩니다. 스테이지 클리어 시 다음 스테이지가 열리게 됩니다.


### 서버 구현

<img src = "https://cdn.discordapp.com/attachments/987651625776709654/1110421511279493240/image.png" width = 700></img>

서버의 구조를 간단하게 보면 위 그림과 같습니다. API 서버가 MySQL과 RedisDB를 수정하여 로직을 수행하고 그 결과를 클라이언트에게 돌려줍니다.


### DB

[DB스키마](https://github.com/beatheat/Com2usEduProject/blob/master/DB/README.md)


### 미들웨어
#### 1. 요청 검증 (CheckUserAuth)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111466180767653889/image.png" width = 700></img><br>
요청 검증 미들웨어는 클라이언트로부터의 요청이 올바른 형식을 갖추고 있는지 검증합니다. 두 가지 단계로 나뉘어 있는데 첫번째는 요청의 마스터 데이터 버전과 클라이언트 데이터 버전을 검증하는 단계이고 두번째는 사용자의 ID값과 사용자에게 발급한 인증토큰을 검증하는 단계입니다. 요청 검증 미들웨어는 요청의 종류의 따라 다르게 작동합니다. 계정생성의 경우 첫번째 단계와 두번째 단계를 건너뛰고 바로 엔드포인트로 요청을 전달합니다. 로그인 요청의 경우 실제로 게임에 접속하기 때문에 첫번째 단계의 검증을 진행합니다. 로그인 시 사용자에게 인증토큰을 발급합니다. 마지막으로 그 외의 모든 요청은 게임진행에 관한 요청이기 때문에 첫번째 단계와 두번째 단계의 검증을 모두 진행하게 됩니다.

### API

API에서 반환하는 데이터 타입은 DB 스키마를 그대로 본 딴 클래스입니다. 
데이터 타입에 대한 상세정보는 아래 링크를 통해 확인해주세요.   
[데이터 타입 참조](https://github.com/beatheat/Com2usEduProject/tree/master/Server/Com2usEduAPIServer/Databases/Schema)

모든 API에서 보내는 요청은 미들웨어에서 검증하는 ClientVersion과 MasterDataVersion, AuthToken, PlayerId, AccountId를 포함해야합니다. 아래 설명에서 등장하는 Request 표에서는 미들웨어에서 검증에 필요한 요청 데이터는 생략합니다.  

### 계정
#### 1. 계정 생성 (CreateAccount) 
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111500502610481172/image.png" width = 700></img><br>  


Request
|Value|Type|Description|
|-|-|-|
|LoginId|string|계정 로그인 ID|
|Password|string|계정 로그인 비밀번호|


Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|

계정생성 요청 시 ID와 비밀번호를 받아 새 계정을 생성하고 AccountDB의 Account 테이블에 삽입합니다. 그 후 기본 플레이어 정보를 생성하여 마찬가지로 GameDB의 관련 테이블에 삽입합니다. 작업 도중 오류가 발생할 시 모든 작업을 롤백하고 오류코드를 반환하고 그렇지 않을 경우엔 None을 반환합니다.

### 2. 로그인(Login)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111514892374118430/image.png" width = 700></img><br>    

Request
|Value|Type|Description|
|-|-|-|
|LoginId|string|계정 로그인 ID|
|Password|string|계정 로그인 비밀번호|


Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|AccountId|int|계정 고유 식별자|
|PlayerId|int|게임 플레이어 고유 식별자|
|AuthToken|string|인증토큰|
|Notice|string|공지사항|


로그인 요청 시 ID와 비밀번호를 받아 AccountDB에 존재하는 계정인지 확인합니다. 존재하는 계정일 경우 GameDB에서 플레이어 정보를 로드합니다. 그 후 인증토큰을 생성하여 Redis에 저장합니다. 마지막으로 공지사항을 Redis에서 로드하고 공지사항, 인증토큰과 플레이어 정보를 반환합니다

### 플레이어

#### 1. 플레이어 정보 로드 (LoadPlayerInfo)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111500544268316702/image.png" width = 700></img><br>    

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|


Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|Player|Player|플레이어 정보|

플레이어 정보 로드는 PlayerId를 받아서 PlayeyId를 키값으로 GameDB의 PlayerTable에서 플레이어 정보를 검색해 반환합니다.



#### 2. 플레이어 아이템 로드 (LoadPlayerItem)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111500544268316702/image.png" width = 700></img><br>    

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|


Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|PlayerItems|PlayerItem[]|플레이어 소유 아이템|

플레이어 아이템 로드는 PlayerId를 받아서 PlayeyId를 키값으로 GameDB의 PlayerItemTable의 플레이어 아이템 정보를 불러와 그대로 반환합니다.


### 메일함

#### 1. 메일 목록 로드 (LoadMailList)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111518894176337971/image.png" width = 700></img><br>    

Request
|Value|Type|Description|Default|
|-|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|-|
|PageNo|int|게임 플레이어 고유 식별자|1|


Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|TotalPageCount|int|플레이어 소유 아이템|
|MailList|Mail[]|선택한 페이지의 메일목록|

메일 목록 로드는 페이지번호를 받아 먼저 플레이어의 메일의 총개수를 구합니다. 메일 총개수를 PageSize(20)로 나누어 총 페이지 수를 구합니다.
그 후 PageSize * PageNo를 offset으로 PageSize를 limit으로 잡고 최근 발송된 메일 순으로 정렬된 MailTable에서 메일을 검색합니다. 그러면 원하는 메일테이블을 PageSize크기로 나눴을 때 PageNo번째의 메일을 획득할 수 있습니다. 또한 메일 목록 로드 로직에서는 데이터 전송량을 줄이기 위해 메일을 검색할 때 메일내용과 메일에 포함된 아이템까지는 검색하지 않습니다. 마지막으로 획득한 메일과 총 페이지 수를 반환합니다.


#### 2. 메일 로드 (LoadMail)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111523619441475604/image.png" width = 700></img><br>    

Request
|Value|Type|Description|Default|
|-|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|
|MailId|int|메일 고유 식별자|


Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|Mail|Mail|선택한 메일 데이터|

메일 로드는 MailId를 통해 MailTable에서 메일을 검색하여 반환합니다. 단 검색한 메일의 PlayerId가 요청의 PlayerId와 다를 경우 메일 소유자가 요청한 것이 아님으로 에러코드를 반환합니다.

#### 3. 메일 아이템 수령 (ReceiveMailItem)

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111527873455591494/image.png" width = 700></img><br>    

Request
|Value|Type|Description|Default|
|-|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|
|MailId|int|메일 고유 식별자|


Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|

메일 아이템 수령은 먼저 MailId를 통해 메일을 검색하고 메일 아이템을 이미 수령했는지 확인합니다. 이미 수령했다면 에러코드를 반환합니다. 또 검색한 메일의 PlayerId가 요청의 PlayerId와 다를 경우 메일 소유자가 요청한 것이 아님으로 에러코드를 반환합니다. 검증이 모두 끝났으면 메일에 포함된 아이템코드를 통해 MasterDB에서 아이템 정보를 로드합니다. 그 후 아이템 정보를 이용해 PlayerItem 데이터를 생성합니다. 생성한 PlayerItem을 PlayerItemTable에 삽입하고 모두 성공하면 메일 아이템 수령여부를 수령으로 바꾸고 종료합니다.

### 출석


#### 1. 출석 정보 로드 (LoadAttendanceInfo)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111500581580832788/image.png" width = 700></img><br>   

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|


Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드| 
|AttendanceInfo|PlayerAttendance|플레이어 출석 정보| 

출석 정보 로드 요청은 PlayerId를 받아서 PlayerId를 키값으로 GameDB의 출석 정보를 불러와 그대로 반환합니다.


#### 2. 출석보상 받기 (ReceiveAttendanceReward)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111500604750172291/image.png" width = 700></img><br>  

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|

Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|

출석보상 받기 요청은 PlayerId를 받아 먼저 PlayerAttendanceTable에서 플레이어의 출석 정보를 불러옵니다. 출석정보에는 플레이어의 마지막 출석일과 연속출석일가 저장되어 있습니다. 마지막 출석일이 오늘과 같다면 보상메일을 발송하지 않습니다. 또한 현재 날짜와 마지막 출석일의 차이가 1일을 초과하면 연속출석일을 0으로 초기화합니다. 그 후 연속출석일에 1을 더한 값에 해당하는 출석보상 아이템을 MasterDB에서 찾습니다. 그 후 출석보상 아이템을 포함한 메일을 생성해 해당 플레이어의 MailTable에 삽입하여 출석보상을 전달합니다. 마지막으로 연속출석일에 1을 더하고 마지막 출석일을 오늘로 변경한 뒤 PlayerAttendanceTable에 값을 업데이트합니다.


### 인앱 아이템 상점

#### 1. 인앱 상점 아이템 수령 (ReceiveInAppPurchaseItem)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111552752405979136/image.png" width = 700></img><br>  


Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|
|BillToken|long|영수증 고유번호|
|ShopCode|int|구매한 아이템 코드|

Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|


인앱 상점 아이템 수령은 플레이어가 앱에서 결제를 하고 받은 영수증을 요청으로 보내면 플레이어의 메일로 아이템을 보냅니다.
영수증 고유번호를 받으면 먼저 BillTable에 영수증을 삽입해봅니다. 만일 이미 같은 영수증이 있다면 중복 영수증 오류를 그대로 반환합니다.
영수증이 정상적으로 추가되었다면 MasterDB에서 ShopCode에 해당하는 아이템 정보를 불러옵니다. 마지막으로 아이템을 포함한 메일을 생성해 MailTable에 추가해 플레이어에게 아이템을 전달합니다.

### 강화

#### 1. 아이템 강화 (EnforcePlayerItem)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111553977381834833/image.png" width = 700></img><br>  

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|
|PlayerItemId|int|플레이어 아이템 고유 식별자|

Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|EnforceState|EnforceState|강화상태(성공,실패여부)|
|EnforcedItem|PlayerItem|강화한 아이템|

아이템 강화는 먼저 PlayerItemId을 통해 PlayerItemTable에서 아이템 정보를 로드합니다. 만약 로드한 플레이어 아이템의 PlayerId와 요청의 PlayerId가 다를 경우 소유주가 다르니 에러코드를 반환합니다. 검증이 끝나면 마스터 데이터에서 플레이어 아이템의 상세 데이터를 로드합니다. 그 후 플레이어 아이템이 강화할 수 있는 타입(무기나 방어구)인지와 강화횟수가 남아있는지 검증합니다. 검증에 통과하지 못한다면 에러코드를 반환합니다. 모든 검증이 끝나면 30%의 확률로 아이템을 강화하고 실패한다면 아이템을 삭제합니다. 강화가 종료된후 강화상태와 강화에 성공했다면 강화한 아이템을 반환합니다.


### 스테이지

#### 1. 스테이지 정보 로드 (LoadPlayerStageInfo)

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111556365446557727/image.png" width = 700></img><br>  

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|

Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|MaxStageCode|int|마지막 스테이지 코드|
|ClearStageCodes|int[]|클리어한 스테이지 코드 리스트|
|AccessibleStageCodes|int[]|입장가능한 스테이지 코드 리스트|

스테이지에서 정보 로드 요청이 오면  PlayerTable에서 플레이어가 클리어한 최고 스테이지 코드를 불러옵니다. 이 코드를 이용해 ClearStageCodes = (1 ~ 최고 스테이지 코드), AccessibleStageCodes = (1 ~ 최고 스테이지 코드 + 1) 두 데이터를 생성해 반환합니다.


#### 2. 스테이지 입장 (EnterStage)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111606414285230130/image.png" width = 700></img><br>  

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|
|StageCode|int|입장할 스테이지 코드|

Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|StageItems|StageItem[]|입장한 스테이지의 아이템 목록|
|StageNpcs|StageNpc[]|입장한 스테이지의 NPC목록|


스테이지 입장 시 PlayerTable에서 최고 클리어 스테이지 코드를 로드해 플레이어가 입장하려는 스테이지가 입장가능한 스테이지인지 먼저 검증하고 그렇지 않다면 에러코드를 반환합니다. 이후 아래와 같은 플레이어 스테이지 정보를 새로 생성해 Redis에 저장합니다. 만일 Redis에 이미 플레이어 정보가 있다면 이미 스테이지에 입장해 있는 상태이기 떄문에 에러코드를 반환합니다. 

```c#
public class PlayerInGameStageInfo
{
	public int PlayerId { get; set; }
	public int PlayingStageCode { get; set; }
	public int HighestClearStageCode { get; set; }
    // 스테이지에서 파밍한 아이템 코드와 개수를 저장 key: 아이템 코드, value :아이템 개수
	public Dictionary<int, int> FarmedStageItemCounts { get; set; } = new ();
    // 스테이지에서 파밍한 NPC 코드와 개수를 저장 key: npc 코드, value: npc 개수
	public Dictionary<int,int> FarmedStageNpcCounts { get; set; } = new();

    // 해당 스테이지에서 파밍할 수 있는 아이템 코드와 최대개수를 저장 key: 아이템 코드, value: 아이템 최대개수
	public Dictionary<int, int> MaxAvailableItemCounts { get; set; } = new();
    // 해당 스테이지에서 파밍할 수 있는 NPC 코드와 최대개수를 저장 key: NPC 코드, value: NPC 최대개수
	public Dictionary<int, int> MaxAvailableStageNpcCounts { get; set; } = new();
}
```

- 스테이지 정보 예시

```
마스터 데이터
스테이지 1 아이템{ {ItemCode : 1, ItemCount :3 }, {ItemCode : 2, ItemCount: 10} }
스테이지 1 NPC{ {NpcCode : 110, NpcCount :5 }, {NpcCode : 135, NpcCount 9} }
```
위의 마스터 데이터를 가지고 있다면 스테이지 1에 진입할때 아래와 같은 스테이지 정보를 만들게 됩니다.
```c#
public class PlayerInGameStageInfo
{
	public int PlayerId ;
	public int PlayingStageCode = 1;
	public int HighestClearStageCode;
	public Dictionary<int, int> FarmedStageItemCounts = new (){
        [1] = 0,
        [2] = 0
    };
	public Dictionary<int,int> FarmedStageNpcCounts = new(){
        [110] = 0,
        [135] = 0
    };

	public Dictionary<int, int> MaxAvailableItemCounts = new(){
        [1] = 3,
        [2] = 10
    };
	public Dictionary<int, int> MaxAvailableStageNpcCounts  = new(){
        [110] = 5,
        [135] = 9
    };
}
```

#### 3. 스테이지 아이템 파밍 (FarmStageItem)

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111601819974647808/image.png" width = 700></img><br>  

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|
|ItemCode|int|파밍한 아이템 코드|
|ItemCount|int|파밍한 아이템 개수|

Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|

스테이지 아이템 파밍은 플레이어로부터 파밍한 아이템의 코드와 개수를 받아 Redis에 저장된 플레이어의 스테이지 정보에 추가합니다. 플레이어가 스테이지에 입장중이 아니거나 현재 스테이지에 속하지 않는 아이템을 파밍했거나 혹은 파밍한 아이템의 개수가 스테이지에서 얻을 수 있는 최대개수보다 많을 경우 에러코드를 반환합니다.


#### 4. 스테이지 NPC 파밍 (FarmStgeNpc)

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111608423600111716/image.png" width = 700></img><br>  


Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|
|NpcCode|int|파밍한 아이템 코드|

Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|



스테이지 NPC 파밍은 플레이어로부터 파밍한 NPC 코드를 받아 Redis에 저장된 플레이어의 스테이지 정보에 추가합니다. NPC는 아이템과 다르게 한개씩만 파밍할 수 있습니다. 플레이어가 스테이지에 입장중이 아니거나 현재 스테이지에 속하지 않는 NPC를 파밍했거나 혹은 파밍한 NPC의 개수가 스테이지에서 얻을 수 있는 최대개수보다 많을 경우 에러코드를 반환합니다.



#### 5. 스테이지 완료 (CompleteStage)

<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111613280805466122/image.png" width = 700></img><br>  


Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|

Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|IsStageCleared|bool|스테이지 클리어 여부|

스테이지 완료 요청이 오면 먼저 Redis에서 스테이지 정보를 불러옵니다. 만일 스테이지 정보가 없다면 에러코드를 반환하고 있다면 로드 후 제거합니다. 그 후 스테이지 정보에서 Npc가 전부 제거되었는지 확인하여 클리어여부를 결정합니다. 클리어하지 않았다면 클리어 여부를 반환합니다. 클리어했다면 스테이지 보상을 MasterDB에서 로드하여 아이템 종류에 맞게 PlayerTable이나 PlayerItemTable에 삽입합니다. 마지막으로 클리어한 스테이지가 지금까지 클리어하지 않았던 최고 스테이지일 경우 PlayerTable에 플레이어가 클리어한 최고 스테이지 값을 업데이트합니다.

### 채팅

채팅은 소켓을 사용하지 않고 클라이언트에서 요청을 지속적으로 보내는 것을 가정하고 개발했습니다.
서버가 시작되면 채팅로비에 인원수 정보를 Redis에서 초기화합니다. RedisList를 통해 0~100 인덱스에 0을 저장합니다.


#### 1. 채팅방 입장 (EnterChatLobby)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111629220280541245/image.png" width = 700></img><br>  


Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|
|LobbyNumber|int|입장할 로비번호(1~100)|

Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|LobbyNumber|int|입장한 로비번호(1~100)|
|ChatHistory|Chat[]|채팅 히스토리|

채팅방 입장 요청은 로비번호를 지정하지 않을 경우 자동으로 지정됩니다. 1번부터 100번 로비까지 존재하는데 로비 수용률이 75%미만이고 로비번호가 작은 순으로 플레이어를 배정합니다. 모든 로비가 가득 찼을 경우 에러코드를 반환합니다. 로비번호가 정해졌으면 먼저 채팅로비 인원수를 불러와 MaxLobbyUserNumber(100)보다 작은지 확인하고 작다면 1을 더합니다. 그 후 ChatUser라는 채팅 사용자 정보를 생성해 Redis에 저장합니다. 이후 ChatUser를 통해 사용자가 자신이 속한 로비에서 채팅을 하고 있는지 판단합니다. 마지막으로 Redis에서 해당 로비의 Chat을 가장 최근 채팅부터 ChatHistorySize(50)만큼 로드하여 접속한 로비번호와 함께 반환합니다.   

Chat은 채팅 한줄을 표현한 클래스입니다. 플레이어의 고유번호인 PlayerId와 화면에 표시될 PlayerNickname 그리고 채팅의 내용인 Content로 구성되어 있습니다. Index는 채팅의 순서입니다. 채팅을 저장하는데 RedisList가 Queue와 같이 사용됩니다. Chat을 RedisList에 한방향으로 계속 삽입하며 RedisList에서의 Index가 Chat의 Index가 됩니다. Index는 또한 Chat의 고유한 지정자로써 활용하여 특정 Chat의 위치를 집어 그 주변의 채팅을 가져올 수 있습니다.

채팅 로비 인원수가 99일때 동시에 여러 사용자가 접속하면 인원을 확인하고 더하는 로직이 원자적으로 작동하지 않기 떄문에 동시에 접속한 사용자가 모두 접속될 수 있습니다. 이를 방지하기 위해 두 동작이 원자적으로 작동될 수 있게 Redis에 EnterLobbyLock_(LobbyNumber)를 생성하여 각 로비마다 로비 입장은 원자적으로 이루어지게 했습니다. 


```c#
public class ChatUser
{
	public int PlayerId { get; set; } = 0;
	public int LobbyNumber { get; set; } = 0;
}

public class Chat
{
	public long Index { get; set; }
	public int PlayerId { get; set; }
	public string PlayerNickname { get; set; }
	public string Content { get; set; }
}
```

#### 2. 채팅방 퇴장 (ExitChatLobby)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111629770464174130/image.png" width = 700></img><br>  

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|

Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|

사용자의 ChatUser정보가 있다면 채팅 로비 인원수를 1 줄이고 ChatUser를 삭제합니다. ChatUser정보가 없다면 에러코드를 반환합니다.


#### 3. 채팅 쓰기 (WriteChat)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111630647598010408/image.png" width = 700></img><br>  

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|
|PlayerNickName|string|플레이어의 별명|
|LobbyNumber|int|입장한 로비번호(1~100)|
|Chat|string|플레이어가 작성한 채팅|



Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|

사용자의 ChatUser정보가 존재하는지 검증하고 Redis에 Chat을 삽입합니다.


#### 4. 채팅 읽기 (ReadChat)
<img src = "https://cdn.discordapp.com/attachments/987652135107850315/1111630978083995751/image.png" width = 700></img><br>  

Request
|Value|Type|Description|
|-|-|-|
|PlayerId|int|게임 플레이어 고유 식별자|
|LobbyNumber|int|입장한 로비번호(1~100)|
|LastChatIndex|long|마지막으로 받은 채팅의 번호|


Response
|Value|Type|Description|
|-|-|-|
|Result|ErrorCode|에러코드|
|LobbyNumber|int|입장한 로비번호(1~100)|
|Chats|Chat[]|최신 채팅 리스트|


사용자의 ChatUser정보가 존재하는지 검증하고 Redis에서 LastChatIndex를 기준으로 최신 Chat을 불러옵니다.
만약 LastChatIndex가 음수라면 에러코드를 반환합니다. 또한 LastChatIndex부터 최신 Chat의 Index차이가 ChatHistorySize(50) 보다 크다면 최신 50개의 Chat만을 반환합니다.

