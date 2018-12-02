using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundLayer : MonoBehaviour
{
    public enum Layer
    {
        Foreground      = 0,
        Closeground     = 1,
        Middleground    = 2,
        Background      = 3
    }

    public Layer m_layer;
}
