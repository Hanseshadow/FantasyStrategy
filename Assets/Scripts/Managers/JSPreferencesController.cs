using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

// String only CRC
public class SafePlayerPrefs
{
    public string key;
    public List<string> properties = new List<string>();

    public void Initialize(string key, List<string> props)
    {
        this.key = key;
        this.properties.Clear();

        foreach (string property in props)
            this.properties.Add(property);
        Save();
    }

    // Generates the checksum
    private string GenerateChecksum()
    {
        string hash = "";
        foreach (string property in properties)
        {
            hash += property + ":";
            if (PlayerPrefs.HasKey(property))
                hash += PlayerPrefs.GetString(property);
        }

        //Debug.Log("hash: " + hash + " key: " + key);

        return JSSecurity.Md5Sum(hash + key);
    }

    // Saves the checksum
    public void Save()
    {
        string checksum = GenerateChecksum();
        PlayerPrefs.SetString("C" + key, checksum);
        PlayerPrefs.Save();
    }

    // Checks if there has been an edit
    public bool HasBeenEdited()
    {
        if (!PlayerPrefs.HasKey("C" + key))
            return true;

        string checksumSaved = PlayerPrefs.GetString("C" + key);
        string checksumReal = GenerateChecksum();

        Debug.Log("HasBeenEdited: " + checksumSaved.Equals(checksumReal));

        return checksumSaved.Equals(checksumReal);
    }
}

[System.Serializable]
public class Preference
{
    public enum PreferenceTypes
    {
        StringPreference,
        IntPreference,
        FloatPreference,
        LongPreference,
    }

    public string m_PreferenceName;
    public PreferenceTypes m_PreferenceType;
    public string m_PreferenceValue;

    public float GetFloatPreference(string name)
    {
        if (PlayerPrefs.HasKey(name))
        {
            return PlayerPrefs.GetFloat(name);
        }

        return float.Parse(m_PreferenceValue);
    }

    public void SetFloatPreference(string name, float val)
    {
        m_PreferenceName = name;
        m_PreferenceType = PreferenceTypes.FloatPreference;
        m_PreferenceValue = val.ToString();

        PlayerPrefs.SetFloat(name, val);
    }

    public int GetIntPreference(string name)
    {
        if (PlayerPrefs.HasKey(name))
        {
            return PlayerPrefs.GetInt(name);
        }

        return int.Parse(m_PreferenceValue);
    }

    public void SetIntPreference(string name, int val)
    {
        m_PreferenceName = name;
        m_PreferenceType = PreferenceTypes.IntPreference;
        m_PreferenceValue = val.ToString();

        PlayerPrefs.SetInt(name, val);
    }

    public string GetStringPreference(string name)
    {
        if (PlayerPrefs.HasKey(name))
        {
            return PlayerPrefs.GetString(name);
        }

        return m_PreferenceValue;
    }

    public void SetStringPreference(string name, string val)
    {
        m_PreferenceName = name;
        m_PreferenceType = PreferenceTypes.StringPreference;
        m_PreferenceValue = val;

        PlayerPrefs.SetString(name, val);
    }
}

// This class is intended for run-time viewing of preferences so that values can be verified they are working properly.
public class JSPreferencesController : MonoBehaviour
{
    // Setting this to true will load the gameobject's serialized values instead of preferences from the database.
    public bool m_ResetPreferences = false;

    public List<Preference> m_Preferences;

    private SafePlayerPrefs m_SPP;

    private static JSPreferencesController _Instance;

    public static JSPreferencesController instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<JSPreferencesController>();
            }

            return _Instance;
        }
    }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void Initialize()
    {
        InitSPP();

        foreach (Preference pref in m_Preferences)
        {
            if (PlayerPrefs.HasKey(pref.m_PreferenceName) && !m_ResetPreferences)
            {
                if (pref.m_PreferenceType == Preference.PreferenceTypes.IntPreference)
                {
                    pref.m_PreferenceValue = PlayerPrefs.GetInt(pref.m_PreferenceName).ToString();
                }
                else if (pref.m_PreferenceType == Preference.PreferenceTypes.FloatPreference)
                {
                    pref.m_PreferenceValue = PlayerPrefs.GetFloat(pref.m_PreferenceName).ToString();
                }
                else
                {
                    pref.m_PreferenceValue = PlayerPrefs.GetString(pref.m_PreferenceName);
                }
            }
            else
            {
                if (pref.m_PreferenceType == Preference.PreferenceTypes.IntPreference)
                {
                    PlayerPrefs.SetInt(pref.m_PreferenceName, int.Parse(pref.m_PreferenceValue));
                }
                else if (pref.m_PreferenceType == Preference.PreferenceTypes.FloatPreference)
                {
                    PlayerPrefs.SetFloat(pref.m_PreferenceName, float.Parse(pref.m_PreferenceValue));
                }
                else
                {
                    PlayerPrefs.SetString(pref.m_PreferenceName, pref.m_PreferenceValue);
                }
            }
        }

        JSUIHelper.BroadcastToAll("PreferencesChanged");

        m_ResetPreferences = false;
    }

    public void InitSPP()
    {
        List<string> stringPreferences = GetStringPreferenceKeys();
        m_SPP = new SafePlayerPrefs();

        m_SPP.key = "C";
        m_SPP.properties = stringPreferences;
    }

    public float GetFloatPreference(string prefName)
    {
        foreach (Preference pref in m_Preferences)
        {
            if (pref.m_PreferenceName == prefName)
            {
                return pref.GetFloatPreference(prefName);
            }
        }

        Debug.LogWarning("PreferencesController.GetFloatPreference() prefName: " + prefName + " not found.");

        return 0f;
    }

    public void SetFloatPreference(string prefName, float prefValue)
    {
        bool exists = false;

        foreach (Preference pref in m_Preferences)
        {
            if (pref.m_PreferenceName == prefName && pref.m_PreferenceType == Preference.PreferenceTypes.FloatPreference)
            {
                pref.SetFloatPreference(prefName, prefValue);
                exists = true;
            }
        }

        if (!exists)
        {
            Preference pref = new Preference();
            pref.SetFloatPreference(prefName, prefValue);
            m_Preferences.Add(pref);
        }

        JSUIHelper.BroadcastToAll("PreferencesChanged");
    }

    public int GetIntPreference(string prefName)
    {
        foreach (Preference pref in m_Preferences)
        {
            if (pref.m_PreferenceName == prefName)
            {
                return pref.GetIntPreference(prefName);
            }
        }

        Debug.LogWarning("PreferencesController.GetIntPreference() prefName: " + prefName + " not found.");

        return 0;
    }

    public void SetIntPreference(string prefName, int prefValue)
    {
        bool exists = false;

        foreach (Preference pref in m_Preferences)
        {
            if (pref.m_PreferenceName == prefName && pref.m_PreferenceType == Preference.PreferenceTypes.IntPreference)
            {
                pref.SetIntPreference(prefName, prefValue);
                exists = true;
            }
        }

        if (!exists)
        {
            Preference pref = new Preference();
            pref.SetIntPreference(prefName, prefValue);
            m_Preferences.Add(pref);
        }

        JSUIHelper.BroadcastToAll("PreferencesChanged");
    }

    public string GetStringPreference(string prefName)
    {
        if (!ValidStringPreferences())
        {
            Debug.Log("Invalid string preferences");
        }

        foreach (Preference pref in m_Preferences)
        {
            if (pref.m_PreferenceName == prefName)
            {
                return pref.GetStringPreference(prefName);
            }
        }

        Debug.LogWarning("PreferencesController.GetStringPreference() prefName: " + prefName + " not found.");

        return "";
    }

    public void SetStringPreference(string prefName, string prefValue)
    {
        bool exists = false;

        foreach (Preference pref in m_Preferences)
        {
            if (pref.m_PreferenceName == prefName && pref.m_PreferenceType == Preference.PreferenceTypes.StringPreference)
            {
                pref.SetStringPreference(prefName, prefValue);
                exists = true;
            }
        }

        if (!exists)
        {
            Preference pref = new Preference();
            pref.SetStringPreference(prefName, prefValue);
            m_Preferences.Add(pref);
        }

        m_SPP.Initialize("C", GetStringPreferenceKeys());

        JSUIHelper.BroadcastToAll("PreferencesChanged");
    }

    public List<string> GetStringPreferenceKeys()
    {
        List<string> stringPreferences = new List<string>();

        foreach (Preference pref in m_Preferences)
        {
            /*
                        if(pref.m_PreferenceName == "CC")
                        {
                            continue;
                        }
            */
            if (pref.m_PreferenceType == Preference.PreferenceTypes.StringPreference)
            {
                stringPreferences.Add(pref.m_PreferenceName);
            }
        }

        return stringPreferences;
    }

    public bool ValidStringPreferences()
    {
        if (m_SPP.HasBeenEdited())
        {
            return false;
        }

        return true;
    }
}
