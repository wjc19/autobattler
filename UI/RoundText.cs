using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoundText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] CanvasGroup uiGroup;
    [SerializeField] GameObject canvasGroup;
    [SerializeField] RectTransform rectTransform;
    LevelController levelController;

    float fadeTime = 1.5f; 
    bool fadeText = false;
    float timeElapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        StartCoroutine(FadeRoundText());       
    }

    private IEnumerator FadeRoundText()
    {
        roundText.text = "Round " + levelController.round.ToString();
        yield return new WaitForSeconds(2f);
        fadeText = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (fadeText = true)
        {
            FadeText();
        }
        if (uiGroup.alpha <= 0)
        {
            Destroy(canvasGroup);
        }
    }

    private void FadeText()
    {
            timeElapsed += Time.deltaTime;
            uiGroup.alpha = Mathf.Lerp(1, 0, timeElapsed / fadeTime);
            float xScale = Mathf.Lerp(1, .5f, timeElapsed / fadeTime);
            float yScale = Mathf.Lerp(1, .5f, timeElapsed / fadeTime);
            rectTransform.localScale = new Vector3(xScale, yScale, rectTransform.localScale.z);
    }

    public void ShowUI()
    {
        uiGroup.alpha = 1;
    }

    public void HideUI()
    {
        uiGroup.alpha = 0;
    }
}
