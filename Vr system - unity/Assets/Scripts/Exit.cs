using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Presentation;
namespace Presentation
{
    public class Exit : MonoBehaviour
    {
        private void OnMouseDown()
        {
            Stats.CreateCsvFile();
        }
    }
};