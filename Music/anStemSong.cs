using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class anStemSong : IanSong
{
    anStemMusicMag Mag;
    AudioMixerGroup Output;
    public List<anSourcerer> SourceHandlers = new List<anSourcerer>();
    public override void FadeOut(float t, Action ondone = null)
    {
        Action<float> ChangeValue = null;
        foreach (anSourcerer s in SourceHandlers)
            ChangeValue += (float v) =>
                s.audioSource.volume = v;
        ondone += () => Destroy(gameObject);
        StartCoroutine(anCore.FadeValue(t, 1, 0, ChangeValue, ondone));
    }
    public override void Play(double startTime)
    {
        foreach (anSourcerer s in SourceHandlers)
            s.audioSource.PlayScheduled(startTime);
        anCore.PlayClipScheduled(Mag.Impact, startTime, Output);
    }
    public override void StopOnCue(double stopTime)
    {
        foreach (anSourcerer s in SourceHandlers)
        {
            AudioSource a = s.audioSource;
            a.SetScheduledEndTime(stopTime);
            s.DeleteAfterTime(stopTime);
        }
        transform.DetachChildren();
        Destroy(gameObject);
    }
    public override void FadeIn(float t)
    {
        Action<float> ChangeValue = null;
        foreach (anSourcerer s in SourceHandlers)
            ChangeValue += (float v) =>
                s.audioSource.volume = v;
        StartCoroutine(anCore.FadeValue(t, 0, 1, ChangeValue));
    }
    public override void Setup(IanMusicMag mag, AudioMixerGroup output)
    {
        Mag = (anStemMusicMag)mag;
        StemData[] Stems = Mag.Stems;
        foreach(StemData data in Stems)
        {
            anSourcerer s = anCore.Setup2DLoopSource(data.Clip, data.Channel);
            s.audioSource.panStereo = data.Pan;
            SourceHandlers.Add(s);
        }
        Output = output;
    }
    public override void StopImmediate()
    {
        foreach (anSourcerer s in SourceHandlers)
            s.audioSource.Stop();
        Destroy(gameObject);
    }
    public override void Mute(bool toMute)
    {
        foreach (anSourcerer s in SourceHandlers)
            s.audioSource.mute = toMute;
    }
}