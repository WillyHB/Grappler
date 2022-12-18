using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class DialogueSystem : MonoBehaviour
{
    public GameObject LeftCharacter;
    public GameObject RightCharacter;

    public List<Button> Options;
    public Image OptionDivider;

    public async void Start()
    {
        await SetText(true, "I CAN'T FUCKING DO THIS SHIT ANYMORE", 100);

        await Task.Delay(5000);

        await SetOption("DON'T KILL YOURSELF MAN PLEASE!", 100, "You can't stop me", "I want to!", "Fine I won't");

    }

    public async Task SetText(bool isPlayer, string text, int textSpeed)
    {
        RightCharacter.SetActive(false);
        LeftCharacter.SetActive(false);

        if (isPlayer)
        {
            RightCharacter.SetActive(true);
            RightCharacter.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
            foreach (char c in text)
            {
                RightCharacter.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += c;
                await Task.Delay(textSpeed);
            }
        }

        else
        {
            LeftCharacter.SetActive(true);

            LeftCharacter.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
            foreach (char c in text)
            {
                LeftCharacter.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += c;
                await Task.Delay(textSpeed);
            }
        }
    }

    public async Task SetOption(string text, int textSpeed, string Option1, string Option2, string Option3)
    {
        RightCharacter.SetActive(false);

        LeftCharacter.SetActive(true);

        LeftCharacter.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        foreach (char c in text)
        {
            LeftCharacter.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += c;
            await Task.Delay(textSpeed);
        }

        await Task.Delay(500);
        Options.ForEach(b => b.gameObject.SetActive(true));

        Options[0].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Option1;
        Options[1].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Option2;
        Options[2].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Option3;
    }
}
