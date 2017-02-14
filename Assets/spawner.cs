using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

    public Transform spawnLocation;
    public GameObject whatToSpawnPrefab;
    public GameObject whatToSpawnClone;
    public GameObject Tunnel;
    public GameObject player;
    private bool destroy = false;
    public GameObject Tunnel1;
    public GameObject Tunnel2;
    public GameObject Tunnel3;
    public GameObject Tunnel4;
    public GameObject Tunnel5;
    public GameObject Tunnel6;
    public GameObject Tunnel7;
    public GameObject Tunnel8;
    public GameObject Tunnel9;
    public GameObject Tunnel10;
    private Vector3 needPos;
    private int count;

    void Start()
    {
        needPos = spawnLocation.position;
        count = 2;
    }

    void Update()
    {
        if(player.transform.position.z < needPos.z+40)
        {
            if (destroy)
            {
                switch (count)
                {
                    case 1:
                        Object.Destroy(Tunnel10);
                        break;
                    case 2:
                        Object.Destroy(Tunnel1);
                        break;
                    case 3:
                        Object.Destroy(Tunnel2);
                        break;
                    case 4:
                        Object.Destroy(Tunnel3);
                        break;
                    case 5:
                        Object.Destroy(Tunnel4);
                        break;
                    case 6:
                        Object.Destroy(Tunnel5);
                        break;
                    case 7:
                        Object.Destroy(Tunnel6);
                        break;
                    case 8:
                        Object.Destroy(Tunnel7);
                        break;
                    case 9:
                        Object.Destroy(Tunnel8);
                        break;
                    default:
                        Object.Destroy(Tunnel9);
                        break;
                }
            }
            else
                destroy = true;

            spawnObject();
        }
        
    }

    void spawnObject()
    {
        
        needPos.z = needPos.z - 1001.664f;

        whatToSpawnClone = Instantiate(whatToSpawnPrefab, needPos, Quaternion.Euler(0, 90, 0)) as GameObject;
        switch (count) {
            case 1:
                Tunnel10 = whatToSpawnClone;
                count++;
                break;
            case 2:
                Tunnel1 = whatToSpawnClone;
                count++;
                break;
            case 3:
                Tunnel2 = whatToSpawnClone;
                count++;
                break;
            case 4:
                Tunnel3 = whatToSpawnClone;
                count++;
                break;
            case 5:
                Tunnel4 = whatToSpawnClone;
                count++;
                break;
            case 6:
                Tunnel5 = whatToSpawnClone;
                count++;
                break;
            case 7:
                Tunnel6 = whatToSpawnClone;
                count++;
                break;
            case 8:
                Tunnel7 = whatToSpawnClone;
                count++;
                break;
            case 9:
                Tunnel8 = whatToSpawnClone;
                count++;
                break;
            default:
                Tunnel9 = whatToSpawnClone;
                count = 1;
                break;
        }
        needPos.z = needPos.z + 890.368f;

    }
}
