using System;
using Extensions;
using UnityEngine;

namespace Graphics
{
    public class DisplayInterior : MonoBehaviour
    {
        public GameObject outside;
        public LayerMask playerMask;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (playerMask.HasLayer(other.gameObject.layer))
            {
                outside.SetActive(false);
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (playerMask.HasLayer(other.gameObject.layer))
            {
                outside.SetActive(true);
            }
        }
    }
}