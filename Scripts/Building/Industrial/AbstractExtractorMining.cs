using System.Collections;
using UnityEngine;
using BuildingComponents;

public class AbstractExtractorMining : MonoBehaviour
{
    //Mini Game Event
    public VoidEvent OnMiniGameEvent;

    //Mining
    public ResourceToInventoryEvent OnMineEvent;
    protected float mineInterval;
    protected int amountToMine;
    protected float timer = 0;
    protected const float displayDuration = 1.5f;
    protected bool isShowingText = false;
    protected bool isBroken = false;
    protected int timesMinedSinceBroken = 0;
    protected float baseBreakChance;
    protected bool playerCanInteract = false;
    protected bool playerInteracted = false;
    protected bool robotBuddyCanInteract = false;
    protected bool isDepracted = false;
    //Mining UI
    public GameObject textPrefabPlusOne;
    public GameObject textPrefabExclamation;
    protected bool isLerping = false;
    private GameObject currentExtractText;
    //GameObjects
    protected GravityBody2D gravityBody;
    protected ResourceType resourceToMine;
    [SerializeField, ReadOnly] protected GameObject linkedGameObject;
    //Extractor Modifiers is determinate by buildingType
    protected BuildingType buildingType;
    protected int TechTier;
    protected float mineAmtModifer;
    protected float breakChanceModifer;
    protected float[] ExtractorMineAmtModifiers = { 0, 1, 1.5f, 2.5f };
    protected float[] ExtractorBCModifiers = { 0, 1, .75f, .5f };
    protected float[] CommercialExtractorMineAmtModifiers = { 0f, 1.25f, 1.6f };
    protected float[] CommercialExtractorBCModifiers = { 0f, .5f, .25f };
    protected float[] IndustrialExtractorMineAmtModifiers = { 0f, 1.3f, 1.8f };
    protected float[] IndustrialExtractorBCModifiers = { 0f, 1, 0f };
    [SerializeField] protected bool isPlaced = false;
    void Awake()
    {
        if (buildingType == BuildingType.Extractor)
        {
            TechTier = 1;
        }
        QueryTechLevel();
        //sets modifers for the first time
        UpdateModifers();
    }
    protected void MineIfPlaced()
    {
        if (isPlaced && !isBroken)
        {
            Mine();
            if (linkedGameObject == null)
            {
                SoundFXManager.Instance.PlaySound(SFX.Cave.MineResource, this.transform, 1f);
                //later we can change it to just change to a sprite with the lights off
                if(currentExtractText != null){
                    Destroy(currentExtractText);
                }
                Destroy(gameObject);
            }
        }
    }
    private void Mine()
    {
        timer += Time.deltaTime;

        if (timer >= mineInterval)
        {
            QueryTechLevel();
            OnMineEvent.Raise(new packet.ResourceToInventory(linkedGameObject, GetCurrentMineAmt(), resourceToMine, true));
            timesMinedSinceBroken += 1;
            timer = 0f;
            if (RollForModuleBreak())
            {
                isBroken = true;
                QueryTechLevel();
                ShowBrokeText();
                timesMinedSinceBroken = 0;
            }
            else
            {
                ShowGainText();
            }
        }

        // Check if the text is currently displayed and fade it away after display duration
        if (isShowingText)
        {
            if (timer >= displayDuration)
            {
                ResetText(currentExtractText);
            }
        }
    }
    private void ResetText(GameObject currText)
    {
        Destroy(currText);
        timer = 0f;
        isLerping = false;
        isShowingText = false;
        currentExtractText = null;
    }
    GameObject SpawnGainText(Vector3 pos)
    {
        isShowingText = true;
        return Instantiate(textPrefabPlusOne, pos, transform.rotation);
    }
    private void ShowGainText()
    {
        Vector3 initPos = GetPositionTextAboveExtractor();
        currentExtractText = SpawnGainText(initPos);
        currentExtractText.GetComponent<TMPro.TextMeshPro>().SetText("+" + GetCurrentMineAmt().ToString());
        Vector3 newPos = new Vector3(-gravityBody.GravityDirection.x, -gravityBody.GravityDirection.y, 0f) / 2;
        StartCoroutine(LerpTextPosition(currentExtractText.transform, initPos + newPos, displayDuration));
    }
    GameObject SpawnBrokeText(Vector3 pos)
    {
        return Instantiate(textPrefabExclamation, pos, transform.rotation);
    }
    private int GetCurrentMineAmt()
    {
        return (int)(amountToMine * mineAmtModifer);
    }
    private void ShowBrokeText()
    {
        if (currentExtractText != null)
        {
            ResetText(currentExtractText);
        }
        Vector3 initPos = GetPositionTextAboveExtractor();
        currentExtractText = SpawnBrokeText(initPos);
    }
    IEnumerator LerpTextPosition(Transform textTransform, Vector3 targetPos, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPos = textTransform.position;

        while (elapsedTime < duration && textTransform != null)
        {
            textTransform.position = Vector3.Lerp(initialPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (textTransform)
        {
            textTransform.position = targetPos;
        }
    }
    Vector3 GetPositionTextAboveExtractor()
    {
        //Position above the extractor relative to extractor
        Vector3 newPos = new Vector3(-gravityBody.GravityDirection.x, -gravityBody.GravityDirection.y, 0f) * 2.3f;
        return transform.position + newPos;
    }
    public void LinkToResource(GameObject resourceObject)
    {
        isPlaced = true;
        if (linkedGameObject == null)
        {
            linkedGameObject = resourceObject;
            resourceToMine = ConvertStrToResourceType(resourceObject.name);
        }
    }
    private void OnDestroy()
    {
        isShowingText = false;
        DragAndDropExtractor.OnPlacementEvent -= LinkToResource;
    }
    private void UpdateModifers()
    {
        switch (buildingType)
        {
            case BuildingType.Extractor:
                mineAmtModifer = ExtractorMineAmtModifiers[TechTier];
                break;
            case BuildingType.CommercialExtractor:
                mineAmtModifer = CommercialExtractorMineAmtModifiers[TechTier];
                break;
            case BuildingType.IndustrialExtractor:
                mineAmtModifer = IndustrialExtractorMineAmtModifiers[TechTier];
                break;
        }
    }
    private ResourceType ConvertStrToResourceType(string str)
    {
        int underScoreIndex = str.IndexOf('_');

        if (underScoreIndex != -1)
        {
            switch (str.Substring(0, underScoreIndex))
            {
                case "Iron":
                    return ResourceType.Iron;
                case "Nickel":
                    return ResourceType.Nickel;
                case "Cobalt":
                    return ResourceType.Cobalt;
                case "Platinum":
                    return ResourceType.Platinum;
                case "Gold":
                    return ResourceType.Gold;
                case "Technetium":
                    return ResourceType.Technetium;
                case "Tungsten":
                    return ResourceType.Tungsten;
                case "Iridium":
                    return ResourceType.Iridium;
                default:
                    Debug.LogError("ExtractorMining.cs --: ConvertStrToResourceType() :-- Switch Statement missed all cases, returning Iron!");
                    return ResourceType.Iron;
            }
        }
        else
        {
            Debug.LogError("ExtractorMining.cs --: ConvertStrToResourceType() :-- Recieved MineEvent Packet does not have correct resource naming, returning Iron!");
            return ResourceType.Iron;
        }
    }
    private bool RollForModuleBreak()
    {
        float breakChance = baseBreakChance * timesMinedSinceBroken;
        //Debug.Log($"baseBreakChance: {baseBreakChance}\tbreakChanceModifer: {breakChanceModifer}\ttimesMinedSinceBroken: {timesMinedSinceBroken}");
        // float breakChance = 0.5f;
        //^Use this for debugging or testing the playerInteract feature
        return Random.value < breakChance;
    }

    //Events
    //------------------------------------------------------------------
    public void QueryTechLevel()
    {
        TechTier = InventoryManager.Instance.GetTechTier(buildingType);
        UpdateModifers();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
        {
            playerCanInteract = true;
        }
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("RobotBuddy")) != 0)
        {
            robotBuddyCanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
        {
            playerCanInteract = false;
        }
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("RobotBuddy")) != 0)
        {
            robotBuddyCanInteract = false;
        }
    }
    public void OnPlayerInteract()
    {
        if (playerCanInteract && isBroken)
        {
            //IMPLEMENT MINI GAME LOGIC HERE
            playerInteracted = true;
            if (CyberneticsManager.Instance.HasCyberneticCharge())
            {
                CyberneticsManager.Instance.UseCharge();
                fix();
                return;
            }
            OnMiniGameEvent.Raise();
        }
    }
    public void OnRobotBuddyInteract()
    {
        if (robotBuddyCanInteract && isBroken)
        {
            //IMPLEMENT MINI GAME LOGIC HERE
            playerInteracted = true;
            OnMiniGameEvent.Raise();
        }
    }
    public void fix()
    {
        if (playerInteracted)
        {
            SoundFXManager.Instance.PlaySound(SFX.Player.Work, this.gameObject.transform, 1f);
            isBroken = false;
            ResetText(currentExtractText);
            playerInteracted = false;
        }
    }
}
