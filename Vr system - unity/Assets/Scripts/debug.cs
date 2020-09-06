using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Presentation
{
    public class debug : MonoBehaviour
    {
        public Toggle toggle;
        public bool DebugOn = false;

        public void BoxClicked()
        {
            toggle.isOn = !toggle.isOn;
            DebugOn = toggle.isOn;
        }
        private void Start()
        {
            toggle.isOn = false;
        }
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

};