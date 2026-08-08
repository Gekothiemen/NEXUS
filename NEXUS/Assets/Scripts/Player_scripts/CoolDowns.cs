using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;

public class CoolDowns : MonoBehaviour
{
    private Playermovement playermovement;
    public float DashDuration;
    public float DashCoolDown;
    public bool DashReady;

    void Start()
    {

        playermovement = GetComponent<Playermovement>();
    }
    public void HandleDash()
    {
        if (DashReady == false)
            return;



        if (DashReady == true && Input.GetKey(KeyCode.LeftShift))
        {
            StartCoroutine(DashDurationRoutine(DashDuration));
        }

    }
    IEnumerator DashDurationRoutine(float DashDuration)
    {
        transform.Translate(playermovement.moveDirection * playermovement.dashSpeed * playermovement.speed * Time.deltaTime, Space.World);

        yield return new WaitForSeconds(DashDuration);

        StartCoroutine(DashCoolDownRoutine(DashCoolDown));
    }
    IEnumerator DashCoolDownRoutine(float DashCoolDown)
    {
        DashReady = false;

        yield return new WaitForSeconds(DashCoolDown);

        DashReady = true;
    }
    
    
}
