using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationSystem : MonoBehaviour
{
    [Header("UI")]
    public Image textBox;
    public Text text;
    public Canvas canvas;
    private Text styleText;

    [Header("Frases que dirá el personaje")]
    public List<string> textToShow;
    [Header("Velocidad del texto")]
    public float textSpeed;

    private int currentLine;
    private bool textEnded;

    private int originalTextSize;

    void Start()
    {
        textEnded = true;

        originalTextSize = text.fontSize;

        text.text = "";
        
        styleText = Instantiate(text, textBox.transform);

        GetPhrase(0);
        StartText(0);
    }

    void GetPhrase(int textID)
    {
        // Limpiar el vector para poner las nuevas frases
        textToShow.Clear();

        TextAsset t = Resources.Load("Phrases/text" + textID) as TextAsset;

        string[] textList = t.text.Split('\n');
        for (int i = 0; i < textList.Length; i++)
        {
            textToShow.Add(textList[i]);
        }
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && textEnded)
        {
            if (currentLine < textToShow.Capacity - 1)
            {
                currentLine++;
                StartText(currentLine);
            }
            else
            {
                // Vaciar el texto si se intenta avanzar cuando este ha acabado
                text.text = "";
            }
        }
    }
    //! Vacía el texto y empieza el efecto del texto con la frase actual
    public void StartText(int phrase)
    {
        text.text = "";
        StartCoroutine(AppearWordsEffect(textToShow[phrase]));
    }

    IEnumerator AppearWordsEffect(string line)
    {
        textEnded = false;

        for (int i = 0; i < line.Length; i++)
        {
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
                styleText.text += "";

            // Escribir el texto letra por letra
            text.text += line[i];
            yield return new WaitForSeconds(textSpeed);
        }
        textEnded = true;
    }
    bool LetterTypeComprober(char letter)
    {
        switch (letter)
        {
            default:
                return false;
                break;

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
}