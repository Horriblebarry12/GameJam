using UnityEngine;

//Holds references to all Product-related stuffs and product-related functions
public class ProductManager : MonoBehaviour
{
    //References
    public GameObject squarePrefab;
    public GameObject trianglePrefab;
    public GameObject semicirclePrefab;

    public GameObject productHolderPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //What should the position and rotation and prefab of a new component be at the given slot, given this component's type?
    public Vector3 GetPositionAt(int slot, string thisType)
    {
        Vector3 position = new Vector3();
        switch(thisType){
            case "square":
                switch(slot)
                {
                    case 0: position = new Vector3(0,0,0); break;
                    case 1: position = new Vector3(-0.5f, 0.5f, 0); break;
                    case 2: position = new Vector3(0, 1f, 0); break;
                    case 3: position = new Vector3(0.5f, 0.5f, 0); break;
                }
                break;
            case "triangle":
                switch(slot)
                {
                    case 0: position = new Vector3(0,0,0); break;
                    case 1: position = new Vector3(-0.25f, Mathf.Sqrt(3)/4, 0); break;
                    case 2: position = new Vector3(0.25f, Mathf.Sqrt(3)/4, 0); break;
                }
                break;
            case "semicircle":
                position = new Vector3(0,0,0); 
                break;
        }
        return position;
    }
    public Quaternion GetRotationAt(int slot, string thisType)
    {
        Vector3 rotation = new Vector3();
        switch(thisType){
            case "square":
                switch(slot)
                {
                    case 0: rotation = new Vector3(0,0,180f); break;
                    case 1: rotation = new Vector3(0,0,90f); break;
                    case 2: rotation = new Vector3(0,0,0f); break;
                    case 3: rotation = new Vector3(0,0,270f); break;
                }
                break;
            case "triangle":
                switch(slot)
                {
                    case 0: rotation = new Vector3(0,0,180f); break;
                    case 1: rotation = new Vector3(0,0,60f); break;
                    case 2: rotation = new Vector3(0,0,300f); break;
                }
                break;
            case "semicircle":
                rotation = new Vector3(0,0,180f); 
                break;
        }
        return Quaternion.Euler(rotation);
    }
    public GameObject GetPrefab(string otherType)
    {
        switch(otherType)
        {
            case "square":
                return squarePrefab;
            case "triangle":
                return trianglePrefab;
            case "semicircle":
                return semicirclePrefab;
        }
        Debug.Log("GetPrefab error: " + otherType);
        return null;
    }
}
