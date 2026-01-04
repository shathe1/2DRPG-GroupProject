using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public Text textUI;
    public float typingSpeed = 0.04f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip typingSound;

    private Coroutine typingRoutine;

    public void PlayText(string fullText)
    {
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeText(fullText));
    }

    IEnumerator TypeText(string fullText)
    {
        textUI.text = "";

        foreach (char c in fullText)
        {
            textUI.text += c;

            if (typingSound != null)
                audioSource.PlayOneShot(typingSound, 0.4f);

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
