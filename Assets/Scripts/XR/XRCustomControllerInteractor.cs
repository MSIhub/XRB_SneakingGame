using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

namespace XR
{
    public class XRCustomControllerInteractor : MonoBehaviour
    {
        private XRBaseControllerInteractor _controller;
        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<XRBaseControllerInteractor>();
            Assert.IsNotNull(_controller, "There is no XRBaseControllerInteractor assigned to this hand:" + gameObject.name);
            _controller.selectEntered.AddListener(ParentInteractable);
            _controller.selectExited.AddListener(UnparentInteractable);
        }
        private void ParentInteractable(SelectEnterEventArgs arg0)
        {
            arg0.interactable.transform.parent = transform;
        }
        private void UnparentInteractable(SelectExitEventArgs arg0)
        {
            arg0.interactable.transform.parent = null;
        }


    }
}
