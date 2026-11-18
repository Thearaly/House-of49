using UnityEngine;
using TMPro;

public class DiceRoller : MonoBehaviour
{
    [Header("UI (optional)")]
    public TMP_Text diceResultText;

    public int Roll()
    {
        int result = Random.Range(1, 7);
        if (diceResultText != null) diceResultText.text = "Dice: " + result;
        Debug.Log("Dice rolled: " + result);
        return result;
    }
}
