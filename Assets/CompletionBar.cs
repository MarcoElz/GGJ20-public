using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletionBar : MonoBehaviour
{
    [SerializeField] Image completionImage;

    private void Start()
    {
        SetValue(1f);
        gameObject.SetActive(false);
    }

    public void SetValue(float percentage)
    {
        completionImage.fillAmount = percentage;
    }

    public void AddValue(float percentage)
    {
        completionImage.fillAmount += percentage;
    }


}
