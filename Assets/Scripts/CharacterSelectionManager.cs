using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charNameText;
    // [SerializeField] private TextMeshProUGUI Confirm;
    [SerializeField] private GameObject[] characters;
    [SerializeField] private GameObject charDes;
    [SerializeField] private GameObject currentCharacter;
    [SerializeField] private Transform spawnPoint;
    private GameObject instantiatedCharacter;

    public void DisplayCharacter(int characterIndex)
    {
        // Hide all characters first
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }

        // Only proceed if index is valid
        if (characterIndex >= 0 && characterIndex < characters.Length)
        {
            characters[characterIndex].SetActive(true);


            switch (characterIndex)
            {
                case 0:
                    DisplayTung();
                    break;
                case 1:
                    DisplayTralalelo();
                    break;
                case 2:
                    DisplayBrBr();
                    break;
            }
        }
    }

    public void DisplayTung()
    {
        charNameText.text = "Tung Tung Tung Sahur";
        charDes.SetActive(true);
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }


        instantiatedCharacter = Instantiate(characters[0], spawnPoint.position, Quaternion.identity);
        currentCharacter = instantiatedCharacter;
        // Confirm.SetActive(true);
    }

    public void DisplayTralalelo()
    {
        charNameText.text = "Tralalelo Tralala";
        charDes.SetActive(true);

        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        instantiatedCharacter = Instantiate(characters[1], spawnPoint.position, Quaternion.identity);
        currentCharacter = instantiatedCharacter;
        // Confirm.SetActive(true);
    }

    public void DisplayBrBr()
    {
        charNameText.text = "Br Br Patapim";
        charDes.SetActive(true);

        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        instantiatedCharacter = Instantiate(characters[2], spawnPoint.position, Quaternion.identity);
        currentCharacter = instantiatedCharacter;
        // Confirm.SetActive(true);
    }
}