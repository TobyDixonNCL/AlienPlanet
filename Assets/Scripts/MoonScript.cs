using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScript : MonoBehaviour
{
    
    int OrbitRadius;
    [SerializeField] Terrain t;
    [SerializeField] float speed = 20.0f;
    [SerializeField] Light light;
    [SerializeField] GameObject lightTarget;

    void Start() {
        OrbitRadius = 2000;
        transform.position = new Vector3(OrbitRadius, 500, 500);
    }

    // Update is called once per frame
    void Update() {
        transform.RotateAround(new Vector3(
            t.GetPosition().x + t.terrainData.heightmapResolution,
            t.GetPosition().y,
            t.GetPosition().z + t.terrainData.heightmapResolution),
            new Vector3(0, 1, 0), speed * Time.deltaTime);

        light.transform.LookAt(lightTarget.transform, Vector3.up);
    }
}
