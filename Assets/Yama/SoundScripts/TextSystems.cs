using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TextSystems : MonoBehaviour
{
    private static string m_writeFile = "Resources/Text/";

    //Textファイルを読み込む
    public static string[][] ReadText(string fileName)
    {
        List<List<string>> textData;        //攻撃範囲
        TextAsset text;                     //テキストアセット
        StringReader reader;                //文字列読み込みクラス
        string line;                        //読み込み文字列
        string[] values;                    //読み込み文字列の単語
        int count;                          //凡庸カウンター
        text = Resources.Load("Text/" + fileName) as TextAsset;
        reader = new StringReader(text.text);
        textData = new List<List<string>>();
        if (reader.Peek() == -1) return ChangeListToArray(textData);
        //テキストファイルデータを取得
        count = 0;
        while (reader.Peek() > -1)
        {
            line = reader.ReadLine();
            values = line.Split('\t');
            textData.Add(new List<string>());
            for (int i = 0; i < values.Length; i++)
            {
                textData[count].Add(values[i]);
            }
            count++;
        }     
        return ChangeListToArray(textData);
    }

    //ListをArrayに変換
    private static string[][] ChangeListToArray(List<List<string>> text)
    {
        string[][] changeText;
        changeText = new string[text.Count][];
        for (int i = 0; i < text.Count; i++)
        {
            changeText[i] = text[i].ToArray();
        }
        return changeText;
    }

    //Textファイルに書き込み
    public static void WriteText(string fileName, string[][] textData)
    {
        StreamWriter write;             //文字列書き込みクラス
        FileStream file;

        //file = new FileInfo(Application.dataPath + "/" + m_writeFile + fileName + ".txt");
        //write = file.AppendText();

        file = new FileStream(Application.dataPath + "/" + m_writeFile + fileName + ".txt", FileMode.Create, FileAccess.Write);
        write = new StreamWriter(file);
        for (int i = 0; i < textData.Length; i++)
        {
            for (int j = 0; j < textData[i].Length; j++)
            {
                write.Write(textData[i][j]);
                if (i == textData[i].Length - 1) continue;
                write.Write('\t');
            }
            write.WriteLine();
        }
        write.Flush();
        write.Close();
    }

}
