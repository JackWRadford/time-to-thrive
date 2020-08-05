using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[System.Serializable]
public class TerrainGen : MonoBehaviour
{
    public Tilemap tilemap;

    private GameObject player;

    [SerializeField]
    NoiseMapGeneration noiseMapGeneration;

    [SerializeField]
    private float mapScale = 0;

    //size of tiles made with perlin noise
    public static int chunkSize = 32;

    [SerializeField]
    private TerrainType[] heightTerrainTypes = null;

    [SerializeField]
    private TerrainType[] heatTerrainTypes = null;

    [SerializeField]
    private TerrainType[] moistureTerrainTypes = null;

    [SerializeField]
    private AnimationCurve moistureCurve = null;

    [SerializeField]
    private AnimationCurve heatCurve = null;

    [SerializeField]
    private Wave[] moistureWaves = null;

    [SerializeField]
    private Wave[] heatWaves = null;

    [SerializeField]
    private Wave[] heightWaves = null;

    [SerializeField]
    private Visualizationmode visualizationmode = Visualizationmode.Height;

    [SerializeField]
    private BiomeRow[] biomes = null;

    [SerializeField]
    private TreeGeneration treeGeneration;

    [SerializeField]
    private RiverGeneration riverGeneration;

    //[SerializeField]
    //private Tile waterTile = null;

    private LevelData levelData = null;

    //test render distance (player setting)
    [SerializeField]
    private int renderDistance = 3;


    void Awake()
    {
        noiseMapGeneration = this.GetComponent<NoiseMapGeneration>();
        treeGeneration = this.GetComponent<TreeGeneration>();
        riverGeneration = this.GetComponent<RiverGeneration>();

        GameEvents.SaveInitiated += Save;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //empty level data object to be filled as chunks are generated / updated
        levelData = new LevelData(chunkSize, chunkSize);

        Load();
    }

    //prepare and save level data
    void Save()
    {
        levelData.SaveChunkData();
    }

    void Load()
    {
        //load saved chunkData into matrix
        levelData.LoadChunkData(levelData);
    }

    void Update()
    {
        //check if should generate/load more chunks depending on players position and loaded chunks
        NeedNewChunk();
    }

    //method to get level data
    public LevelData GetLevelData()
    {
        return this.levelData;
    }

    //method to check if new chunk needs to be generated/ loaded
    private void NeedNewChunk()
    {
        Vector3Int playerPos = new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y, 0);

        //find coordinates of chunk that player is in
        // int chunkX = (int)(playerPos.x / TerrainGen.chunkSize);
        // int chunkY = (int)(playerPos.y / TerrainGen.chunkSize);
        int chunkX = GetChunkCoordsFromWorld(playerPos.x, playerPos.y)[0];
        int chunkY = GetChunkCoordsFromWorld(playerPos.x, playerPos.y)[1];;


        //Debug.Log("player in: " + chunkX + ", " + chunkY);

        //loop through all chunk coordinates that should be loaded dependant on player's position
        for (int i = chunkX - renderDistance; i < chunkX + (renderDistance + 1); i++)
        {
            for (int j = chunkY - renderDistance; j < chunkY + (renderDistance + 1); j++)
            {
                if(levelData.FindChunk(i,j) != null)
                {
                    //Debug.Log("found" + levelData.FindChunk(i,j).offsetX + "," + levelData.FindChunk(i,j).offsetZ);
                    //if chunk not rendered load/generate the relevent chunk
                    if(levelData.FindChunk(i,j).rendered != true)
                    {
                        //chunk exists, load it
                        LoadChunk(i,j);
                        //spawn chunk objects
                        SpawnSavedObjects(levelData.FindChunk(i,j));
                        //Debug.Log("load: " + i + ", " + j);
                    }else{
                        //Debug.Log("Chunk already rendered");
                    }
                    
                }else{
                    //chunk doesn't exist, generate it
                    GenerateChunk(i,j);
                    //Debug.Log("Generate:" + i + ":" + j);
                }
            }
        }
    }

    //method to get chunk coordinated from world coordinates
    public int[] GetChunkCoordsFromWorld(float x, float y)
    {
        int[] coords = new int[2];

        if(x < 0)
        {
            x-=1*TerrainGen.chunkSize;
        }
        if(y < 0)
        {
            y-=1*TerrainGen.chunkSize;
        }
        coords[0] = (int)(x / TerrainGen.chunkSize);
        coords[1] = (int)(y / TerrainGen.chunkSize);

        return coords;
    }

    //method to spawn saved objects in chunk
    public void SpawnSavedObjects(TileData tileData)
    {
        Dictionary<List<float>, List<dynamic>> objects = tileData.GetObjectGOs();
        //Debug.Log("loaded objects: " + objects.Count.ToString());
        foreach (var objPos in objects)
        {
            //loop through stacked objects at a position
            foreach (var obj in objPos.Value)
            {
                GameObject go = Resources.Load<GameObject>("Placeable/" + obj.GetTitle());
                //Debug.Log(obj.GetTitle().ToString());
                //spawn at position saved in data (including offset)
                go.GetComponent<StackDetails>().SetPlaceing(false);
                GameObject goi = Instantiate(go, new Vector3(obj.position[0], obj.position[1], 0), Quaternion.identity); //as GameObject;
                //goi.name = obj.GetTitle();
                goi.GetComponent<ILoadState>().LoadState(obj);
                //load state from save onto instantiated gameObject
            }
            //populate list of objects for chunk (GameObjects)
            //tileData.AddObject(obj.Key[0], obj.Key[1], obj.Value.GetTitle(), obj.Value);
        }
    }

    //method to generate new chunk at given coordinates
    private void GenerateChunk(int i, int j)
    {
        //print("generate chunk:" + i.ToString() + ", " + j.ToString());

        //generate tileData (chunkData) and generate tile (chunk)
        TileData tileData =  GenerateTile(i, j);
        levelData.AddTileData(tileData, i, j);
        //create chunkData and add to array of chunkData
        //ChunkData chunkData = new ChunkData(tileData);
        

        //generate trees for chunk
        treeGeneration.GenerateTrees(tileData);
        //TODO: save trees in tileData in levelData tileData array (and other enetities created)

        tileData.rendered = true;
    }

    //mehtod to load specified chunk from tilesData (levelData)
    private void LoadChunk(int i, int j)
    {
        //print("load chunk:" + i.ToString() + ", " + j.ToString());
        

        //find specified chunk in level data and render the chunk
        TileData td = levelData.FindChunk(i,j);
        if(td != null)
        {
            //print(td.offsetX.ToString() + " " + td.offsetZ.ToString());
            //CalculateBiomes(td.chosenHeightTerrainTypes, td.chosenHeatTerrainTypes, td.chosenMoistureTerrainTypes, td.offsetX * chunkSize, td.offsetZ * chunkSize, td.chosenBiomes);
            LoadBiomes(td.offsetX * chunkSize, td.offsetZ * chunkSize, td.chosenBiomes);
            //Debug.Log("load: " + i + ":" + j);
            //load trees (and other entities)

            td.rendered = true;
        }
    }

    private TileData GenerateTile(int oX, int oZ)
    {
        int tileDepth = chunkSize;
        int tileWidth = tileDepth;

        //offset passed into function
        float offsetX = oX * chunkSize;
        float offsetZ = oZ * chunkSize;

        //generate heightMap using Perlin Noise
        float[,] heightMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, this.heightWaves);

        //generate heatMap using Perlin Noise
        float[,] heatMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, this.heatWaves);
        //add height values to heat map values to make higher regions colder
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                heatMap[zIndex, xIndex] += this.heatCurve.Evaluate(heightMap[zIndex, xIndex]) * heightMap[zIndex, xIndex];
            }
        }

        //generate moistureMap using Perlin Noise
        float[,] moistureMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, this.moistureWaves);
        //subtract height values to moisture map values to make higher regions dryer
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                heatMap[zIndex, xIndex] -= this.moistureCurve.Evaluate(heightMap[zIndex, xIndex]) * heightMap[zIndex, xIndex];
            }
        }

        //calculate terrainTypes for each map
        TerrainType[,] chosenHeightTerrainTypes = new TerrainType[tileDepth, tileWidth];
        TerrainType[,] chosenHeatTerrainTypes = new TerrainType[tileDepth, tileWidth];
        TerrainType[,] chosenMoistureTerrainTypes = new TerrainType[tileDepth, tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                //height
                chosenHeightTerrainTypes[zIndex, xIndex] = GetTerrainTypeForParameter(heightMap[zIndex,xIndex], heightTerrainTypes);
                //heat
                chosenHeatTerrainTypes[zIndex, xIndex] = GetTerrainTypeForParameter(heatMap[zIndex,xIndex], heatTerrainTypes);
                //moisture
                chosenMoistureTerrainTypes[zIndex, xIndex] = GetTerrainTypeForParameter(moistureMap[zIndex,xIndex], moistureTerrainTypes);
            }
        }

        //build new chosenBiomes matrix for (chunk)
        Biome[,] chosenBiomes = new Biome[tileDepth, tileWidth];
        
        //render tile(s) depending on visualization mode specified
        switch (this.visualizationmode)
        {
            case Visualizationmode.Height:
            //render height map
            //RenderTiles(heightMap, (int)offsetX, (int)offsetZ);
            RenderTiles(chosenHeightTerrainTypes, (int)offsetX, (int)offsetZ);
            break;
            //render heat map
            case Visualizationmode.Heat:
            //RenderTiles(heatMap, (int)offsetX, (int)offsetZ);
            RenderTiles(chosenHeatTerrainTypes, (int)offsetX, (int)offsetZ);
            break;
            //render moisture map
            case Visualizationmode.Moisture:
            //RenderTiles(moistureMap, (int)offsetX, (int)offsetZ);
            RenderTiles(chosenMoistureTerrainTypes, (int)offsetX, (int)offsetZ);
            break;
            //render biomes map
            case Visualizationmode.Biome:
            //build biomes from heat, height and moisture maps
            //CalculateBiomes(heightMap, heatMap, moistureMap, (int)offsetX, (int)offsetZ, chosenBiomes);
            chosenBiomes = CalculateBiomes(chosenHeightTerrainTypes, chosenHeatTerrainTypes, chosenMoistureTerrainTypes, (int)offsetX, (int)offsetZ);
            break;

            default:
            //default to height map
            //RenderTiles(heightMap, (int)offsetX, (int)offsetZ);
            RenderTiles(chosenHeightTerrainTypes, (int)offsetX, (int)offsetZ);
            break;
        }
        
        // for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        // {
        //     for (int xIndex = 0; xIndex < tileWidth; xIndex++)
        //     {
        //         if(chosenBiomes[zIndex,xIndex] != null)
        //         {
        //             print(chosenBiomes[zIndex, xIndex].name.ToString());
        //         }else{
        //             print("water");
        //         }
        //     }
        // }

        Dictionary<List<float>, List<dynamic>> chunkObjects = new Dictionary<List<float>, List<dynamic>>();

        //build tileData for (chunk)
        TileData tileData = new TileData(heightMap, heatMap, moistureMap, chosenHeightTerrainTypes,
        chosenHeatTerrainTypes, chosenMoistureTerrainTypes, chosenBiomes, oX, oZ, chunkObjects);

        return tileData;
    }

    //method to render tiles from pre-calculated TerrainType matrix (more efficient)
    void RenderTiles(TerrainType[,] chosenTerrainTypes, int oX, int oZ)
    {
        int tileDepth = chosenTerrainTypes.GetLength(0);
        int tileWidth = chosenTerrainTypes.GetLength(1);

        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                //render tile from terraintype matrix
                //tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), chosenTerrainTypes[zIndex, xIndex].tile);
                tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), Resources.Load<Tile>("Tiles/" + chosenTerrainTypes[zIndex, xIndex].name));
            }
        }
    }

    //method to calculate correct Tile (height, heat, moisture)
    // void RenderTiles(float[,] selectedMap, int oX, int oZ)
    // {
    //     int tileDepth = selectedMap.GetLength(0);
    //     int tileWidth = selectedMap.GetLength(1);

    //     //Color[] colorMap = new Color[tileDepth * tileWidth];
    //     for (int zIndex = 0; zIndex < tileDepth; zIndex++)
    //     {
    //         for (int xIndex = 0; xIndex < tileWidth; xIndex++)
    //         {
    //             //int colorIndex = zIndex * tileWidth + xIndex;
    //             //parameter = height, heat, moisture
    //             float parameter = selectedMap[zIndex, xIndex];

    //             //get terrain type for given height and add it to the tilemap
    //             tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), GetTerrainTypeForParameter(parameter).tile);
                
    //         }
    //     }
    // }

    //method to return terrainType depending on the height, heat, moisture input
    // TerrainType GetTerrainTypeForParameter(float param)
    // {
    //     TerrainType[] terrainArray = null;
    //     //pick tile set to use depending on visualization input
    //     switch (this.visualizationmode)
    //     {
    //         case Visualizationmode.Height:
    //         // height map
    //         terrainArray = heightTerrainTypes;
    //         break;
    //         // heat map
    //         case Visualizationmode.Heat:
    //         terrainArray = heatTerrainTypes;
    //         break;
    //         // moisture map
    //         case Visualizationmode.Moisture:
    //         terrainArray = moistureTerrainTypes;
    //         break;

    //         default:
    //         //default to height map
    //         terrainArray = heightTerrainTypes;
    //         break;
    //     }

    //     foreach(TerrainType terrainType in terrainArray)
    //     {
    //         if(param < terrainType.threshold)
    //         {
    //             //correct height
    //             return terrainType;
    //         }
    //     }
    //     //if no terrainTypes apply return the last (highest) one (sometimes perlinNoise return > 1 (or < 0))
    //     return terrainArray[terrainArray.Length -1];
    // }

    TerrainType GetTerrainTypeForParameter(float param, TerrainType[] terrainTypes)
    {
        foreach(TerrainType terrainType in terrainTypes)
        {
            if(param < terrainType.threshold)
            {
                //correct height
                return terrainType;
            }
        }
        //if no terrainTypes apply return the last (highest) one (sometimes perlinNoise return > 1 (or < 0))
        return terrainTypes[terrainTypes.Length -1];
    }


    //method to build biome texture dependent on heat, moisture and height
    // private void CalculateBiomes(float[,] heightMap, float[,] heatMap, float[,] moistureMap, int oX, int oZ, Biome[,] chosenBiomes)
    // {
    //     int tileDepth = heightMap.GetLength(0);
    //     int tileWidth = heightMap.GetLength(1);

    //     for (int zIndex = 0; zIndex < tileDepth; zIndex++)
    //     {
    //         for (int xIndex = 0; xIndex < tileWidth; xIndex++)
    //         {
    //             //get height terrain type for point
    //             float heightValue = heightMap[zIndex, xIndex];
    //             TerrainType heightTerrainType = GetTerrainTypeForParameter(heightValue, heightTerrainTypes);
    //             //if water region render water tile (water not conform to biomes atm), else calculate correct biome Tile
    //             if(heightTerrainType.name != "water")
    //             {
    //                 //define biome by heat and moisture terrain types
    //                 float heatValue = heatMap[zIndex, xIndex];
    //                 TerrainType heatTerrainType = GetTerrainTypeForParameter(heatValue, heatTerrainTypes);
                    
    //                 float moistureValue = moistureMap[zIndex, xIndex];
    //                 TerrainType moistureTerrainType = GetTerrainTypeForParameter(moistureValue, moistureTerrainTypes);

    //                 //use biomes table to calculate correct biome for respective heat and moisture values
    //                 Biome biome = this.biomes [moistureTerrainType.index].biomes [heatTerrainType.index];

    //                 //save biome in chosenBiomes matrix when not water
    //                 chosenBiomes[zIndex, xIndex] = biome;
                    
    //                 tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), biome.tile);
    //             }
    //             else
    //             {
    //                 //render normal water Tile (height value)
    //                 tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), GetTerrainTypeForParameter(heightValue, heightTerrainTypes).tile);
    //             }
    //         }
    //     }
    // }

    //method to build biome texture dependent on heat, moisture and height
    private Biome[,] CalculateBiomes(TerrainType[,] chosenHeightTerrainTypes, TerrainType[,] chosenHeatTerrainTypes, TerrainType[,] chosenMoistureTerrainTypes, int oX, int oZ)
    {
        int tileDepth = chosenHeightTerrainTypes.GetLength(0);
        int tileWidth = chosenHeightTerrainTypes.GetLength(1);

        Biome[,] chosenBiomes = new Biome[tileDepth,tileWidth];

        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                //if water region render water tile (water not conform to biomes atm), else calculate correct biome Tile
                if(chosenHeightTerrainTypes[zIndex,xIndex].name != "water")
                {
                    //use biomes table to calculate correct biome for respective heat and moisture values
                    Biome biome = this.biomes [chosenMoistureTerrainTypes[zIndex,xIndex].index].biomes [chosenHeatTerrainTypes[zIndex,xIndex].index];

                    //save biome in chosenBiomes matrix when not water
                    //print(biome.name.ToString() + " : " + zIndex.ToString() + " : " + xIndex.ToString());
                    chosenBiomes[zIndex, xIndex] = biome;
                    
                    // tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), biome.tile);
                    tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), Resources.Load<Tile>("Tiles/" + biome.name));
                }
                else
                {
                    //render normal water Tile (height value)
                    // tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), chosenHeightTerrainTypes[zIndex,xIndex].tile);
                    tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), Resources.Load<Tile>("Tiles/" + "water"));
                }
            }
        }
        return chosenBiomes;
    }

    //method to render saved biomes for a given chunk
    private void LoadBiomes(int oX, int oZ, Biome[,] chosenBiomes)
    {
        int tileDepth = chosenBiomes.GetLength(0);
        int tileWidth = chosenBiomes.GetLength(1);

        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                //if water region render water tile (water not conform to biomes atm), else calculate correct biome Tile
                if(chosenBiomes[zIndex, xIndex] != null)
                {
                    //tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0),chosenBiomes[zIndex, xIndex].tile);
                    //print(chosenBiomes[zIndex, xIndex].name);
                    tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0),Resources.Load<Tile>("Tiles/" + chosenBiomes[zIndex, xIndex].name));
                }
                else
                {
                    //render normal water Tile 
                    tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), Resources.Load<Tile>("Tiles/" + "water"));
                }
            }
        }
    }

    // private Tile GetTileFromName(string name)
    // {
    //     Tile tile = null;

    //     switch (name)
    //     {
    //         case "desert":
    //         tile = 

    //         default:
    //         //water

    //         break;
    //     }

    //     return tile;
    // }
}

[System.Serializable]
public class TerrainType
{
    public string name;
    //height, heat, moisture
    public float threshold;
    //public Tile tile;
    public int index;
}

[System.Serializable]
public class Wave
{
    public float seed;
    public float amplitude;
    public float frequency;
}

//enum visualization mode for height, heat, moisture maps
enum Visualizationmode {Height, Heat, Moisture, Biome}

//class for biomes
[System.Serializable]
public class Biome
{
    public string name;
    //public Tile tile;
    public int index;
}


//class for rows of biomes
[System.Serializable]
public class BiomeRow
{
     public Biome[] biomes;
}

//class to store a (chunk's) data
[System.Serializable]
public class TileData
{
    //terrain data in chunk
    public float[,] heightMap;
    public float[,] heatMap;
    public float[,] moistureMap;
    public TerrainType[,] chosenHeightTerrainTypes;
    public TerrainType[,] chosenHeatTerrainTypes;
    public TerrainType[,] chosenMoistureTerrainTypes;
    public Biome[,] chosenBiomes;
    public int offsetX;
    public int offsetZ;

    [System.NonSerialized]
    public bool rendered = false;

    //object data in chunk
    public Dictionary<List<float>, List<dynamic>> objectsGO;
    
    //gameObjects in chunk
    [System.NonSerialized]
    public Dictionary<List<float>, List<GameObject>> gObjects = new Dictionary<List<float>, List<GameObject>>();


    public TileData(float[,] heightMap, float[,] heatMap, float[,] moistureMap,
    TerrainType[,] chosenHeightTerrainTypes, TerrainType[,] chosenHeatTerrainTypes, TerrainType[,] chosenMoistureTerrainTypes,
    Biome[,] chosenBiomes, int oX, int oZ, Dictionary<List<float>, List<dynamic>> chunkObjects)
    {
        this.heightMap = heightMap;
        this.heatMap = heatMap;
        this.moistureMap = moistureMap;
        this.chosenHeightTerrainTypes = chosenHeightTerrainTypes;
        this.chosenHeatTerrainTypes = chosenHeatTerrainTypes;
        this.chosenMoistureTerrainTypes = chosenMoistureTerrainTypes;
        this.chosenBiomes = chosenBiomes;
        this.offsetX = oX;
        this.offsetZ = oZ;
        this.objectsGO = chunkObjects;
    }

    //method to get objects in chunk
    public Dictionary<List<float>, List<dynamic>> GetObjectGOs()
    {
        return this.objectsGO;
    }

    //method to add object to chunk 
    public void AddObjectData(float x, float y, string title, dynamic data)
    {
        List<float> pos = new List<float>{x+0.5f,y+0.5f};

        //add object data and position to matrix to be saved
        foreach (var objPos in objectsGO.Keys)
        {
            if(objPos.SequenceEqual(pos))
            {
                //Debug.Log("NOT NULL");
                //if list of objects already exists for specified position
                //add new data to current list
                objectsGO[objPos].Add(data);
                //gObjects[objPos].Add(go);
                return;
            }
        }    
        //no objects saved in that location
        //Debug.Log("NULL");
        //if no list of objects for specified position
        //create new list of data for specified position and add new data
        List<dynamic> dataList = new List<dynamic>();
        dataList.Add(data);
        objectsGO.Add(pos, dataList);

        // List<GameObject> goList = new List<GameObject>();
        // goList.Add(go);
        // gObjects.Add(pos, goList);
    }

    //method to add object to chunk (just add GameObject not data)
    public void AddObjectGO(float x, float y, string title, GameObject go)
    {
        List<float> pos = new List<float>{x+0.5f,y+0.5f};

        //add object data and position to matrix to be saved
        foreach (var objPos in gObjects.Keys)
        {
            if(objPos.SequenceEqual(pos))
            {
                //Debug.Log("NOT NULL");
                //if list of objects already exists for specified position
                //add new data to current list
                gObjects[objPos].Add(go);
                return;
            }
        }    
        //no objects saved in that location
        //Debug.Log("NULL");
        //if no list of objects for specified position
        //create new list of data for specified position and add new data
        List<GameObject> goList = new List<GameObject>();
        goList.Add(go);
        gObjects.Add(pos, goList);
    }

    //method to remove object from chunk
    public void RemoveObject(float x, float y)
    {
        //Debug.Log("Remove item From:" + this.offsetX + ", " + this.offsetZ);
        List<float> pos = new List<float>{x+0.5f,y+0.5f};
        foreach (var objPos in objectsGO.Keys)
        {
            //Debug.Log(objPos[0].ToString() + "," + objPos[1].ToString() + ":" + pos[0].ToString() + "," + pos[1].ToString());
            if(objPos.SequenceEqual(pos))
            {
                //Debug.Log("remove");
                //Debug.Log("remove from dictionary:" + objectsGO[objPos]);
                //remove whole list of data for specified position
                //Debug.Log("remove: " + objPos.ToString());
                objectsGO.Remove(objPos);
                return;
            }
        }
    }
    //method to remove GO list and destroy ones on top
    public void RemoveObjectGO(float x, float y)
    {
        //Debug.Log("Remove item From:" + this.offsetX + ", " + this.offsetZ);
        List<float> pos = new List<float>{x+0.5f,y+0.5f};
        foreach (var objPos in gObjects.Keys)
        {
            //Debug.Log(objPos[0].ToString() + "," + objPos[1].ToString() + ":" + pos[0].ToString() + "," + pos[1].ToString());
            if(objPos.SequenceEqual(pos))
            {
                //Debug.Log("remove");
                //Debug.Log("remove from dictionary:" + objectsGO[objPos]);
                //remove whole list of data for specified position
                //Debug.Log("remove: " + objPos.ToString());

                for (int i = gObjects[objPos].Count - 1; i >= 0; i--)
                {
                    //Debug.Log("remove");
                    gObjects[objPos][i].GetComponent<StackDetails>().DestroyGO();
                }

                gObjects.Remove(objPos);
                return;
            }
        }
    }

    //method to check if space is free in chunk
    public bool IsSpaceFree(float x, float y, GameObject obj)
    {
        List<float> pos = new List<float>{x+0.5f,y+0.5f};
        foreach (var objPos in objectsGO.Keys)
        {
            if(objPos.SequenceEqual(pos))
            {
                // //if heldItem is external construction check for foundation (if no foundation return false)
                // if((obj.GetComponent<StackDetails>().isExternalConstruction)&&(!obj.GetComponent<StackDetails>().isFoundation))
                // {
                //     //check for foundation
                    
                // }

                //check if last obj in list canBeUnder and this obj canBeOver (inheritance is better?)
                if((objectsGO[objPos][objectsGO[objPos].Count-1].canBeUnder)&&(obj.GetComponent<StackDetails>().canBeOver))
                {
                    //object is a wall make sure not over max height of 2
                    if((obj.GetComponent<StackDetails>().isWall)&&(objectsGO[objPos].Count >= 2))
                    {
                        return false;
                    }

                    //object can be stacked
                    return true;
                }

                return false;
            }
        }
        //Debug.Log("space free");
        //there is no specified list of data for specified position
        return true;
    }

    //method to check if space is free in chunk (GameObject list)
    public bool IsGOSpaceFree(float x, float y, GameObject obj)
    {
        List<float> pos = new List<float>{x+0.5f,y+0.5f};
        foreach (var objPos in gObjects.Keys)
        {
            if(objPos.SequenceEqual(pos))
            {
                //if heldItem is external construction check for foundation (if no foundation return false)
                if((obj.GetComponent<StackDetails>().isExternalConstruction)&&(!obj.GetComponent<StackDetails>().isFoundation))
                {
                    //check for foundation in first position in list for specified position
                    if(gObjects[objPos][0].GetComponent<StackDetails>().isFoundation)
                    {
                        //check if object to be placed is a roof
                        if(obj.GetComponent<StackDetails>().isRoof)
                        {
                            //check there is not already a roof
                            foreach (var placedObj in gObjects[objPos])
                            {
                                if(placedObj.GetComponent<StackDetails>().isRoof)
                                {
                                    return false;
                                }
                            }
                            return true;

                            //check there are two walls in the same orientation (below)
                            // int r0c = 0;
                            // int r1c = 0;
                            // int r2c = 0;
                            // int r3c = 0;
                            // foreach (var placedObj in gObjects[objPos])
                            // {
                            //     if(placedObj.GetComponent<StackDetails>().isWall)
                            //     {
                            //         //save number of walls for each orientation and check for 2 of any orientation
                            //         switch(placedObj.GetComponent<WallController>().orientation)
                            //         {
                            //             case 0:
                            //                 r0c++;
                            //                 break;
                            //             case 1:
                            //                 r1c++;
                            //                 break;
                            //             case 2:
                            //                 r2c++;
                            //                 break;
                            //             case 3:
                            //                 r3c++;
                            //                 break;
                            //             default:
                            //                 break;
                            //         }
                            //     }
                            //     if((r0c >=2)||(r1c >=2)||(r2c >=2)||(r3c >=2))
                            //     {
                            //         return true;
                            //     }
                            // }

                            //check if there is a roof in an adjacent position (NESW)
                            // List<float> offsetN = new List<float>{objPos[0]+0,objPos[1]+1};
                            // List<float> offsetE = new List<float>{objPos[0]+1,objPos[1]+0};
                            // List<float> offsetS = new List<float>{objPos[0]+0,objPos[1]-1};
                            // List<float> offsetW = new List<float>{objPos[0]-1,objPos[1]+0};
                            // // Debug.Log("Adjacent position N: " + offsetN[0] + "," + offsetN[1]);
                            // // Debug.Log("Adjacent position E: " + offsetE[0] + "," + offsetE[1]);
                            // // Debug.Log("Adjacent position S: " + offsetS[0] + "," + offsetS[1]);
                            // // Debug.Log("Adjacent position W: " + offsetW[0] + "," + offsetW[1]);

                            // //do sequence matching for each (if exists check for adjacent roof)
                            // foreach (var adjObjPos in gObjects.Keys)
                            // {
                            //     //Debug.Log(adjObjPos[0].ToString() + "," + adjObjPos[1].ToString());
                            //     if((adjObjPos.SequenceEqual(offsetN))||(adjObjPos.SequenceEqual(offsetE))||(adjObjPos.SequenceEqual(offsetS))
                            //     ||(adjObjPos.SequenceEqual(offsetW)))
                            //     {
                            //         //Debug.Log("Found adjacent GOs data");
                            //         //found adjacent list of GOs, check for roof
                            //         foreach (var adjacentObj in gObjects[adjObjPos])
                            //         {
                            //             if(adjacentObj.GetComponent<StackDetails>().isRoof)
                            //             {
                            //                 return true;
                            //             }
                            //         }
                            //     }
                            // }


                            // foreach (var adjacentObj in gObjects[offsetN])
                            // {
                            //     if(adjacentObj.GetComponent<StackDetails>().isRoof)
                            //     {
                            //         return true;
                            //     }
                            // }
                            // foreach (var adjacentObj in gObjects[offsetE])
                            // {
                            //     if(adjacentObj.GetComponent<StackDetails>().isRoof)
                            //     {
                            //         return true;
                            //     }
                            // }
                            // foreach (var adjacentObj in gObjects[offsetS])
                            // {
                            //     if(adjacentObj.GetComponent<StackDetails>().isRoof)
                            //     {
                            //         return true;
                            //     }
                            // }
                            // foreach (var adjacentObj in gObjects[offsetW])
                            // {
                            //     if(adjacentObj.GetComponent<StackDetails>().isRoof)
                            //     {
                            //         return true;
                            //     }
                            // }
                        }


                        //check if object to be placed is a wall
                        if(obj.GetComponent<StackDetails>().isWall)
                        {
                            // REMOVE???
                            //if((gObjects[objPos][gObjects[objPos].Count-1].GetComponent<StackDetails>().canBeUnder)&&(obj.GetComponent<StackDetails>().canBeOver))// REMOVE???
                            //{
                                //check either no walls or wall that is same orientation as one being placed
                                // if(gObjects[objPos].Count > 2)
                                // {
                                //     return false;
                                // }
                                if(gObjects[objPos].Count == 1)
                                {
                                    //just foundation
                                    return true;
                                }
                                //if there is a wall (on foundation)
                                //else if(gObjects[objPos][1].GetComponent<StackDetails>().isWall)
                                //{
                                    //check not more than two of the oreientation being placed
                                    int c = 0;
                                    foreach (var placedObj in gObjects[objPos])
                                    {
                                        if(placedObj.GetComponent<StackDetails>().isWall)
                                        {
                                            if(placedObj.GetComponent<WallController>().orientation == obj.GetComponent<WallController>().orientation)
                                            {
                                                //found wall of same orientation being placed (increment)
                                                c++;
                                            }
                                        }
                                        if(c >=2)
                                        {
                                            return false;
                                        }

                                        //if door in same orientation (false)
                                        if(placedObj.GetComponent<StackDetails>().isDoor)
                                        {
                                            if(placedObj.GetComponent<WallController>().orientation == obj.GetComponent<WallController>().orientation)
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    return true;
                                //}
                                //else
                                //{
                                    //return false;
                                //}
                            //}
                        }

                        //check if object to be placed is a door
                        if(obj.GetComponent<StackDetails>().isDoor)
                        {
                            // if(gObjects[objPos].Count == 1)
                            // {
                            //     //just foundation
                            //     return true;
                            // }
                            //if there are any walls in same orinetation then can not place
                            foreach (var placedObj in gObjects[objPos])
                            {
                                if((placedObj.GetComponent<StackDetails>().isWall)||(placedObj.GetComponent<StackDetails>().isDoor))
                                {
                                    if(placedObj.GetComponent<WallController>().orientation == obj.GetComponent<WallController>().orientation)
                                    {
                                        //found wall of same orientation being placed
                                        return false;
                                    }
                                }
                            }
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                //check if last obj in list canBeUnder and this obj canBeOver (inheritance is better?)
                // if((gObjects[objPos][gObjects[objPos].Count-1].GetComponent<StackDetails>().canBeUnder)&&(obj.GetComponent<StackDetails>().canBeOver))
                // {
                //     if((obj.GetComponent<StackDetails>().isWall)&&(gObjects[objPos].Count >= 2))
                //     {
                //         return false;
                //     }
                //     //object can be stacked
                //     return true;
                // }
                //return false;
            }
        }
        if(obj != null)
        {
            if((obj.GetComponent<StackDetails>().isExternalConstruction)&&(!obj.GetComponent<StackDetails>().isFoundation))
            {
                //Debug.Log("no foundation");
                return false;
            }
        }
        //there is no specified list of data for specified position
        return true;
    }

    //method to check if object is to be placed above another
    public bool IsAboveAnother(float x, float y, GameObject obj)
    {
        List<float> pos = new List<float>{x+0.5f,y+0.5f};
        foreach (var objPos in objectsGO.Keys)
        {
            if(objPos.SequenceEqual(pos))
            {
                //Debug.Log("space filled");
                //there is a list of data for specified position
                //check if last obj in list canBeUnder and this obj canBeOver (inheritance is better?)
                if((objectsGO[objPos][objectsGO[objPos].Count-1].canBeUnder)&&(obj.GetComponent<StackDetails>().canBeOver)) // REMOVE???
                {
                    //object can be stacked
                    return true;
                }

                return false;
            }
        }
        return false;
    }

    //method to check if object is to be placed above another (GOs)
    public bool IsGOAboveAnother(float x, float y, GameObject obj)
    {
        List<float> pos = new List<float>{x+0.5f,y+0.5f};
        foreach (var objPos in gObjects.Keys)
        {
            if(objPos.SequenceEqual(pos))
            {
                //Debug.Log("space filled");
                //there is a list of data for specified position
                //check if last obj in list canBeUnder and this obj canBeOver (inheritance is better?)
                // if((gObjects[objPos][gObjects[objPos].Count-1].GetComponent<StackDetails>().canBeUnder)&&(obj.GetComponent<StackDetails>().canBeOver))
                // {
                //     //check if there is a foundation and no other walls (of the same orientation)
                //     if((!gObjects[objPos][0].GetComponent<StackDetails>().isFoundation)&&(gObjects[objPos].Count != 1))
                //     {
                //         return true;
                //     }
                //     else if((gObjects[objPos][0].GetComponent<StackDetails>().isFoundation)&&(gObjects[objPos].Count > 1))
                //     {
                //         return true;
                //     }
                // }

                //if roof being placed it will always be above
                if(obj.GetComponent<StackDetails>().isRoof)
                {
                    return true;
                }

                //check that item to be placed can be on top
                if(obj.GetComponent<StackDetails>().canBeOver)
                {
                   foreach (var placedObj in gObjects[objPos])
                    {
                        if(placedObj.GetComponent<StackDetails>().isWall)
                        {
                            if(placedObj.GetComponent<WallController>().orientation == obj.GetComponent<WallController>().orientation)
                            {
                                //found wall of same orientation being placed (place above)
                                return true;
                            }
                        }
                    } 
                }

                return false;
            }
        }
        return false;
    }

    //method to get world coords for chunk
    public int[] GetWorldCoordsForChunk()
    {
        int[] coords = new int[2];
        coords[0] = offsetX*TerrainGen.chunkSize;
        coords[1] = offsetZ*TerrainGen.chunkSize;

        return coords;
    }
}

//class to store all the merged tiles data
public class LevelData
{
    private int tileDepthInVertices;
    private int tileWidthInVertices;

    // public TileData[,] positiveTilesData;
    // public TileData[,] negativeIJTilesData;
    // public TileData[,] negativeITilesData;
    // public TileData[,] negativeJTilesData;

    public List<TileData> positiveTilesData;
    public List<TileData> negativeIJTilesData;
    public List<TileData> negativeITilesData;
    public List<TileData> negativeJTilesData;

    //to save chunkData
    // public ChunkData[,] positiveChunkData;
    // public ChunkData[,] negativeIJChunkData;
    // public ChunkData[,] negativeIChunkData;
    // public ChunkData[,] negativeJChunkData;

    //private Dictionary<string, ChunkData[,]> chunkDataMatricies;

    //private int chunkArraySize = 1000; //extend when player walks furthur
    

    public LevelData(int tileDepthInVertices, int tileWidthInVertices)
    {
    // positiveTilesData = new TileData[chunkArraySize,chunkArraySize];
    // negativeIJTilesData = new TileData[chunkArraySize,chunkArraySize];
    // negativeITilesData = new TileData[chunkArraySize,chunkArraySize];
    // negativeJTilesData = new TileData[chunkArraySize,chunkArraySize];
    positiveTilesData = new List<TileData>();
    negativeIJTilesData = new List<TileData>();
    negativeITilesData = new List<TileData>();
    negativeJTilesData = new List<TileData>();

    //for saving chunk data
    // positiveChunkData = new ChunkData[200,200];
    // negativeIJChunkData = new ChunkData[200,200];
    // negativeIChunkData = new ChunkData[200,200];
    // negativeJChunkData = new ChunkData[200,200];

    this.tileDepthInVertices = tileDepthInVertices;
    this.tileWidthInVertices = tileWidthInVertices;
    }

    public void SaveChunkData()
    {
        //prepare data to be saved
        // chunkDataMatricies = new Dictionary<string, ChunkData[,]>();

        // chunkDataMatricies.Add("positive", positiveChunkData);
        // chunkDataMatricies.Add("negativeIJ", negativeIJChunkData);
        // chunkDataMatricies.Add("negativeI", negativeIChunkData);
        // chunkDataMatricies.Add("negativeJ", negativeJChunkData);

        // SaveSystem.Save<Dictionary<string, ChunkData[,]>>(chunkDataMatricies, "Chunks");

        // Dictionary<string, TileData[,]> tileDataMatricies = new Dictionary<string, TileData[,]>();
        Dictionary<string, List<TileData>> tileDataMatricies = new Dictionary<string, List<TileData>>();

        tileDataMatricies.Add("positive", positiveTilesData);
        tileDataMatricies.Add("negativeIJ", negativeIJTilesData);
        tileDataMatricies.Add("negativeI", negativeITilesData);
        tileDataMatricies.Add("negativeJ", negativeJTilesData);

        //Debug.Log("Saving objects: " + positiveTilesData[0,0].GetObjectGOs().Count.ToString());

        // SaveSystem.Save<Dictionary<string, TileData[,]>>(tileDataMatricies, "Chunks");
        SaveSystem.Save<Dictionary<string, List<TileData>>>(tileDataMatricies, "Chunks");


        Debug.Log("saved chunks");
    }

    public void LoadChunkData(LevelData levelData)
    {
        if(SaveSystem.SaveExists("Chunks"))
        {
            Debug.Log("Chunks Save Exists");
            //unload saved tileData into 4 tilesData matricies
            // Dictionary<string, TileData[,]> tileDataMatricies = SaveSystem.Load<Dictionary<string, TileData[,]>>("Chunks");
            Dictionary<string, List<TileData>> tileDataMatricies = SaveSystem.Load<Dictionary<string, List<TileData>>>("Chunks");
            //this.positiveTilesData = tileDataMatricies["positive"];
            foreach (TileData d in tileDataMatricies["positive"])
            {
                if(d != null)
                {
                    //Debug.Log("loaded:" + d.offsetX + "," + d.offsetZ);
                    TileData td = new TileData(d.heightMap, d.heatMap, d.moistureMap, d.chosenHeightTerrainTypes,
                    d.chosenHeatTerrainTypes, d.chosenMoistureTerrainTypes, d.chosenBiomes, d.offsetX, d.offsetZ, d.objectsGO);
                    levelData.AddTileData(td, td.offsetX, td.offsetZ);
                } 
            }
            //this.negativeIJTilesData = tileDataMatricies["negativeIJ"];
            foreach (TileData d in tileDataMatricies["negativeIJ"])
            {
                if(d != null)
                {
                    //Debug.Log("loaded:" + d.offsetX + "," + d.offsetZ);
                    TileData td = new TileData(d.heightMap, d.heatMap, d.moistureMap, d.chosenHeightTerrainTypes,
                    d.chosenHeatTerrainTypes, d.chosenMoistureTerrainTypes, d.chosenBiomes, d.offsetX, d.offsetZ, d.objectsGO);
                    levelData.AddTileData(td, td.offsetX, td.offsetZ);
                } 
            }
            //this.negativeITilesData = tileDataMatricies["negativeI"];
            foreach (TileData d in tileDataMatricies["negativeI"])
            {
                if(d != null)
                {
                    //Debug.Log("loaded:" + d.offsetX + "," + d.offsetZ);
                    TileData td = new TileData(d.heightMap, d.heatMap, d.moistureMap, d.chosenHeightTerrainTypes,
                    d.chosenHeatTerrainTypes, d.chosenMoistureTerrainTypes, d.chosenBiomes, d.offsetX, d.offsetZ, d.objectsGO);
                    levelData.AddTileData(td, td.offsetX, td.offsetZ);
                } 
            }
            //this.negativeJTilesData = tileDataMatricies["negativeJ"];
            foreach (TileData d in tileDataMatricies["negativeJ"])
            {
                if(d != null)
                {
                    //Debug.Log("loaded:" + d.offsetX + "," + d.offsetZ);
                    TileData td = new TileData(d.heightMap, d.heatMap, d.moistureMap, d.chosenHeightTerrainTypes,
                    d.chosenHeatTerrainTypes, d.chosenMoistureTerrainTypes, d.chosenBiomes, d.offsetX, d.offsetZ, d.objectsGO);
                    levelData.AddTileData(td, td.offsetX, td.offsetZ);
                } 
            }
        }
        else
        {
            Debug.Log("Chunks Save Doesn't Exist");
        }
    }

     //method to convert coordinates
    //  public TileCoordinate ConvertToTileCoordinate(int zIndex, int xIndex)
    //  {
    //     int tileZIndex = (int)Mathf.Floor((float)zIndex/(float)this.tileDepthInVertices);
    //     int tileXIndex = (int)Mathf.Floor((float)xIndex/(float)this.tileWidthInVertices);

    //     int coordinateZIndex = (zIndex % this.tileDepthInVertices);
    //     int coordinateXIndex = (xIndex % this.tileWidthInVertices);

    //     TileCoordinate tileCoordinate = new TileCoordinate(tileZIndex, tileXIndex, coordinateZIndex, coordinateXIndex);
    //     return tileCoordinate;
    //  }

    public void AddTileData(TileData tileData, int tileXIndex, int tileZIndex)
    {
        if(tileZIndex < 0 && tileXIndex < 0)
        {
            //negativeIJTilesData[System.Math.Abs(tileXIndex), System.Math.Abs(tileZIndex)] = tileData;
            //Debug.Log("add" + tileData.offsetX + ", " + tileData.offsetZ);
            negativeIJTilesData.Add(tileData);

            //ChunkData chunkData = new ChunkData(tileData);
            //negativeIJChunkData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)] = chunkData;
        }
        else if(tileZIndex < 0 && tileXIndex >= 0)
        {
            //negativeITilesData[System.Math.Abs(tileXIndex), System.Math.Abs(tileZIndex)] = tileData;
            //Debug.Log("add" + tileData.offsetX + ", " + tileData.offsetZ);
            negativeITilesData.Add(tileData);

            //ChunkData chunkData = new ChunkData(tileData);
            //negativeIChunkData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)] = chunkData;
        }
        else if(tileZIndex >= 0 && tileXIndex < 0)
        {
            //negativeJTilesData[System.Math.Abs(tileXIndex), System.Math.Abs(tileZIndex)] = tileData;
            //Debug.Log("add" + tileData.offsetX + ", " + tileData.offsetZ);
            negativeJTilesData.Add(tileData);

            //ChunkData chunkData = new ChunkData(tileData);
            //negativeJChunkData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)] = chunkData;
        }
        else
        {
            //positiveTilesData[System.Math.Abs(tileXIndex), System.Math.Abs(tileZIndex)] = tileData;
            //Debug.Log("add" + tileData.offsetX + ", " + tileData.offsetZ);
            positiveTilesData.Add(tileData);

            //ChunkData chunkData = new ChunkData(tileData);
            //positiveChunkData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)] = chunkData;
        }
    }

     //method to find chunk from correct data structure depending on coordinates
    public TileData FindChunk(int tileXIndex, int tileZIndex)
    {
        TileData tileData = null;

        if(tileZIndex < 0 && tileXIndex < 0)
        {
            //tileData = negativeIJTilesData[System.Math.Abs(tileXIndex), System.Math.Abs(tileZIndex)];
            foreach (TileData td in negativeIJTilesData)
            {
                if(td != null)
                {
                    //Debug.Log(td.offsetX + ":" + tileXIndex + ", " + td.offsetZ + ":" + tileZIndex);
                    if((td.offsetX == tileXIndex)&&(td.offsetZ == tileZIndex))
                    {
                        tileData =  td;
                    }
                }
            }
            

            //Debug.Log("found" + tileData.offsetX + "," + tileData.offsetZ);
        }
        else if(tileZIndex < 0 && tileXIndex >= 0)
        {
            //tileData = negativeITilesData[System.Math.Abs(tileXIndex), System.Math.Abs(tileZIndex)];
            foreach (TileData td in negativeITilesData)
            {
                if(td != null)
                {
                    //Debug.Log(td.offsetX + ":" + tileXIndex + ", " + td.offsetZ + ":" + tileZIndex);
                    if((td.offsetX == tileXIndex)&&(td.offsetZ == tileZIndex))
                    {
                        tileData = td;
                    }
                }
            }

            //Debug.Log("found" + tileData.offsetX + "," + tileData.offsetZ);
        }
        else if(tileZIndex >= 0 && tileXIndex < 0)
        {
            //tileData = negativeJTilesData[System.Math.Abs(tileXIndex), System.Math.Abs(tileZIndex)];
            foreach (TileData td in negativeJTilesData)
            {
                if(td != null)
                {
                    //Debug.Log(td.offsetX + ":" + tileXIndex + ", " + td.offsetZ + ":" + tileZIndex);
                    if((td.offsetX == tileXIndex)&&(td.offsetZ == tileZIndex))
                    {
                        tileData = td;
                    }
                }
            }

            //Debug.Log("found" + tileData.offsetX + "," + tileData.offsetZ);
        }
        else
        {
            //tileData = positiveTilesData[System.Math.Abs(tileXIndex), System.Math.Abs(tileZIndex)];
            foreach (TileData td in positiveTilesData)
            {
                if(td != null)
                {
                    //Debug.Log(td.offsetX + ":" + tileXIndex + ", " + td.offsetZ + ":" + tileZIndex);
                    if((td.offsetX == tileXIndex)&&(td.offsetZ == tileZIndex))
                    {
                        tileData = td;
                    }
                }
            }

            //Debug.Log("found" + tileData.offsetX + "," + tileData.offsetZ);
        }
        // Debug.Log("found" + tileData.offsetX + "," + tileData.offsetZ);
        return tileData;
    }
}

//class to represent a coordinate in the Tile coordinate system
// public class TileCoordinate
// {
//     public int tileZIndex;
//     public int tileXIndex;
//     public int coordinateZIndex;
//     public int coordinateXIndex;

//     public TileCoordinate(int tileZIndex, int tileXIndex, int coordinateZIndex, int coordinateXIndex)
//     {
//         this.tileZIndex = tileZIndex;
//         this.tileXIndex = tileXIndex;
//         this.coordinateZIndex = coordinateZIndex;
//         this.coordinateXIndex = coordinateXIndex;
//     }
// }