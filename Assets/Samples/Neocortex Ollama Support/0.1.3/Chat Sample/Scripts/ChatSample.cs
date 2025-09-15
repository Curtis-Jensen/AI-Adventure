using UnityEngine;
using Neocortex.Data;
using System.Text.RegularExpressions;

public class AIManager : MonoBehaviour
{
    [SerializeField] NeocortexChatPanel chatPanel;
    [SerializeField] NeocortexTextChatInput chatInput;
    [SerializeField] OllamaModelDropdown modelDropdown;
    [SerializeField, TextArea] string systemPrompt;

    OllamaRequest request;
    readonly Regex actionPattern = new(@"\{(.*?)\}", RegexOptions.Compiled);

    void Start()
    {
        request = new OllamaRequest();
        request.OnChatResponseReceived += OnChatResponseReceived;
        request.ModelName = modelDropdown.options[0].text;
        chatInput.OnSendButtonClicked.AddListener(OnUserMessageSent);
        modelDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        request.AddSystemMessage(systemPrompt);
    }

    private void OnDropdownValueChanged(int index)
    {
        request.ModelName = modelDropdown.options[index].text;
    }

    private void OnChatResponseReceived(ChatResponse response)
    {
        string message = response.message;
        
        GetActions(message);

        chatPanel.AddMessage(message, false);
    }

    void GetActions(string message)
    {
        // Check for actions in curly brackets
        var matches = actionPattern.Matches(message);
        foreach (Match match in matches)
        {
            string action = match.Groups[1].Value.Trim().ToLower();
            ExecuteAction(action);
        }
    }

    private void ExecuteAction(string action)
    {
        switch (action)
        {
            case "pull_lever":
                PullLever();
                break;
            default:
                Debug.LogError($"Unknown action: {action}");
                break;
        }
    }

    private void PullLever()
    {
        Debug.Log("AI pulled the lever!");
    }

    private void OnUserMessageSent(string message)
    {
        request.Send(message);
        chatPanel.AddMessage(message, true);
    }
}