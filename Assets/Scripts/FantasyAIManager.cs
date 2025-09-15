using UnityEngine;
using Neocortex.Data;
using System.Text.RegularExpressions;

namespace Neocortex.Samples
{
    public class FantasyAIManager : MonoBehaviour
    {
        [SerializeField] NeocortexChatPanel chatPanel;
        [SerializeField] NeocortexTextChatInput chatInput;
        [SerializeField] OllamaModelDropdown modelDropdown;
        [SerializeField, TextArea(5, 999)] string systemPrompt;

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
            Debug.LogWarning($"Executing action: {action}");

            if (action.Contains("set_background"))
            {
                SetBackground(action);
            }
            switch (action)
            {
                default:
                    Debug.LogError($"Unknown action: {action}");
                    break;
            }
        }

        void SetBackground(string action)
        {
            string[] parts = action.Split(':');
            string backgroundName = parts[1].Trim();

            //Need to actually change the image here
        }

        private void OnUserMessageSent(string message)
        {
            request.Send(message);
            chatPanel.AddMessage(message, true);
        }
    }
}