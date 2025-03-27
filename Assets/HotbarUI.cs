using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    public PlayerShoot PS;
    public GameObject SelectorUI;
    public Image[] HotbarImages;

    void Start()
    {
        PS = FindFirstObjectByType<PlayerShoot>();
    }

    void Update()
    {
        if (PS == null) return;

        if (PS.GetCurrentGunIndex() != -1)
        {
            SelectorUI.SetActive(true);
            SelectorUI.transform.position = HotbarImages[PS.GetCurrentGunIndex()].transform.position;
        }
        else
        {
            SelectorUI.SetActive(false);
        }

        for (int i = 0; i < HotbarImages.Length; i++)
        {
            if (i < PS.guns.Count && PS.guns[i] != null)
            {
                Sprite sprite = PS.guns[i].GetSprite();
                HotbarImages[i].sprite = sprite;
                HotbarImages[i].enabled = true;
                HotbarImages[i].preserveAspect = true;

                // Set to native size first
                HotbarImages[i].SetNativeSize();

                // Calculate scaling factor so that the sprite fits inside a 100x100 box
                Vector2 nativeSize = HotbarImages[i].rectTransform.sizeDelta;
                float scaleFactor = Mathf.Min(100f / nativeSize.x, 100f / nativeSize.y, 1f); // 1f prevents scaling up
                HotbarImages[i].rectTransform.sizeDelta = nativeSize * scaleFactor;
            }
            else
            {
                HotbarImages[i].enabled = false;
            }
        }
    }
}
