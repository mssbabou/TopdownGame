using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    public PlayerShoot PS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PS = FindFirstObjectByType<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PS == null) return;


    }
}
