using UnityEngine;

public class spoonBehavior : MonoBehaviour
{

    public SpoonGameManager spoonGameManager;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "plate")
        {
            spoonGameManager.getFood();
        }
        if(collision.gameObject.tag == "mouth")
        {
            spoonGameManager.eatFood();
        }
    }
}
