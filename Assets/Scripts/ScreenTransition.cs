using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    public Image Sprite;
    public float TransitionSpeed;
    public static ScreenTransition Instance {get; private set; }
    public bool StartTransition;

    public void Start() {

        Instance = this;
        if (StartTransition) IntoLevel();
    }

    public async Task IntoLevel() {
        Sprite.rectTransform.localPosition = new Vector2(0, 0);
        LeanTween.moveLocalX(Sprite.gameObject, Sprite.rectTransform.rect.width, TransitionSpeed).setEaseInOutQuad();
        await Task.Delay((int)(TransitionSpeed * 1000));
    }

    public async Task OutOfLevel() {
        Sprite.rectTransform.localPosition = new Vector2(-Sprite.rectTransform.rect.width, 0);
        
        LeanTween.moveLocalX(Sprite.gameObject, 0, TransitionSpeed).setEaseInOutQuad();
        await Task.Delay((int)(TransitionSpeed * 1000));
    }
}
