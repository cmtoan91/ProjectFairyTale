using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static partial class Core
{
    public delegate void GameEvent(object sender, params object[] args);

    static Dictionary<string, GameEvent> _eventBag = new Dictionary<string, GameEvent>();

    public static void SubscribeEvent(object eventName, GameEvent eventObj)
    {
        SubscribeEvent(eventName.ToString(), eventObj);
    }
    public static void BroadcastEvent(object eventName, object sender, params object[] args)
    {
        BroadcastEvent(eventName.ToString(), sender, args);
    }
    public static void SubscribeEvent(string eventName, GameEvent eventObj)
    {
        //check if event is existing, if so subscribe to it
        GameEvent existing = null;
        if (_eventBag.TryGetValue(eventName, out existing))
            existing += eventObj; //delegate subscribe
        else
            existing = eventObj;

        _eventBag[eventName] = existing;//store it in the bag
    }

    public static void SubscribeEvent(EventType eventType, GameEvent eventObj)
    {
        SubscribeEvent(eventType.ToString(), eventObj);
    }

    public static void BroadcastEvent(string eventName, object sender, params object[] args)
    {
        //try to fetch event
        GameEvent existing = null;
        if (_eventBag.TryGetValue(eventName, out existing))
            existing(sender, args); //broadcast to all subscribers on that event
    }
    public static void UnsubscribeEvent(string eventName, GameEvent eventObj)
    {
         //try to fetch event
        GameEvent existing = null;
        if (_eventBag.TryGetValue(eventName, out existing))
            existing -= eventObj; //delegate unsubscribe

        if (existing == null)
            _eventBag.Remove(eventName); //remove empty events
        else
            _eventBag[eventName] = existing;
    }

    public static void UnsubscribeEvent(EventType eventType, GameEvent eventObj)
    {
        UnsubscribeEvent(eventType.ToString(), eventObj);
    }

    public static void ClearAllEvents()
    {
        _eventBag.Clear();
    }

    public static string GetAbsolutePath(string localPath)
    {
        return string.Format("{0}/{1}",Application.dataPath, localPath);
    }
}
