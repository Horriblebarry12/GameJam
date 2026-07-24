using UnityEngine;
using System.Collections.Generic;

//Holds references to all Product-related stuffs and product-related functions
public class ProductManager : MonoBehaviour
{
    //References
    public GameObject squarePrefab;
    public GameObject trianglePrefab;
    public GameObject semicirclePrefab;

    public GameObject productHolderPrefab;

    //first and second should not be null
    public bool CompareProducts(ProductHolder first, ProductHolder second)
    {
        List<GameObject> firstComponents = GetComponents(first.transform.GetChild(0).gameObject);
        List<GameObject> secondComponents = GetComponents(first.transform.GetChild(0).gameObject);
        if(firstComponents.Count != secondComponents.Count)
        {
            return false;
        }
        for(int i = 0; i < firstComponents.Count; i++)
        {
            if(CompareProductsFromStart(firstComponents[i], secondComponents[0]))
            {
                return true;
            }
        }
        return false;
    }

    //When called by a converter, pass target product first and player's product second
    public bool CompareProductsFromStart(GameObject first, GameObject second)
    {
        ProductComponent firstScript = first.GetComponent<ProductComponent>();
        ProductComponent secondScript = second.GetComponent<ProductComponent>();
        
        if(firstScript.componentType != secondScript.componentType)
        {
            return false;
        }

        bool matchInAnyCase = false;
        for(int i = 0; i < firstScript.attachedComponents.Length; i++)
        {
            bool matchInThisCase = true;
            for(int j = 0; j < firstScript.attachedComponents.Length; j++)
            {
                GameObject firstAttachment = firstScript.attachedComponents[j];
                GameObject secondAttachment = secondScript.attachedComponents[(i+j)%firstScript.attachedComponents.Length];
                //Only one object exists? NO MATCH
                if(firstAttachment == null && secondAttachment != null)
                {
                    matchInThisCase = false;
                    break;
                }
                if(firstAttachment != null && secondAttachment == null)
                {
                    matchInThisCase = false;
                    break;
                }
                if(firstAttachment != null && secondAttachment != null)
                {
                    if(!CompareProductsFromStart(firstAttachment, secondAttachment))
                    {
                        matchInThisCase = false;
                        break;
                    }
                }
            }
            if(matchInThisCase)
            {
                matchInAnyCase = true;
                break;
            }
        }
        return matchInAnyCase;
    }

    private List<GameObject> GetComponents(GameObject currentComponent)
    {
        List<GameObject> components = new List<GameObject>();
        components.Add(currentComponent);
        GameObject[] attachedComponents = currentComponent.GetComponent<ProductComponent>().attachedComponents;
        for(int i = 0; i < attachedComponents.Length; i++)
        {
            if(attachedComponents[i] != null)
            {
                components.AddRange(GetComponents(attachedComponents[i]));
            }
        }
        return components;
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
