using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInFadeOut : MonoBehaviour
{
    public event System.Action OnFadeInDone = delegate { };
    public event System.Action OnFadeOutDone = delegate { };
    public Image mainImage;

    public float t = 1;

    public float speed = 1;

    [SerializeField]
    private bool isFading = false;

    public void FadeIn()
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadingIn());
        }
    }

    IEnumerator FadingIn()
    {
        while (true)
        {
            Color temp = mainImage.color;
            temp.a += Time.deltaTime * speed;
            mainImage.color = temp;

            if (temp.a >= 1)
            {
                
                isFading = false;
                OnFadeInDone();
                break;
            }

            yield return null;
        }
    }

    public void FadeOut()
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadingOut());
        }
    }

    IEnumerator FadingOut()
    {
        while (true)
        {
            Color temp = mainImage.color;
            temp.a -= Time.deltaTime * speed;
            mainImage.color = temp;

            if (temp.a <= 0)
            {
                isFading = false;
                OnFadeOutDone();
                break;
            }

            yield return null;
        }
    }

}
