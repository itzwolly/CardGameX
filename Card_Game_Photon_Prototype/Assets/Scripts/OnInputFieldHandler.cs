using UnityEngine;
using UnityEngine.UI;

public class OnInputFieldHandler : MonoBehaviour {
    [SerializeField] private InputField _deckNameInputField;
    private string _deckName;

    public InputField DeckNameInput {
        get { return _deckNameInputField; }
    }

    public void SetDeckData(string pName) {
        _deckName = pName;

        _deckNameInputField.text = _deckName;
    }

    public void OnDeckNameEndEdit() {
        if (_deckNameInputField.text == "") {
            _deckNameInputField.text = _deckName;
        }
    }
}
