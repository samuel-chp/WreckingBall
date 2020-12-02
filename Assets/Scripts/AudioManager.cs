using UnityEngine;
using System;
using extOSC;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    
    // OSC Connexion
    private OSCTransmitter _transmitter;

    public static AudioManager Instance;

    private void Start()
    {
        // One AudioManager only
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        // OSC initialization
        _transmitter = GetComponent<OSCTransmitter>();
    }

    public void Play(string soundName)
    {
        // Find sound
        Sound s = Array.Find(sounds, sound => sound.soundName == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound '"+soundName+"' not found!");
            return;
        }
        
        // Play sound
        // Create message
        var message = new OSCMessage("/playSound");
        // Populate values.
        message.AddValue(OSCValue.String("Default"));
        message.AddValue(OSCValue.String(s.absolutePath));      // filename
        message.AddValue(OSCValue.Float(s.volume)); // volume 
        message.AddValue(OSCValue.Float(s.pitch));  // pitch
        // Send message
        _transmitter.Send(message);
    }
}
