using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract classes are kinda like a combination of a normal class and an interface
public abstract class Detector : MonoBehaviour
{
    public abstract void Detect(AIData aiData);
}
