# AVGErrorCheck
Detect Error of AVG

## 목차
[1. 프로젝트 개요](#프로젝트-개요)  
[2. 개발 리뷰](#개발-리뷰)  
[3. 개발 환경](#개발-환경)   
[4. 개발 내용](#개발-내용)       

## 프로젝트 개요 (2017.01)
* **명칭**  
AGV 에러 체크
* **장르**  
Util Program
* **개요**  
공장의 AGV(**Automated Guided Vehicles**)가 작동 불능인 상태일 때, 적색 LED와 경보음을 울려 알림
* **소개**   
'17년 겨울에 학교 선배 회사에서 일할때, 맡아서 진행했던 프로젝트. 기존에 있던 AGV 컨트롤 프로그램은 수정하면 안되고 새로운 프로그램을 개발해야 했던 상황.

## 개발 리뷰
과제 외에 처음으로 코딩으로 이루어낸 프로젝트이다. 선배가 이 프로젝트를 처음 나에게 맡긴다고 했을 때, 너무 부담이 되었다. 고작 3학년으로 올라가는 학부생일 뿐이였는데 사업 중에 하나를 내가 맡게된 것이였다. 하지만 선배는 '내가 너 커버 못치면 너한테 주지도 않았어' 라는 말로 격려 해주셨다. 이때부터 이 말이 머리에 박혀 더 열심히 공부하고 개발했던것 같다. 이 당시, C#을 생전 처음 다뤄보았다. 하지만 Java와 MFC를 다룰줄 알아서 금방 숙달 되었다. Serial 통신, PLC 처음 접해보는 것들이 많았지만 선배의 도움과 구글링으로 결국엔 완성해서 기한내에 테스트까지 완료하고 납품할 수 있었다. 나에게는 개발자로서의 진로가 정해지는 계기가 아니였나 싶다.

## 개발 환경
* Window
* C#
* Visual Studio Spy++
* PLC(Programmable Login Controller)
* Serial Communication

## 개발 내용
* PLC로 LED 램프 컨트롤
    * **적색**(비프음), **황색**, **녹색**
* Serial 통신으로 PLC 컨트롤
    * 어떤 문자열를 보내면 되는지 확인
        ```
        55 AA 01 02 00 06 01 09
        55 AA 01 02 00 06 01 0A
        55 AA 01 02 00 06 01 0B
        55 AA 01 02 00 06 01 08
        ```
        다음과 같은 문자열을 변환하여 포트로 보내면 동작하는 것을 확인
* **기존 프로그램**에서 에러 감지
    * Visual Studio Spy++ 를 이용
    <img width="500" alt="ezgif com-gif-maker" src="https://user-images.githubusercontent.com/27637757/98365479-d6169a00-2075-11eb-8b45-778511408d4b.JPG">
    
        우상단의 빨간색 박스가 깜빡거리는 것 감지

* C#을 이용해 프로그램 실행시 포트를 연결하고 창 최소화
* 0.5초동안 깜빡거리는 횟수가 0이 아니면 에러로 판단
* 에러판단 시 지정한 Serial을 포트를 통해 전송
    * 열려있는(켜져있는) 램프를 닫고 닫혀있는(꺼져있는) 램프를 열어야 하기 떄문에 두 개의 램프를 컨트롤 하는 Serial을 보낸다
