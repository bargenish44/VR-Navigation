using UnityEngine;
using System.Collections;

public class BackHandler : MonoBehaviour
{
    void Awake()
    {
        Input.backButtonLeavesApp = true;
    }
}