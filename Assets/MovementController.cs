using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.IO;
using System.Net.Sockets;

public class MovementController : MonoBehaviour
{
    private bool debug = true;
    private Vector3 velocity;
    private float speed;
    private Vector3 dir;

    public Text mpm_text;
    public Text left_emg_text;
    public Text right_emg_text;

    public float leftX;
    public float leftY;
    public float leftZ;
    public float rightX;
    public float rightY;
    public float rightZ;

    private float leftEMG;
    private float rightEMG;

    public int countEMG;
    public float countLeftEMG;
    public float countRightEMG;

    public Boolean obstacleHit;
    private bool isRunning = true;
    public DateTime endTime;
    public bool endSim = false;

    public static System.Timers.Timer t;
    public static LoadSceneOnScript endSimulation;


    void Start()
    {
        obstacleHit = false;
        velocity = Vector3.zero;
        dir = transform.forward;
        speed = PlayerPrefs.GetFloat("selSpeed") * 5000f / 60f;    // kilometers/hour to meters/min
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

        if (durationPref == 0)
        {
            durationPref = 1;
        }
        else if (durationPref == 1)
        {
            durationPref = 2;
        }
        else if (durationPref == 2)
        {
            durationPref = 4;
        }
        else if (durationPref == 3)
        {
            durationPref = 5;
        }
        else if (durationPref == 4)
        {
            durationPref = 6;
        }
        else if (durationPref == 5)
        {
            durationPref = 8;
        }
        else if (durationPref == 6)
        {
            durationPref = 10;
        }
        endTime = DateTime.Now;
        endTime.AddMinutes((double) durationPref);
        PlayerPrefs.SetInt("selDuration", durationPref);

        t = new System.Timers.Timer((double) durationPref * 60000);
        Debug.Log(String.Format("DurationPref = {1}, Duration = {0}", t.Interval, durationPref));
        t.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_Elapsed);
        t.AutoReset = false;
        t.Enabled = true;

        //Creates Child Thread to start TCP Client
        if (debug)
        {
            ThreadStart childref_File_Streamer = new ThreadStart(File_Streamer);
            Thread childThread_File_Streamer = new Thread(childref_File_Streamer);
            childThread_File_Streamer.Start();
        }

        //Creates Child Thread to test/
        else
        {
            ThreadStart childref_TCP_Client = new ThreadStart(TCP_Client);
            Thread childThread_TCP_Client = new Thread(childref_TCP_Client);
            childThread_TCP_Client.Start();
        }
    }

    // Once the selected duration has been reached, move to the end screen
    private void OnTimedEvent_Elapsed(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        isRunning = false;
        Debug.Log(String.Format("Timer fired!"));
        endSim = true;
        //endSimulation = new LoadSceneOnScript();
        //endSimulation.LoadByIndex(2);
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
        else if (Input.GetKeyDown(KeyCode.Escape) || endSim == true)  // Panic button to kill simulation early
        {
            // This is a horrible way of dealing with the timer, but deadlines....
            isRunning = false;
            //PlayerPrefs.SetFloat("countLeftEMG", (countLeftEMG / countEMG));
            //PlayerPrefs.SetFloat("countRightEMG", (countRightEMG / countEMG));
            LoadSceneOnScript endSimulation = new LoadSceneOnScript();
            endSimulation.LoadByIndex(2);
        }

        //if (DateTime.Now >= endTime)
        //{
        //    LoadSceneOnScript endSimulation = new LoadSceneOnScript();
        //    endSimulation.LoadByIndex(2);
        //}

        updateMPMText();
        updateEMGText();


        velocity = Vector3.zero;
        velocity = dir / dir.magnitude * Time.deltaTime * speed * (float)(Math.Log(Convert.ToInt32(speed), 25)) / 35; //calculate velocity
        if(speed > 0)
            transform.position += velocity; //move character forward
        PlayerPrefs.SetFloat("countLeftEMG", (countLeftEMG / countEMG));
        PlayerPrefs.SetFloat("countRightEMG", (countRightEMG / countEMG));
    }

    void updateMPMText()
    {
        mpm_text.text = String.Format("{0:0,0} mpm", speed/5);    // We should truncate/round this value
    }

    void updateEMGText()
    {
        left_emg_text.text = String.Format("Left EMG: {0} mV", leftEMG);
        right_emg_text.text = String.Format("Right EMG: {0} mV", rightEMG);
    }

    void TCP_Client()
    {
        //here is where the TCP Client will run to communicate with the Control Application
        Int32 port = 50040;		//command interface port
        TcpClient client = new TcpClient("127.0.0.1", port);
        StreamWriter commandWrite = new StreamWriter(client.GetStream());
        commandWrite.AutoFlush = true;
        StreamReader commandRead = new StreamReader(client.GetStream());
        Char[] commandBuffer = new Char[2048];
        Byte[] accData = new Byte[192];
        Byte[] emgData = new Byte[64];
        commandRead.Read(commandBuffer, 0, commandBuffer.Length); //read connection response
        //Debug.Log(String.Format("command = {0}", new String(commandBuffer)));   //debugging only


        commandWrite.Write("START\r\n\r\n");    //start sensors

        commandRead.Read(commandBuffer, 0, commandBuffer.Length);   //read "OK"
        //commandWrite.Write("ENDIANNESS?\r\n\r\n");
        //commandRead.Read(commandBuffer, 0, commandBuffer.Length);
        //Debug.Log(String.Format("command = {0}", new String(commandBuffer)));   //debugging only
        
        
        //Accelerometer Port
        port = 50042;
        TcpClient accelerometerPort = new TcpClient("127.0.0.1", port);    //connect to sensor port
        NetworkStream accStream = accelerometerPort.GetStream();

        //EMG Port
        port = 50043;
        TcpClient emgPort = new TcpClient("127.0.0.1", port);
        NetworkStream emgStream = emgPort.GetStream();
        
        while (isRunning)
        {
            accStream.Read(accData, 0, accData.Length);
            emgStream.Read(emgData, 0, emgData.Length);

            leftZ = BitConverter.ToSingle(accData, 0);  //Trigno X axis is parallel to arrow
            leftX = BitConverter.ToSingle(accData, 4);  //Trigno Y axis is perpendicular to arrow, on same plane as arrow
            leftY = BitConverter.ToSingle(accData, 8);  //Trigno Z axis is perpendicular to x-y plane
            rightZ = BitConverter.ToSingle(accData, 12);
            rightX = BitConverter.ToSingle(accData, 16);
            rightY = BitConverter.ToSingle(accData, 20);           

            // Commented out below two lines so the program would compile
            leftEMG = BitConverter.ToSingle(emgData, 0);
            rightEMG = BitConverter.ToSingle(emgData, 4);
            
            AppendAllBytes("TestData/AccelerometerTestData.data", accData);
            AppendAllBytes("TestData/EMGTestData.data", emgData);
            

            /*Debug.Log(String.Format("leftX = {0}", leftX));
            Debug.Log(String.Format("leftY = {0}", leftY));
            Debug.Log(String.Format("leftZ = {0}", leftZ));
            Debug.Log(String.Format("rightX = {0}", rightX));
            Debug.Log(String.Format("rightY = {0}", rightY));
            Debug.Log(String.Format("rightZ = {0}", rightZ));*/
            //Thread.Sleep(50);
        }



        Debug.Log("Reached STOP");
        commandWrite.WriteLine("STOP\r\n");
    }
    
    void File_Streamer()
    {
        //Put code to read from EMG and Accelerometer data files here.
        Byte[] accData = new Byte[192];
        Byte[] emgData = new Byte[64];
        FileStream accStream = new FileStream("TestData/AccelerometerTestData.data", FileMode.Open);
        FileStream emgStream = new FileStream("TestData/EMGTestData.data", FileMode.Open);

        while (isRunning)
        {

            accStream.Read(accData, 0, accData.Length);
            emgStream.Read(emgData, 0, emgData.Length);

            leftX = BitConverter.ToSingle(accData, 0);
            leftZ = BitConverter.ToSingle(accData, 4);
            leftY = BitConverter.ToSingle(accData, 8);
            rightX = BitConverter.ToSingle(accData, 12);
            rightZ = BitConverter.ToSingle(accData, 16);
            rightY = BitConverter.ToSingle(accData, 20);

            // Commented out below two lines so the program would compile
            leftEMG = BitConverter.ToSingle(emgData, 0);
            rightEMG = BitConverter.ToSingle(emgData, 4);

            countLeftEMG += leftEMG;
            countRightEMG += rightEMG;
            countEMG++;

            /*Debug.Log(String.Format("leftX = {0}", leftX));
            Debug.Log(String.Format("leftY = {0}", leftY));
            Debug.Log(String.Format("leftZ = {0}", leftZ));
            Debug.Log(String.Format("rightX = {0}", rightX));
            Debug.Log(String.Format("rightY = {0}", rightY));
            Debug.Log(String.Format("rightZ = {0}", rightZ));
            Debug.Log(String.Format("rightEMG = {0}", rightEMG));*/
            Thread.Sleep(10);
        }
        Debug.Log("Reached end of File\n\n");
    }

    public static void AppendAllBytes(string path, byte[] bytes)
    {
        using (var stream = new FileStream(path, FileMode.Append))
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }

}
