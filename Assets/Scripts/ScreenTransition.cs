using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    public enum TransitionType 
    {
        ScreenWipe,
        Fade,
    }
    public Image Sprite;
    public float TransitionSpeed;
    public static ScreenTransition Instance {get; private set; }
    public bool StartTransition;
    public TransitionType Transition;

    public void OnEnable() 
    {

        Instance = this;
    }

    public void Start() 
    {
        if (StartTransition) IntoLevel();
    }

    public void OnDisable() 
    {
        Instance = null;

    }

    public async Task IntoLevel() 
    {
        Sprite.rectTransform.localPosition = new Vector2(0, 0);

        if (Transition == TransitionType.ScreenWipe)
            LeanTween.moveLocalX(Sprite.gameObject, Sprite.rectTransform.rect.width, TransitionSpeed).setEaseInOutQuad();
        else
            LeanTween.alphaCanvas(Sprite.GetComponentInParent<CanvasGroup>(), 0, TransitionSpeed).setEaseLinear();
        await Task.Delay((int)(TransitionSpeed * 1000));
    }

    public async Task OutOfLevel() 
    {
        
        if (Transition == TransitionType.ScreenWipe)
        {
            Sprite.rectTransform.localPosition = new Vector2(-Sprite.rectTransform.rect.width, 0);
            LeanTween.moveLocalX(Sprite.gameObject, 0, TransitionSpeed).setEaseInOutQuad();
        }

        else 
        {
            LeanTween.alphaCanvas(Sprite.GetComponentInParent<CanvasGroup>(), 1, TransitionSpeed).setEaseLinear();
            Sprite.rectTransform.localPosition = new Vector2(0, 0);
        }
        await Task.Delay((int)(TransitionSpeed * 1000));
    }
}
