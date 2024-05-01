using System.Collections;
using TMPro;
using UnityEngine;
using BuildingComponents;
public class Collectable : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private ResourceToInventoryEvent resourceToInventory;
    [Header("Mutable")]
    [SerializeField] private TextMeshProUGUI reminderText;

    [Header("ReadOnly")]
    private bool isInRange;
    private int itemValue;
    private string collectableItem;

    // Not for display
    private BuildingComponents.ResourceType resourceToCollect;
    public static Collectable Instance {get;private set;}
    private KeyCode interactKey = KeyCode.E;
    private bool playerInteracted;
    public VoidEvent OnCaveMiniGameEvent;
    private CollectableResourceGen resourceGen = new();

    // -------------------------------------------------------------------
    // Class
    // Start is called before the first frame update
    // void Start()
    // {
    //     if (Instance != null && Instance != this)
    //     {
    //         Destroy(gameObject);
    //     }
    //     else
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    // }

    void Start()
    {
        collectableItem = this.gameObject.tag;

        switch (collectableItem)
        {
            case "Rock":
                switch(this.gameObject.GetComponent<SpriteRenderer>().sprite.name)
                {
                    case "objects-bg_0":
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.S_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                    case "objects-bg_5":
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.S_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                    case "objects-bg_1":
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.A_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                    case "objects-bg_6":
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.A_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                    case "objects-bg_2":
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.B_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                    case "objects-bg_7":
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.B_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                    case "objects-bg_3":
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.C_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                    case "objects-bg_":
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.C_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                    case "objects-bg_4":
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.D_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                    case "objects-bg_9":
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.D_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                    default:
                        resourceToCollect = resourceGen.GetRandomResource(AsteroidClass.D_Class);
                        itemValue = resourceGen.GenerateRandomAmount(resourceToCollect);
                        break;
                }
                break;
            case "GeologicalPhenomena":
                itemValue = 1;
                resourceToCollect = BuildingComponents.ResourceType.TechPoint;
                break;
            default:
                itemValue = 1;
                resourceToCollect = BuildingComponents.ResourceType.Iron;
                Debug.LogError("[Collectable.cs] -: Start() missed all cases, this is unaccetable");
                break;
        }
    }

    // Update is called once per frame
    public void OnGenericInteract()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            playerInteracted = true;

            if(BuildingTierManager.Instance.GetTierOf(BuildingComponents.BuildingType.Exosuit) >= 3){
                //dont trigger Mini Game
                OnCaveMiniGameWin();
                return;
            }
            OnCaveMiniGameEvent.Raise();
        }
    }

    // update text
    IEnumerator reminderTextCoroutine()
    {
        switch (collectableItem)
        {
            case "Rock":
                reminderText.text = "+" + itemValue.ToString() + " " + resourceToString(resourceToCollect) + "!";
                break;

            case "GeologicalPhenomena":
                reminderText.text = "+" + itemValue.ToString() + " Tech point";
                break;
        }

        // update the UI
        OnPlayerCollect();

        yield return new WaitForSeconds(1);

        reminderText.text = "";
        this.gameObject.SetActive(false);
    }

    // delete the object once it comes into contact with the player
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("RobotBuddy"))
        {
            isInRange = true;
            reminderText.text = "Collect Material";
        }
    }

    // reset text when player is out of range
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("RobotBuddy"))
        {
            isInRange = false;
            reminderText.text = "";
        }
    }

    // update the UI
    void OnPlayerCollect()
    {
        resourceToInventory.Raise(new packet.ResourceToInventory(this.gameObject, itemValue, resourceToCollect, true));
    }

    public void OnCaveMiniGameWin(){
        if(playerInteracted){
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.enabled = false;
            StartCoroutine(reminderTextCoroutine());
        }
    }

    public string resourceToString(ResourceType resource){
        switch (resource)
        {
            case ResourceType.Iron:
                return "Iron";
            case ResourceType.Nickel:
                return "Nickel";
            case ResourceType.Cobalt:
                return "Cobalt";
            case ResourceType.Platinum:
                return "Platinum";
            case ResourceType.Gold:
                return "Gold";
            case ResourceType.Technetium:
                return "Technetium";
            case ResourceType.Tungsten:
                return "Tungsten";
            case ResourceType.Iridium:
                return "Iridium";
            default:
                Debug.LogError("Collectable.cs --: resourceToString() :-- Failed to toString " + resource +" returning Iron");
                return "Iron";
        }
    }

}
