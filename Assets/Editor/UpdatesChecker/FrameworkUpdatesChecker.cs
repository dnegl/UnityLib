using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;

public class FrameworkUpdatesChecker
{
    private AssetConfig _localConfig;
    private AssetConfig _remoteConfig;

    private string _configLocalPath;
    private string _configName = "assetConfig.json";

    public bool Ckeck()
    {
        TryToFindConfigPath(_configName);
        TryToDownload(_configLocalPath, TryParseLocalConfig);
        if(_localConfig == null)
        {
            Debug.LogError("AssetConfig local is emty");
            return false;
        }
        TryToDownload(_localConfig.AssetRemotePath + _configName, TryParseRemoteConfig);
        if (_remoteConfig == null)
        {
            Debug.LogError("AssetConfig remote is emty");
            return false;
        }

        return _localConfig.Version < _remoteConfig.Version ? true : false;     
    }

    public void Update()
    {
        TryToDownload(_remoteConfig.AssetRemotePath + _remoteConfig.AssetName, TryToSaveFile);
    }

    private void TryToFindConfigPath(string configName)
    {
        try
        {
            var configs = Directory.GetFiles(Application.dataPath, configName, SearchOption.AllDirectories);
            foreach (string config in configs)
            {
                _configLocalPath = config;
            }
        }
        catch (Exception e)
        {
            Debug.Log("The process failed: {0}" +  e.ToString());
        }
    }

    private void TryToDownload(string path, Action<WWW> onComplete)
    {
        IEnumerator enumerator = StartDownloading(path, onComplete);
        enumerator.MoveNext();
        while (!((WWW)(enumerator.Current)).isDone) ;
        enumerator.MoveNext();
    }

    private IEnumerator StartDownloading(string url, Action<WWW> onComplete)
    {
        using (var www = new WWW(url))
        {
            yield return www;
            onComplete(www);
        }
    }

    private void TryParseLocalConfig(WWW www)
    {
        _localConfig = JsonUtility.FromJson<AssetConfig>(www.text);
    }

    private void TryParseRemoteConfig(WWW www)
    {
        _remoteConfig = JsonUtility.FromJson<AssetConfig>(www.text);
    }

    private void TryToSaveFile(WWW www)
    {
        ByteArrayToFile(Application.dataPath + "/StreamingAssets/" + _remoteConfig.AssetName, www.bytes);
        DeletePreviousVersion();
        AssetDatabase.ImportPackage(Application.dataPath + "/StreamingAssets/" + _remoteConfig.AssetName, false);
    }

    private void DeletePreviousVersion()
    {
        FileUtil.DeleteFileOrDirectory(Application.dataPath + _localConfig.AssetLocalPath);
        Debug.Log(Application.dataPath + _localConfig.AssetLocalPath);
    }

    private bool ByteArrayToFile(string fileName, byte[] byteArray)
    {
        try
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
                return true;
            }
        }
        catch (IOException ex)
        {
            Debug.Log("Exception caught in process: " +  ex);
            return false;
        }
    }
}

public class AssetConfig
{
    public ConfigVersion Version;
    public string AssetName;
    public string AssetRemotePath;
    public string AssetLocalPath;
}

[Serializable]
public class ConfigVersion
{
    public int Major;
    public int Minor;
    public int Revision;

    public static bool operator <(ConfigVersion first, ConfigVersion second)
    {
        return first.Major < second.Major || first.Minor < second.Minor || first.Revision < second.Revision ? true : false;
    }

    public static bool operator >(ConfigVersion first, ConfigVersion second)
    {
        return first.Major > second.Major || first.Minor > second.Minor || first.Revision > second.Revision ? true : false;
    }
}
