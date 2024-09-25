using UnityEngine;
public class IKControl : MonoBehaviour
{
    [SerializeField] Transform lookObj; //объект за которым следим
    [SerializeField] GameObject enemy;//враг
    Animator animator;
    PlayerLook playerlook;
    private bool ikActive = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerlook = GetComponent<PlayerLook>();
    }

    void OnAnimatorIK()
    {
        if (ikActive)
        {
            //управление головой
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(lookObj.position);
            //управление левой рукой   
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, lookObj.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, lookObj.rotation);
            //передаем врага в PlayerLook    
            playerlook.FindEnemy(enemy.gameObject);
            //отключаем гравитацию и is kinematic врага
            enemy.gameObject.GetComponent<Enemy>().Death(false);
        }
        else
        {
            //передача управления рукой аниматору
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            //передача управления головой аниматору
            animator.SetLookAtWeight(0);
            //отключаем управление врагом в PlayerLook
            playerlook.FindEnemy(null);
            //Включаем гравитацию
            enemy.gameObject.GetComponent<Enemy>().OffTelekinesis();
        }
    }

    public void Watch()
    {
        ikActive = !ikActive;
    }
}