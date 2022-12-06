using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TerrainScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float height_mod;
    [SerializeField] float res_mod;
    float[,] heights;

    [SerializeField] TerrainData terrainData;

    [SerializeField] GameObject flower;

    Ray ray;
    RaycastHit hit;

    [SerializeField] int flower_count;

    void Awake() {
        // Get terrain data
        // terDat = gameObject.GetComponent<TerrainData>();

        // Set height strength
        height_mod = 0.7f;
        res_mod = 50.0f;

        // Get heights

        int x_res = terrainData.heightmapResolution;
        int y_res = terrainData.heightmapResolution;

        heights = terrainData.GetHeights(0, 0, x_res, y_res);

        var radial_map = GenRadialMap(terrainData.GetHeights(0, 0, x_res, y_res), x_res, y_res);
        

        for (int y = 0; y < y_res; y++) {
            for (int x = 0; x < x_res; x++) {
                float rad = radial_map[y,x];
                float lowres = Mathf.PerlinNoise(x / res_mod, y / res_mod) * height_mod;
                float midres =  Mathf.PerlinNoise(x / (res_mod / 10), y / (res_mod/10)) * (height_mod * 0.1f);
                float highres = Mathf.PerlinNoise(x / (res_mod / 100), y / (res_mod/100)) * (height_mod * 0.01f);

                heights[y,x] = rad * (lowres + midres + highres);
                // heights[y,x] = (lowres + midres + highres);
            }
        }

        terrainData.SetHeights(0, 0, heights);

        GenSplatMaps();
        // GenFolige();
    }

    float[,] GenRadialMap(float[,] heights, int xres, int yres) {
        float [,] map = heights;
        var center = new Vector2(xres * 0.5f, yres * 0.5f);
        
        for(int y  = 0; y < yres; y++){
            for(int x  = 0; x < xres; x++){
                var distFromCenter = Vector2.Distance(center, new Vector2(x, y));
                map[x,y] = (0.5f - (distFromCenter / xres));
            }
        }

        return map;
    }

    void GenSplatMaps() {         
        // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
        float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        float maxHeight = terrainData.bounds.max.y;
         
        for (int ypos = 0; ypos < terrainData.alphamapHeight; ypos++) {
            for (int xpos = 0; xpos < terrainData.alphamapWidth; xpos++) {
                // Normalise x/y coordinates
                float y = (float)ypos/(float)terrainData.alphamapHeight;
                float x = (float)xpos/(float)terrainData.alphamapWidth;
                 
                // Get height
                float height = terrainData.GetHeight(Mathf.RoundToInt(y * terrainData.heightmapResolution),
                    Mathf.RoundToInt(x * terrainData.heightmapResolution) );
    
                float[] splatWeights = new float[terrainData.alphamapLayers];

                // Get height as a ratio 
                float h_ratio = height / maxHeight;

                // Snow
                // splatWeights[0] = height / maxHeight > 0.8 ? (height/maxHeight * 1.2f) : height/maxHeight * 0.6f;
                if (h_ratio > 0.8f) splatWeights[0] = 1.0f;
                else splatWeights[0] = Mathf.Pow(1.0f - Mathf.Abs(h_ratio - 0.9f), 2);

                // Stone
                // splatWeights[1] = height / maxHeight;
                if (h_ratio < 0.7f && h_ratio > 0.4f) splatWeights[1] = 1.0f;
                else splatWeights[1] = Mathf.Pow(1.0f - Mathf.Abs(h_ratio - 0.55f), 2);

                // Grass
                // splatWeights[2] = (height / maxHeight) < 0.4f ? 1.0f - (height/maxHeight) : (height/maxHeight) / 2f;
                if (h_ratio < 0.3f) splatWeights[2] = 1f;
                else splatWeights[2] = Mathf.Pow(1.0f - Mathf.Abs(h_ratio - 0.15f), 2);
                 
                 
                 
                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights.Sum();
                 
                for(int i = 0; i<terrainData.alphamapLayers; i++){
                    splatWeights[i] /= z;    
                    splatmapData[xpos, ypos, i] = splatWeights[i];
                }
            }
        }
    
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }

    void GenFolige() {

        ray.direction = Vector3.down;

        for (int i = 0; i < flower_count; i++){
            Vector3 pos = new Vector3(
                Random.Range(transform.position.x, transform.position.x + (terrainData.heightmapResolution * 2)),
                500,
                Random.Range(transform.position.z, transform.position.z + (terrainData.heightmapResolution * 2))
            );

            ray.origin = pos;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                if (hit.transform.name == "Terrain") {
                    Instantiate(flower, hit.point, transform.rotation);
                }
            }

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);

        }
    }

    void Update() {
        
    }
}
