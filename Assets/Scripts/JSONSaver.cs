using System.IO;
using UnityEngine;

public class JSONSaver
{
    private const string JSON_FILENAME = "Save.json";

    public void SaveToJson(PixelImageList pixList)
    {
        string json = JsonUtility.ToJson(pixList);
        using var writer = new StreamWriter(Application.persistentDataPath + JSON_FILENAME);
        writer.WriteLine(json);
    }

    public PixelImageList LoadFromJSON()
    {
        string json = "";
        using (var reader = new StreamReader(Application.persistentDataPath + JSON_FILENAME))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
                json += line;
        }
        return JsonUtility.FromJson<PixelImageList>(json);
    }
}
