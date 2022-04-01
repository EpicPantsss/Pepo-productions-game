using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
[RequireComponent(typeof(AudioSource))]
public class ConversationSystem : MonoBehaviour
{
    [Header("UI")]
    public Image textBox;
    public Image optionsBox;
    public Text text;
    public Canvas canvas;
    public Button optionButton;
    // private Text styleText;

    [Header("Frases que dirá el personaje")]
    public List<string> textToShow;
    public string[][] optionsList;// Aquí se guarda la lista con todas las opciones
    private List<string> optionsToChoose = new List<string>();

    [Header("Velocidad del texto")]
    public float textSpeed;
    [Header("Linea actual")]
    public int currentLine;
    private bool textEnded;

    private bool onOptions;

    private int currentOptions;

    private int originalTextSize;

    private AudioSource audioSource;

    bool endText;

    private UnityAction buttonAction;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        textEnded = true;

        originalTextSize = text.fontSize;

        text.text = "";

        //--------------styleText = Instantiate(text, textBox.transform);

        optionsBox.gameObject.SetActive(false);

        GetText(0);
        StartText(0);
    }

    void GetText(int textID)
    {
        // Limpiar el vector para poner las nuevas frases y opciones
        textToShow.Clear();

        TextAsset t = Resources.Load("Phrases/text" + textID) as TextAsset;

        // Diálogos
        string[] textList = t.text.Split('\n');
        
        bool optionsHere = false;
        int optionsNumber = 0;
        int[] aux = new int[0];// Número de opciones a elegir
        for (int i = 0; i < textList.Length; i++)
        {
            if (textList[i] != "")// Si el texto está vacío no se guardará en la lista
            {
                if (!optionsHere)
                    textToShow.Add(textList[i]);// Añadir los diálogos a la lista

                // Comprobar que no se añada ningún texto de las opciones
                if (textList[i][0] == '{')
                {
                    optionsHere = true;
                    optionsNumber++;
                    aux = new int[optionsNumber];
                }
                else if (textList[i][0] == '}')
                {
                    optionsHere = false;
                }
                else if (optionsHere)
                {
                    aux[optionsNumber - 1]++;
                }
                    
            }
        
        }
        
        optionsList = new string[optionsNumber][];
        for (int i = 0; i < optionsNumber; i++)
        {
            optionsList[i] = new string[aux[i]];
        }

        optionsNumber = 0;
        int aux2 = 0;
        // Opciones
            /// Guardar las opciones en listas para organizarlas
        for (int i = 0; i < textList.Length; i++)
        {
            if (textList[i][0] == '{')
            {
                i++; /// Pasamos a la siguiente línea, para supuestamente empezar a guardar el texto de las opciones

                while (i <= textList.Length)/// Para evitar un bucle infinito o salirte del array
                {
                    if (textList[i][0] != '}')
                    {
                        optionsToChoose.Add(textList[i]);
                    }
                    else
                    {
                        /// Si se encuentra el '}', se romperá el bucle y se añadirán las opciones a la 
                        /// lista, y se limpiará el vector con el texto de las opciones para poder 
                        /// volverlo a usar
                        for (int j = 0; j < optionsToChoose.Count; j++)
                            optionsList[optionsNumber][j] = optionsToChoose[j];
                        optionsToChoose.Clear();
                        
                        break; 
                    }

                    i++; /// Sumará hasta encontrar el '}'
                }
            }
        }
        
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && textEnded && !onOptions)
        {
            if (currentLine < textToShow.Capacity - 1)
            {
                currentLine++;
                if (currentLine >= textToShow.Count)
                    SceneManager.LoadScene("Level1");
                StartText(currentLine);
            }
            else
            {
                // Vaciar el texto si se intenta avanzar cuando este ha acabado
                text.text = "";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !textEnded)
        {
            text.text = textToShow[currentLine];
            StopAllCoroutines();
            textEnded = true;
        }
    }

    // Vacia el texto y empieza el efecto del texto con la frase actual
    public void StartText(int phrase)
    {
        audioSource.Play();
        text.text = "";
        
        if (textToShow[phrase][0] == '{')
        {
            for (int i = 0; i < textToShow[phrase].Length; i++) // Busca el número que esté cerca al '{' (para hacer más flexible la escritura de las conversaciones) - Números permitidos 0-9
            { 
                if (char.IsDigit(textToShow[phrase][i]))
                {
                    string num = "";
                    num += textToShow[phrase][i];
                    OptionChoose(int.Parse(num));
                    break;
                }
            }
        }
        else
            StartCoroutine(AppearWordsEffect(textToShow[phrase]));
    }

    private void OptionChoose(int optionsID)
    {
        onOptions = true;

        optionsBox.gameObject.SetActive(true);


        string num = "";
        for (int i = 0; i < optionsList[currentOptions].Length; i++)
        {
            num = "";///Vaciar la string para usarla otra vez

            // Creas tantos botones como opciones haya, unity ya lo sitúa en una buena posición por el componente de optionsBox
            Button newButton = Instantiate(optionButton, optionsBox.rectTransform);


            // Añadir la acción al botón
            int optionLength = optionsList[currentOptions][i].Length - 2;
            num += optionsList[currentOptions][i][optionLength];
                /// Acción
            buttonAction = () => ButtonAction(int.Parse(num));
            newButton.onClick.AddListener(buttonAction);


            Text optionsText = newButton.GetComponentInChildren<Text>();
            optionsText.text = optionsList[currentOptions][i];
        }
        // Como ya se habrá elegido entre estas opciones, se pasa a la siguiente
        if (currentOptions < optionsList.Length)
            currentOptions++;
    }

    public void ButtonAction(int textToShowNow)
    {
        currentLine = textToShowNow;
        StartText(currentLine);

        onOptions = false;
        optionsBox.gameObject.SetActive(false);
    }

    IEnumerator AppearWordsEffect(string line)
    {
        textEnded = false;

        for (int i = 0; i < line.Length; i++)
        {
            /*
             * Parte en la que aplicas estilos al texto, no funciona correctamente
            if (line[i] == '#')
            {
                i++;
                // Estilo del texto
                LetterTypeComprober(line[i]);

                i++;/// Volver a sumar para que no se escriba el comando de estilo

                // Escribe la palabra con el nuevo estilo hasta que se encuentra un espacio o se llega al final de la frase
                while (i < line.Length - 1 && line[i] != ' ')
                {
                    styleText.text += line[i];
                    text.text += "  ";
                    i++;
                    yield return new WaitForSeconds(textSpeed);
                }
            }
            else
                styleText.text += "  ";
            */

            // Escribir el texto letra por letra
            text.text += line[i];
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Stop();
        textEnded = true;
    }
    /*
    bool LetterTypeComprober(char letter)
    {
        switch (letter)
        {
            case 'b':
                styleText.fontStyle = FontStyle.Bold;
                break;

            case 'i':
                styleText.fontStyle = FontStyle.Italic;
                break;

            case '+':
                styleText.fontSize += 5;
                break;

            case '-':
                styleText.fontSize -= 5;
                break;
        }
        return true;
    }
    */
}