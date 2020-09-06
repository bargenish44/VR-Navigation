using UnityEngine;
using System.Collections;

namespace Presentation
{
    public class BackHandler : MonoBehaviour
    {
        void Awake()
        {
            Input.backButtonLeavesApp = true;
        }
    }
};