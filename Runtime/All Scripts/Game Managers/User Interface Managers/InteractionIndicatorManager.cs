using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionIndicatorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactComponent;
    [SerializeField] private float alphaTweenTime;

    #region Event Subscriptions

    private void OnEnable()
    {
        InteractionController.OnSelect += ObjectSelected;
        InteractionController.OnDeselect += ObjectDeselected;
    }

    private void OnDisable()
    {
        InteractionController.OnSelect -= ObjectSelected;
        InteractionController.OnDeselect -= ObjectDeselected;
    }

    #endregion

    private void ObjectSelected()
    {
        // Activate the interact text component (maybe with a tween?)
        interactComponent.enabled = true;
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, TweenTextAlpha, interactComponent.alpha, 1f, alphaTweenTime);
    }

    private void ObjectDeselected()
    {
        // Deactivate interact component
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, TweenTextAlpha, interactComponent.alpha, 0f, alphaTweenTime);
    }

    private void TweenTextAlpha(float f)
    {
        interactComponent.alpha = f;
    }
}
