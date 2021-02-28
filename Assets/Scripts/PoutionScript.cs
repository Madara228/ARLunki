using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lean.Touch;
namespace LiquidVolumeFX
{
    public class PoutionScript : MonoBehaviour
    {
        bool isTouch;
        public Text t;
        [SerializeField] Lunka currentLunka;
        public List<Lunka> collider_list;
        bool spilling;
        private void LateUpdate()
        {
            float min_dist = Vector3.Distance(transform.position, collider_list[0].transform.position);
            foreach(var lunka in collider_list)
            {
                if (Vector3.Distance(transform.position, lunka.transform.position)<min_dist && currentLunka !=lunka && !spilling)
                {
                    if (currentLunka != null) currentLunka._visible.SetActive(false);
                    currentLunka = lunka;
                    currentLunka._visible.SetActive(true);
                }
            }            
        }
        public void FillObject()
        {
            transform.DOMove(new Vector3(currentLunka.transform.position.x, transform.position.y, currentLunka.transform.position.z), 1f);
            currentLunka._visible.SetActive(false);
            StartCoroutine(fill());
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Lunka>() && !collider_list.Contains(other.gameObject.GetComponent<Lunka>()))
            {
                collider_list.Add(other.gameObject.GetComponent<Lunka>());
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<Lunka>())
            {
                if (collider_list.Contains(currentLunka) && other.gameObject.GetComponent<Lunka>() ==currentLunka)
                {
                    currentLunka._visible.SetActive(false);
                    currentLunka = null;
                }
                collider_list.Remove(other.gameObject.GetComponent<Lunka>());
                
            }
        }
        private IEnumerator fill()
        {
            spilling = true;
            this.GetComponent<LeanDragTranslate>().enabled = false;
            while (currentLunka.GetComponentInChildren<LiquidVolume>().level <0.9f)
            {
                currentLunka.GetComponentInChildren<LiquidVolume>().level += 0.01F;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(2f);
            Destroy(currentLunka.gameObject.GetComponent<BoxCollider>());
            currentLunka._visible.SetActive(false);
            currentLunka = null;
            this.GetComponent<LeanDragTranslate>().enabled = true;
            spilling = false;
        }
    }
}
