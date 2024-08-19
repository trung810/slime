using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Health plrHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        totalhealthBar.fillAmount = plrHealth.currHealth / 100;
    }
    private void Update()
    {
        currenthealthBar.fillAmount = plrHealth.currHealth / 100;
    }
}
