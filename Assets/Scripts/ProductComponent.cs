using UnityEngine;
using System.Collections.Generic;

public class ProductComponent : MonoBehaviour
{
    //references
    ProductManager productManager;

    //"square", "triangle", "semicircle"
    string componentType;
    //Each object in the array is one slot
    GameObject[] attachedComponents;


    // Update is called once per frame
    void Update()
    {
        
    }

    //Always called by an external script
    public void Initialize(string type)
    {
        productManager = GameObject.FindWithTag("ProductManager").GetComponent<ProductManager>();
        componentType = type;
        switch(type){
            case "square":
                attachedComponents = new GameObject[4]; break;
            case "triangle":
                attachedComponents = new GameObject[3]; break;
            case "semicircle":
                attachedComponents = new GameObject[1]; break;
        }
    }

    //New component attached to this one
    private void NewAttachment(int slot, string otherType){
        attachedComponents[slot] = Instantiate(productManager.GetPrefab(otherType), transform);
        attachedComponents[slot].transform.localPosition = productManager.GetPositionAt(slot, componentType);
        attachedComponents[slot].transform.localRotation = productManager.GetRotationAt(slot, componentType);
        attachedComponents[slot].GetComponent<ProductComponent>().Initialize(otherType);
        attachedComponents[slot].GetComponent<ProductComponent>().attachedComponents[0] = gameObject;
    }

    //Delete component attached to this one
    private void DeleteAttachment(int slot)
    {
        Object.Destroy(attachedComponents[slot]);
        attachedComponents[slot] = null;
    }

    //Recursively creates many new attachments according to a string blueprint
    //Blueprint example: "square(triangle(square,0),0,square(0,triangle(square,0),triangle(0,0)),semicircle)"
    public void PrintBlueprint(string blueprint)
    {
        Debug.Log("printBlueprint: " + blueprint);
        List<string> blueprints = new List<string>();
        int parenthesesDepth = 0;
        int previousCommaPosition = -1;

        //Separate each item in comma'd list into blueprints array
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
                blueprints.Add(blueprint.Substring(previousCommaPosition + 1,i - (previousCommaPosition + 1)));
                previousCommaPosition = i;
            }
        }
        blueprints.Add(blueprint.Substring(previousCommaPosition + 1));

        //For each item in blueprints array, initialize component and printBlueprint again, if applicable
        for(int i = 0; i < blueprints.Count; i++){
            if(!blueprints[i].Equals("0")){
                int slot = (i+1)%attachedComponents.Length;
                if(blueprints[i].IndexOf('(') != -1){
                    NewAttachment(slot, blueprints[i].Substring(0,blueprints[i].IndexOf('(')));
                    attachedComponents[slot].GetComponent<ProductComponent>().PrintBlueprint(blueprints[i].Substring(blueprints[i].IndexOf('(') + 1, blueprints[i].Length - (blueprints[i].IndexOf('(') + 1) - 1));
                }
                else
                {
                    NewAttachment(slot, blueprints[i]);
                }
            }
            
        }
    }
}
