using System;
using System.Collections.Generic;
using UnityEngine;

namespace Casual {

    public abstract class CasualEngine : Singleton<CasualEngine>, IEngine {

        private Dictionary<Type, IService> services { get; } = new Dictionary<Type, IService>();

        private CasualInput input = null;

        public CasualInput Input {
            get {
                if(input == null ) {
                    input = FindObjectOfType<CasualInput>();
                }
                return input;
            }
        }

        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void OnApplicationPause(bool paused) { }

        #region IServiceLocator
        public virtual T GetService<T>() where T : IService {
            if(services.ContainsKey(typeof(T))) {
                return (T)services[typeof(T)];
            } else {
                throw new ServiceNotRegisteredException(typeof(T));
            }
        }

        public virtual T Register<I, T>(T service) 
            where I : IService 
            where T : I {
            services[typeof(I)] = service;
            Debug.Log($"registered service {typeof(T).FullName}");
            return service;
        }
        #endregion



        public static T Get<T>() where T : CasualEngine {
            return (instance as T);
        }


        public T Cast<T>() where T : CasualEngine {
            return (this as T);
        }
    }

    public class ServiceNotRegisteredException : UnityException {
        public Type serviceType { get; }

        public ServiceNotRegisteredException(Type type) {
            serviceType = type;
        }

        public override string Message =>
            string.Format($"Service {serviceType.FullName} not implemented");

    }


}
