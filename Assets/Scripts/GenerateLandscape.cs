using UnityEngine;
using System.Collections;


public class Block{
    public int type;
    public bool vis;

    public Block(int t, bool v)
    {
        type = t;
        vis = v;
    }
}


public class GenerateLandscape : MonoBehaviour {

    public static int width = 64;
    public static int depth = 64;
    public static int height = 64;
    public int heightScale = 20;
    public float detailScale = 25.0f;

    public GameObject snowBlock;
    public GameObject grassBlock;
    public GameObject sandBlock;
    public GameObject waterBlock;

    Block[,,] worldBlocks = new Block[width,height,depth];

	void Start () {

        int seed = (int)Network.time * 10;
        for (int z = -depth; z < depth; z++)
        {
            for (int x = -width; x < width; x++)
            {
                int y = (int)(Mathf.PerlinNoise((x + seed)/detailScale, (z + seed)/detailScale) * heightScale);
                Vector3 blockPos = new Vector3(x, y, z);

                CreateBlock(y, blockPos, true);
                while (y > 0)
                {
                    y--;
                    blockPos = new Vector3(x, y, z);
                    CreateBlock(y, blockPos, false);
                }


            }
        }	
	}

    void CreateBlock(int y, Vector3 blockPos, bool create){
        if (y > (heightScale / 16) * 15)
        {
            if (create)
                Instantiate(snowBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(1, create);
        } 
        else if (y > (heightScale / 16) * 5)
        {
            if(create)
                Instantiate(grassBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(2, create);
        } 
        else if (y > (heightScale / 16) * 4)
        {
            if(create)
                Instantiate(sandBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(3, create);
        }
        else
        {
            if(create)
                Instantiate(waterBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(4, create);
        }
    }

    void DrawBlock(Vector3 blockPos){
        if (blockPos.x < 0 || blockPos.x >= width || blockPos.y < 0 || blockPos.y >= height || blockPos.z < 0 || blockPos.z >= depth)
            return;
        if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
            return;

        if (!worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].vis)
        {
            worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].vis = true;
            if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 1)
            {
                Instantiate(snowBlock, blockPos, Quaternion.identity);
            }
            else if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 2)
            {
                Instantiate(grassBlock, blockPos, Quaternion.identity);
            }
            else if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 3)
            {
                Instantiate(sandBlock, blockPos, Quaternion.identity);
            }
            else if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 4)
            {
                Instantiate(waterBlock, blockPos, Quaternion.identity);
            }
        }
    }


    void Update(){
        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
            if (Physics.Raycast(ray, out hit, 4.0f)){
                Vector3 blockPos = hit.transform.position;

                //bottom block don't delete it
                if((int) blockPos.y == 0) return;

                worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = null;

                Destroy(hit.transform.gameObject);

                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int z = -1; z <= 1; z++)
                        {
                            if (!(x == 0 && y == 0 && z == 0))
                            {
                                Vector3 neighbor = new Vector3(blockPos.x + x, blockPos.y + y, blockPos.z + z);
                                DrawBlock(neighbor);
                            }                            
                        }                        
                    }                    
                }
            }
        }
    }
}
