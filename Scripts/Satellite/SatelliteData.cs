[System.Serializable]

public class SatelliteData
{
    public string satelliteName;
    public bool isBuilt;

    public SatelliteData(string name, bool built)
    {
        satelliteName = name;
        isBuilt = built;
    }
}
