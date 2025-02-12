using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IntroFadeOut : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    void Start()
    {
        GetComponent<Image>().DOFade(0, duration);
    }
}
