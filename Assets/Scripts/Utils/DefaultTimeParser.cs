using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using UnityEngine;

public class DefaultTimeParser : IJsonTimeParser
{
    private string[] apiParseKeys = new string[]
    {
        "datetime",
        "dateTime",
        "Datetime",
        "DateTime",
        "currentLocalTime"
    };
    public bool TryGetParsedTime(string json, out DateTime parsedTime)
    {
        parsedTime = DateTime.MinValue;
        var key = string.Empty;
        for (int i = 0; i < apiParseKeys.Length; i++)
        {
            if (string.IsNullOrEmpty(key))
            {
                var temp_key = apiParseKeys[i];
                if (json.Contains(temp_key))
                {
                    key = temp_key;
                    break;
                }
            }
        }
        if (!string.IsNullOrEmpty(key))
        {
            var jsonObject = JObject.Parse(json);
            if (jsonObject.TryGetValue(key, out JToken token))
                return DateTime.TryParse(token.ToString(), null, DateTimeStyles.RoundtripKind, out parsedTime);
            else
            {
                foreach (var kvpData in jsonObject)
                {
                    if (kvpData.Key.Contains(key) && DateTime.TryParse(kvpData.Value.ToString(), null, DateTimeStyles.RoundtripKind, out parsedTime))
                        return true;
                }
                Debug.LogError("It was not possible to convert the date and time in the service response.");
                return false;
            }
        }
        else
        {
            Debug.LogError("The date and time could not be found in the service's response.");
            return false;
        }
    }
}