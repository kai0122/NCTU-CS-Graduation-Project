using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


class PlayerPrefsHelper
{

    public static string ObjectToStr<T>(T _saveMe)
    {
     
        BinaryFormatter _bin = new BinaryFormatter();
        MemoryStream _mem = new MemoryStream();
        _bin.Serialize(_mem, _saveMe);

        return Convert.ToBase64String(
            _mem.GetBuffer()
        );
    }

    public static T StrToObject<T>(string _data) where T : class
    {
        if (!String.IsNullOrEmpty(_data))
        {
            BinaryFormatter _bin = new BinaryFormatter();
            try
            {
                MemoryStream _mem = new MemoryStream(Convert.FromBase64String(_data));

                T _obj = _bin.Deserialize(_mem) as T;

                return _obj;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                return null;
            }

        }
        else
        {
            return null;
        }

    }

}

