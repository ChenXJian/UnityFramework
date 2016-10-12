using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class ConfigFile
{
    private Dictionary<string,Dictionary<string, string>> m_SectiontList = new Dictionary<string,Dictionary<string, string>>();

    string m_path;

    public Dictionary<string, Dictionary<string, string>> GetSectionList()
    {
        return m_SectiontList;
    }

    public bool Load(string path)
    {
        m_path = path;

        m_SectiontList.Clear();

        if (File.Exists(path) == false)
            return false;

        try
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            bool readBool = parse(sr);
            fs.Close();

            if (readBool == false)
            {
                Debug.LogError("failed to parse config: " + path);
            }

            return readBool;
        }
        catch(Exception e)
        {
            Debug.LogError(e.ToString());
            //Debug.LogError("failed to read config file: " + path);
            return false;
        }
    }

    public bool Load(byte[] buff)
    {
        MemoryStream ms = new MemoryStream(buff);
        StreamReader sr = new StreamReader(ms);
        return parse(sr);
    }

    public bool Load(Stream ms)
    {
        StreamReader sr = new StreamReader(ms);
        return parse(sr);
    }

    public bool Save()
    {
        if (String.IsNullOrEmpty(m_path) == false)
            return Write(m_path);
        else
            return false;
    }

    bool parse(StreamReader stream)
    {
        string szLine;
        string curIndex = "";//当前索引值

        char[] char_1 = new char[1] { '#' };
        char[] char_2 = new char[2] { '[', ']' };
        char[] char_3 = new char[] { '=' };


        //Dictionary<string, string> CurrentSection = new Dictionary<string, string>();

        while (!stream.EndOfStream)
        {
            szLine = stream.ReadLine();

            //字符串不能为null
            if (string.IsNullOrEmpty(szLine))
            {
                continue;
            }

            //截取#开头前面的有用字符
            string[] str = szLine.Split(char_1);
            szLine = str[0];


            //字符串不能为null
            if (string.IsNullOrEmpty(szLine))
            {
                continue;
            }

            //如果是索引[**]
            if (szLine[0] == '[')
            {
                if (!(szLine[szLine.Length - 1] == ']') || szLine.Length <= 2)
                {
                    return false;
                }
                string[] strsplit = szLine.Split(char_2);
                curIndex = strsplit[1];
                m_SectiontList.Add(curIndex.Trim(), new Dictionary<string, string>());
            }
            else
            {
                if (string.IsNullOrEmpty(curIndex))
                {
                    return false;
                }

                string[] strsplit = szLine.Split(char_3);
                if (string.IsNullOrEmpty(strsplit[0]) || string.IsNullOrEmpty(strsplit[0]) || strsplit.Length > 2)
                {
                    return false;
                }
                m_SectiontList[curIndex].Add(strsplit[0].ToString().Trim(), strsplit[1].ToString().Trim());
            }
        }
        return true;
    }

#region  读取
    string FindValue(string section, string key, string default_value)
    {
        if (m_SectiontList.ContainsKey(section))
        {
            if (m_SectiontList[section].ContainsKey(key))
            {
                return m_SectiontList[section][key];
            }
        }
        return default_value;
        
    }
    
    public int GetInt(string section, string key, int default_value=0)
    {
        string midStr = FindValue(section,key,"");
        return midStr == "" ? default_value : int.Parse(midStr);  
    }
	
    public uint  GetUInt32(string section,string key, uint default_value=0)
    {
        string midStr = FindValue(section, key, "");
        return midStr == "" ? default_value : uint.Parse(midStr);
    }
	
    public bool  GetBool(string section, string key, bool default_value=false)
    {
        string midStr = FindValue(section, key, "");

        if (midStr == "1")
            return true;

        if (midStr == "true")
            return true;

        //return false;

        return midStr == "" ? default_value : bool.Parse(midStr);
    }
	
    public float   GetFloat(string section,string key, float default_value=0.0f)
    {
        string midStr = FindValue(section, key, "");
        return midStr == "" ? default_value : float.Parse(midStr);
    }
    
    public string GetString(string section, string key, string default_value="")
    {
        return FindValue(section, key, default_value);
    }
#endregion

    public bool Write(string path)
    {
        try
        {
            string dir = Path.GetDirectoryName(path);
            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }

            FileStream fstream = new FileStream(path, FileMode.Create);
            return Write(fstream);
        }
        catch (Exception e)
        {
            //Debug.LogError(e.ToString());
            //Debug.LogError("failed to write config file: " + path);
            Debug.LogError(e.ToString());
            return false;
        }
    }

    public bool Write(Stream stream)
    {
        try
        {
            StreamWriter sw = new StreamWriter(stream);
            foreach (KeyValuePair<string, Dictionary<string, string>> temp in m_SectiontList)
            {
                sw.WriteLine("[{0}]", temp.Key);
                foreach (KeyValuePair<string, string> kvp in m_SectiontList[temp.Key])
                {
                    sw.WriteLine("{0} = {1}", kvp.Key, kvp.Value);
                }
            }
            sw.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }
    }

    #region 写（步骤：SetValue -> Write）
    void SetValue(string section, string key, string define_value)
    {
        if (m_SectiontList.ContainsKey(section))
        {
            if (m_SectiontList[section].ContainsKey(key))
            {
                m_SectiontList[section][key] = define_value;
            }
            else
            {
                m_SectiontList[section].Add(key, define_value);
            }
        }
        else
        {
            m_SectiontList.Add(section, new Dictionary<string, string>());
            m_SectiontList[section].Add(key, define_value);
        }
    }

    public void SetInt(string section, string key, int define_value)
    {
        SetValue(section, key, define_value.ToString());
    }

    public void SetUInt32(string section, string key, uint define_value)
    {
        SetValue(section, key, define_value.ToString());
    }

    public void SetBool(string section, string key, bool define_value)
    {
        SetValue(section, key, define_value ? "true" : "false");
    }

    public void SetFloat(string section, string key, float define_value)
    {
        SetValue(section, key, define_value.ToString());
    }

    public void SetString(string section, string key, string define_value)
    {
        SetValue(section, key, define_value);
    }
    #endregion 
}

