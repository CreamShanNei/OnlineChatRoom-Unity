using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine.UI;
using System.Threading;
 
public class clientself : MonoBehaviour {
    public bool isNumberIP;
    private byte[] data = new byte[1024];//这个是一个数据容器
    private byte[] nicknamedata = new byte[1024];//这个是一个数据容器
    public InputField inputcontext;
    public InputField inputIP;
    public InputField inputPort;
    public InputField inputNickname;
    private Socket TcpClient;
    string getmsg,oldmessage;
    private Thread thread;
    public Text te;
    // Use this for initialization
    public void LinkToServer ()
    {
        ConnectToServer();
       
    }
	
    public void switchNumberIP()
    {
        if(isNumberIP)
        {
            isNumberIP = false;
        }
        else
        {
            isNumberIP = true;
        }
    }
    
	// Update is called once per frame
	void Update () {
        if (getmsg != null && getmsg!= oldmessage)
        {
            te.text += getmsg+"\n" ;
            oldmessage = getmsg;
        }
       
    }
    void ConnectToServer()
    {
        TcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        if(!isNumberIP)
        {
            IPHostEntry ipHostInfo = Dns.Resolve(inputIP.text);
            IPAddress myip = ipHostInfo.AddressList[0];
            TcpClient.Connect(new IPEndPoint(myip, 2000));  
        }
        else
        {
            int port = 2000;
            inputPort.text = 2000.ToString();
            if(Convert.ToInt32(inputPort.text)>0)
            {
                port = Convert.ToInt32(inputPort.text);
            }

            TcpClient.Connect(new IPEndPoint(IPAddress.Parse(inputIP.text), port)); 
        }
        //发起连接
        
 
        //创建新线程来接受消息
        thread = new Thread(GetmessageFromServer);
        thread.Start();
    }
 
    public void SendMessageToServer(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        byte[] nicknamedata = Encoding.UTF8.GetBytes(inputNickname.text);
        TcpClient.Send(data);
        TcpClient.Send(nicknamedata);
    }
 
 
    public void GetmessageFromServer()
    {
        
        while (true)
        {
            //在接收数据之前  判断一下socket连接是否断开
            if (TcpClient.Connected==false)
            {
               // TcpClient.Close();
                break;//跳出循环 终止线程的执行
            }
 
            int getservermsglength = TcpClient.Receive(data);
            getmsg = Encoding.UTF8.GetString(data, 0, getservermsglength);
            Debug.Log(getmsg);
           
        }
    }
 
    public void SendMessage()
    {
        string getmessages = inputcontext.text;
        SendMessageToServer(getmessages);
    }
}