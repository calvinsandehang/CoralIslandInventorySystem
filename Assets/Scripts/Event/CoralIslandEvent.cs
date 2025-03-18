using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoralIslandEvent
{
    public static Action<bool> OnToggleSfxSound;
    public static Action<float> OnOnSfxVolumeChanged;
    public static Action<AudioClip> OnStopSfx;
}
