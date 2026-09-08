using System; // [Serializable] 등 기본 어트리뷰트 사용을 위한 System 네임스페이스 참조
using System.Collections.Generic; // List<T> 데이터구조 사용을 위한 System.Collections.Generic 참조

namespace Toneiverse // 메인 프로젝트 네임스페이스
{
    /// <summary>서버 API 요청과 응답의 JSON 구조를 표현하는 데이터 객체 모음입니다.</summary>
    namespace DTO // 통신용 데이터 구조체(Data Transfer Objects) 모음
    {
        [Serializable] // JsonUtility 직렬화 지원 어트리뷰트
        public class UserInfo // 유저 정보 변경 DTO
        {
            public string name; // 이름
            public string pw; // 비밀번호
            public string token; // 인증 토큰
            public string sex; // 성별
            public string year; // 출생 연도
        }

        [Serializable]
        public class ProfileSetupJson // 프로필 설정 DTO
        {
            public string token; // 토큰
            public string sex; // 성별
            public string year; // 연도
        }

        [Serializable]
        public class Lipstick // 립스틱 정보 저장 DTO
        {
            public string token, hex_code; // 토큰 및 립스틱 HEX 색상 코드
        }

        [Serializable]
        public class InfoJson // 유저 프로필 상세 데이터 DTO
        {
            public string name, hex_code, color_id, msg, description, token, email, cname, sex, year; // 상세 필드 모음
        }

        [Serializable]
        public class Json<T> // 단일 응답 제네릭 래퍼 클래스
        {
            public T result; // 데이터 결과 필드
        }

        [Serializable]
        public class PutJson // 수정 결과 응답 DTO
        {
            public string result; // 결과 상태
            public string token; // 토큰
        }

        [Serializable]
        public class Token // 인증 토큰 저장용 DTO
        {
            public string token; // 토큰 값
        }

        [Serializable]
        public class SignUpJson // 회원가입 응답 DTO
        {
            public string result; // 가입 결과
            public string token; // 토큰 값
        }

        [Serializable]
        public class JsonList<T> // 리스트 응답 제네릭 래퍼 클래스
        {
            public List<T> result; // 결과 리스트
        }

        [Serializable]
        public class LLMResponse // LLM 추천 데이터 DTO
        {
            public string hex_code, cname, result; // 색상 코드, 색상명, 추천 텍스트
        }

        [Serializable]
        public class ColorJson // 퍼스널컬러 항목 DTO
        {
            public string color_id, hex_code, cname; // 컬러ID, 색상코드, 색상명
        }

        [Serializable]
        public class Message // 채팅 데이터 DTO
        {
            public string chat_id, name, msg; // 채팅ID, 전송자, 메시지 본문
        }
    }

    /// <summary>Build Settings에 등록된 앱 씬의 인덱스입니다.</summary>
    public enum SceneIndex // Unity Build Settings에 등록된 씬 순서 열거형
    {
        Title = 0, // 타이틀 화면
        SignUp = 1, // 회원가입 화면
        Test = 2, // 퍼스널컬러 측정 화면
        Result = 3, // 결과 확인 화면
        Chat = 4, // 채팅 화면
        Update = 5, // 정보 수정 화면
        GetPW = 6, // 비밀번호 찾기 화면
        ProfileSetup = 7, // 프로필 설정 화면
        LipstickCheck = 8 // 립스틱 확인 화면
    }
}
