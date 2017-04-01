using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Collections;
using System.Timers;

public class MovementController : MonoBehaviour
{

    private Vector3 velocity;
    private float speed;
    private Vector3 dir;

    public float leftX;
    public float leftY;
    public float leftZ;
    public float rightX;
    public float rightY;
    public float rightZ;
        void Start()
        {
            velocity = Vector3.zero;
            dir = transform.forward;
            speed = PlayerPrefs.GetInt("speed");
            
            //Creates Child Thread to retrieve Postional Information
            ThreadStart childref_getXYZ = new ThreadStart(getXYZ);
            Thread childThread_getXYZ = new Thread(childref_getXYZ);
            childThread_getXYZ.Start();
        }

    void Start()
    {
        velocity = Vector3.zero;
        dir = transform.forward;
        int speedPref = PlayerPrefs.GetInt("selSpeed");
        speed = speedPref * 30f;
        int durationPref = PlayerPrefs.GetInt("selDuration");
        System.Timers.Timer t = new System.Timers.Timer();
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
            speed += 15f;
        }
        else if (Input.GetKeyDown("s"))
        {
            speed -= 15f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))  // Panic button to kill simulation early
        {
            LoadSceneOnScript endSimulation = new LoadSceneOnScript();
            endSimulation.LoadByIndex(2);
        }


            velocity = Vector3.zero;
        velocity = dir / dir.magnitude * Time.deltaTime * speed * (float)(Math.Log(Convert.ToInt32(speed), 25)) / 35; //calculate velocity
        transform.position += velocity; //move character forward

        }
    
        void getXYZ ()
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
        Debug.Log(String.Format("command = {0}", new String(commandBuffer)));   //debugging only


        commandWrite.Write("START\r\n\r\n");    //start sensors

        commandRead.Read(commandBuffer, 0, commandBuffer.Length);   //read "OK"
        commandWrite.Write("ENDIANNESS?\r\n\r\n");
        commandRead.Read(commandBuffer, 0, commandBuffer.Length);
        Debug.Log(String.Format("command = {0}", new String(commandBuffer)));   //debugging only

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
        }*/

        port = 50042;
        TcpClient accelerometerPort = new TcpClient("127.0.0.1", port);
        NetworkStream accStream = accelerometerPort.GetStream();
        bool running = true;
        float[] values = new float[48];
        commandWrite.WriteLine("START\r\n");
        command = commandRead.ReadLine();
        if (command == "OK")
        {
            float leftX, leftY, leftZ, rightX, rightY, rightZ;
            while (running)
            {
                accStream.Read(data, 0, 192);
                leftX = System.BitConverter.ToSingle(data, 0);
                leftY = System.BitConverter.ToSingle(data, 4);
                leftZ = System.BitConverter.ToSingle(data, 8);
                rightX = System.BitConverter.ToSingle(data, 12);
                rightY = System.BitConverter.ToSingle(data, 16);
                rightZ = System.BitConverter.ToSingle(data, 20);
            }
        }
        Debug.Log("Reached STOP");
        commandWrite.WriteLine("STOP\r\n");
    }
}
