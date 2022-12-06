using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    
    [SerializeField] private TMPro.TMP_Text fps;
    [SerializeField] private TMPro.TMP_Text memory_usage;

    float deltaTime = 0.0f;
    float fps_count = 0.0f;

    void Start() {
    }

    // Update is called once per frame
    void Update() {
        deltaTime += Time.deltaTime;
        deltaTime /= 2;
        fps.text = $"fps: {Mathf.Round(1.0f/deltaTime)}";
    }
}
