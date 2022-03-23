using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ConversationSystem : MonoBehaviour
{
    [Header("UI")]
    public Image textBox;
    public Text text;
    [Header("Frases que dirá el personaje")]
    public List<string> textToShow;
    [Header("Velocidad del texto")]
    public float textSpeed;

    private int currentLine;
    private bool textEnded;

    void Start()
    {
        textEnded = true;

        GetPhrase(0);
        StartText(0);
    }

    void GetPhrase(int textID)
    {
        // Limpiar el vector para poner las nuevas frases
        textToShow.Clear();

        StringReader reader = new StringReader("text" + textID.ToString() + ".txt");
        string a = reader.ReadLine();
        textToShow.Add(reader.ReadToEnd());
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
        }
    }
    //! Vacía el texto y empieza el efecto del texto con la frase actual
    public void StartText(int phrase)
    {
        text.text = "";
        AppearWordsEffect(textToShow[phrase]);
    }
    IEnumerator AppearWordsEffect(string line)
    {
        textEnded = false;
        string textStyle = "";
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '#')
            {
                i++;
                // Estilo del texto
                LetterTypeComprober(line[i]);
                // Escribe la palabra con el nuevo estilo hasta que se encuentra un espacio o se llega al final de la frase
                while (i < line.Length - 1 && line[i] != ' ')
                {
                    text.text += line[i];
                    i++;
                    yield return new WaitForSeconds(textSpeed);
                }
            }

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
                text.fontStyle = FontStyle.Bold;
                break;

            case 'i':
                text.fontStyle = FontStyle.Italic;
                break;

            case '+':
                text.fontSize += 5;
                break;

            case '-':
                text.fontSize -= 5;
                break;
        }
        return true;
    }
}