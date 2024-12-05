using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Collections.Generic;
using System;

public static class DialogueManager 
{
    private static XDocument _dialogueDoc = XDocument.Load("Assets/Kylian/Dialogue.xml"); // to do change path 
    

    public static string GetDialogue(string group, string dialogueName)
    {
        IEnumerable<XElement> dialoguePossibilities = _dialogueDoc.Descendants(group);
        foreach (var item in dialoguePossibilities)
        {
            return item.Element(dialogueName).Value;
        }
        return "";
    }

    
}
