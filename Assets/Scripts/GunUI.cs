using TMPro;
using UnityEngine;

public class GunUI : MonoBehaviour
{
    public static GunUI instance;

    public TextMeshProUGUI gunText;
    private int gunCount = 0;

    private void Awake()
    {
        instance = this;
        UpdateText();
    }

    public void AddGun()
    {
        gunCount++;
        UpdateText();
    }

    void UpdateText()
    {
        gunText.text = "Guns: " + gunCount;
    }
    public int GetGunCount()
    {
        return gunCount;
    }
}
