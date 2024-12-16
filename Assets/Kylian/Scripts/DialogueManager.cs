using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueManager
{
    private static XDocument _dialogueDoc;

    static DialogueManager()
    {
        TextAsset dialogueText = Resources.Load<TextAsset>("Dialogues");
        if (dialogueText == null)
        {
            Debug.LogError("Dialogue.xml not found in Resources!");
            return;
        }
        _dialogueDoc = XDocument.Parse(dialogueText.text);
    }

    public static string GetDialogue(string group, string dialogueName)
    {
        if (_dialogueDoc == null)
        {
            Debug.LogWarning("Dialogue document is null. Cannot get dialogue.");
            return "";
        }

        IEnumerable<XElement> dialoguePossibilities = _dialogueDoc.Descendants(group);
        foreach (XElement item in dialoguePossibilities)
        {
            XElement element = item.Element(dialogueName);
            if (element != null)
            {
                return element.Value;
            }
        }
        return "";
    }
}

