using System.IO;
using UnityEngine;
using UnityEditor;
public class MyTools : Editor
{
    [MenuItem("Tools/CreateBundle")]
    static void CreateAssetBundle()
    {
        string path = "AB";//写入文件
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        //path:打包路径，BuildAssetBundleOptions.None：打包方式， BuildTarget.StandaloneWindows64：打包的目标平台
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        Debug.Log("Created Bundle!");
    }
}