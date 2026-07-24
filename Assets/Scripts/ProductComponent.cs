using UnityEngine;
using System.Collections.Generic;

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
    void NewAttachment(int slot, string otherType){
        attachedComponents[slot] = Instantiate(GetPrefab(otherType), GetPositionAt(slot, componentType), GetRotationAt(slot, componentType), transform);
        attachedComponents[slot].GetComponent<ProductComponent>().Initialize(otherType);
        attachedComponents[slot].GetComponent<ProductComponent>().attachedComponents[0] = gameObject;
    }

    //Delete component attached to this one
    void DeleteAttachment(int slot)
    {
        Object.Destroy(attachedComponents[slot]);
        attachedComponents[slot] = null;
    }

    //Recursively creates many new attachments according to a string blueprint
    //Blueprint example: "square(triangle(square,0),0,square(0,triangle(square,0),triangle(0,0)),semicircle)"
    public void PrintBlueprint(string blueprint)
    {
        List<string> blueprints = new List<string>();
        int parenthesesDepth = 0;
        int previousCommaPosition = -1;
        for(int i = 0; i < blueprint.Length; i++)
        {
            if(blueprint[i] == '(')
            {
                parenthesesDepth++;
            }
            else if (blueprint[i] == ')')
            {
                parenthesesDepth--;
            }
            else if (blueprint[i] == ',' && parenthesesDepth == 0)
            {
                blueprints.Add(blueprint.Substring(previousCommaPosition + 1,i));
                previousCommaPosition = i;
            }
        }
        blueprints.Add(blueprint.Substring(previousCommaPosition + 1,blueprint.Length));
        for(int i = 0; i < blueprints.Count; i++){
            //Instantiate
            if(!blueprints[i].Equals("0")){
                int slot = (i+1)%attachedComponents.Length;
                Debug.Log(blueprints[i].Substring(0,blueprints[i].IndexOf('(')));
                NewAttachment(slot, blueprints[i].Substring(0,blueprints[i].IndexOf('(')));
                Debug.Log(blueprints[i].Substring(blueprints[i].IndexOf('(') + 1, blueprints[i].IndexOf(')')));
                attachedComponents[slot].GetComponent<ProductComponent>().PrintBlueprint(blueprints[i].Substring(blueprints[i].IndexOf('(') + 1, blueprints[i].IndexOf(')')));
            }
            
        }
    }

    //What should the position and rotation and prefab of a new component be at the given slot, given this component's type?
    Vector3 GetPositionAt(int slot, string thisType)
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
    Quaternion GetRotationAt(int slot, string thisType)
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

    GameObject GetPrefab(string otherType)
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
