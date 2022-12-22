using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    public struct Character
    {
        public Image Potrait;
        public TMPro.TextMeshProUGUI Text;
    }

    public Character Player;
    public Character Other;

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

    private void SetActiveCharacters(bool player, bool other)
    {

        Player.Potrait.gameObject.SetActive(player);
        Player.Text.gameObject.SetActive(player);


        Other.Potrait.gameObject.SetActive(other);
        Other.Text.gameObject.SetActive(other);
    }

    public async Task SetText(string text, int textSpeed, bool isPlayer)
    {
        continuePressed = false;

        TMPro.TextMeshProUGUI txt = isPlayer ? Player.Text : Other.Text;

        SetActiveCharacters(isPlayer, !isPlayer);

        Continue.gameObject.SetActive(false);
        txt.text = "";

        foreach (char c in text)
        {
            txt.text += c;
            await Task.Delay(textSpeed);
        }

        await Task.Delay(500);
        Continue.gameObject.SetActive(true);

        while (!continuePressed)
        {
            await Task.Yield();
        }

        SetActiveCharacters(false, false);
    }

    public async Task<int> SetOption(string text, int textSpeed, string Option1 = null, string Option2 = null, string Option3 = null)
    {
        chosenOption = null;

        SetActiveCharacters(false, true);
        Continue.gameObject.SetActive(false);

        Other.Text.text = "";
        foreach (char c in text)
        {
            Other.Text.text += c;
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
        SetActiveCharacters(false, false);

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
