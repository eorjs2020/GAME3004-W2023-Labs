using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UIElements;
using UnityEditor;

[System.Serializable]
class PlayerData
{
    public string playerPosition;
    public string playerRotation;

    public PlayerData()
    {
        playerPosition = ""; //create and empty containter
        playerRotation = "";
    }
}

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
        // Binary Example
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerData.dat");
        PlayerData data = new PlayerData();

        data.playerPosition = JsonUtility.ToJson(player.position);
        data.playerRotation = JsonUtility.ToJson(player.localEulerAngles);
        bf.Serialize(file, data);
        file.Close();
       

        //Player Prefs Example
        /* var positionString = JsonUtility.ToJson(player.position);
        
        var RotationString = JsonUtility.ToJson(player.localEulerAngles);

        var cameraString = JsonUtility.ToJson(playerCam.localEulerAngles);

        PlayerPrefs.SetString("position", positionString);
        PlayerPrefs.SetString("roation ", RotationString);

        PlayerPrefs.SetString("camera ", cameraString);
        PlayerPrefs.Save();*/




    }

    // Data Deserialiaztion = Decoidng the data
    private void LoadData()
    {
        // Binary Example
        if(File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            var position = JsonUtility.FromJson<Vector3>(data.playerPosition);
            var rotation = JsonUtility.FromJson<Vector3>(data.playerRotation);
            player.position = position;
            player.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);            
            player.gameObject.GetComponent<CharacterController>().enabled = true;
        }

        //Player Prefs Example
        /* var position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("position"));
         var rotation = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("roation"));
         var camera = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("camera"));

         player.gameObject.GetComponent<CharacterController>().enabled = false;

         player.position = position;
         player.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
         playerCam.rotation = Quaternion.Euler(camera.x, camera.y, camera.z);
         player.gameObject.GetComponent<CharacterController>().enabled = true;*/


    }
    private void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/playerData.dat");
        }
        //PlayerPrefs.DeleteAll();
    }

    public void OnSaveButton_Pressed()
    {

    }

    public void OnLoadButton_Pressed()
    {

    }
}
