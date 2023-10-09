using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingDamageSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject floatingDamagePrefab;

    [SerializeField]
    private ElementInfoSO elementInfo;

    [SerializeField]
    private int textPoolSize = 5;

    [SerializeField]
    private bool testSpawner = false;

    [SerializeField]
    private int testDamageAmount = 5;

    private int textIndex = 0;
    private GameObject[] prefabInstances;
    private string textToDisplay;



    private void OnValidate()
    {
        textPoolSize = Mathf.Max(1, textPoolSize);
    }
    private void Start()
    {
        prefabInstances = new GameObject[textPoolSize];
        CreatePool();

        if (testSpawner) StartCoroutine(TestText());

    }

    private void OnDestroy()
    {
        DestroyPool();
    }

    private void OnDisable()
    {
        DestroyPool();
    }

    /// <summary>
    /// Creates text objects to display the floating damage numbers.
    /// </summary>
    private void CreatePool()
    {
        if (prefabInstances != null)
        {
            for (int i = 0; i < prefabInstances.Length; i++)
            {
                GameObject textInstance = Instantiate(floatingDamagePrefab);
                textInstance.SetActive(false);
                prefabInstances[i] = textInstance;
            }
        }  
    }

    private void DestroyPool()
    {
        for (int i = prefabInstances.Length - 1; i > 0; i--)
        {
            if (prefabInstances[i] != null) Destroy(prefabInstances[i]);
        }
    }

    /// <summary>
    /// Spawns floating damage text with given damage amount, and using the color
    /// matching the element type.
    /// </summary>
    /// <param name="damageToDisplay"></param>
    /// <param name="damageElementColor"></param>
    public void SpawnText(float damageToDisplay, ElementType element, DamageData data, Transform spawnTransform)
    {
        GameObject currentTextInstance = prefabInstances[textIndex];
        Color textColor = GetElementColor(element);
        string damageText = Mathf.Floor(damageToDisplay).ToString();

        if (currentTextInstance != null)
        {
            TextMeshProUGUI text = currentTextInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.color = textColor;

                if (data.isStrong) damageText += "+";
                if (data.isResistant) damageText += "-";

                if (data.isCrit) damageText = damageText + "<color=\"red\">!";
                text.text = damageText;
                
                currentTextInstance.transform.position = RandomizeSpawnPosition(spawnTransform);
                currentTextInstance.SetActive(true);
                textIndex = (textIndex + 1) % textPoolSize;
            }
        }
    }
    private Vector3 RandomizeSpawnPosition(Transform baseTransform)
    {
        float xPos = Random.Range(-.2f, .2f);
        float yPos = Random.Range(.5f, .7f);

        Vector3 spawnPos = new Vector3(baseTransform.position.x + xPos,
            baseTransform.position.y + yPos, baseTransform.position.z);
        
        return spawnPos;
    }

    private Color GetElementColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Fire: return elementInfo.FireColor;
            case ElementType.Nature: return elementInfo.NatureColor;
            case ElementType.Water: return elementInfo.WaterColor;
            case ElementType.None: return elementInfo.NoneColor;
            default: return elementInfo.NoneColor;
        }
    }

    private IEnumerator TestText()
    {
        DamageData data = new DamageData();
        for (int i = 0; i < prefabInstances.Length; i++)
        {
            SpawnText(testDamageAmount, ElementType.None, data, this.transform);
            SpawnText(testDamageAmount, ElementType.None, data, this.transform);
            yield return new WaitForSeconds(.5f);
        }  
    }
}
