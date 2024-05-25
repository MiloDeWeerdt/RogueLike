using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void MoveOrHit(Actor actor, Vector2 direction)
    {
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);
        if (target == null)
        {
            Move(actor, direction);
        }
        else
        {
            Hit(actor, target);
        }
    }
    static public void Move(Actor actor, Vector2 direction)
    {
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position+ (Vector3)direction);
        if (target == null)
        {
            actor.Move(direction);
            actor.UpdateFieldOfView();
        }
        EndTurn(actor);
    }
    static public void Hit(Actor actor, Actor target)
    {
        int damage = actor.Power - target.Defense;

        target.DoDamage(Mathf.Max(damage, 0));

        string message;
        if (damage > 0)
        {
            message = $"{actor.name} hits {target.name} for {damage} damage!";
            UIManager.Instance.AddMessage(message, actor.GetComponent<Player>() ? Color.white : Color.red);
        }
        else
        {
            message = $"{actor.name} attacks {target.name}, but does no damage.";
            UIManager.Instance.AddMessage(message, actor.GetComponent<Player>() ? Color.white : Color.red);
        }

        EndTurn(actor);
    }
        static private void EndTurn(Actor actor)
    {
        Player playerComponent = actor.GetComponent<Player>();
        if (playerComponent != null)
        {
            GameManager.Get.StartEnemyTurn();
        }
    }
}
