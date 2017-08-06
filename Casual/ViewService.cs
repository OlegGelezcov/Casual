using System;
using System.Collections.Generic;
using UnityEngine;

namespace Casual {

    public interface IViewService : IService {
        bool hasModals { get; }
        GameObject ShowView(object viewType, object data = null);
        void RemoveView(object viewType);
        void RemoveAll();
    }

    public abstract class ViewSerive : GameElement, IViewService {

        public abstract bool hasModals { get; }

        public abstract GameObject ShowView(object viewType, object data = null);

        public abstract void RemoveView(object viewType);

        public abstract void RemoveAll();

        public abstract void Setup(object data);

    }


}
