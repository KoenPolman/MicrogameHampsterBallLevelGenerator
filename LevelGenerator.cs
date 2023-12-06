using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject player;                               //the gameobject of the player is being kept track of to have the level instantiate and de-instantiate in the right places
    [SerializeField] GameObject[] levelChunks;                        //collection of prefabs the level is built out of
    [SerializeField] int courseWidth;                                 //the with in number of prefabs side to side
    [SerializeField] int initialGenerateAhead;                        //when the level first renders the level generator generates a starting area, this variable decides the length of that starting area
    [SerializeField] float tileSize = 2;                              //the size of the prefab it was 2 for testing purposes but if things go according to plan when writing this it will be more
    [SerializeField] int generateDistance = 25;                       //the distance the level generates ahead of the player
    [SerializeField] int removeDistance = 10;                         //the distance the level gets removed behind of the player
    private List<GameObject[]> levelTiles = new List<GameObject[]>(); //list of game object arrays the level is built out of
    private int levelRowCount = 0;                                    //this variable keeps track of how far the level has generated
    void Start()
    {
        GenerateStartingArea();
    }
    void Update()
    {
        float playerZ = player.transform.position.z;                                                                   //registers the location of the player on the z axis
        while (playerZ + generateDistance > levelRowCount * tileSize)                                                  //checks if there are non-generated rows in the generate distance and loops till its caught up
        {
            GenerateRow(levelRowCount);
            if (levelTiles.Count > 0 && playerZ - removeDistance > (levelTiles[0][0].transform.position.z / tileSize)) //checks if there are generated rows in the remove distance
            {
                DestroyRow(levelTiles[0]);                                                                             //desroys a row at the last index in the list
            }
        }
    }
    /// <summary>
    /// generates a starting area for the player so that the player does not see the level generating
    /// </summary>
    private void GenerateStartingArea()
    {
        for (int x = 0; x < courseWidth + initialGenerateAhead; x++) //the loops for the course with plus the initialGeneratAhead distanse
        {
            GenerateRow(x);
        }
    }
    /// <summary>
    /// generates a row, parameter is row count that wich indicates where the row should be instantiated
    /// </summary>
    /// <param name="rowPosition"></param>
    private void GenerateRow(int rowPosition) 
    {
        GameObject[] levelRow = new GameObject[courseWidth];                                                                                      //creates a array with gameobjects and sets the length according to the course width

        for (int blockPosition = 0; blockPosition < courseWidth; blockPosition++)                                                                 //repeats for the amount of course width
        {
            GameObject prefabToBeUsed = levelChunks[UnityEngine.Random.Range(0, levelChunks.Length)];                                             //creates a new game object with one of the appointed prefabs in the editor
            GameObject obj = Instantiate(prefabToBeUsed, new Vector3(blockPosition * tileSize, 0, rowPosition * tileSize), Quaternion.identity);  //instantiates the prefab from the previous line at a specific location and stores it in a temporary variable
            levelRow[blockPosition] = obj;                                                                                                        //stores the game object in a array for later referencing
        }
        this.levelTiles.Add(levelRow);                                                                                                            //stores the game object array in a list for later referencing
        levelRowCount++;                                                                                                                          //keeps track of how far the level has generated so the prefabs can be placed at the right position
    }
    /// <summary>
    /// destroys a row, expects a array with game objects
    /// </summary>
    /// <param name="row"></param>
    private void DestroyRow(GameObject[] row)
    {
        foreach (GameObject Block in row) //repeats for every game object in the array
        {
            Destroy(Block);               //and destroys it
        }
        levelTiles.RemoveAt(0);           //removes the entry in the list that contains the level row at index 0, if done as intended it should be the array this method uses
    }
}
