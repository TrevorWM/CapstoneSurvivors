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
    private float displayDuration;

    [SerializeField]
    private int textPoolSize;

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
        displayDuration = Mathf.Max(1, displayDuration);
    }
    private void Start()
    {
        prefabInstances = new GameObject[textPoolSize];
        CreatePool();

        if (testSpawner) StartCoroutine(TestText());

    }
    /// <summary>
    /// Creates text objects to display the floating damage numbers.
    /// </summary>
    private void CreatePool()
    {
        for (int i = 0; i < prefabInstances.Length; i++)
        {
            GameObject textInstance = Instantiate(floatingDamagePrefab);
            textInstance.SetActive(false);
            textInstance.transform.SetParent(transform, false);
            prefabInstances[i] = textInstance;
        }
    }

    /// <summary>
    /// Spawns floating damage text with given damage amount, and using the color
    /// matching the element type.
    /// </summary>
    /// <param name="damageToDisplay"></param>
    /// <param name="damageElementColor"></param>
    public void SpawnText(float damageToDisplay, ElementType element)
    {
        GameObject currentTextInstance = prefabInstances[textIndex];
        Color textColor = GetElementColor(element);

        if (currentTextInstance != null)
        {
            TextMeshProUGUI text = currentTextInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = testDamageAmount.ToString();
                text.color = textColor;
                currentTextInstance.transform.position = RandomizeSpawnPosition();
                currentTextInstance.SetActive(true);
                textIndex = (textIndex + 1) % textPoolSize;

                StartCoroutine(DespawnText(currentTextInstance, displayDuration)); 
            }
        }
    }
    private Vector3 RandomizeSpawnPosition()
    {
        float xPos = Random.Range(-.2f, .2f);
        float yPos = Random.Range(.5f, .7f);

        Vector3 spawnPos = new Vector3(transform.position.x + xPos,
            transform.position.y + yPos, transform.position.z);
        
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

    private IEnumerator DespawnText(GameObject textInstance, float duration)
    {
        yield return new WaitForSeconds(duration);
        textInstance.SetActive(false);
    }

    private IEnumerator TestText()
    {
        for (int i = 0; i < prefabInstances.Length; i++)
        {
            SpawnText(testDamageAmount, ElementType.None);
            yield return new WaitForSeconds(.5f);
        }  
    }
}
