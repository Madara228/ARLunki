using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.UI;using Random = UnityEngine.Random;
    public class MessageBehaivour : MonoBehaviour
    {
        private static MessageBehaivour messageinstance;
        
        public static MessageBehaivour getInstance()
        {
            if (messageinstance == null)
            {
                Debug.Log("Create Obj: UIDebug");
                messageinstance = new GameObject("MessageBehaivour", typeof(MessageBehaivour), typeof(CanvasScaler)).GetComponent<MessageBehaivour>();
                messageinstance.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                messageinstance.GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            }
            return messageinstance;
        }
        [SerializeField]
        private GameObject messagePrefab;

        [SerializeField]
        private GameObject errorPrefab;
        
        private GameObject messageText;
        private GameObject errorText;
        private void Awake()
        {
            messageinstance = this;
            messageText = Instantiate(messagePrefab, transform.position, Quaternion.identity);
            messageText.transform.parent = this.gameObject.transform;
            messageText.transform.localScale = new Vector2(0, 0);
            errorText = Instantiate(errorPrefab, transform.position, Quaternion.identity);
            errorText.transform.parent = this.gameObject.transform;
            errorText.transform.localScale = new Vector2(0, 0);
            errorText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Screen.width-errorText.GetComponent<RectTransform>().sizeDelta.y);
        }

        void UseText(string line, MessageType mType)
        {
            switch (mType)
            {
                case MessageType.Notification:
                    messageText.GetComponentInChildren<Text>().text = line;
                    messageText.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutBounce).OnComplete(()=>StartCoroutine(returner(messageText)));
                    break;
                case MessageType.Error:
                    errorText.GetComponentInChildren<Text>().text = line;
                    errorText.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutBounce).OnComplete(()=>StartCoroutine(returner(errorText)));
                    break;
            }
        }

        private IEnumerator returner(GameObject obj)
        {
            yield return new WaitForSeconds(2f);
            returnBack(obj);
        }
        void returnBack(GameObject obj)
        {
            obj.transform.DOScale(Vector3.zero, 0.3f);
        }
        public static void Call(string text, MessageType type)
        {
            getInstance().UseText(text,type);
        }
    }

    public static class MessageCaller
    {
        public static void CallMessage(string text,MessageType type)
        {
            MessageBehaivour.Call(text,type );
        }
    }

public enum MessageType
{
    Notification,
    Error
}
