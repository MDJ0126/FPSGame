using System.IO;
using System.Text;
using UnityEditor;

public static class MakeEum
{
    private static string DEST_SCRIPT_PARENT_FOLDER_PATH = "Assets/Scripts/Utils/";
    private static string[] DO_NOT_NEED_INCLUDE = { ".prefab", "Assets/Resources/" };

    [MenuItem("My Project/Make Enum/ Make Parts Enum")]
    /// <summary>
    /// 파츠 리소스 Enum 스크립트 생성
    /// </summary>
    private static void MakePartsEnum()
    {
        const string SOURCE_FOLDER_PATH = "Assets/Resources/Prefabs/Parts";
        const string ENUM_TYPE_NAME = "ePartsResources";
        StringBuilder sb = new StringBuilder();

        // 리소스 폴더 안의 모든 파일 가져오기
        string[] files = Directory.GetFiles(SOURCE_FOLDER_PATH, "*", SearchOption.AllDirectories);

        sb.AppendLine("using System.ComponentModel;").AppendLine();
        sb.AppendLine($"public enum {ENUM_TYPE_NAME}");
        sb.AppendLine("{");
        foreach (string file in files)
        {
            (string fullPath, string fileName) path = GetResourcePath(file);
            if (file.EndsWith(".prefab"))
            {
                sb.AppendLine($"\t[Description(\"{path.fullPath}\")]");
                sb.AppendLine($"\t{path.fileName},");
            }
        }
        sb.AppendLine("}");
        File.WriteAllText($"{DEST_SCRIPT_PARENT_FOLDER_PATH}{ENUM_TYPE_NAME}.cs", sb.ToString());
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 리소스 주소와 파일 이름 가져오기
    /// </summary>
    /// <param name="originalFullPath"></param>
    /// <returns></returns>
    private static (string fullPath, string fileName) GetResourcePath(string originalFullPath)
    {
        string path = originalFullPath.Replace("\\", "/");
        string fileName = Path.GetFileNameWithoutExtension(path);
        foreach (var notNeed in DO_NOT_NEED_INCLUDE)
        {
            path = path.Replace(notNeed, "");
        }
        return (path, fileName);
    }
}