using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if APPSFLYER_INT
using AppsFlyerSDK;
#endif
namespace BoGD
{
    /// <summary>
    /// Appsflyer singleton
    /// </summary>
    public class AppsFlyerAPI : AnalyticsBase
    {
        [SerializeField]
        private string              devKey = "r9vNC83N8nYpCzYGigyjUh";
        [SerializeField]            
        private string              iOSAppId = "";
        [SerializeField]            
        private bool                isDebug = false;
        [SerializeField]            
        private bool                autoTrackSubscriptions = false;

        private string              autoTrackingSubscriptionsPluginName = "ural.games.afsubscriptions.AutoRevenue";
        private AndroidJavaClass    autoTrackingSubscriptionsClass = null;
        private AndroidJavaObject   autoTrackingSubscriptionsInstance = null;

        private AndroidJavaClass AutoTrackingSubscriptionsClass
        {
            get
            {
                if(autoTrackingSubscriptionsClass == null)
                {
                    autoTrackingSubscriptionsClass = new AndroidJavaClass(autoTrackingSubscriptionsPluginName);
                }
                return autoTrackingSubscriptionsClass;
            }
        }

        private AndroidJavaObject AutoTrackingSubscriptionsInstance
        {
            get
            {
                if(autoTrackingSubscriptionsInstance == null)
                {
                    autoTrackingSubscriptionsInstance = AutoTrackingSubscriptionsClass.CallStatic<AndroidJavaObject>("getInstance");
                }
                return autoTrackingSubscriptionsInstance;
            }
        }

        public override StaticType StaticType
        {
            get
            {
                return StaticType.AnalyticsAppsFlyer;
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            StartCoroutine(Initialization());
        }

        private IEnumerator Initialization()
        {
#if APPSFLYER_INT
            AppsFlyer.setIsDebug(isDebug);
            //Debug.LogError("APPSFLYER START INIT");
#endif
            yield return new WaitForSeconds(0.5f);
#if APPSFLYER_INT
            AppsFlyer.initSDK(devKey, iOSAppId);
#endif

            //Debug.LogError("APPSFLYER END INIT");
#if UNITY_IOS && !UNITY_EDITOR
            AppsFlyeriOS.waitForATTUserAuthorizationWithTimeoutInterval(30);
#endif

#if APPSFLYER_INT
            //Debug.LogError("APPSFLYER TIMEOUT SET");
            AppsFlyer.startSDK();
            //Debug.LogError("APPSFLYER START SDK");
            Inited = true;
            //AppsFlyer.getAppsFlyerId();
#endif

            if (autoTrackSubscriptions)
            {
                if(Application.platform == RuntimePlatform.Android)
                {
                    var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    var unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

                    Debug.LogError("AutoSubs: Init Start");
                    AutoTrackingSubscriptionsInstance.Call("Init", unityActivity);
                    Debug.LogError("AutoSubs: Init End");
                }
            }
        }

        public void FinishValidateReceipt(string validateResult)
        {
            Debug.Log(name + ":: got didFinishValidateReceipt  = " + validateResult);
        }

        public void FinishValidateReceiptWithError(string error)
        {
            Debug.Log(name + ":: got idFinishValidateReceiptWithError error = " + error);
        }

        public override bool Validate(IInAppItem item, System.Action<IInAppItem, bool> callback)
        {
            //AppsFlyer.validateReceipt(item.ID, item.LocalizedPrice.ToString(), item.ISO, item.TransactionID, null);            
            return true;
        }


        public override void RemoveUserData()
        {
            base.RemoveUserData();
#if APPSFLYER_INT
            AppsFlyer.stopSDK(true);
//            AppsFlyer.anonymizeUser(true);            	
//#if UNITY_IOS && !UNITY_EDITOR
//            AppsFlyeriOS.disableSKAdNetwork(true);
//#endif
#endif
        }

        public override void SendPurchase(IInAppItem item)
        {
            base.SendPurchase(item);
            if (!Inited)
            {
                return;
            }

            if (!Active)
            {
                return;
            }

#if APPSFLYER_INT
            Dictionary<string, string> data = new Dictionary<string, string> ();
            data.Add(AFInAppEvents.CURRENCY, item.ISO);
            data.Add(AFInAppEvents.REVENUE, item.LocalizedPrice.ToString().Replace(",", "."));
            data.Add(AFInAppEvents.QUANTITY, "1");
            data.Add(AFInAppEvents.CONTENT_ID, item.ID);
            AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, data);
#endif
        }

        public override void SendADS(string eventName, Dictionary<string, object> data)
        {
            if (!Inited)
            {
                return;
            }

            if (!Active)
            {
                return;
            }

#if APPSFLYER_INT         
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach(var item in data)
            {
                dictionary[item.Key] = item.Value.ToString();
            }

            AppsFlyer.sendEvent(eventName, dictionary);
#endif
        }
    }

}
