using System;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace ServiceLocator
{
    public interface IService { }

    public class ServiceLocator
    {
        private readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();

        
        public static ServiceLocator Current { get; private set; }


        private ServiceLocator()
        {
            Debug.Log($"Initialized {GetType().Name}.");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            Current = new ServiceLocator();
            
            // !!! REGISTER SERVICES HERE
            Register(new StencilService());
            Register(new GameService());
            // !!!
        }

        public static T Get<T>() where T : IService
        {
            string key = typeof(T).Name;
            if (!Current._services.ContainsKey(key))
                throw new Exception($"{key} not registered with {Current.GetType()}.Name");

            return (T) Current._services[key];
        }

        public static void Register<T>(T service) where T : IService
        {
            string key = typeof(T).Name;
            if (Current._services.ContainsKey(key))
            {
                Debug.Log($"Attempted to register service of type {key} which is already registered with the {Current.GetType().Name}.");
                return;
            }

            Debug.Log($"Registered {key}!");
            Current._services.Add(key, service);
        }

        public static void Unregister<T>() where T : IService
        {
            string key = typeof(T).Name;
            if (!Current._services.ContainsKey(key))
            {
                Debug.Log($"Attempted to unregister service of type {key} which is not registered with the {Current.GetType().Name}.");
                return;
            }

            Current._services.Remove(key);
        }
    }
}