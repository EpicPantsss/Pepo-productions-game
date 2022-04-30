using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    [Header("Frases que dir� el personaje")]
    public List<string> textToShow;

    public List<List<string>> optionsList = new List<List<string>>(); // Aqu� se guarda la lista con todas las opciones
    
    private List<string> optionsToChoose = new List<string>();

    [Header("Velocidad del texto")]
    public float textSpeed;

    private int currentLine;
    private bool textEnded;

    private bool onOptions;

    private int originalTextSize;

    private AudioSource audioSource;

    bool endText;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        textEnded = true;

        originalTextSize = text.fontSize;

        text.text = "";

        //--------------styleText = Instantiate(text, textBox.transform);

        optionsBox.enabled = false;

        GetText(0);
        StartText(0);
    }

    void GetText(int textID)
    {
        // Limpiar el vector para poner las nuevas frases y opciones
        textToShow.Clear();

        TextAsset t = Resources.Load("Phrases/text" + textID) as TextAsset;

        // Di�logos
        string[] textList = t.text.Split('\n');
        
        bool optionsHere = false;
        int optionsNumber = 0;
        int[] aux = new int[0];// N�mero de opciones a elegir
        for (int i = 0; i < textList.Length; i++)
        {
            if (textList[i] != "")// Si el texto est� vac�o no se guardar� en la lista
            {
                if (!optionsHere)
                    textToShow.Add(textList[i]);// A�adir los di�logos a la lista

                // Comprobar que no se a�ada ning�n texto de las opciones
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
      
        optionsList.Capacity = optionsNumber;
        List<string> auxList = new List<string>();
        for (int i = 0; i < optionsNumber; i++)
        {
            optionsList.Add(auxList);
            for (int j = 0; j < aux[i]; j++)
                optionsList[i].Add("");
        }

        optionsNumber = 0;
        // Opciones
            /// Guardar las opciones en listas para organizarlas
        for (int i = 0; i < textList.Length; i++)
        {
            if (textList[i][0] == '{')
            {
                i++; /// Pasamos a la siguiente l�nea, para supuestamente empezar a guardar el texto de las opciones

                while (i <= textList.Length)/// Para evitar un bucle infinito o salirte del array
                {
                    if (textList[i][0] != '}')
                    {
                        optionsToChoose.Add(textList[i]);
                    }
                    else
                    {
                        /// Si se encuentra el '}', se romper� el bucle y se a�adir�n las opciones a la 
                        /// lista, y se limpiar� el vector con el texto de las opciones para poder 
                        /// volverlo a usar
                        for (int j = 0; j < optionsToChoose.Count; j++)
                            optionsList[optionsNumber][j] = optionsToChoose[j];
                        optionsToChoose.Clear();
                        
                        break; 
                    }

                    i++; /// Sumar� hasta encontrar el '}'
                }
            }
        }
        */
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
        /*
        if (textToShow[phrase][0] == '{')
        {
            for (int i = 0; i < textToShow[phrase].Length; i++) // Busca el n�mero que est� cerca al '{' (para hacer m�s flexible la escritura de las conversaciones) - N�meros permitidos 0-9
                if (char.IsDigit(textToShow[phrase][i]))
                    OptionChoose(textToShow[phrase][i]);
        }
        else
        */
            StartCoroutine(AppearWordsEffect(textToShow[phrase]));
    }

    private void OptionChoose(int optionsID)
    {
        onOptions = true;

        optionsBox.gameObject.SetActive(true);


        string num = "";
        for (int i = 0; i < optionsList[currentOptions].Count; i++)
        {
            // Creas tantos botones como opciones haya, unity ya lo sit�a en una buena posici�n por el componente de optionsBox
            Button newButton = new Button; 
            Instantiate(optionButton, optionsBox.rectTransform);


            // A�adir la acci�n al bot�n
            int optionLength = optionsList[currentOptions][i].Length - 2;
            num += optionsList[currentOptions][i][optionLength];
                /// Acci�n
            buttonAction = () => ButtonAction(int.Parse(num));
            newButton.onClick.AddListener(buttonAction);
                /// Quitar el n�mero del texto
            string optionText = "";
            for (int j = 0; j < optionsList[currentOptions][i].Length - 4; j++)
                optionText += optionsList[currentOptions][i][j];


            // Ponerle el texto correspondiente al bot�n
            Text optionsText = newButton.GetComponentInChildren<Text>();
            optionsText.text = optionText;
        }
        // Como ya se habr� elegido entre estas opciones, se pasa a la siguiente
        if (currentOptions < optionsList.Count)
            currentOptions++;
    }

    private void ButtonAction(int textToShowNow)
    {
        currentLine = textToShowNow;
    }

    IEnumerator AppearWordsEffect(string line)
    {
        textEnded = false;

        for (int i = 0; i < line.Length; i++)
        {
            /* //-----------------------------------------------------------------
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
