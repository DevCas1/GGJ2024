using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class ResultsHandler : MonoBehaviour
{
    public TextMeshProUGUI resultsText;
    public TextMeshProUGUI pointsFor;
    public TextMeshProUGUI goesTo;
    public TextMeshProUGUI challengeText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI bonuspointsText;
    public TextMeshProUGUI finalsequenceText;
    public TextMeshProUGUI finalwinningsText;
    public TextMeshProUGUI finalgoesTo;
    public TextMeshProUGUI firstPlace;
    public TextMeshProUGUI secondPlace;
    public TextMeshProUGUI thirdPlace;
    public TextMeshProUGUI fourthPlace;

    [SerializeField] private List<string> winnings;

    [SerializeField] private List<int> playerPoints = new List<int>();
    private List<float> playerDistances = new List<float>(); 
    private List<int> playerChaosScores = new List<int>();
    private List<int> playerBounces = new List<int>();
    private int currentStep;
    private string currentChallenge;
    private int currentWinner;
    private int currentPoints;

    void Awake()
    {
        ClearAllLists();
        DeactivateAllTexts();
        currentStep = 0;
        GetAllResultValues();
        StartCoroutine("StepZero");
    }

    void DeactivateAllTexts()
    {
        pointsFor.gameObject.SetActive(false);
        goesTo.gameObject.SetActive(false);
        challengeText.gameObject.SetActive(false);
        playerText.gameObject.SetActive(false);
        bonuspointsText.gameObject.SetActive(false);
        finalsequenceText.gameObject.SetActive(false);
        finalwinningsText.gameObject.SetActive(false);
        finalgoesTo.gameObject.SetActive(false);
        firstPlace.gameObject.SetActive(false);
        secondPlace.gameObject.SetActive(false);
        thirdPlace.gameObject.SetActive(false);
        fourthPlace.gameObject.SetActive(false);

    }

    void ClearAllLists()
    {
        playerPoints.Clear();
        playerDistances.Clear();
        playerChaosScores.Clear();
        playerBounces.Clear();
    }

    IEnumerator StepZero()
    {
        yield return new WaitForSeconds(3.0f);
        resultsText.gameObject.SetActive(false);
        challengeText.gameObject.SetActive(true);
        playerText.gameObject.SetActive(true);
        bonuspointsText.gameObject.SetActive(true);
        currentChallenge = "";
        currentWinner = -1;
        challengeText.text = "";
        playerText.text = "";
        bonuspointsText.text = "";
        NextStep();
        
        yield return null;

    }

    void GetAllResultValues()
    {
        for (int i = 0; i < 4; i++)
        {
            playerPoints.Add(Random.Range(0, 10));
            playerBounces.Add(Random.Range(0, 10));
            playerDistances.Add(Random.Range(0, 10));
            playerChaosScores.Add(Random.Range(0, 10));
        }
        // Get the challenge values from the game manager and put them into the lists in order of players
        // Player 1 = 0
        // Player 2 = 1
        // Player 3 = 2
        // Player 4 = 3
    }

    void NextStep()
    {
        currentStep += 1;
        switch (currentStep)
        {
            case 1:
                currentChallenge = "least crumbs";
                currentWinner = playerPoints.IndexOf(playerPoints.Min());
                currentPoints = 5;
                playerPoints[playerPoints.IndexOf(playerPoints.Min())] += currentPoints;
                pointsFor.gameObject.SetActive(true);
                goesTo.gameObject.SetActive(true);
                StartCoroutine("AStep");
                break;
            case 2:
                currentChallenge = "longest distance";
                currentWinner = playerDistances.IndexOf(playerDistances.Max());
                currentPoints = 10;
                playerPoints[playerDistances.IndexOf(playerDistances.Max())] += currentPoints;
                pointsFor.gameObject.SetActive(true);
                goesTo.gameObject.SetActive(true);
                StartCoroutine("AStep");
                break;
            case 3:
                currentChallenge = "most bounces";
                currentWinner = playerBounces.IndexOf(playerBounces.Max());
                currentPoints = 15;
                playerPoints[playerBounces.IndexOf(playerBounces.Max())] += currentPoints;
                pointsFor.gameObject.SetActive(true);
                goesTo.gameObject.SetActive(true);
                StartCoroutine("AStep");
                break;
            case 4:
                currentChallenge = "most chaos";
                currentWinner = playerChaosScores.IndexOf(playerChaosScores.Max());
                currentPoints = 20;
                playerPoints[playerChaosScores.IndexOf(playerChaosScores.Max())] += currentPoints;
                pointsFor.gameObject.SetActive(true);
                goesTo.gameObject.SetActive(true);
                StartCoroutine("AStep");
                break;
            case 5:
                currentChallenge = "";
                currentWinner = -1;
                challengeText.text = "";
                playerText.text = "";
                bonuspointsText.text = "";
                StartCoroutine("FinalSequence");
                break;
        }

    }

    IEnumerator AStep()
    {
        yield return new WaitForSeconds(2.0f);
        challengeText.gameObject.SetActive(true);
        challengeText.text = currentChallenge;
        yield return new WaitForSeconds(0.5f);
        playerText.gameObject.SetActive(true);
        playerText.text = "Player " + (currentWinner + 1);
        yield return new WaitForSeconds(0.5f);
        bonuspointsText.gameObject.SetActive(true);
        bonuspointsText.text = "+ " + currentPoints + " P";
        yield return new WaitForSeconds(4.0f);
        DeactivateAllTexts();
        NextStep();
        yield return null;

    }

    IEnumerator FinalSequence()
    {

        yield return new WaitForSeconds(2.0f);
        finalsequenceText.gameObject.SetActive(true);
        finalsequenceText.text = "We have a winner...";
        yield return new WaitForSeconds(4.0f);
        finalsequenceText.text = "The title of ";
        yield return new WaitForSeconds(1.5f);
        finalwinningsText.text = winnings[Random.Range(0, winnings.Count)];
        finalwinningsText.gameObject.SetActive(true);
        finalgoesTo.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        fourthPlace.text = "Loser " + (playerPoints.IndexOf(playerPoints.Min()) + 1);
        finalsequenceText.gameObject.SetActive(false);
        finalgoesTo.gameObject.SetActive(false);
        finalwinningsText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        fourthPlace.gameObject.SetActive(true);
        playerPoints[playerPoints.IndexOf(playerPoints.Min())] += 999;
        thirdPlace.text = "Player " + (playerPoints.IndexOf(playerPoints.Min()) + 1);
        yield return new WaitForSeconds(1.5f);
        thirdPlace.gameObject.SetActive(true);
        playerPoints[playerPoints.IndexOf(playerPoints.Min())] += 999;
        secondPlace.text = "Player " + (playerPoints.IndexOf(playerPoints.Min()) + 1);
        yield return new WaitForSeconds(1.5f);
        secondPlace.gameObject.SetActive(true);
        playerPoints[playerPoints.IndexOf(playerPoints.Min())] += 999;
        firstPlace.text = "Player " + (playerPoints.IndexOf(playerPoints.Min()) + 1);
        yield return new WaitForSeconds(1.5f);
        firstPlace.gameObject.SetActive(true);
        playerPoints[playerPoints.IndexOf(playerPoints.Min())] += 999;
        yield return new WaitForSeconds (6f);
        SceneManager.LoadScene(0);

        yield return null;

    }


}
