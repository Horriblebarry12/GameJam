using UnityEngine;

public class ProductComponent : MonoBehaviour
{
    //references
    public GameObject squarePrefab;
    public GameObject trianglePrefab;
    public GameObject semicirclePrefab;

    //"square", "triangle", "semicircle"
    string componentType;
    //Each object in the array is one slot
    GameObject[] attachedComponents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Always called by an external script
    public void Initialize(string type)
    {
        componentType = type;
        switch(type){
            case "square":
                attachedComponents = new GameObject[4]; break;
            case "triangle":
                attachedComponents = new GameObject[3]; break;
            case "semicircle":
                attachedComponents = new GameObject[1]; break;
        }
        Debug.Log("Initialized " + type);
    }

    //New component attached to this one
    void NewAttachment(int slot, string otherType, GameObject prefab){
        attachedComponents[slot] = Instantiate(prefab, getPositionAt(slot, componentType), getRotationAt(slot, componentType), transform);
        attachedComponents[slot].GetComponent<ProductComponent>().Initialize(otherType);
        attachedComponents[slot].GetComponent<ProductComponent>().attachedComponents[0] = gameObject;
    }

    //Delete component attached to this one
    void DeleteAttachment(int slot)
    {
        Object.Destroy(attachedComponents[slot]);
        attachedComponents[slot] = null;
    }

    //What should the position and rotation of a new component be at the given slot, given this component's type?
    Vector3 getPositionAt(int slot, string thisType)
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
    Quaternion getRotationAt(int slot, string thisType)
    {
        Vector3 rotation = new Vector3();
        switch(thisType){
            case "square":
                switch(slot)
                {
                    case 0: rotation = new Vector3(0,0,180f); break;
                    case 1: rotation = new Vector3(0,0,270f); break;
                    case 2: rotation = new Vector3(0,0,0f); break;
                    case 3: rotation = new Vector3(0,0,90f); break;
                }
                break;
            case "triangle":
                switch(slot)
                {
                    case 0: rotation = new Vector3(0,0,180f); break;
                    case 1: rotation = new Vector3(0,0,300f); break;
                    case 2: rotation = new Vector3(0,0,60f); break;
                }
                break;
            case "semicircle":
                rotation = new Vector3(0,0,180f); 
                break;
        }
        return Quaternion.Euler(rotation);
    }
}
