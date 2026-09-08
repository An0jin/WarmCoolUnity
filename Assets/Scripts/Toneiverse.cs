using System;
using System.Collections.Generic;

namespace Toneiverse
{
    namespace DTO
    {
        [Serializable]
        public class UserInfo
        {
            public string name;
            public string pw;
            public string token;
            public string sex;
            public string year;
        }

        [Serializable]
        public class ProfileSetupJson
        {
            public string token;
            public string sex;
            public string year;
        }

        [Serializable]
        public class Lipstick
        {
            public string token, hex_code;
        }

        [Serializable]
        public class InfoJson
        {
            public string name, hex_code, color_id, msg, description, token, email, cname, sex, year;
        }

        [Serializable]
        public class Json<T>
        {
            public T result;
        }

        [Serializable]
        public class PutJson
        {
            public string result;
            public string token;
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
        public class LLMResponse
        {
            public string hex_code, cname, result;
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
        Title = 0,
        SignUp = 1,
        Test = 2,
        Result = 3,
        Chat = 4,
        Update = 5,
        GetPW = 6,
        ProfileSetup = 7,
        LipstickCheck = 8
    }
}