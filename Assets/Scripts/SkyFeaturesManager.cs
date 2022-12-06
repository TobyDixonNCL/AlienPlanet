using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkyFeaturesManager : MonoBehaviour {

    [SerializeField] private GameObject ship1;
    [SerializeField] private GameObject ship2;
    [SerializeField] private GameObject ship3;
    [SerializeField] private GameObject ship4;

    private List<GameObject> ships;


    void Start() {
        ships = new List<GameObject>();
        ships.Add(ship1);
        ships.Add(ship2);
        ships.Add(ship3);
        ships.Add(ship4);
    }

    // Update is called once per frame
    void Update() {
        // Random chance to spawn ship;

        if (Random.Range(0, 1000) > 990) {
            int choice = Random.Range(0, ships.Count);

            Vector3 pos = new Vector3(-300, 200, Random.Range(100, 900));
            GameObject ship = Instantiate(ships.ElementAt(choice), pos, Quaternion.Euler(0, 90, 0));

            if (pos.x > 450) ship.transform.rotation = Quaternion.Euler(0, -90 + Random.Range(-5, 5), 0);
            else ship.transform.rotation = Quaternion.Euler(0, 90 + Random.Range(-5, 5), 0);

        }
        
    }
}
