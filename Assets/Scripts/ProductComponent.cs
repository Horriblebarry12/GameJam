using UnityEngine;
using System.Collections.Generic;

public class ProductComponent : MonoBehaviour
{
    //references
    ProductManager productManager;

    //"square", "triangle", "semicircle"
    public string componentType;
    //Each object in the array is one slot
    public GameObject[] attachedComponents;


    // Update is called once per frame
    void Update()
    {
        
    }

    //Always called by an external script
    public void Initialize(string type)
    {
        productManager = GameObject.FindWithTag("ProductManager").GetComponent<ProductManager>();
        componentType = type;
        attachedComponents = new GameObject[productManager.GetNumberOfSides(type)];
    }

    //New component attached to this one
    private void NewAttachment(int slot, string otherType){
        attachedComponents[slot] = Instantiate(productManager.GetPrefab(otherType), transform);
        attachedComponents[slot].transform.localPosition = productManager.GetPositionAt(slot, componentType);
        attachedComponents[slot].transform.localRotation = productManager.GetRotationAt(slot, componentType);
        attachedComponents[slot].GetComponent<ProductComponent>().Initialize(otherType);
        attachedComponents[slot].GetComponent<ProductComponent>().attachedComponents[0] = gameObject;
    }

    private void NewCross(int slot)
    {
        GameObject cross = Instantiate(productManager.GetPrefab("cross"), transform);
        cross.transform.localPosition = productManager.GetPositionAt(slot, componentType);
        cross.transform.localRotation = productManager.GetRotationAt(slot, componentType);
    }

    //Delete component attached to this one
    private void DeleteAttachment(int slot)
    {
        Object.Destroy(attachedComponents[slot]);
        attachedComponents[slot] = null;
    }

    //Recursively creates many new attachments according to a string blueprint
    //Blueprint is the string that represents the product.
    //DrawCross depends on whether this blueprint creates a goal or a real product.
    public void PrintBlueprint(string blueprint, bool drawCross)
    {
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
            int slot = (i+1)%attachedComponents.Length;
            if(blueprints[i].Equals("0")){
                if(drawCross)
                {
                    NewCross(slot);
                }
            }
            else if(blueprints[i].Equals("-1"))
            {
                //This component's pointer is set to itself to signal that Compare functions should stop.
                attachedComponents[slot] = gameObject;
            }
            else
            {
                if(blueprints[i].IndexOf('(') != -1){
                    NewAttachment(slot, blueprints[i].Substring(0,blueprints[i].IndexOf('(')));
                    attachedComponents[slot].GetComponent<ProductComponent>().PrintBlueprint(blueprints[i].Substring(blueprints[i].IndexOf('(') + 1, blueprints[i].Length - (blueprints[i].IndexOf('(') + 1) - 1), drawCross);
                }
                else
                {
                    NewAttachment(slot, blueprints[i]);
                    if(drawCross && productManager.GetNumberOfSides(blueprints[i]) > 1)
                    {
                        string blueprintWithCrosses = "";
                        for(int j = 0; j < productManager.GetNumberOfSides(blueprints[i]) - 1; j++)
                        {
                            if(j == 0)
                            {
                                blueprintWithCrosses += "0";
                            }
                            else{
                                blueprintWithCrosses += ",0";
                            }
                        }
                        attachedComponents[slot].GetComponent<ProductComponent>().PrintBlueprint(blueprintWithCrosses, true);
                    }
                }
            }
            
        }
    }
}
