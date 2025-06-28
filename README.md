# 🎮 WarmCoolUnity - Unity AR 기반 퍼스널 컬러 소셜 시스템

## 📌 프로젝트 개요

**WarmCoolUnity**는 사용자의 얼굴 이미지를 기반으로 퍼스널 컬러를 분석하고, AR 가상 메이크업을 적용하며, 같은 퍼스널 컬러 그룹의 사용자들과 실시간 채팅까지 가능하게 하는 Unity 기반 통합 뷰티 소셜 시스템입니다.

FastAPI 서버와 YOLOv11-CLS 모델을 연동하여 퍼스널 컬러를 분류하고, Unity에서 ARFoundation 및 Photon을 통해 실시간 인터랙션을 제공합니다.

---

## 🎯 주요 기능

- 📷 Unity ARFoundation으로 얼굴 이미지 캡처
- 🛰 FastAPI 서버로 이미지 전송 및 분석 결과 수신
- 💄 분석 결과에 따라 AR 가상 메이크업 적용
- 💬 Photon Chat으로 같은 퍼스널 컬러 그룹 사용자 간 실시간 채팅
- 🤖 LLM 기반 상황과 퍼스널 컬러에 맞는 립스틱 추천
---


## 🏗 시스템 구성

프로젝트는 다음 다섯 개의 주요 리포지토리로 구성되어 있습니다:

### 1. [WarmCoolYolo](https://github.com/An0jin/WarmCoolYolo)

- YOLO12 기반 퍼스널 컬러 분류 모델
- Roboflow를 통한 데이터셋 관리
- 모델 학습 및 평가 파이프라인

### 2. [WarmCoolFastapi](https://github.com/An0jin/WarmCoolFastapi)

- FastAPI 기반 백엔드 서버
- YOLOv12 모델 서빙
- RESTful API 엔드포인트 제공
- Postgresql 데이터베이스 연동

### 3. [WarmCoolUnity](https://github.com/An0jin/WarmCoolUnity)

- Unity 기반 AR 애플리케이션
- ARFoundation을 통한 얼굴 인식
- 가상 메이크업 적용
- Photon 기반 실시간 채팅

### 4. [WarmCoolSQL](https://github.com/An0jin/WarmCoolSQL)

- 채팅 정보 관리
- 유저 정보 관리
- 퍼스널 컬러 해설

### 5. [WarmCoolDataset](https://github.com/An0jin/WarmCoolDataset)

- roboflow를 활용한 데이터 수집
- github를 활용한 데이터 수집
- open CV를 활용한 데이터 증강

### 6. [WarmCoolVim](https://github.com/An0jin/WarmCoolVim)

- vim으로 작성한 쉘 스크립트
- 쉘 스크립트를 활용한 도커 조작
- 쉘 스크립트를 활용한 오라클 클라우드 접속
- 쉘 스크립트를 활용한 웹호스팅

---


## 🖼️ 에셋 출처

- ### 폰트
    - [신촌랩소디체](https://noonnu.cc/font_page/1577)

- ### 참고 이미지
    - [![AR Face Assets](https://img.shields.io/badge/-AR%20Face%20Assets-000000?style=flat&logo=unity&logoColor=white)](https://assetstore.unity.com/packages/essentials/asset-packs/ar-face-assets-184187)


---

## 🛠 사용 기술

### 🧑‍💻 코딩

- ![Unity](https://img.shields.io/badge/-Unity-000000?style=flat&logo=unity&logoColor=white)
- ![ARFoundation](https://img.shields.io/badge/-ARFoundation(Unity)-000000?style=flat&logo=unity&logoColor=white)
- ![Photon Chat](https://img.shields.io/badge/-Photon%20Chat(Unity)-004480?style=flat&logo=photon&logoColor=white)

### 🎨 UI/UX 디자인
- ![ChatGPT 4.0](https://img.shields.io/badge/-ChatGPT%204.0-74AA9C?style=flat&logo=openai&logoColor=white)
- ![Photoshop](https://img.shields.io/badge/-Photoshop-31A8FF?style=flat&logo=adobe-photoshop&logoColor=white)