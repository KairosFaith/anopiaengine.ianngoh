using UnityEngine;
using UnityEngine.Audio;
public class AnopiaSourcerer : MonoBehaviour
{
    public AudioSource Source;
    float MasterVolume = 1;
    public float Volume
    {
        set
        {
            Source.volume = value * MasterVolume;
        }
        get => Source.volume * MasterVolume;
    }
    public float Pitch
    {
        set
        {
            Source.pitch = value;
        }
        get => Source.pitch;
    }
    #region Effects
    AudioDistortionFilter DistortionEffect;
    public float Distortion
    {
        set
        {
            if(!DistortionEffect)
            {
                DistortionEffect = GetComponent<AudioDistortionFilter>();
                //if (!DistortionEffect) DistortionEffect = gameObject.AddComponent<AudioDistortionFilter>();
            }
            DistortionEffect.distortionLevel = value;
        }
        get => DistortionEffect.distortionLevel;
    }
    AudioHighPassFilter HighPassFilterEffect;
    public float HighPass
    {
        set
        {
            if(!HighPassFilterEffect)
            {
                HighPassFilterEffect = GetComponent<AudioHighPassFilter>();
                //if (!HighPassFilterEffect) HighPassFilterEffect = gameObject.AddComponent<AudioHighPassFilter>();
            }
            HighPassFilterEffect.cutoffFrequency = value;
        }
        get => HighPassFilterEffect.cutoffFrequency;
    }
    //TODO add more effects
    #endregion
    void GetSourceComponent()
    {
        if (Source == null)
            Source = GetComponent<AudioSource>();
        //Source.loop = true;
    }
    public void SetData(ClipData data)
    {
        GetSourceComponent();
        Source.volume = MasterVolume = data.Gain;
        Source.clip = data.Clip;
    }
    public void SetData(AudioClip clip, float volume)
    {
        GetSourceComponent();
        Source.volume = MasterVolume = volume;
        Source.clip = clip;
    }
    public void SetOutput(AudioMixerGroup output)
    {
        GetSourceComponent();
        Source.outputAudioMixerGroup = output;
    }
    public void PanToOpposite()
    {
        StartCoroutine(AnopiaAudioCore.PanToOpposite(Source));
    }
    //back door player
    public void PlayOneShot(string SoundId)//random
    {
        anClipMag m = (anClipMag)AnopiaAudioCore.FetchMag(SoundId);
        AudioClip c = m.RandomClip(out float g);
        Source.PlayOneShot(c,g);
    }
    public void PlayOneShot(string SoundId, int key)//specified
    {
        ClipData d = AnopiaAudioCore.FetchData(SoundId, key);
        Source.PlayOneShot(d);
    }
    public void PlayScheduled(AudioClip clip, double timeCode)
    {
        Source.clip = clip;
        Source.PlayScheduled(timeCode);
    }
    public void StopScheduled(double timeCode, bool toDelete = false)
    {
        Source.SetScheduledEndTime(timeCode);
        Source.loop = false;
        if (toDelete)
            StartCoroutine(AnopiaAudioCore.DeleteWhenDone(Source,timeCode));
    }
}