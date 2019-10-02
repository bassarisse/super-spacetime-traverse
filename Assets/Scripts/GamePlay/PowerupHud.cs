using UnityEngine;
using System.Collections;

public class PowerupHud : MonoBehaviour
{
    void Awake()
    {
        OnExplode();
    }

    public void OnExplode()
    {
        this.gameObject.SetActive(false);
    }

    public void OnMaxEnergy()
    {
        this.gameObject.SetActive(true);
    }

}
