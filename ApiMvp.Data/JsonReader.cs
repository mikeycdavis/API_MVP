using System.IO;

namespace ApiMvp.Data
{
    public static class JsonReader
    {
        public static string ReadJsonFile(string filePath)
        {
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string json = streamReader.ReadToEnd();
                return json;
            }
        }

        public static void WriteJsonFile(string filePath, string jsonToWrite)
        {
            File.WriteAllText(filePath, jsonToWrite);
        }
    }
}
