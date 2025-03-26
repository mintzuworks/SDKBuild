using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mintzuworks.FakeSample
{
    public class FakeLobbyManager : MonoBehaviour
    {
        #region Panel Loading
        [Title("Panel Loading")]
        public Transform panelLoading;

        public void ShowLoading()
        {
            panelLoading.gameObject.SetActive(true);
        }

        public void HideLoading()
        {
            panelLoading.gameObject.SetActive(false);
        }
        #endregion

    }
}
