using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWorld : MonoBehaviour
{

    public ObjectManager objectManager;
    public GameObject tree;
    
    void Awake()
    {
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
    }

    public void Generate()
    {
        GenerateTrees(16);
    }
    
    public void GenerateTrees(int numberOfTrees)
    {
        for(int i = 0; i < numberOfTrees; i++){
            float x = Random.Range(-10, 11) + 0.5f;
            float y = Random.Range(-10, 11) + 0.5f;
            //float[] randPos = {x,y,0};
            if(objectManager.IsSpaceFree(x,y))
            {
                //GameObject t = Instantiate(tree, new Vector3(x,y,0), Quaternion.identity);
                //objectManager.AddObject(x,y,"Tree", t);

                GameObject t = Instantiate(tree, new Vector3(x,y,0), Quaternion.identity);
                TreeData td = new TreeData(t.GetComponent<TallTree>());
                objectManager.AddObject(x,y,"Tree",td);
            }
            else
            {
                Debug.Log("Stopped duplicate");
                i--;
            }
        }
    }
}
