using UnityEngine;

public class spoonBehavior : MonoBehaviour
{

    public SpoonGameManager spoonGameManager;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "plate" && !spoonGameManager.spoonFull.activeSelf)
        {
            spoonGameManager.getFood();
        }
        if(collision.gameObject.tag == "mouth" && spoonGameManager.spoonFull.activeSelf)
        {
            spoonGameManager.eatFood();
        }
    }
}
