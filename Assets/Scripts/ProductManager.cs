using UnityEngine;
using System.Collections.Generic;

//Holds references to all Product-related stuffs and product-related functions
public class ProductManager : MonoBehaviour
{
    //References
    [SerializeField] GameObject squarePrefab;
    [SerializeField] GameObject trianglePrefab;
    [SerializeField] GameObject semicirclePrefab;
    [SerializeField] GameObject crossPrefab;

    [SerializeField] public GameObject productHolderPrefab;

    //This overload gives the productHolder a parent
    public GameObject GenerateProduct(string blueprint, Vector3 position, Quaternion rotation, Transform parent, bool drawCross)
	{
		GameObject productHolder = Instantiate(productHolderPrefab, position, rotation, parent);
		string topComponentType = blueprint.Substring(0, blueprint.IndexOf('('));
		GameObject topComponent = Instantiate(GetPrefab(topComponentType), productHolder.transform);
		topComponent.GetComponent<ProductComponent>().Initialize(topComponentType);
		topComponent.GetComponent<ProductComponent>().PrintBlueprint(blueprint.Substring(blueprint.IndexOf('(') + 1, blueprint.Length - (blueprint.IndexOf('(') + 1) - 1), drawCross);
        return productHolder;
	}

    //This overload does not give the productHolder a parent
    public GameObject GenerateProduct(string blueprint, Vector3 position, Quaternion rotation, bool drawCross)
	{
		GameObject productHolder = Instantiate(productHolderPrefab, position, rotation);
		string topComponentType = blueprint.Substring(0, blueprint.IndexOf('('));
		GameObject topComponent = Instantiate(GetPrefab(topComponentType), productHolder.transform);
		topComponent.GetComponent<ProductComponent>().Initialize(topComponentType);
		topComponent.GetComponent<ProductComponent>().PrintBlueprint(blueprint.Substring(blueprint.IndexOf('(') + 1, blueprint.Length - (blueprint.IndexOf('(') + 1) - 1), drawCross);
        return productHolder;
	}

    //Compare two products, referencing only their productHolders
    //First is always the goal. Second is always what the player made.
    public bool CompareProducts(GameObject firstProductHolder, GameObject secondProductHolder)
    {
        List<GameObject> firstComponents = GetComponents(firstProductHolder.transform.GetChild(0).gameObject, null);
        List<GameObject> secondComponents = GetComponents(secondProductHolder.transform.GetChild(0).gameObject, null);
        for(int i = 0; i < secondComponents.Count; i++)
        {
            Debug.Log("checking start point" + i);
            if(CompareProductsFromStartingComponent(firstComponents[0], secondComponents[i], null, null))
            {
                return true;
            }
        }
        return false;
    }

    //Compare two products, given a specific starting component for each one
    //First is always the goal. Second is always what the player made.
    public bool CompareProductsFromStartingComponent(GameObject firstComponent, GameObject secondComponent, GameObject firstIgnoreComponent, GameObject secondIgnoreComponent)
    {
        ProductComponent firstScript = firstComponent.GetComponent<ProductComponent>();
        ProductComponent secondScript = secondComponent.GetComponent<ProductComponent>();
        
        if(firstScript.componentType != secondScript.componentType)
        {
            return false;
        }

        bool matchInAnyCase = false;
        for(int i = 0; i < secondScript.attachedComponents.Length; i++)
        {
            bool matchInThisCase = true;
            for(int j = 0; j < secondScript.attachedComponents.Length; j++)
            {
                //Debug.Log("checking: " + firstScript.componentType + " " + i + " " + j);
                GameObject firstAttachment = firstScript.attachedComponents[(i+j)%firstScript.attachedComponents.Length];
                GameObject secondAttachment = secondScript.attachedComponents[j];

                //Check for "-1" marker
                if(firstAttachment != firstComponent)
                {
                    bool firstNull = (firstAttachment == null);
                    bool secondNull = (secondAttachment == null);
                    
                    //If only one component is null, they are not the same.
                    if(firstNull != secondNull) //XOR
                    {
                        matchInThisCase = false; //Debug.Log("Failed null check " + firstNull + " " + secondNull);
                        break;
                    }

                    //If neither component is null, continue.
                    if(!firstNull && !secondNull)
                    {
                        bool firstIgnore = (firstAttachment == firstIgnoreComponent);
                        bool secondIgnore = (secondAttachment == secondIgnoreComponent);

                        //If only one component is the previous component, they are not the same.
                        if(firstIgnore != secondIgnore) //XOR
                        {
                            matchInThisCase = false; //Debug.Log("Failed ignore check" + firstIgnore + " " + secondIgnore);
                            break;
                        }
                        //If neither component is the previous component, continue.
                        if(!firstIgnore && !secondIgnore)
                        {
                            if(!CompareProductsFromStartingComponent(firstAttachment, secondAttachment, firstComponent, secondComponent))
                            {
                            matchInThisCase = false; //Debug.Log("Failed compare check" + firstScript.componentType + " " + i + " " + j);
                            break;
                            }
                        }
                    }
                    //Debug.Log("Success: check: " + firstScript.componentType + " " + i + " " + j);
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

    //If GetComponents is called by itself, ignore the component (ignoreComponent) that GetComponents was previous called from. (previous currentComponent)
    private List<GameObject> GetComponents(GameObject currentComponent, GameObject ignoreComponent)
    {
        List<GameObject> components = new List<GameObject>();
        components.Add(currentComponent);
        GameObject[] attachedComponents = currentComponent.GetComponent<ProductComponent>().attachedComponents;
        for(int i = 0; i < attachedComponents.Length; i++)
        {
            if(attachedComponents[i] == null)
            {
                //Stop scanning because attachedComponents[i] is empty
            }
            else if(attachedComponents[i] == currentComponent)
            {
                //Stop scanning because attachedComponents[i] had a "-1" marker
            }
            else if(attachedComponents[i] == ignoreComponent)
            {
                //Stop scanning because attachedComponents[i] is the previous component already scanned
            }
            else
            {
                components.AddRange(GetComponents(attachedComponents[i], currentComponent));
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
    public GameObject GetPrefab(string type)
    {
        switch(type)
        {
            case "square":
                return squarePrefab;
            case "triangle":
                return trianglePrefab;
            case "semicircle":
                return semicirclePrefab;
            case "cross":
                return crossPrefab;
        }
        return null;
    }

    public int GetNumberOfSides(string type)
    {
        switch(type)
        {
            case "square":
                return 4;
            case "triangle":
                return 3;
            case "semicircle":
                return 1;
        }
        return -1;
    }
}
