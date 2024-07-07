using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndScreen : MonoBehaviour
{
    public UIDocument endScreen;

    public void ShowEndScreen(decimal playerBill, decimal realBill)
    {

        var endScreen = GetComponent<UIDocument>();
        var officialScore = endScreen.rootVisualElement.Q("OfficialScore") as Label;
        var yourScore = endScreen.rootVisualElement.Q("YourScore") as Label;
        var status = endScreen.rootVisualElement.Q("Status") as Label;
        var flavorText = endScreen.rootVisualElement.Q("FlavorText") as Label;

        officialScore.text = "Real Sales : " + realBill.ToString("0.00");
        yourScore.text = "Your Bill : " + playerBill.ToString("0.00");
        if ((realBill == 0.0M) && (playerBill != 0.0M)) {
            status.text = "PIPPED! You Lose!";
            flavorText.text = "How did you even lose this? The customers didn't buy anything!";
        } 
        else
        {
            var result = (playerBill / realBill) * 100;
            Debug.Log($"Customer sales were ${realBill.ToString("0.00")}");
            Debug.Log($"PlayerInput was ${playerBill.ToString("0.00")}");
            if (result >= (100 + Constants.WinThreshold)) 
            {
                status.text = "PIPPED! You Lose!";
                flavorText.text = "You've charged the customers too much money! They are rioting and our sales are falling!";
                Debug.Log($"Result is {result.ToString("0.00")}%, lost because the player charged too much money.");
            }
            else if (result <= (100 - Constants.WinThreshold)) 
            {
                status.text = "PIPPED! You Lose!";
                flavorText.text = "You've lost the company too much money! Our shareholders are furious!";
                Debug.Log($"Result is {result.ToString("0.00")}%, lost because the player lost too much money.");
            } else if (result == 100) {
                Debug.Log("Won with a Perfect Score");
                status.text = "You Win!";
                flavorText.text = "Perfect score! You're an exemplary employee! You've earned a bathroom break!";
            } else {
                status.text = "You Win!";
                flavorText.text = "Good enough, you're within the margin of error. Keep working on those numbers!";
                Debug.Log($"Result is {result.ToString("0.00")}%, won within the margin of error +/- {Constants.WinThreshold}%.");
            }
        }

    }
}
