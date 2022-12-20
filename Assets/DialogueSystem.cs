using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class DialogueSystem : MonoBehaviour
{
    public Image Potrait;
    public TMPro.TextMeshProUGUI Text;

    public Button Continue;

    public GameObject[] Options;
    public GameObject OptionDivider;

    private string text;
    private string currentText;

    private bool continuePressed;
    private int? chosenOption = null;


    public void Open()
    {

    }

    public void Close()
    {

    }

    public async Task SetText(string text, int textSpeed)
    {
        continuePressed = false;

        Continue.gameObject.SetActive(true);
        Text.text = "";

        foreach (char c in text)
        {
            Text.text += c;
            await Task.Delay(textSpeed);
        }

         while (!continuePressed)
        {
            await Task.Yield();
        }
    }

    public async Task<int> SetOption(string text, int textSpeed, string Option1 = null, string Option2 = null, string Option3 = null)
    {
        chosenOption = null;

        Continue.gameObject.SetActive(false);

        Text.text = "";
        foreach (char c in text)
        {
            Text.text += c;
            await Task.Delay(textSpeed);
        }

        await Task.Delay(500);

        OptionDivider.SetActive(true);
        Options[0].SetActive(Option1 != null);
        Options[1].SetActive(Option2 != null);
        Options[2].SetActive(Option3 != null);

        Options[0].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Option1;
        Options[1].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Option2;
        Options[2].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Option3;

        while (chosenOption == null)
        {
            await Task.Yield();
        }


        OptionDivider.SetActive(false);
        Options[0].SetActive(false);
        Options[1].SetActive(false);
        Options[2].SetActive(false);

        return chosenOption.Value;
    }

    public void OnContinueClick()
    {
        continuePressed = true;
    }

    public void OnOptionClick(int option)
    {
        chosenOption = option;
    } 
}
