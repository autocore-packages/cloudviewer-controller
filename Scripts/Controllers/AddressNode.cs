using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class AddressNode : MonoBehaviour
    {
        public Action<AddressConfig> OnAddressConfigChange;
        private AddressConfig config;
        public AddressConfig Config
        {
            get
            {
                return config;
            }
            set
            {
                config = value;
                OnAddressConfigChange.Invoke(value);
            }
        }
    }

}
