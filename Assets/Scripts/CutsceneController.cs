using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutsceneController : MonoBehaviour
{
    public TypewriterEffect typewriter;
    public string nextSceneName = "Level1";

    [TextArea(2, 3)]
    public string[] storyPhrases;

    private int index = 0;
    private bool canContinue = false;

    void Start()
    {
        PlayCurrentPhrase();
    }

    void Update()
    {
        if (canContinue && Input.anyKeyDown)
        {
            index++;

            if (index >= storyPhrases.Length)
            {
                LoadNextScene();
            }
            else
            {
                PlayCurrentPhrase();
            }
        }
    }

    void PlayCurrentPhrase()
    {
        canContinue = false;
        typewriter.PlayText(storyPhrases[index]);
        Invoke(nameof(EnableContinue), 0.6f);
    }

    void EnableContinue()
    {
        canContinue = true;
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
