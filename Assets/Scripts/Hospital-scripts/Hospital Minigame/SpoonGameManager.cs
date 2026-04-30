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

    private bool isSpoonFull = false;

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
        if (score > 3)
        {
            SetGameStateInactive();
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
        armParent.SetActive(false);
        plate.gameObject.GetComponent<selectable>().Deactivate(); 
    }

    public void getFood(){
        isSpoonFull = true;
        spoonFull.SetActive(true);
    }
    
    public void eatFood(){
        isSpoonFull = false;
        spoonFull.SetActive(false); 
        score += scoreAmount;  
    }

    public GameStates getGameState()
    {
        return gameState;
    }
}
