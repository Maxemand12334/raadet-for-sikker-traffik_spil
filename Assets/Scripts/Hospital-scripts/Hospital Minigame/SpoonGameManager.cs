using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEditor.Rendering;
using UnityEngine;

public class SpoonGameManager : MonoBehaviour
{
    public BoxCollider spoon;
    public BoxCollider mouth;
    public BoxCollider plate;
    public GameObject spoonFull;
    public GameObject armParent;
    public Camera cam;

    public Transform plateGamePosition;

    public float scoreAmount = 1;

    public float score = 0;

    public enum GameStates
    {
        active,
        inactive
    }

    private GameStates gameState;


    void Start()
    {
        gameState = GameStates.inactive;
        spoonFull.SetActive(false);
    }
    void Update()
    {
        if (score > 2)
        {
            SetGameStateInactive();
            score = 0;
        }
    }

    public void SetGameStateActive()
    {
        gameState = GameStates.active;
        plate.gameObject.GetComponent<selectable>().Activate(plateGamePosition.position);
        armParent.SetActive(true);

    }

    public void SetGameStateInactive()
    {
        gameState = GameStates.inactive;
        armParent.GetComponent<ArmController>().FloatOutRight();
        StartCoroutine(armFloatOut());
        plate.gameObject.GetComponent<selectable>().Deactivate(); 

    }

    public void getFood(){
        spoonFull.SetActive(true);
    }
    
    public void eatFood(){
        spoonFull.SetActive(false); 
        score += scoreAmount;  
    }

    public GameStates getGameState()
    {
        return gameState;
    }

    IEnumerator armFloatOut()
    {
        
        yield return new WaitForSeconds(1f);
        armParent.SetActive(false);
    }

}
