using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementInfoSO", menuName = "ScriptableObjects/Element Info/ElementInfoSO", order = 0)]
public class ElementInfoSO : ScriptableObject
{
    [Header("Fire")]
    
    [SerializeField]
    private Color fireColor;

    [Header("Nature")]

    [SerializeField]
    private Color natureColor;

    [Header("Water")]

    [SerializeField]
    private Color waterColor;

    [Header("No Element")]

    [SerializeField]
    private Color noneColor;

    public Color FireColor { get => fireColor; }
    public Color NatureColor { get => natureColor; }
    public Color WaterColor { get => waterColor; }
    public Color NoneColor { get => noneColor; }
}
