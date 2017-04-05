using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.IO;
using System.Net.Sockets;

public class MovementController : MonoBehaviour
{

    private Vector3 velocity;
    private float speed;
    private Vector3 dir;

    public Text mpm_text;

    public float leftX;
    public float leftY;
    public float leftZ;
    public float rightX;
    public float rightY;
    public float rightZ;

    public Boolean obstacleHit = false;


    void Start()
    {
        velocity = Vector3.zero;
        dir = transform.forward;
        int speedPref = PlayerPrefs.GetInt("selSpeed");
        speed = speedPref * 30f;
        int durationPref = PlayerPrefs.GetInt("selDuration");
        int obstacleFrequency = PlayerPrefs.GetInt("selObstacleFrequency");
        int obstaclesRemaining = 0;

        // To-Do: Figure out best way to accomodate for # per minute in combination with speed
        if (obstacleFrequency == 1)
        {   // Low - 4 per minute = 1/15s
            obstaclesRemaining = 4 * durationPref;
        }
        else if (obstacleFrequency == 2)
        {   // Medium - 6 per minute = 1/10s
            obstaclesRemaining = 6 * durationPref;
        }
        else if (obstacleFrequency == 3)
        {   // High - 10 per minute = 1/6s
            obstaclesRemaining = 10 * durationPref;
        }
        PlayerPrefs.SetInt("obstacleTotal", obstaclesRemaining);

        int obstacleType = PlayerPrefs.GetInt("selObstacleType");
        // To-Do: Should we change this from a dropdown to checkboxes to accomodate randomized selection of obstacles?
        switch (obstacleType)
        {
            case 0: // None
                break;
            case 1: // Boxes
                break;
            case 2: // Strings
                break;
            case 3: // Other
                break;
        }

        System.Timers.Timer t = new System.Timers.Timer();
        if (durationPref == 0)
            durationPref = 10000;
        t.Interval = durationPref * 60000;
        t.Elapsed += OnTimedEvent;
        t.AutoReset = false;


        //Creates Child Thread to retrieve Postional Information
        ThreadStart childref_getXYZ = new ThreadStart(getXYZ);
        Thread childThread_getXYZ = new Thread(childref_getXYZ);
        childThread_getXYZ.Start();

        t.Enabled = true;
    }

    // Once the selected duration has been reached, move to the end screen
    private static void OnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        LoadSceneOnScript endSimulation = new LoadSceneOnScript();
        endSimulation.LoadByIndex(2);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("w"))
        {
            speed += 5f;
        }
        else if (Input.GetKeyDown("s"))
        {
            speed -= 5f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))  // Panic button to kill simulation early
        {
            LoadSceneOnScript endSimulation = new LoadSceneOnScript();
            endSimulation.LoadByIndex(2);
        }

        updateMPMText();
        

        velocity = Vector3.zero;
        velocity = dir / dir.magnitude * Time.deltaTime * speed * (float)(Math.Log(Convert.ToInt32(speed), 25)) / 35; //calculate velocity
        if(speed > 0)
            transform.position += velocity; //move character forward

    }

    void updateMPMText()
    {
        mpm_text.text = String.Format("{0} mpm", speed/5); //speed needs to be replaced to the closest aproximation to mpm
    }

    void getXYZ()
    {
        //here is where the TCP Client will run to communicate with the Control Application
        Int32 port = 50040;		//command interface port
        TcpClient client = new TcpClient("127.0.0.1", port);
        StreamWriter commandWrite = new StreamWriter(client.GetStream());
        commandWrite.AutoFlush = true;
        StreamReader commandRead = new StreamReader(client.GetStream());
        Char[] commandBuffer = new Char[2048];
        Byte[] data = new Byte[192];

        commandRead.Read(commandBuffer, 0, commandBuffer.Length); //read connection response
        //Debug.Log(String.Format("command = {0}", new String(commandBuffer)));   //debugging only


        commandWrite.Write("START\r\n\r\n");    //start sensors

        commandRead.Read(commandBuffer, 0, commandBuffer.Length);   //read "OK"
        //commandWrite.Write("ENDIANNESS?\r\n\r\n");
        //commandRead.Read(commandBuffer, 0, commandBuffer.Length);
        //Debug.Log(String.Format("command = {0}", new String(commandBuffer)));   //debugging only

        port = 50042;
        TcpClient accelerometerPort = new TcpClient("127.0.0.1", port);    //connect to sensor port
        NetworkStream accStream = accelerometerPort.GetStream();
        bool running = true;
        //float leftX, leftY, leftZ, rightX, rightY, rightZ;
        while (running)
        {
            accStream.Read(data, 0, data.Length);
            
            leftX = BitConverter.ToSingle(data, 0);
            leftY = BitConverter.ToSingle(data, 4);
            leftZ = BitConverter.ToSingle(data, 8);
            rightX = BitConverter.ToSingle(data, 12);
            rightY = BitConverter.ToSingle(data, 16);
            rightZ = BitConverter.ToSingle(data, 20);
            /*Debug.Log(String.Format("leftX = {0}", leftX));
            Debug.Log(String.Format("leftY = {0}", leftY));
            Debug.Log(String.Format("leftZ = {0}", leftZ));
            Debug.Log(String.Format("rightX = {0}", rightX));
            Debug.Log(String.Format("rightY = {0}", rightY));
            Debug.Log(String.Format("rightZ = {0}", rightZ));*/
            //Thread.Sleep(50);
        }

        float[] values = new float[48];

        Debug.Log("Reached STOP");
        commandWrite.WriteLine("STOP\r\n");
    }
}
