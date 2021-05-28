using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lean.Touch;
using System;

namespace LiquidVolumeFX
{
    public class PoutionScript : MonoBehaviour
    {
        bool isTouch;
        public Text t;
        [SerializeField] Lunka currentLunka;
        private LiquidVolume lv;
        public List<Lunka> collider_list;
        Coroutine _fillenumerator;
        bool spilling;



        #region BeginTapAndEndTap
        /// <summary>
        //реализация работы наполнения сосуда жидкости через зажатие на кнопку
        //максимальны гавнокод, как по мне, но работает не хуже швейцарских часов баля
        /// </summary>
        public void StartFillObject()
        {
            transform.DOMove(new Vector3(currentLunka.transform.position.x, transform.position.y, currentLunka.transform.position.z), 1f);
            #region oldcode
            //currentLunka._visible.SetActive(false);
            //StartCoroutine(fill());
            #endregion
            _fillenumerator = StartCoroutine(filling());
        }
        public void StopFilling()
        {
            StopCoroutine(_fillenumerator);
        }
        private IEnumerator filling()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                lv.level += 0.005f;
            }
        }
        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Lunka>() && !collider_list.Contains(other.gameObject.GetComponent<Lunka>()))
            {
                collider_list.Add(other.gameObject.GetComponent<Lunka>());
            }
            float min_dist = Vector3.Distance(transform.position, collider_list[0].transform.position);
            foreach (var lunka in collider_list)
            {
                if (Vector3.Distance(transform.position, lunka.transform.position) <= min_dist && currentLunka != lunka && !spilling)
                {
                    if (currentLunka != null) currentLunka._visible.SetActive(false);
                    currentLunka = lunka;
                    lv = currentLunka.GetComponentInChildren<LiquidVolume>();
                    currentLunka._visible.SetActive(true);
                }
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
        private void OnTriggerStay(Collider other)
        {
            try
            {
                if (currentLunka.gameObject != null)
                    t.text = "Уровень жидкости " + currentLunka.GetComponentInChildren<LiquidVolume>().level * 100f + "%";
            }
            catch(Exception e)
            {
                Debug.Log("sorry");
            }

        }
        
        //private IEnumerator fill()
        //{
        //    spilling = true;
        //    this.GetComponent<LeanDragTranslate>().enabled = false;
        //    while (currentLunka.GetComponentInChildren<LiquidVolume>().level <0.9f)
        //    {
        //        currentLunka.GetComponentInChildren<LiquidVolume>().level += 0.01F;
        //        yield return new WaitForEndOfFrame();
        //    }
        //    yield return new WaitForSeconds(2f);
        //    Destroy(currentLunka.gameObject.GetComponent<BoxCollider>());
        //    currentLunka._visible.SetActive(false);
        //    currentLunka = null;
        //    this.GetComponent<LeanDragTranslate>().enabled = true;
        //    spilling = false;
        //}
    }
}
