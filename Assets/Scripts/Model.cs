using System;
using System.Collections.Generic;

[Serializable]
public class InfoJson{
    public string name,hex_code, color_id,msg,description,token,email,cname;
}
[Serializable]
public class Json<T>{
    public T result;
}
[Serializable]
public class Token{
    public string token;
}
[Serializable]
public class SignUpJson{
    public string result;
    public string token;
}
[Serializable]
public class JsonList<T>{
    public List<T> result;
}
[Serializable]
public class ColorInfo
{
    public string hex_code, cname;
}

[Serializable]
public class ColorJson{
    public string color_id, hex_code, cname;
}

[Serializable]
public class Message
{
    public string chat_id, name, msg;
}
enum Scene
{
    Title,
SignUp,
Test,
    Result,
    Chat,
    Update,
    GetPW
}