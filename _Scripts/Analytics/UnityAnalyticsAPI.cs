using System.Collections.Generic;

namespace BoGD
{
    public class UnityAnalyticsAPI : AnalyticsBase
    {
        public override StaticType StaticType => StaticType.AnalyticsUnity;

        public override void SendEvent(string eventName, Dictionary<string, object> data)
        {
            if (!Active)
            {
                return;
            }

            data["eventName"] = eventName;
            //Debug.LogError("Event: " + data.GetString());
            UnityEngine.Analytics.Analytics.CustomEvent(eventName, data);
        }

        public override void SendPurchase(IInAppItem item)
        {
            if (!Active)
            {
                return;
            }

            UnityEngine.Analytics.Analytics.Transaction(item.ID, item.LocalizedPrice, item.ISO);
        }

        public override void RemoveUserData()
        {
            base.RemoveUserData();

            UnityEngine.Analytics.Analytics.deviceStatsEnabled = false;
        }
    }
}