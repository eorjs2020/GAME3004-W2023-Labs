using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


public class GameSaveManager : MonoBehaviour
{

    public Transform player;
    public Transform playerCam;

    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().transform;
        playerCam = player.gameObject.GetComponentInChildren<CameraController>().transform;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SaveData();
        }
    }

    //Data Serialization = Encoidng the data
    private void SaveData()
    {
        var positionString = JsonUtility.ToJson(player.position);
        
        var RotationString = JsonUtility.ToJson(player.localEulerAngles);

        var cameraString = JsonUtility.ToJson(playerCam.localEulerAngles);

        PlayerPrefs.SetString("position", positionString);
        PlayerPrefs.SetString("roation ", RotationString);

        PlayerPrefs.SetString("camera ", cameraString);
        PlayerPrefs.Save();
    }

    // Data Deserialiaztion = Decoidng the data
    private void LoadData()
    {
        var position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("position"));
        var rotation = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("roation"));
        var camera = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("camera"));

        player.gameObject.GetComponent<CharacterController>().enabled = false;

        player.position = position;
        player.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        playerCam.rotation = Quaternion.Euler(camera.x, camera.y, camera.z);
        player.gameObject.GetComponent<CharacterController>().enabled = true;
    }
    private void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnSaveButton_Pressed()
    {

    }

    public void OnLoadButton_Pressed()
    {

    }
}
