using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//completely useless class as the current data never gets sent through, delete file after tutorial is finished most likely
public class DataParser : MonoBehaviour {

    private static string KILLS_SYMBOL = "[KILLS]";
    private static string DEATHS_SYMBOL = "[DEATHS]";

    public static string valuesToData(int kills, int deaths)
    {
        return KILLS_SYMBOL + kills + "/" + DEATHS_SYMBOL + deaths;
    }

	public static int dataToKills(string data)
    {
        return int.Parse(dataToValue(data, KILLS_SYMBOL));
    }

    public static int dataToDeaths(string data)
    {
        return int.Parse(dataToValue(data, DEATHS_SYMBOL));
    }

    private static string dataToValue(string data, string symbol)
    {
        string[] values = data.Split('/');
        foreach (string value in values)
        {
            if (value.StartsWith(symbol))
            {
                return value.Substring(symbol.Length);
            }
        }
        Debug.LogError(symbol + " not found in " + data);
        return "";
    }
}
