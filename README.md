# 🎨 Toneiverse (Unity AR Showcase)

이 리포지토리는 실제 Google Play 스토어에 출시된 AI 퍼스널 컬러 진단 서비스 **Toneiverse**의 Unity 프론트엔드 및 AR 기술 스택을 집약한 기술 쇼케이스입니다. ARFoundation을 통한 실시간 얼굴 인식과 소셜 연동 기능이 통합된 완성도 높은 프로덕션 수준의 Unity 애플리케이션입니다.

---

## 💎 Technical Highlights

### 1. AR Reality Interaction (ARFoundation)

Unity의 **ARFoundation**을 활용하여 사용자에게 몰입감 있는 퍼스널 컬러 체험을 제공합니다.

- **Real-time Face Tracking**: 고성능 AR Face Mesh 기술을 적용하여 정교한 얼굴 윤곽 트래킹 구현.
- **Dynamic Virtual Makeup**: 이미지 분석 결과에 따른 퍼스널 컬러 맞춤형 가상 메이크업 레이어링 최적화.
- **High-Fidelity Capture**: 실제 분석을 위해 조명 및 각도를 고려한 최적의 얼굴 데이터 캡처 기능.

### 2. Real-time Social Networking (Photon)

퍼스널 컬러를 매개로 한 실시간 커뮤니티 환경을 **Photon**을 통해 구현했습니다.

- **Personal Color Grouping**: 사용자의 진단 결과(웜톤/쿨톤)에 따라 자동으로 채팅 채널(Photon Chat) 배정.
- **Seamless Synchronization**: 실시간 메시징 기술 최적화 및 안정성 확보.
- **In-App Social Experience**: 사용자들이 뷰티 팁을 공유할 수 있는 소셜 공간 구축.

### 3. Production Architecture & UX

사용자 경험과 시스템 안정성을 고려한 설계를 갖추고 있습니다.

- **Efficient Data Handling**: `JsonUtility`를 활용한 가벼운 데이터 파이싱 및 `UnityWebRequest` 기반의 비동기 통신 최적화.
- **Automated Deployment**: 상용 도메인(`duckdns`) 연동을 통한 안정적인 서비스 엔드포인트 관리.

---

## 🚀 Key Achievements

- **Immersive AR Experience**: 최신 AR 페이스 메시 기술을 통한 자연스러운 가상 메이크업 시뮬레이션.
- **Production Integration**: 실시간 통신 및 AR 기술이 집약된 실제 배포 수준의 완성도.

---

## 🛠️ Technology Stack

| 카테고리                      | 사용 기술 (Stack)                                                                                                                                                                                                                                                                                                      |
| :---------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **OS**                  | ![Windows](https://img.shields.io/badge/-Windows-0078D6?style=flat&logo=windows&logoColor=white)                                                                                                                                                                                                                         |
| **AR & Development**    | ![Unity](https://img.shields.io/badge/-Unity-2D2D2D?style=flat&logo=unity&logoColor=white) ![ARFoundation](https://img.shields.io/badge/-ARFoundation-000000?style=flat&logo=unity&logoColor=white) ![C#](https://img.shields.io/badge/-C%23-239120?style=flat&logo=c-sharp&logoColor=white)                                 |
| **Networking & Social** | ![Photon](https://img.shields.io/badge/-Photon%20Engine-004480?style=flat&logo=photon&logoColor=white) ![UnityWebRequest](https://img.shields.io/badge/-UnityWebRequest-000000?style=flat&logo=unity&logoColor=white)                                                                                                     |
| **Design & Creative**   | ![Gemini](https://img.shields.io/badge/-Gemini-8E75B2?style=flat&logo=googlegemini&logoColor=white) ![NotebookLM](https://img.shields.io/badge/-NotebookLM-4285F4?style=flat&logo=google&logoColor=white) ![Adobe Photoshop](https://img.shields.io/badge/-Photoshop-31A8FF?style=flat&logo=adobe-photoshop&logoColor=white) |
