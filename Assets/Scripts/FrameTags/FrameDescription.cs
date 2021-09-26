using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.Coreference.Mention;
using OpenNLP.Tools.Parser;
using OpenNLP.Tools.Lang.English;
using System.IO;

public class FrameDescription : MonoBehaviour
{
    public InputField DescriptionSource;
    public static string RawFrameInput;
    private string LastSceneTags = "";
    public static Parse[] ParsedParts;

    public delegate void OnDescriptionChangeDelegate(string Input);
    public static event OnDescriptionChangeDelegate OnDescriptionChangedEvent;
    void Start()
    {
        Debug.Log("FrameDescription starts");
        if (DescriptionSource == null)
        {
            DescriptionSource = GameObject.Find("DescriptionField").GetComponent<InputField>();
            // GameObject.Find("DescriptionField").GetComponent<InputField>().text = "azaz";
            Debug.Log(string.Format("text: {0}", GameObject.Find("DescriptionField").GetComponent<InputField>().text));
        }
        // GameObject.Find("DescriptionField").GetComponent<InputField>().text = "azaz";
    }

    public void MarkAndParseTree(string text)
    {
        RawFrameInput = text;
        string itemMarkedInput = RawFrameInput.ExcludeCameraTags();
        if (!string.IsNullOrEmpty(itemMarkedInput) && !Equals(LastSceneTags, itemMarkedInput))
        {
            LastSceneTags = itemMarkedInput;
            itemMarkedInput = MarkItems(itemMarkedInput);
            ParsedParts = TreeParsing(itemMarkedInput).ToArray();
        }

        OnDescriptionChangedEvent?.Invoke(RawFrameInput);
    }
    
    public void OnInputEnter()
    {
        Debug.Log($"From FrameDescription: {DescriptionSource.text}");
        
        if (!string.IsNullOrWhiteSpace(DescriptionSource.text) && DescriptionSource.text != RawFrameInput)
        {
            MarkAndParseTree(DescriptionSource.text);
        }
    }
    private Parse[] TreeParsing(string input)
    {
        var modelPath = Directory.GetCurrentDirectory() + @"/Models/"; //TODO: replace \ to /
        var parser = new EnglishTreebankParser(modelPath);
        var treeParsing = parser.DoParse(Helper.ExcludeCameraTags(input));

        return treeParsing.GetTagNodes();
    }
    private string MarkItems(string rawInput)
    {
        string result = rawInput;
        var altNames = Helper.DictSortByLength(AvailableObjectsController.GetAlternateNames());
        foreach(var name in altNames)
        {
            result = result.Replace(name.Key, name.Value);
        }

        return result;
    }
}
