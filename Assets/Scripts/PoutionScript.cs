using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LiquidVolumeFX
{
    public class PoutionScript : MonoBehaviour
    {
        bool isTouch;
        public Text t;
        Lunka currentLunka;
        private void LateUpdate()
        {
            UIDebug.Log(transform.position);
            if (Input.touchCount>0)
            {
                isTouch = true;
            }
            else
            {
                isTouch = false;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Lunka>())
            {
                StartCoroutine(check(other));
            }
        }
        private IEnumerator check(Collider other)
        {
            yield return new WaitForSeconds(0.1f);
            if (!isTouch)
            {
                currentLunka = other.gameObject.GetComponent<Lunka>();
                StartCoroutine(fill());
            }
        }
        private IEnumerator fill()
        {
            Debug.Log("fill");  
            yield return new WaitForSeconds(0.1f);
            while (currentLunka.GetComponentInChildren<LiquidVolume>().level <0.9f)
            {
                currentLunka.GetComponentInChildren<LiquidVolume>().level += 0.01F;
                yield return new WaitForEndOfFrame();
            }   
        }
    }
}
