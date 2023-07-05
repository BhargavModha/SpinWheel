using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class SpinWheelAnimations : MonoBehaviour
{
    [Header("All Timelines (GameObject)")]
    public GameObject allTimelines;     // object that has all unity timelines
    
    [Header("Spin Wheel Extra")]
    public GameObject wheel;
    public GameObject wheelBorder;
    public GameObject wheelPointer;
    public GameObject PlayButton;

    [Header("End Sequence Object")]
    public GameObject EndResultObject;

    
    private string imageObjectName = "ItemImage";
    private string textObjectName = "ItemQuantity";

    void Start()
    {   
        SpinWheelManager.instance.onSpinStart += HidePlayButton;
        SpinWheelManager.instance.onSpinEnd += HideSpinWheelObjects;
        SpinWheelManager.instance.onSpinEnd += SetEndResultObject;
        SpinWheelManager.instance.onSpinEnd += StartEndResult;
        SpinWheelManager.instance.onSpinReset += ShowAllWheelObjects;
        SpinWheelManager.instance.onSpinReset += ResetResult;
    }

    public void HidePlayButton(){
        PlayButton.SetActive(false);
    }

    public void HideSpinWheelObjects(SpinWheelItem item){
        wheel.SetActive(false);
        wheelBorder.SetActive(false);
        wheelPointer.SetActive(false);
    }

    public void SetEndResultObject(SpinWheelItem item){

        Transform getItemImage = EndResultObject.transform.Find(imageObjectName);
        Transform getItemQuantity = EndResultObject.transform.Find(textObjectName);
        
        Image itemImage = getItemImage.GetComponent<Image>();
        TextMeshProUGUI itemQuantity = getItemQuantity.GetComponent<TextMeshProUGUI>();

        // assigns sprite and text from scriptable object
        itemImage.sprite = item.itemSprite;
        itemQuantity.text = item.quantityText;     
    }

    public void ShowAllWheelObjects(){
        wheel.SetActive(true);
        wheelBorder.SetActive(true);
        wheelPointer.SetActive(true);
        PlayButton.SetActive(true);
    }


    public void StartEndResult(SpinWheelItem item){
        StartCoroutine(EndAnimation());
    }
    public void ResetResult(){
        ResetTimeline("EndResult");
    }

    private IEnumerator EndAnimation(){
        yield return StartTimeline("EndResult");
    }

    // Find alltimeline gameobject and call playtimeline function 
    public Coroutine StartTimeline(string timelineName){ 
        Transform getTimeline = allTimelines.transform.Find(timelineName);

        if(getTimeline!=null){ 
            PlayableDirector timeline = getTimeline.GetComponent<PlayableDirector>(); 
            Coroutine temp = StartCoroutine(PlayTimeline(timeline)); 
            return temp; 
        } 
        else{ 
            Debug.Log("TIME LINE "+ timelineName +" NOT FOUND"); 
            return null; 
        } 
    } 
    
    public IEnumerator PlayTimeline(PlayableDirector timeline) 
    { 
        timeline.Play(); 
        yield return new WaitForSeconds((float)timeline.duration); 
        timeline.Stop(); 
    }

    // Set timeline to start and resets any running coroutines
    public void ResetTimeline(string timelineName){
        Transform getTimeline = allTimelines.transform.Find(timelineName); 
 
        if(getTimeline != null){ 
            PlayableDirector timeline = getTimeline.GetComponent<PlayableDirector>(); 
            timeline.time = 0.0f;
            timeline.Evaluate();
            timeline.Stop();
            StopAllCoroutines();
        } 
    }


}
