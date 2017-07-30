using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitScript : MonoBehaviour
{
    public string Name;

    //###STATS###
    public int MaxHealth;
    public int Health;
    public int Strength;
    public int Speed;
    public int Efficiency;

    public int TotalActionTokens;
    public int CurrentActionTokens;
    public int MaxPower;
    public int CurrentPower;


    public Vector2 StartingPosition;
    public GridPosition CurrentPosition;

    public Dictionary<string, BaseAction> Actions = new Dictionary<string, BaseAction>();

    // Use this for initialization
    public void StartBase ()
    {
        GameManager.TurnSystem().RegisterUnit(this);
        CurrentPosition = new GridPosition(StartingPosition);
        SetPosition(CurrentPosition);
        CurrentPower = MaxPower;
        gameObject.transform.position = CurrentPosition.ToVector2();
    }

    public virtual void StartTurn()
    {
        CurrentActionTokens = TotalActionTokens;
    }

    public bool IsTurn()
    {
        return GameManager.TurnSystem().IsTurn(this);
    }

    public void SetPosition(GridPosition target)
    {
        GameManager.GridSystem().FreePosition(CurrentPosition);
        GameManager.GridSystem().FillPosition(target);
        CurrentPosition = target;
        if(GameManager.GameActive)
            gameObject.transform.position = CurrentPosition.ToVector2();
    }

    public void LoadBaseActions()
    {
        Actions.Add("Move", new MovementAction(this));
        Actions.Add("Pass", new PassTurnAction(this));
        Actions.Add("BasicAttack", new BasicAttackAction(this));
        Actions.Add("Power", new ConsumePowerAction(this));
        Actions.Add("Heal", new HealingAction(this));
        Actions.Add("Push", new PushAction(this));
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        GameManager.UISystem().Log(Name + " Was Killed!");
        GameManager.GridSystem().FreePosition(CurrentPosition);
        GameManager.TurnSystem().UnRegisterUnit(this);
        Destroy(gameObject);
    }

    public void PassTurnAnimation()
    {
        StartCoroutine(IdleAnimation());
    }

    public virtual IEnumerator IdleAnimation()
    {
        GameManager.GameActive = false;
        yield return new WaitForSecondsRealtime(0.5f);
        GameManager.GameActive = true;
    }

    public void MoveAnimation(Vector2 target)
    {
        StartCoroutine(MoveAnimationCo(target));
    }

    public virtual IEnumerator MoveAnimationCo(Vector2 target)
    {
        GameManager.GameActive = false;
        var startTime = Time.time;
        var startMarker = CurrentPosition.ToVector2();
        var journeyLength = Vector2.Distance(startMarker, target);
        var speed = 3f;

        while (Math.Abs(gameObject.transform.position.x - target.x) > 0.01f || Math.Abs(gameObject.transform.position.y - target.y) > 0.0f)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector2.Lerp(startMarker, target, fracJourney);
            yield return new WaitForEndOfFrame();
        }

        gameObject.transform.position = CurrentPosition.ToVector2();

        GameManager.GameActive = true;
    }

    public void AttackAnimation(Vector2 target)
    {
        StartCoroutine(AttackAnimationCo(target));
    }

    public virtual IEnumerator AttackAnimationCo(Vector2 target)
    {
        GameManager.GameActive = false;

        yield return new WaitForSeconds(0.3f);
        var attack = Instantiate(GameManager.AnimationSystem().AttackImage);
        attack.transform.position = target;
        yield return new WaitForSeconds(0.5f);
        Destroy(attack);
        yield return new WaitForSeconds(0.3f);

        GameManager.GameActive = true;
    }

    public void ConsumePowerAnimation(Vector2 target)
    {
        StartCoroutine(ConsumePowerAnimationCo(target));
    }

    public virtual IEnumerator ConsumePowerAnimationCo(Vector2 target)
    {
        GameManager.GameActive = false;
        yield return new WaitForSeconds(0.3f);
        var attack = Instantiate(GameManager.AnimationSystem().RechargeImage);
        attack.transform.position = target;

        yield return new WaitForSeconds(0.3f);
        Destroy(attack);
        yield return new WaitForSeconds(0.3f);
        attack = Instantiate(GameManager.AnimationSystem().RechargeImage);
        attack.transform.position = target;
        yield return new WaitForSeconds(0.3f);
        Destroy(attack);
        yield return new WaitForSeconds(0.3f);
        GameManager.GameActive = true;
    }


    public void HealAnimation()
    {
        StartCoroutine(HealAnimationCo());
    }

    public virtual IEnumerator HealAnimationCo()
    {
        GameManager.GameActive = false;
        yield return new WaitForSeconds(0.3f);
        var attack = Instantiate(GameManager.AnimationSystem().HealImage);
        attack.transform.position = transform.position;

        yield return new WaitForSeconds(0.3f);
        Destroy(attack);
        yield return new WaitForSeconds(0.3f);
        attack = Instantiate(GameManager.AnimationSystem().HealImage);
        attack.transform.position = transform.position;
        yield return new WaitForSeconds(0.3f);
        Destroy(attack);
        yield return new WaitForSeconds(0.3f);
        GameManager.GameActive = true;
    }
}
