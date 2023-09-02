using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeOrb : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UpgradeOrbSO upgradeOrbSO;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("OnInteract", 1f, .1f);
    }


    public void OnInteract()
    {
        Debug.Log("I was interacted with!");
        string upgradeString = upgradeOrbSO.RollUpgrade();
        Debug.LogFormat("I rolled a {0}", upgradeString);
    }

    public void ShowInteractUI()
    {
        Debug.Log("Press F to Interact");
    }
}
