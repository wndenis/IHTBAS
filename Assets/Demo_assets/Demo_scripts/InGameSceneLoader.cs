using UnityEngine;


public class InGameSceneLoader : MonoBehaviour {
    public int sceneNumber;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.gameManager.LoadScene("testscene2");
            //GameManager.gameManager.
            /*while (!loading.isDone)
            {

            }*/
            //SceneManager.LoadScene(sceneNumber);
        }
    }
}
