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
        [SerializeField] private List<Lunka> filled = new List<Lunka>();
        private void Start()
        {
            MessageBehaivour.Call("Необходимо заполнить 10 лунок на 70-80%", MessageType.Notification);
        }

        
        #region BeginTapAndEndTap
        /// <summary>
        //реализация работы наполнения сосуда жидкости через зажатие на кнопку
        //максимальны гавнокод, как по мне, но работает не хуже швейцарских часов баля
        /// </summary>
        public void StartFillObject()
        {
            if (currentLunka != null)
            {
                transform.DOMove(new Vector3(currentLunka.transform.position.x, transform.position.y, currentLunka.transform.position.z), 1f);
                #region oldcode
                //currentLunka._visible.SetActive(false);
                //StartCoroutine(fill());
                #endregion
                _fillenumerator = StartCoroutine(filling());
            }
        }
        public void StopFilling()
        {
            StopCoroutine(_fillenumerator);
            if (!(lv.level > 0.7f && lv.level < 0.8f))
            {
                MessageBehaivour.Call("Ошибка", MessageType.Error);
                if(!filled.Contains(currentLunka))
                    filled.Add(currentLunka);
                if (filled.Count >= 10)
                {
                    foreach (var fil in filled)
                    {
                        if(!(fil.GetComponent<LiquidVolume>().level>0.7f && fil.GetComponent<LiquidVolume>().level<0.8f))
                            MessageBehaivour.Call("Задание выполнено не полностью!", MessageType.Notification);
                        break;
                    }
                    MessageBehaivour.Call("Задание выполнено!", MessageType.Notification);
                }
            }
        }
        private IEnumerator filling()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                lv.level += 0.008f;
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
                    t.text = "Уровень жидкости " + (int)Mathf.Round(currentLunka.GetComponentInChildren<LiquidVolume>().level * 100f) + "%";
            }
            catch(Exception e)
            {
                Debug.Log("sorry");
            }

        }
       
    }
}
