using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    }
}
