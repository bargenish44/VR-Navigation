using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Presentation
{
    public class SpheresContainer : MonoBehaviour
    {
        // Helps to find spheres that have been set to false active. 
        private Dictionary<string, GameObject> spheresByName = new Dictionary<string, GameObject>();

        public Dictionary<string, GameObject> GetSpheres() { return spheresByName; }
    }

};