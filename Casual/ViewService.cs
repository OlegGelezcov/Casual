using Casual.Ravenhill;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Casual {

    public interface IViewService : IService {
        bool hasModals { get; }
        GameObject ShowView(RavenhillViewType viewType, object data = null);
        void ShowViewWithDelay(RavenhillViewType viewType, float delay, object data = null);
        void ShowViewWithCondition(RavenhillViewType viewType, System.Func<bool> predicate, object data = null);
        void RemoveView(RavenhillViewType viewType);
        void RemoveView(RavenhillViewType viewType, float delay);
        void RemoveAll();
        bool ExistView(RavenhillViewType viewType);
        float LastViewRemovedTime { get; }
        float LastViewRemoveInterval { get; }
        bool noModals { get; }
        GameObject GetView(RavenhillViewType viewType);
    }

    public abstract class ViewSerive : GameElement, IViewService {

        public abstract bool hasModals { get; }

        public abstract GameObject ShowView(RavenhillViewType viewType, object data = null);

        public abstract void RemoveView(RavenhillViewType viewType);

        public abstract void RemoveAll();

        public abstract void Setup(object data);

        public abstract bool ExistView(RavenhillViewType viewType);

        public abstract void ShowViewWithDelay(RavenhillViewType viewType, float delay, object data = null);

        public abstract void ShowViewWithCondition(RavenhillViewType viewType, System.Func<bool> predicate, object data = null);

        public abstract void RemoveView(RavenhillViewType viewType, float delay);

        public abstract float LastViewRemovedTime { get; }

        public float LastViewRemoveInterval {
            get {
                return (Time.time - LastViewRemovedTime);
            }
        }

        public bool noModals {
            get {
                return (!hasModals);
            }
        }

        public abstract GameObject GetView(RavenhillViewType viewType);
    }


}
