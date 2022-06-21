﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoGD
{
    public class AnalyticsBase : StaticBehaviour, IAnalytics
    {
        public virtual bool Inited
        {
            get;
            protected set;
        }

        public virtual bool Active
        {
            get;
            set;
        } = true;

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