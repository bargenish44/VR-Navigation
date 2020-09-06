using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Presentation
{
    public class ContinueAudio : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }

};