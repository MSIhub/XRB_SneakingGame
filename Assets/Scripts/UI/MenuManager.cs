using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MenuManager : MonoBehaviour
    {
        
        [SerializeField] private List<GameObject> _windows;

        public void OpenWindow(int index)
        {
            foreach (var win in _windows)
            {
                win.SetActive(false);
            }
            _windows[index].SetActive(true);
        }

        public void CloseMenu()
        {
            gameObject.SetActive(false);
        }
    }
}
