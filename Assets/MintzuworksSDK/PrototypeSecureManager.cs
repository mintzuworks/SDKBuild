using Mintzuworks.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mintzuworks
{
    [DisallowMultipleComponent]
    public class PrototypeSecureManager : MonoBehaviour
    {
        public PrototypeSecure secured_x_auth;
        public PrototypeSecure secured_x_permission;
        private void Start()
        {
            PrototypeHttp.x_permission_key = secured_x_permission.data;
            PrototypeHttp.x_auth_key = secured_x_auth.data;
        }
    }
}