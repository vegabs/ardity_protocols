using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class SerialControllerASCII : MonoBehaviour
{
    public InputField velocidadASCII;
    public InputField textoEnviarASCII;
    public InputField textoRecibirASCII;
    public InputField lastTramaEnviadaASCII;
    public InputField lastTramaRecibidaASCII;

    public Button abrirPuertoASCII;
    public Button readASCII;
    public Button writeASCII;
    public Button autoModeButton;

    public Dropdown availablePorts;

    public SerialController serialController;

    public bool btnReadPressed = false;
    public bool btnWritePressed = false;
    public bool autoModeOn = false;

    void Start()
    {
        velocidadASCII.readOnly = true;
        textoRecibirASCII.readOnly = true;
        lastTramaEnviadaASCII.readOnly = true;
        lastTramaRecibidaASCII.readOnly = true;

        readASCII.onClick.AddListener(readPortASCII);
        writeASCII.onClick.AddListener(writePortASCII);
        abrirPuertoASCII.onClick.AddListener(openPortASCII);
        autoModeButton.onClick.AddListener(autoModeASCII);

        List<string> portList = new List<string>(SerialPort.GetPortNames());
        availablePorts.ClearOptions();
        availablePorts.AddOptions(portList);

    }

    void autoModeASCII()
    {
        autoModeOn = !autoModeOn;
        if (autoModeOn)
        {
            autoModeButton.image.color = Color.red;
            autoModeButton.GetComponentInChildren<Text>().text = "Salir del modo automatico";
            InvokeRepeating("readPortASCII", 0.5f, 1f);
        }
        else
        {
            autoModeButton.image.color = new Color32(108, 241, 213, 255);
            autoModeButton.GetComponentInChildren<Text>().text = "Modo Automático";
            CancelInvoke();
        }
    }

    void openPortASCII()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        velocidadASCII.text = serialController.baudRate.ToString();
        Debug.Log(serialController.portName);

        // leer el puerto
        string dataOpen = serialController.ReadSerialMessage();

        // procesar la respuesta
        if (ReferenceEquals(dataOpen, SerialController.SERIAL_DEVICE_CONNECTED))
        {
            Debug.Log("[OPEN] Conexión establecida");
        }
        else if (ReferenceEquals(dataOpen, SerialController.SERIAL_DEVICE_DISCONNECTED))
        {
            Debug.Log("[OPEN] Conexión desconectada");
        }
        else
        {
            Debug.Log("[OPEN] Mensaje: " + dataOpen);
        }
    }


    void writePortASCII()
    {
        string mensajeEnviar = textoEnviarASCII.text + "\n";
        serialController.SendSerialMessage(mensajeEnviar);

        Debug.Log("[WRITE] Enviando: " + mensajeEnviar);
        lastTramaEnviadaASCII.text = mensajeEnviar;

        btnWritePressed = true;
    }

    void readPortASCII()
    {
        serialController.SendSerialMessage("read\n");
        
        Debug.Log("[READ] Enviando: read");
        lastTramaEnviadaASCII.text = "read\n";

        btnReadPressed = true;
    }

    void OnMessageArrived(string msg)
    {
        lastTramaRecibidaASCII.text = msg;

        if(msg == null)
        {
            Debug.Log("El mensaje recibido es nulo");
            return;
        }

        if(btnReadPressed == true)
        {
            btnReadPressed = false;
            textoRecibirASCII.text = msg;
        }

    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {

    }

    void Update()
    {

    }
}
