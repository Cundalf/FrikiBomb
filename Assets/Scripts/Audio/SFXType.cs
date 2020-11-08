using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXType : MonoBehaviour
{
    public enum SoundType
    {
        BOMB_SOUND,
        ERROR,
        CORRECT,
        EXPLOSION,
        WIN
    }

    public SoundType type;
}
