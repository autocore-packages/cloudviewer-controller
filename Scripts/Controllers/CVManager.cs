using System;
using UnityEngine;

namespace Assets.Scripts
{

    public class CVManager : MonoBehaviour
    {
        public static CVManager Instance;
        private GameObject selectedGo;
        public GameObject SelectedGO
        {
            get { return selectedGo; }
            set
            {
                if (value == selectedGo) return;
                selectedGo = value;
                var traffic = selectedGo.GetComponentInParent<TrafficLight>();
                if (traffic!=null)
                {
                    OnChangeTL.Invoke(traffic);
                }
            }
        }
        public Action<TrafficLight> OnChangeTL;
        public WebRequestServer webRequesetServer;
        GameObject goWebRequesetServer;
        public AddressNode addressNode;
        AddressNode goAddressNode;
        private void Awake()
        {
            Instance = this;
            if (webRequesetServer == null)
            {
                webRequesetServer = Instantiate(goWebRequesetServer).GetComponent<WebRequestServer>();
            }
            if (addressNode == null)
            {
                addressNode = Instantiate(goAddressNode).GetComponent<AddressNode>();
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            addressNode.OnAddressConfigChange += SetAddress;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawLine(ray.origin, hit.point);
                    SelectedGO = hit.collider.gameObject;
                }
            }

        }

        private void SetAddress(AddressConfig config)
        {
            webRequesetServer.postAddress = config.getTrafficAddress;
            webRequesetServer.getAddress = config.postTrafficAddress;
        }
    }
}
