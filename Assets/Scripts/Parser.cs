using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

public static class Parser
{
    public static string[] ParseSearch(string address, object key)
    {
        /// <summary> </summary>
        string stringKey = key.ToString();
        string[] rows = ParseRows(address);
        foreach (string row in rows)
        {
            var rowData = row.Split('\t');
            if (rowData[0] == stringKey)
                return rowData;
        }
        Debug.LogError("그거 데이터에 없어요 :" + stringKey);
        return null;
    }


    public static string[] ParseRows(string address)
    {
        string text = (Resources.Load<TextAsset>(address) as TextAsset).text;
        return text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
    }
}
