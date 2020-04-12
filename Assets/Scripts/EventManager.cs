using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenericScripts
{
    public enum EventType
    {
        
    };

    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public delegate void OnEvent(EventType eventType, Component sender, object param = null);

        private Dictionary<EventType, List<OnEvent>> 
            _listeners = new Dictionary<EventType, List<OnEvent>>();
        
        public void AddListener(EventType eventType, OnEvent listener)
        {
            if (_listeners.TryGetValue(eventType, out var listenList))
            {
                listenList.Add(listener);
                return;
            }

            listenList = new List<OnEvent>();
            listenList.Add(listener);
            _listeners.Add(eventType, listenList);
        }

        public void PostNostrification(EventType eventType, Component sender, object param = null)
        {
            if (!_listeners.TryGetValue(eventType, out var listenList))
                return;
            foreach (var eventListener in listenList)
                eventListener(eventType, sender, param);
        }
    }
}