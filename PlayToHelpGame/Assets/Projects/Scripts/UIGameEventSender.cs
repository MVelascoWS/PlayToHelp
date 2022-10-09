using UnityEngine;
using Doozy.Engine;

public class UIGameEventSender : MonoBehaviour
{
    
    public string eventNameTest;
    public void SendGameEvent(string gameEvent)
    {
        //Debug.Log(gameEvent + "sended");
        GameEventMessage.SendEvent(gameEvent);        
    }

    
    public void TestGameEvent()
    {
        GameEventMessage.SendEvent(eventNameTest);
    }
}
