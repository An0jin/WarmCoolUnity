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

---

## 🏗 시스템 구성

프로젝트는 다음 다섯 개의 주요 리포지토리로 구성되어 있습니다:

### 1. [WarmCoolYolo](https://github.com/anyoungjin20040106/WarmCoolYolo)

- YOLOv11-CLS 기반 퍼스널 컬러 분류 모델
- Roboflow를 통한 데이터셋 관리
- 모델 학습 및 평가 파이프라인

### 2. [WarmCoolFastapi](https://github.com/anyoungjin20040106/WarmCoolFastapi)

- FastAPI 기반 백엔드 서버
- YOLOv11-CLS 모델 서빙
- RESTful API 엔드포인트 제공
- Postgresql 데이터베이스 연동

### 3. [WarmCoolUnity](https://github.com/anyoungjin20040106/WarmCoolUnity)

- Unity 기반 AR 애플리케이션
- ARFoundation을 통한 얼굴 인식
- 가상 메이크업 적용
- Photon 기반 실시간 채팅

### 4. [WarmCoolSQL](https://github.com/anyoungjin20040106/WarmCoolSQL)

- 채팅 정보 관리
- 유저 정보 관리
- 퍼스널 컬러 해설

### 5. [WarmCoolDataset](https://github.com/anyoungjin20040106/WarmCoolDataset)

- roboflow를 활용한 데이터 수집
- github를 활용한 데이터 수집
- 데이터 라벨링

---

## 🛠 사용 기술

- ![Unity](https://img.shields.io/badge/-Unity-000000?style=flat&logo=unity&logoColor=white)
- ![ARFoundation](https://img.shields.io/badge/-ARFoundation(Unity)-000000?style=flat&logo=unity&logoColor=white)
- ![Photon Chat](https://img.shields.io/badge/-Photon%20Chat(Unity)-004480?style=flat&logo=photon&logoColor=white)
---

## 🖼️ 에셋 출처

- ### 폰트
    - [신촌랩소디체](https://noonnu.cc/font_page/1577)

- ### 참고 이미지
    - [![AR Face Assets](https://img.shields.io/badge/-AR%20Face%20Assets-000000?style=flat&logo=unity&logoColor=white)](https://assetstore.unity.com/packages/essentials/asset-packs/ar-face-assets-184187)

- ### UI/UX 이미지
    - ![ChatGPT 4.0](https://img.shields.io/badge/-ChatGPT%204.0-74AA9C?style=flat&logo=openai&logoColor=white)


---


## 💻 기술 스택

- **AI/ML**: ![Ultralytics(YOLOv11-CLS)](https://img.shields.io/badge/YOLOv11--CLS-111F68?style=flat&logo=Ultralytics&logoColor=white)
- **백엔드**: ![FastAPI](https://img.shields.io/badge/-FastAPI-009688?style=flat&logo=fastapi&logoColor=white), ![docker](https://img.shields.io/badge/-docker-2496ED?style=flat&logo=docker&logoColor=white), ![amazonec2](https://img.shields.io/badge/-AWS%20EC2-FF9900?style=flat&logo=amazonec2&logoColor=white)
- **DB** : ![Postgresql](https://img.shields.io/badge/-postgresql-4169E1?style=flat&logo=postgresql&logoColor=white), ![amazonrds](https://img.shields.io/badge/-amazonrds-527FFF?style=flat&logo=amazonrds&logoColor=white)
- **프론트엔드**: ![Unity(ARFoundation)](https://img.shields.io/badge/-ARFoundation-000000?style=flat&logo=unity&logoColor=white)
- **네트워킹**: ![Photon Chat](https://img.shields.io/badge/-Photon%20Chat-004480?style=flat&logo=photon&logoColor=white)
- **데이터 수집**: ![Roboflow](https://img.shields.io/badge/-roboflow-6706CE?style=flat&logo=roboflow&logoColor=white),![github](https://img.shields.io/badge/-github-000000?style=flat&logo=github&logoColor=white)
- **디자인**: ![Photoshop](https://img.shields.io/badge/-Photoshop-31A8FF?style=flat&logo=adobe-photoshop&logoColor=white)