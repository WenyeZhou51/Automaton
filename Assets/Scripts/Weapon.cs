using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    //public int cardSlots;
    public List<Cards> cards;
    public int index = 0;
    private Transform player;
    private float nextAttack = 0;
    public int size;
    public int reloadTime;
    [SerializeField] List<Cards> defaultCards;
    public void addToWeapon(Cards card, int insertIndex) {
        Debug.Log("insert at:"+insertIndex);
        if (insertIndex>=0 && insertIndex < size) {
            cards.RemoveAt(insertIndex);
            cards.Insert(insertIndex, card);
        }

    }

    public void removeFromWeapon(int removeIndex)
    {
        Debug.Log("remove at:" + removeIndex);
        if (removeIndex >= 0 && removeIndex < size && cards[removeIndex]!=null)
        {
            cards.RemoveAt(removeIndex);
            cards.Insert(removeIndex, null);
        }

    }
    private void Awake()
    {
        cards = new List<Cards>();
        player = GetComponent<Transform>();
        for (int i = 0; i < size; i++) {
            cards.Add(null);
        }

        for (int i = 0; i < defaultCards.Count; i++)
        {
            Cards defCard = defaultCards[i];
            this.addToWeapon(defCard,i);
        }


    }
    

public void fireWeapon() {
        bool reloaded = false;
        //add time delay
        if (Time.time >= nextAttack) {
            float fireCost = 0;
            AuxCards curCard;
            ActionCards actionCard = GetTargetActionCard(index);
            if (actionCard != null)
            {
                actionCard.Use(player.position, player.rotation);
                while (!(cards[index] is ActionCards))
                {
                    if (cards[index] != null) {
                        curCard = (AuxCards)cards[index];
                        curCard.applyMod(actionCard);
                        fireCost += curCard.getCost();
                    }
                    
                    if (index < cards.Count - 1)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                        if (reloaded == false) {
                            fireCost += reloadTime;
                        }
                        reloaded = true;
                    }
                }

                fireCost += actionCard.getCost();
                nextAttack = Time.time + fireCost / 100f;
                
                if (index < cards.Count - 1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
            }
            
        }
        
    }
    //do flag modification in weapons class
    private ActionCards GetTargetActionCard(int curIndex) {
        int entryIndex = curIndex;
        if (cards[curIndex] != null && cards[curIndex] is ActionCards)
        {
            return (ActionCards)cards[curIndex];
        }
        else {
            if (curIndex < cards.Count - 1)
            {
                curIndex++;
            }
            else
            {
                curIndex = 0;
            }
        }
        
        while (!(cards[curIndex] is ActionCards) && curIndex != entryIndex) {
            if (curIndex < cards.Count - 1)
            {
                curIndex++;
            }
            else { 
                curIndex = 0;
            }
            
        }
        return (ActionCards) cards[curIndex];
    }
}
