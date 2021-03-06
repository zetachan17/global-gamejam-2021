using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    
    public Animator animator;
    //for picking up weapon I guess
    public BoxCollider pickUpCollider;
    public BoxCollider damageCollider;

    //variables for the stats
    //make it public so designer can set the value
    public int durability = 20;
    public int damage = 10;
    public double scoreValue = 10;
    public int weaponType;

    [SerializeField]
    public int durabilityLostPerHit = 10;

    //variable for attacking status
    bool isAttacking;
    float attackCooldown;

    int attackCount;

    public Sprite icon;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("WeaponType", weaponType);
        damageCollider.enabled = false;
    }

    private void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void startAttacking()
    {
        if (isAttacking)
        {
            print("Stop spamming you idiot");
            //prevent repeating motion

            if (attackCooldown <= 0)
                stopAttacking();

            return;
        }
        print("test");
        //tbh maybe move this bool to PlayerController instead
        isAttacking = true;
        //starting animation
        animator.SetInteger("WeaponType", weaponType);
        animator.SetBool("IsAttacking", true);
        attackCooldown = 1.5f;
        damageCollider.enabled = true;
    }

    //Animation event will call this function when the animation is done
    public void stopAttacking()
    {
        //this sequence is important so it doesn't cause bugs where the isAttacking is stucked at true forever 
        Debug.Log("Stop attacking");

        animator.SetBool("IsAttacking", false);

        isAttacking = false;
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Started On trigger enter");

        if (other.tag == "Enemy" && !other.isTrigger)
        {
            Debug.Log("Found Enemy");
            //Damage Enemy
            //Destroy(other);
            durability -= 1;
            GameController.ItemDamaged(this, durabilityLostPerHit);
            other.GetComponent<EnemyController>().ApplyDamage(damage);
            SoundManager.PlayEnemyHit();

            //Maybe move this to player controller instead
            if (durability <= 0)
            {
                weaponBreak();
            }
        }

    }

    //Maybe handles weapon break logic in player controller instead
    //need to remove from inventory after all
    private void weaponBreak()
    {
        
    }

    public void PickedUp() {
        pickUpCollider.enabled = false;
    }

    public bool GetIsAttacking() {
        return isAttacking;
    }

    //Only will happen when weapon gets destroyed
    public void StopAnimation() {
        animator.enabled = false;
    }
}
