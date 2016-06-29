using UnityEngine;
using System.Collections;

public class GenerateLandscape : MonoBehaviour {

    public int width = 64;
    public int depth = 64;
    public int heightScale = 20;
    public float detailScale = 25.0f;

    public GameObject snowBlock;
    public GameObject grassBlock;
    public GameObject sandBlock;
    public GameObject waterBlock;


	void Start () {

        int seed = (int)Network.time * 10;
        for (int z = -depth; z < depth; z++)
        {
            for (int x = -width; x < width; x++)
            {
                int y = (int)(Mathf.PerlinNoise((x + seed)/detailScale, (z + seed)/detailScale) * heightScale);
                Vector3 blockPos = new Vector3(x, y, z);

                CreateBlock(y, blockPos);


            }
        }	
	}

    void CreateBlock(int y, Vector3 blockPos){
        if (y > (heightScale / 16) * 15)
        {
            Instantiate(snowBlock, blockPos, Quaternion.identity);
        } 
        else if (y > (heightScale / 16) * 5)
        {
            Instantiate(grassBlock, blockPos, Quaternion.identity);
        } 
        else if (y > (heightScale / 16) * 4)
        {
            Instantiate(sandBlock, blockPos, Quaternion.identity);
        }
        else
        {
            Instantiate(waterBlock, blockPos, Quaternion.identity);
        }
    }


    void Update(){
        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
            if (Physics.Raycast(ray, out hit, 4.0f)){
                Destroy(hit.transform.gameObject);
            }
        }
    }
}
