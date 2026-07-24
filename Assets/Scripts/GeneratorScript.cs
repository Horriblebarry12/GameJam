using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    //References
    ProductManager productManager;

    string[] blueprints = 
    {
        "square(triangle(square,0),0,square(0,triangle(square,0),triangle),semicircle)"
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        productManager = GameObject.FindWithTag("ProductManager").GetComponent<ProductManager>();
        GenerateProduct(blueprints[0]);
        test = true;
    }

    // Update is called once per frame
    bool test;
    void Update()
    {
        if(test){
        //    GenerateProduct(blueprints[0]);
            test = false;
        }
    }

    void GenerateProduct(string blueprint)
    {
        GameObject productHolder = Instantiate(productManager.productHolderPrefab,transform.position,transform.rotation);
        string topComponentType = blueprint.Substring(0,blueprint.IndexOf('('));
        GameObject topComponent = Instantiate(productManager.GetPrefab(topComponentType), productHolder.transform);
        topComponent.GetComponent<ProductComponent>().Initialize(topComponentType);
        topComponent.GetComponent<ProductComponent>().PrintBlueprint(blueprint.Substring(blueprint.IndexOf('(') + 1, blueprint.Length - (blueprint.IndexOf('(') + 1) - 1));
    }
}
