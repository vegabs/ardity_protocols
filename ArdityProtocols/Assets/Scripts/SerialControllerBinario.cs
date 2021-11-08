using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System.Text;
using System;

public class SerialControllerBinario : MonoBehaviour
{
    public InputField velocidadBinario;
    public InputField textoEnviarBinario;
    public InputField textoRecibirBinario;
    public InputField lastTramaEnviadaBinario;
    public InputField lastTramaRecibidaBinario;

    public Button abrirPuertoBinario;
    public Button readBinario;
    public Button writeBinario;
    public Button autoModeButton;

    public Dropdown availablePorts;

    public SerialControllerCustomDelimiter serialController;

    public bool btnReadPressed = false;
    public bool btnWritePressed = false;
    public bool autoModeOn = false;

    void Start()
    {
        velocidadBinario.readOnly = true;
        textoRecibirBinario.readOnly = true;
        lastTramaEnviadaBinario.readOnly = true;
        lastTramaRecibidaBinario.readOnly = true;

        readBinario.onClick.AddListener(readPortBinario);
        writeBinario.onClick.AddListener(writePortBinario);
        abrirPuertoBinario.onClick.AddListener(openPortBinario);
        autoModeButton.onClick.AddListener(autoModeBinario);

        List<string> portList = new List<string>(SerialPort.GetPortNames());
        availablePorts.ClearOptions();
        availablePorts.AddOptions(portList);

    }

    void Update()
    {
        
    }

    void autoModeBinario()
    {
        autoModeOn = !autoModeOn;
        if (autoModeOn)
        {
            autoModeButton.image.color = Color.red;
            autoModeButton.GetComponentInChildren<Text>().text = "Salir del modo automatico";
            InvokeRepeating("readPortBinario", 0.5f, 1f);
        }
        else
        {
            // autoModeButton.image.color = new Color32(108, 241, 213, 255);
            // autoModeButton.GetComponentInChildren<Text>().text = "Modo Automático";
            CancelInvoke();
        }
    }

    void readPortBinario()
    {
        byte[] mensajeEnviarByte = Encoding.ASCII.GetBytes("R");
        serialController.SendSerialMessage(mensajeEnviarByte);
        btnReadPressed = true;
        lastTramaEnviadaBinario.text = BitConverter.ToString(mensajeEnviarByte);
    }

    void writePortBinario()
    {
        byte[] mensajeByte = Encoding.ASCII.GetBytes(textoEnviarBinario.text);
        serialController.SendSerialMessage(mensajeByte);

        Debug.Log("[WRITE] Enviando: " + BitConverter.ToString(mensajeByte));
        lastTramaEnviadaBinario.text = BitConverter.ToString(mensajeByte);
        btnWritePressed = true;
    }

    void openPortBinario()
    {
        serialController = GameObject.Find("SerialControllerCustomDelimiter").GetComponent<SerialControllerCustomDelimiter>();
        velocidadBinario.text = serialController.baudRate.ToString();
        Debug.Log(serialController.portName);

        // leer el puerto
        byte[] bufferDataOpen = serialController.ReadSerialMessage();

    }

    void OnMessageArrived(byte[] msg)
    {
        Debug.Log(msg);
        lastTramaRecibidaBinario.text = BitConverter.ToString(msg);

        if (msg == null)
        {
            Debug.Log("El mensaje recibido es nulo");
            return;
        }

        if (btnReadPressed == true)
        {
            btnReadPressed = false;
            textoRecibirBinario.text = BitConverter.ToString(msg);
        }

    }
}
