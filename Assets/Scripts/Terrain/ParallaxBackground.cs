using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Dictionary<ParallaxBackgroundLayer.Layer, Transform[]> m_layers = new Dictionary<ParallaxBackgroundLayer.Layer, Transform[]>();
    private Dictionary<ParallaxBackgroundLayer.Layer, float> m_speeds = new Dictionary<ParallaxBackgroundLayer.Layer, float>();
    private Dictionary<Transform, Vector3> m_offsets = new Dictionary<Transform, Vector3>();

    private Transform m_yarn;

    private void Start()
    {
        m_yarn = GameObject.FindGameObjectWithTag("YarnPhysics").transform;

        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
            m_offsets.Add(child, child.position - m_yarn.position);
        }

        var layersDefined = Enum.GetValues(typeof(ParallaxBackgroundLayer.Layer));
        float speedModifier = 0.9f;
        foreach (ParallaxBackgroundLayer.Layer layer in layersDefined)
        {
            List<Transform> childrenInLayer = children.Where(t => t.GetComponent<ParallaxBackgroundLayer>().m_layer == layer).ToList();

            m_layers.Add(layer, childrenInLayer.ToArray());
            m_speeds.Add(layer, speedModifier);
            speedModifier /= layersDefined.Length;
        }
    }

    void Update ()
    {
        var layersDefined = Enum.GetValues(typeof(ParallaxBackgroundLayer.Layer));
        foreach (ParallaxBackgroundLayer.Layer layer in layersDefined)
        {
            float layerSpeed = m_speeds[layer];

            foreach (Transform child in m_layers[layer])
            {
                Vector3 offset = m_offsets[child];

                child.position = new Vector3
                    (
                        m_yarn.position.x + offset.x,
                        m_yarn.position.y + offset.y,   // temp, while parallax effect is being fixed
                        0.0f
                    );
            }
        }
	}
}
