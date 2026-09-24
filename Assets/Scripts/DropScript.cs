using UnityEngine;

public class DropScript : MonoBehaviour
{
    public int tipo; //0 = sem arma, 1 = pistola, 2 = rifle, 3 = granada, 4 = bazooka, 5 = BFG
    public int municao; //quantidade de munição que vai dar pro player

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerscript p = other.gameObject.GetComponent<playerscript>();
            switch (tipo)
            {
                case 1:
                    if (p._pistolMAX < p.pistolMAX && p._pistolMAX + municao <= p.pistolMAX)
                    {
                        p._pistolMAX += municao;
                    }
                    else
                    {
                        p._pistolMAX = p.pistolMAX;
                    }
                    break;
                case 2:
                    if (p._rifleMAX < p.rifleMAX && p._rifleMAX + municao <= p.rifleMAX)
                    {
                        p._rifleMAX += municao;
                    }
                    else
                    {
                        p._rifleMAX = p.rifleMAX;
                    }
                    break;
                case 3:
                    if (p._granadaMAX < p.granadaMAX && p._granadaMAX + municao <= p.granadaMAX)
                    {
                        p._granadaMAX += municao;
                    }
                    else
                    {
                        p._granadaMAX = p.granadaMAX;
                    }
                    break;
                case 4:
                    if (p._rocketMAX < p.rocketMAX && p._rocketMAX + municao <= p.rocketMAX)
                    {
                        p._rocketMAX += municao;
                    }
                    else
                    {
                        p._rocketMAX = p.rocketMAX;
                    }
                    break;
                case 5:
                    if (p._doideraMAX < p.doideraMAX && p._doideraMAX + municao <= p.doideraMAX)
                    {
                        p._doideraMAX += municao;
                    }
                    else
                    {
                        p._doideraMAX = p.doideraMAX;
                    }
                    break;
            }
            Destroy(gameObject);
        }
    }
}