using System;
using System.Collections.Generic;

[Serializable]
public class InfoJson{
    public string user_id,name,gender,hex_code, color_id,msg,description;
    public int year;
}
[Serializable]
public class Json<T>{
    public T result;
}
[Serializable]
public class JsonList<T>{
    public List<T> result;
}

[Serializable]
public class ColorJson{
    public string color_id, hex_code, description;
}