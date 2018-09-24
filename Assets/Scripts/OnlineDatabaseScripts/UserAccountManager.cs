using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DatabaseControl;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour {

    public static UserAccountManager instance;

    private void Awake()
    {
        if (instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }


    public static string loggedIn_Username { get; protected set; } //stores username once logged in
    private static string loggedIn_Password = ""; //stores password once logged in
    public static string loggedInData { get; protected set; }

    public static bool isLoggedIn { get; protected set; }

    public string loggedInSceneName = "Lobby";
    public string loggedOutSceneName = "LoginMenu";

    public delegate void onDataRecievedCallback(string data);

    public void logOut()
    {
        loggedIn_Username = "";
        loggedIn_Password = "";

        isLoggedIn = false;

        Debug.Log("LOGGED OUT");
        SceneManager.LoadScene(loggedOutSceneName);
    }

    public void loggedIn(string username, string password)
    {
        loggedIn_Username = username;
        loggedIn_Password = password;
        isLoggedIn = true;
        Debug.Log("LOGGED IN");
        SceneManager.LoadScene(loggedInSceneName);
    }

    public void sendData(string data)
    { //called when the 'Send Data' button on the data part is pressed
        if (isLoggedIn)
        {
            //ready to send request
            StartCoroutine(sendSendDataRequest(loggedIn_Username, loggedIn_Password, data)); //calls function to send: send data request
        }
    }

    IEnumerator sendSendDataRequest(string username, string password, string data)
    {
        IEnumerator eee = DatabaseControl.DCF.SetUserData(username, password, data);
        while (eee.MoveNext())
        {
            yield return eee.Current;
        }
        WWW returneddd = eee.Current as WWW;
        if (returneddd.text == "ContainsUnsupportedSymbol")
        {
            //One of the parameters contained a - symbol
            Debug.Log("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
        }
        if (returneddd.text == "Error")
        {
            //Error occurred. For more information of the error, DC.Login could
            //be used with the same username and password
            Debug.Log("Data Upload Error: Contains Unsupported Symbol '-'");
        }
    }

    public void getData(onDataRecievedCallback onDataRecieved)
    { //called when the 'Get Data' button on the data part is pressed

        if (isLoggedIn)
        {
            //ready to send request
            StartCoroutine(sendGetDataRequest(loggedIn_Username, loggedIn_Password, onDataRecieved)); //calls function to send get data request
        }
    }

    //onDataReceived is breaking this so my score board is not working or any of the kill death count update
    IEnumerator sendGetDataRequest(string username, string password, onDataRecievedCallback onDataRecieved)
    {
        string data = "ERROR";
        IEnumerator eeee = DatabaseControl.DCF.GetUserData(username, password);
        while (eeee.MoveNext())
        {
            yield return eeee.Current;
        }
        WWW returnedddd = eeee.Current as WWW;
        if (returnedddd.text == "Error")//get an error here while sending data something about the out of date database code that I used from brackey's tutorial
        {
            //Error occurred. For more information of the error, DC.Login could
            //be used with the same username and password
            Debug.Log("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
        }
        else
        {
            if (returnedddd.text == "ContainsUnsupportedSymbol")
            {
                //One of the parameters contained a - symbol
                Debug.Log("Get Data Error: Contains Unsupported Symbol '-'");
            }
            else
            {
                //Data received in returned.text variable
                string DataRecieved = returnedddd.text;
                data = DataRecieved;
            }
        }
        //loggedInData = data;
        if (onDataRecieved != null)
        {
            onDataRecieved.Invoke(data);
        }
    }
}
