using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueManager
{
    private static XDocument _dialogueDoc;
    private static bool _isLoaded = false;

    private static void EnsureLoaded()
    {
        if (_isLoaded) return;

        TextAsset dialogueText = Resources.Load<TextAsset>("Dialogues");
        if (dialogueText == null)
        {
            Debug.LogError("Dialogue file not found in Resources! Make sure a TextAsset named 'Dialogues' is in Assets/Resources/");
            return;
        }
        _dialogueDoc = XDocument.Parse(dialogueText.text);
        _isLoaded = true;
    }

    public static string GetDialogue(string group, string dialogueName)
    {
        EnsureLoaded();
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
        Debug.LogWarning($"Dialogue not found for group: {group}, name: {dialogueName}");
        return "";
    }
}