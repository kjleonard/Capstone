using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.IO;
using System.Net.Sockets;

public class MovementController : MonoBehaviour
{
	private bool debug = false;
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
    public bool endSim = false;

    public static System.Timers.Timer t;
    public static LoadSceneOnScript endSimulation;

    /** Initializes variables (with some utilizing PlayerPrefs), starts a duration timer, and creates
     * a child thread to either read in sensor data directly from Trigno sensors via the Trigno Control 
     * Panel or from a file of test data. */

    void Start()
    {
        obstacleHit = false;
        velocity = Vector3.zero;
        dir = transform.forward;
        speed = PlayerPrefs.GetFloat("selSpeed") * 5000f / 60f;    // kilometers/hour to meters/min
        int durationPref = PlayerPrefs.GetInt("selDuration");


        t = new System.Timers.Timer(durationPref);
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

    /** When the duration timer fires, stops collecting sensor data and loads the End Screen. */

    // Once the selected duration has been reached, move to the end screen
    private void OnTimedEvent_Elapsed(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        isRunning = false;
        Debug.Log(String.Format("End Simulation Timer Fired!"));
        endSim = true;
        //endSimulation = new LoadSceneOnScript();
        //endSimulation.LoadByIndex(2);
    }

    /** Provides functionality to manually modify player speed and end the simulation early,
     * calls updateMPMText and potentially updateEMGText if enabled, and moves the player
     * forward each frame utilizing the velocity/speed of the player. */

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
        //updateEMGText();


        velocity = Vector3.zero;
        velocity = dir / dir.magnitude * Time.deltaTime * speed * (float)(Math.Log(Convert.ToInt32(speed), 25)) / 35; //calculate velocity
        if(speed > 0)
            transform.position += velocity; //move character forward
        PlayerPrefs.SetFloat("countLeftEMG", (countLeftEMG / countEMG));
        PlayerPrefs.SetFloat("countRightEMG", (countRightEMG / countEMG));
    }

    /** Updates the displayed movement speed text */

    void updateMPMText()
    {
        mpm_text.text = String.Format("{0:0,0} mpm", speed/5);
    }

    /** Updates the displayed left and right EMG text */

    void updateEMGText()
    {
        left_emg_text.text = String.Format("Left EMG: {0} mV", leftEMG);
        right_emg_text.text = String.Format("Right EMG: {0} mV", rightEMG);
    }

    /** Initializes a TCP client to communicate with the Trigno Control Application;
     * Reads in data continually, sets variables with positional information so feet objects
     * can be updated in FeetScript.cs, and tracks EMG readings so that average EMG readings can
     * be determined and saved as PlayerPrefs when we move to the End Screen. */

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
		/*commandWrite.Write("UPSAMPLING?\r\n\r\n");
		commandRead.Read(commandBuffer, 0, commandBuffer.Length);
		Debug.Log(String.Format("command = {0}", new String(commandBuffer)));*/
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
        port = 50041;
        TcpClient emgPort = new TcpClient("127.0.0.1", port);
        NetworkStream emgStream = emgPort.GetStream();
        
        while (isRunning)
        {
            accStream.Read(accData, 0, accData.Length);
            emgStream.Read(emgData, 0, emgData.Length);

            leftZ = BitConverter.ToSingle(accData, 24);  //Trigno X axis is parallel to arrow
            leftX = BitConverter.ToSingle(accData, 28);  //Trigno Y axis is perpendicular to arrow, on same plane as arrow
            leftY = BitConverter.ToSingle(accData, 32);  //Trigno Z axis is perpendicular to x-y plane
			rightZ = BitConverter.ToSingle(accData, 36);
            rightX = BitConverter.ToSingle(accData, 40);
            rightY = BitConverter.ToSingle(accData, 44);           

            
            leftEMG = BitConverter.ToSingle(emgData, 8);
            rightEMG = BitConverter.ToSingle(emgData, 12);
            
			countLeftEMG += leftEMG;
			countRightEMG += rightEMG;
			countEMG++;

            //AppendAllBytes("TestData/AccelerometerTestData.data", accData);
            //AppendAllBytes("TestData/EMGTestData.data", emgData);
            

			//Debug.Log(String.Format("leftX = {0} leftY = {0} leftZ = {0}", leftX, leftY, leftZ));
            //Debug.Log(String.Format("leftY = {0}", leftY));
            //Debug.Log(String.Format("leftZ = {0}", leftZ));
            //Debug.Log(String.Format("rightX = {0}", rightX));
            //Debug.Log(String.Format("rightY = {0}", rightY));
            //Debug.Log(String.Format("rightZ = {0}", rightZ));
            //Thread.Sleep(50);
        }



        Debug.Log("Reached STOP");
        commandWrite.WriteLine("STOP\r\n");
    }

    /** Initializes a file streamer to read from a file of test sensor data;
     * Reads in data continually, sets variables with positional information so feet objects
     * can be updated in FeetScript.cs, and tracks EMG readings so that average EMG readings can
     * be determined and saved as PlayerPrefs when we move to the End Screen. */

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

    /** Used to assist with reading test sensor data from a file as a continuous stream of bytes. */

    public static void AppendAllBytes(string path, byte[] bytes)
    {
        using (var stream = new FileStream(path, FileMode.Append))
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }

}
