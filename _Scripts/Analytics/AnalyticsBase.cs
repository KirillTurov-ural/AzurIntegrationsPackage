using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoGD
{
    public class AnalyticsBase : StaticBehaviour, IAnalytics
    {
        [SerializeField]
        private DataIntPrefs activeFlag = new DataIntPrefs("int.analytics.active", 1);

        public virtual bool Inited
        {
            get;
            protected set;
        }

        public virtual bool Active
        {
            get => activeFlag.Value == 1;
            set => activeFlag.Value = value ? 1 : 0;
        } 

        public override StaticType StaticType => StaticType.Analytics;

        public virtual bool Validate(IInAppItem item, System.Action<IInAppItem, bool> callback)
        {
            return true;
        }

        public virtual void SendPurchase(IInAppItem item)
        {
        }

        public virtual void SendEvent(string eventName, Dictionary<string, object> data)
        {
        }

        public virtual void SendADS(string eventName, Dictionary<string, object> data)
        {
        }

        public virtual void SendBuffer()
        {

        }

        public virtual void RemoveUserData()
        {
            Active = false;
        }

        public virtual Dictionary<string, string> GetDataForRemove()
        {
            return null;
        }
    }

    public interface IAnalytics : IStatic
    {
        bool Inited
        {
            get;
        }

        bool Active
        {
            get;
        }

        bool Validate(IInAppItem item, System.Action<IInAppItem, bool> callback);
        void SendPurchase(IInAppItem item);
        void SendEvent(string eventName, Dictionary<string, object> data);
        void SendADS(string eventName, Dictionary<string, object> data);

        void SendBuffer();
        void RemoveUserData();

        Dictionary<string, string> GetDataForRemove();
    }
}