using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Playables;
using UnityEditor;
using System.IO;


public class SpinWheel : MonoBehaviour
{
    [Header("Spin Wheel Conatiner (All items)")]
    public SpinWheelContainer spinWheelData;
    List<SpinWheelItem> allSpinWheelItems;

    [Header("Spin Wheel Settings")]
    public bool spinClockwise = true;
    public int minRotations = 4;
    public int maxRotations = 8;
    public float spinDuration = 2;

    [Header("Object Ref")]
    public GameObject wheelSpin;        // object to spin
    public GameObject allTimelines;     // object that has all unity timelines

    private List<int> weightedList = new List<int>();
    private bool isSpinning;
    private float anglePerItem;


    // Struct to store random variables/settings
    private struct RandomItemContainer{
        public int Index;
        public int Value;
        public List<int> WeightedOptions;
        public int AmountOfFullRotations;
        
        public RandomItemContainer(List<int> weightedOptions, int minRotations, int maxRotations)
        {
            WeightedOptions = weightedOptions;
            Index = Random.Range(0, WeightedOptions.Count);
            Value = WeightedOptions[Index];
            AmountOfFullRotations = Random.Range(minRotations, maxRotations);
        }
    }

    private void Start(){
        allSpinWheelItems = spinWheelData.spinWheelItems;
        isSpinning = false;
        anglePerItem = 360f / allSpinWheelItems.Count;

        PopulateWeightedList();
    }

    // Populate weighted list based on SpinWheelContainer scriptable Object
    public void PopulateWeightedList(){

        weightedList.Clear();
        foreach (SpinWheelItem item in allSpinWheelItems){   
            int index = allSpinWheelItems.IndexOf(item);
            for (int i = 0; i < item.dropChance; i++) {
                weightedList.Add(index);
            }
        }
    }


    private void Update(){
        if (!isSpinning && Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine(SpinTheWheel());
            SpinWheelManager.instance.SpinStart();
        }
    }


    public void PlayButtonClick(){
        if (!isSpinning) {
            StartCoroutine(SpinTheWheel());
            SpinWheelManager.instance.SpinStart();
        }
    }

    public void ClaimButtonClick(){
        SpinWheelManager.instance.SpinReset();
        ResetTimeline("EndResult");
    }


    private IEnumerator SpinTheWheel()
    {
        // Get random item from weighted list
        RandomItemContainer randomInfo = new RandomItemContainer(weightedList, minRotations, maxRotations);

        // Get angle for selected item
        float pointerAngle = 90f; // top: 90, right: 0f, bottom: -90fn left: +-180f
        float segmentCenter = -(anglePerItem / 2f);
        float itemNumberAngle = ((randomInfo.Value) * anglePerItem) + pointerAngle + segmentCenter;

        // Clockwise or anticlockwise
        float targetAngle;
        if (spinClockwise == true){
            targetAngle = -((360f-itemNumberAngle) + 360f * randomInfo.AmountOfFullRotations);
        }
        else{
            targetAngle = (itemNumberAngle + 360f * randomInfo.AmountOfFullRotations);
        }

        // Log result
        Debug.Log($"Will land on {allSpinWheelItems[randomInfo.Value].name + " " + allSpinWheelItems[randomInfo.Value].quantityText} after spinning {randomInfo.AmountOfFullRotations} times", this);

        // Timer
        float currentTimer = 0f;
        float totalSpinTime = randomInfo.AmountOfFullRotations * spinDuration;

        // Get current wheel angle
        float currentAngle = wheelSpin.transform.localEulerAngles.z % 360;

        // Spinning the wheel
        isSpinning = true;
        while (currentTimer < totalSpinTime)
        {
            float lerpFactor = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1, currentTimer / totalSpinTime)));
            wheelSpin.transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp( currentAngle, targetAngle, lerpFactor));
            currentTimer += Time.deltaTime;

            yield return null;
        }
        isSpinning = false;

        // Trigger events
        SpinWheelManager.instance.SpinEnd(allSpinWheelItems[randomInfo.Value]);
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

    
    // ---------------------------------------------------------------------------------------
    // TEST ----------------------------------------------------------------------------------

    [ContextMenu("RunDropTest")]
    public void UNITTEST_DropRate(){

        float testSize = 1000f;
        string path = "Assets/Resources/test.txt";

        StreamWriter writer = new StreamWriter(path, false);
        List<int> allOutcomes = new List<int>();
        int[] perItem = new int[allSpinWheelItems.Count];

        writer.WriteLine("===== DROP TEST ===== Total Drops: " + testSize +"\n");

        for(int i=0; i<testSize; i++){
            RandomItemContainer randomInfo = new RandomItemContainer(weightedList, minRotations, maxRotations);
            perItem[randomInfo.Value] += 1;
            allOutcomes.Add(randomInfo.Value);
        }

        // Calculate Percentage and display in console
        for(int i=0; i<allSpinWheelItems.Count; i++){
            string debugText = allSpinWheelItems[i].name + " " + allSpinWheelItems[i].quantityText + " | Total Drops: " + perItem[i]+ " | DropRate: " + (perItem[i]/testSize)*100f + "%";
            writer.WriteLine(debugText);
            Debug.Log(debugText);
        }

        writer.WriteLine("\n===== LOG =====\n");

        for(int i=0; i<testSize; i++){
            writer.WriteLine(allSpinWheelItems[allOutcomes[i]].name +" "+ allSpinWheelItems[allOutcomes[i]].quantityText);
        }

        writer.Close();
    }

}
