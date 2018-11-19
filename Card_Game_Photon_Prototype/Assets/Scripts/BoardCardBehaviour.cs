using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCardBehaviour : MonoBehaviour {
    public Text txtName;
    public InputField txtDescription;
    public Text txtCost;
    public Text txtAttack;
    public Text txtHealth;

    public void SetMonsterCardText(MonsterCardData pData) {
        txtName.text = pData.Name;
        txtDescription.text = pData.Description;
        txtCost.text = pData.RegCost.ToString();
        txtAttack.text = pData.Attack.ToString();
        txtHealth.text = pData.Health.ToString();
    }

    //TODO: ADD
    public void Attack() {

    }
}
