using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Collections;

public class MovementController : MonoBehaviour {

    private Vector3 velocity;
    private float speed;
    private Vector3 dir;
    
    private int x;
    private int y;
    private int z;
    

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

        // Update is called once per frame
        void Update () {

        if (Input.GetKeyDown("w"))
            speed += 15f;
        else if (Input.GetKeyDown("s"))
            speed -= 15f;

        velocity = Vector3.zero;
        velocity = dir / dir.magnitude * Time.deltaTime * speed  * (float)(Math.Log(Convert.ToInt32(speed), 25)) / 35; //calculate velocity
        transform.position += velocity; //move character forward

        }
    
        void getXYZ ()
        {
            //here is where the TCP Client will run to communicate with the Control Application
            Int32 port = 50040;		//command interface port
            TcpClient client = new TcpClient ("127.0.0.1", port);
            StreamWriter commandWrite = new StreamWriter(client.GetStream ());
            StreamReader commandRead = new StreamReader (client.GetStream ());
            //Boolean[] activeSensors = new Boolean[16];		//check to see which sensors are active
            Byte[] data = new Byte[192];
            string command;
            /*for (int i ; i < 16 ; i++) {
                command = string.Format ("SENSOR %d PAIRED?\r\n", i); //check which sensors are on and paired
                data = System.Text.Encoding.ASCII.GetBytes(command);
                StreamReader reader = new StreamReader (client.GetStream ());
                stream.WriteAsync (data, 0, 100);
                stream.ReadAsync (data, 0, 192);
                command = System.Text.Encoding.ASCII.GetString (data);
                if (command == "Yes") {
                    activeSensors [i] = true;
                    command = string.Format ("SENSOR %d SETMODE 2\r\n", i);
                } else {
                    activeSensors [i] = false;
                }
            }*/

            port = 50042;
            TcpClient accelerometerPort = new TcpClient ("127.0.0.1", port);
            NetworkStream accStream = accelerometerPort.GetStream ();
            bool running = true;
            float[] values = new float[48];
            commandWrite.WriteLine("START\r\n");
            command = commandRead.ReadLine ();
            if (command == "OK") {
                float leftX, leftY, leftZ, rightX, rightY, rightZ;
                while (running) {
                    accStream.Read (data, 0, 192);
                    leftX = System.BitConverter.ToSingle (data, 0);
                    leftY = System.BitConverter.ToSingle (data, 4);
                    leftZ = System.BitConverter.ToSingle (data, 8);
                    rightX = System.BitConverter.ToSingle (data, 12);
                    rightY = System.BitConverter.ToSingle (data, 16);
                    rightZ = System.BitConverter.ToSingle (data, 20);
                }
            }
        }
}
