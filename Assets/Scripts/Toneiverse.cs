using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Toneiverse
{
    public static class Validator
    {
        public static bool MatchPw(string pw)
        {
            if (string.IsNullOrEmpty(pw))
                return false;
            return Regex.IsMatch(pw, pwPattern);
        }
        public static bool MatchEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            return Regex.IsMatch(email, emailPattern);
        }
        private const string pwPattern = "^[a-zA-Z0-9`~!@#$%^&*()_\\-+=\\[\\]{}|;:'\",<.>/?]{8,16}$";
        private const string emailPattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.(com|net|org|kr)$";
    }
    namespace DTO
    {
        [Serializable]
        public class Lipstick
        {
            public string token, hex_code;
        }
        [Serializable]
        public class InfoJson
        {
            public string name, hex_code, color_id, msg, description, token, email, cname;
        }
        [Serializable]
        public class Json<T>
        {
            public T result;
        }
        [Serializable]
        public class Token
        {
            public string token;
        }
        [Serializable]
        public class SignUpJson
        {
            public string result;
            public string token;
        }
        [Serializable]
        public class JsonList<T>
        {
            public List<T> result;
        }
        [Serializable]
        public class ColorInfo
        {
            public string hex_code, cname;
        }

        [Serializable]
        public class ColorJson
        {
            public string color_id, hex_code, cname;
        }

        [Serializable]
        public class Message
        {
            public string chat_id, name, msg;
        }
    }

    public enum SceneIndex
    {
        Title = 0,  // 번호를 명시하는 게 안전합니다 (Build Settings 순서와 일치)
        SignUp = 1,
        Test = 2,
        Result = 3,
        Chat = 4,
        Update = 5,
        GetPW = 6
    }
}