using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IntroFadeOut : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    [SerializeField] AudioSource doorSFX;
    [SerializeField] AudioSource lightSFX;
    void Start()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade() {
        doorSFX.Play();
        yield return new WaitForSeconds(4f);
        lightSFX.Play();
        GetComponent<Image>().DOFade(0, duration).SetEase(Ease.OutQuart);
        yield return null;
    }
}
