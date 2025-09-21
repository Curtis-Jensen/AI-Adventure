using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RockPaperScissors : MonoBehaviour
{
    public TMP_Text resultText;
    public Button rockButton;
    public Button paperButton;
    public Button scissorsButton;
    public Slider playerHealthSlider;
    public Slider computerHealthSlider;
    
    [SerializeField] private int startingHealth = 2;  // Set this in inspector (2 for best of 3)
    private int playerHealth;
    private int computerHealth;
    private string[] choices = { "Rock (o)", "Paper [-]", "Scissors >8" };
    
    void Start()
    {
        ResetGame();
    }
    
    void ResetGame()
    {
        rockButton.onClick.AddListener(() => PlayGame(0));     // 0 = Rock
        paperButton.onClick.AddListener(() => PlayGame(1));    // 1 = Paper
        scissorsButton.onClick.AddListener(() => PlayGame(2)); // 2 = Scissors

        playerHealth = startingHealth;
        computerHealth = startingHealth;
        UpdateHealthUI();
        resultText.text = "Choose your weapon!";
    }
    
    void UpdateHealthUI()
    {
        playerHealthSlider.value = playerHealth;
        computerHealthSlider.value = computerHealth;
    }

    void PlayGame(int playerChoice)
    {
        // Get computer's choice (random)
        int computerChoice = Random.Range(0, 3);
        
        // Determine the winner
        string result = DetermineWinner(playerChoice, computerChoice);
        
        // Update health based on result
        if (result.Contains("You win!"))
        {
            computerHealth--;
        }
        else if (result.Contains("Computer wins!"))
        {
            playerHealth--;
        }
        
        UpdateHealthUI();
        
        // Check for game over
        if (playerHealth <= 0 || computerHealth <= 0)
        {
            string gameOverResult = playerHealth <= 0 ? "Computer Wins the Match!" : "You Win the Match!";
            string gameOverEmoticon = playerHealth <= 0 ? ">:D" : ":D";
            resultText.text = $"You chose {choices[playerChoice]}\n" +
                            $"Computer chose {choices[computerChoice]}\n" +
                            $"{gameOverResult} {gameOverEmoticon}\n\n" +
                            "Press any button to play again!";
            
            // Add listeners for reset
            rockButton.onClick.RemoveAllListeners();
            paperButton.onClick.RemoveAllListeners();
            scissorsButton.onClick.RemoveAllListeners();
            
            rockButton.onClick.AddListener(ResetGame);
            paperButton.onClick.AddListener(ResetGame);
            scissorsButton.onClick.AddListener(ResetGame);
            return;
        }
        
        // Display the round result
        string resultEmoticon = result.Contains("win!") ? ":D" : 
                               result.Contains("tie") ? ":|" : ":(";
        
        resultText.text = $"You chose {choices[playerChoice]}\n" +
                         $"Computer chose {choices[computerChoice]}\n" +
                         $"{result} {resultEmoticon}";
    }

    string DetermineWinner(int playerChoice, int computerChoice)
    {
        // If choices are the same, it's a tie
        if (playerChoice == computerChoice)
            return "It's a tie!";
            
        // Check winning conditions
        // Rock beats Scissors (0 beats 2)
        // Paper beats Rock (1 beats 0)
        // Scissors beats Paper (2 beats 1)
        bool playerWins = 
            (playerChoice == 0 && computerChoice == 2) ||
            (playerChoice == 1 && computerChoice == 0) ||
            (playerChoice == 2 && computerChoice == 1);
            
        return playerWins ? "You win!" : "Computer wins!";
    }
}
