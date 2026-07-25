using UnityEngine;

public class BobManager : MonoBehaviour
{
    //References
    [SerializeField] ProductManager productManager;

    //(test)
    //0 means empty. -1 means "Doesn't Matter, stop checking past this point."
    string[] puzzleGoalBlueprints = 
    {
        "square(square(-1,-1,-1),-1,-1,-1)",
        "square(triangle(square,0),0,square(0,triangle(square,0),triangle),semicircle)",
        //"square(triangle(square,0),0,-1,semicircle)",
        "square(0,0,triangle(0,square(0,square(0,triangle(square,0),triangle),semicircle)),0)"
    };

    GameObject puzzleGoal = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetPuzzleGoal(puzzleGoalBlueprints[0]);
        //Debug.Log(CheckProduct(productManager.GenerateProduct(puzzleGoalBlueprints[0], Vector3.zero, transform.rotation, false)));
        //Debug.Log(CheckProduct(productManager.GenerateProduct(puzzleGoalBlueprints[2], Vector3.zero, transform.rotation, false)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPuzzleGoal(string puzzleGoalBrintprint)
    {
        //puzzleGoal = productManager.GenerateProduct(puzzleGoalBrintprint, Vector3.up, transform.rotation, transform, false);
        puzzleGoal = productManager.GenerateProduct(puzzleGoalBrintprint, Vector3.up, transform.rotation, transform, true);
    }

    bool CheckProduct(GameObject playerProduct)
    {
        return productManager.CompareProducts(puzzleGoal, playerProduct);
    }
}
