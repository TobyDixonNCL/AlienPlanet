using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] private float speed;

    void Start() {
        speed = 150.0f;
    }

    // Update is called once per frame
    void Update() {

        Vector3 movement = Vector3.right * speed * Time.deltaTime;

        // transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // transform.Translate(x * speed * Time.deltaTime, y * speed * Time.deltaTime, z * speed * Time.deltaTime);
        transform.position += movement;


        transform.Rotate(Vector3.forward, Time.deltaTime * speed);


        if (transform.position.x > 1200) Destroy(gameObject);
    }
}
