using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BlinkTitle : MonoBehaviour
{
    public float minAlpha = 0.2f;
    public float maxAlpha = 1.0f;

    private TextMeshProUGUI textMesh;
    private float currentAlpha = 1.0f;
    private bool isFadingOut = false;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText()
    {
        while (true)
        {
            if (isFadingOut)
            {
                currentAlpha = Mathf.Lerp(currentAlpha, minAlpha, Time.deltaTime * 2);
                if (currentAlpha <= minAlpha + 0.01f)
                {
                    isFadingOut = false;
                }
            }
            else
            {
                currentAlpha = Mathf.Lerp(currentAlpha, maxAlpha, Time.deltaTime * 4);
                if (currentAlpha >= maxAlpha - 0.01f)
                {
                    isFadingOut = true;
                    yield return new WaitForSeconds(4);
                }
            }

            textMesh.alpha = currentAlpha;
            yield return null;
        }
    }
}