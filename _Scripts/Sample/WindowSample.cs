using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoGD
{
    [System.Serializable]
    public class ItemIdContainer
    {
        [SerializeField]
        private string  name = "idfv";
        [SerializeField]
        private Text    text = null;

        public string Name => name;

        public void Set(string value)
        {
            text.text = value;
        }
    }
    public class WindowSample : MonoBehaviourBase, ISubscriber
    {
        [Header("BUTTONS")]
        [SerializeField]
        private Button                          buttonBanner = null;
        
        [SerializeField]
        private Button                          buttonInterstitial = null;
        [SerializeField]
        private Button                          buttonRewarded = null;
        [SerializeField]
        private Button                          buttonShowMediationDebugger = null;
        [SerializeField]
        private Button                          buttonSendStart = null;
        [SerializeField]
        private Button                          buttonSendFinish = null;
        [SerializeField]
        private Button                          buttonRemoveUserData = null;

        [Header("TEXTS")]
        [SerializeField]
        private List<ItemIdContainer>           texts = null;


        [Header("SAMPLE DATA")]
        [SerializeField]
        private DataInt                         soft = new DataInt("resources.soft");
        [SerializeField]
        protected DataInt                       battlesAnalytics = new DataInt("longs.battles");
        [SerializeField]
        private ReferencePriceAds               rewardedSample = null;
        [SerializeField]
        private int                             rewardedBonus = 5;

        private Dictionary<string, ItemIdContainer> items = null;
        private Dictionary<string, ItemIdContainer> Items
        {
            get
            {
                if(items == null)
                {
                    items = new Dictionary<string, ItemIdContainer>();
                    foreach(var text in texts)
                    {
                        items[text.Name] = text;
                    }                    
                }
                return items;
            }
        }

        protected Dictionary<string, object>    analyticsFixedData = new Dictionary<string, object>();
        protected float                         timeStartLevel = 0;
        private bool                            bannerEnabled = false;

#region ISubscriber
        public string Description
        {
            get => name;
            set => name = value;
        }


        public void Reaction(Message message, params object[] parameters)
        {
            switch (message)
            {
                case Message.AnalyticsIdReceived:
                    SetText(parameters.Get<string>(0), parameters.Get<string>(1));
                    break;
            }
        }
#endregion

        private void SetText(string name, string value)
        {
            ItemIdContainer item = null;
            if(Items.TryGetValue(name, out item))
            {
                item.Set(value);
            }
        }

        private void Start()
        {
            buttonBanner.onClick.AddListener(SwitchBanner);
            buttonInterstitial.onClick.AddListener(ShowInter);
            buttonRewarded.onClick.AddListener(ShowRewarded);
            buttonSendStart.onClick.AddListener(SendStart);
            buttonSendFinish.onClick.AddListener(SendFinish);
            buttonShowMediationDebugger.onClick.AddListener(ShowDebugger);
            buttonRemoveUserData.onClick.AddListener(RemoveUserData);

            buttonSendFinish.interactable = false;

            //AFTER LOADING YOUR PROFILE DATA 
            AdsManager.Reaction(Message.ProfileLocalLoaded);
            SetText("money", soft.Value.ToString());

            Analytics.AddSubscriber(this);
        }

        private void OnDestroy()
        {
            Analytics.RemoveSubscriber(this);
        }

        private void RemoveUserData()
        {
            Analytics.RemoveUserData();
        }

        /// <summary>
        /// Sample Code for Mediation Debugger
        /// </summary>
        private void ShowDebugger()
        {
            AdsManager.Reaction(Message.ShowDebugger);
        }

        /// <summary>
        /// Sample Code for Rewarded, Callback - after close rewarded ads
        /// </summary>
        private void ShowRewarded()
        {
            rewardedSample.Pay(AfterAdsShow);
        }

        /// <summary>
        /// Rewarded callback sample
        /// </summary>
        /// <param name="result"></param>
        private void AfterAdsShow(bool result)
        {
            if (result)
            {
                Debug.LogError("GRANT REWARD!!!");
                soft.Increment(rewardedBonus);
                SetText("money", soft.Value.ToString());
            }
        }

        /// <summary>
        /// Show interstitial sample
        /// </summary>
        private void ShowInter()
        {
            AdsManager.Reaction(Message.RequestADS, RequestADSType.Interstitial, "common");
        }

        /// <summary>
        /// Show/Hide banner sample
        /// </summary>
        private void SwitchBanner()
        {
            bannerEnabled = !bannerEnabled;
            AdsManager.Reaction(Message.RequestADS, RequestADSType.Banner, "banner", bannerEnabled);
            SetText("banner", "BANNER (" + (bannerEnabled ? "ON" : "OFF") + ")");
        }

        /// <summary>
        /// Send level_start event sample
        /// </summary>
        private void SendStart()
        {
            var data = GetParametersForSendStart("level_winter");
            Analytics.SendEvent("level_start", data);
            Analytics.SendBuffer();

            buttonSendFinish.interactable = true;
            buttonSendStart.interactable = false;            
        }


        /// <summary>
        /// Send level_finish event sample
        /// </summary>
        private void SendFinish()
        {
            var data = GetParametersForSendFinish();
            Analytics.SendEvent("level_finish", data);
            Analytics.SendBuffer();

            buttonSendFinish.interactable = false;
            buttonSendStart.interactable = true;
        }

        /// <summary>
        /// Sample parameters for level_start
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Dictionary<string, object> GetParametersForSendStart(params object[] parameters)
        {
            battlesAnalytics.Increment(1);
            analyticsFixedData.Clear();
            analyticsFixedData["level_number"] = 1;//number of level
            analyticsFixedData["level_name"] = parameters.Get<string>();//name of level
            analyticsFixedData["level_loop"] = 1;//number of loop if nesessary

            analyticsFixedData["level_count"] = battlesAnalytics.Value;//number of game
            analyticsFixedData["level_diff"] = "normal";//difficult
            analyticsFixedData["level_random"] = 0;//random or not?
            analyticsFixedData["level_type"] = "normal";//type of level
            analyticsFixedData["game_mode"] = "classic";//game mode
            timeStartLevel = Time.time;//start time
            return analyticsFixedData;
        }

        /// <summary>
        /// Sample parameters for level_finish
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Dictionary<string, object> GetParametersForSendFinish(params object[] parameters)
        {
            var result = parameters.Get<string>();
            var analyticsData = new Dictionary<string, object>();

            foreach (var data in analyticsFixedData)
            {
                analyticsData[data.Key] = data.Value;
            }

            var progress = Random.Range(0, 101);

            analyticsData["progress"] = progress;//level progress
            analyticsData["result"] = progress == 100? "win" : "lose";//result - win, lose, leave
            analyticsData["time"] = (long)(Time.time - timeStartLevel);//duration game
            analyticsData["continue"] = 1;//ads continues

            return analyticsData;
        }

    }
}