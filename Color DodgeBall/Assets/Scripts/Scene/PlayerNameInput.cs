using TMPro;
using UnityEngine;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private MainMenu mainMenu;

    public void PlayGameWithName()
    {
        string nombre = "Jugador";

        if (nameInput != null && !string.IsNullOrWhiteSpace(nameInput.text))
        {
            nombre = nameInput.text;
        }

        GameFlowData.SetNombreJugador(nombre);

        if (mainMenu != null)
        {
            mainMenu.PlayGame();
        }
        else
        {
            Debug.LogWarning("Falta asignar el MainMenu en PlayerNameInput.");
        }
    }
}