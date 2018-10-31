using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class AssetConfigCustomImporter : AssetPostprocessor
{
    private static string _pattern;
    private static string _configName = "assetConfig.json";

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        _pattern = String.Format(@"^(.*?(\b{0}\b)[^$]*)$", _configName);
        foreach (string str in importedAssets)
        {
            ResolveConfigPath(str);
        }

        foreach (string str in movedAssets)
        {
            ResolveConfigPath(str);
        }
    }

    private static void ResolveConfigPath(string str)
    {
        if (Regex.IsMatch(str, _pattern))
        {
            TryToDownload(str, Application.dataPath + "/../" + str, TryParseLocalConfig);
        }
    }

    private static void TryToDownload(string localPath, string path, Action<WWW, string> onComplete)
    {
        IEnumerator enumerator = StartDownloading(localPath, path, onComplete);
        enumerator.MoveNext();
        while (!((WWW)(enumerator.Current)).isDone) ;
        enumerator.MoveNext();
    }

    private static IEnumerator StartDownloading(string localPath, string url, Action<WWW, string> onComplete)
    {
        using (var www = new WWW(url))
        {
            yield return www;
            onComplete(www, localPath);
        }
    }

    private static void TryParseLocalConfig(WWW www, string path)
    {
        var config = JsonUtility.FromJson<AssetConfig>(www.text);
        var splitedPath = path.Split('/');
        var sb = new StringBuilder();
        for (int i = 0; i < splitedPath.Length; i++)
        {
            if (i == 0 || i == splitedPath.Length -1)
                continue;
            sb.Append('/');
            sb.Append(splitedPath[i]);
        }
        config.AssetLocalPath = sb.ToString();
        TryToSaveChanges(config);
        Debug.Log(config.AssetLocalPath);
    }

    private static void TryToSaveChanges(AssetConfig config)
    {
        using (FileStream fs = new FileStream(Application.dataPath + config.AssetLocalPath + "/" + _configName, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(JsonUtility.ToJson(config));
            }
        }
    }
}
