
public enum PFTGameStateType 
{
    Debug = -1,
    Unassigned = 0,
    DrawPhase = 10,
    PlayerTurn = 101,
}

public enum PFTGameMessageType
{
    Debug = -1,
    Unassigned = 0,
}

public enum PFTCameraControlType
{
    Inverted = -1,
    Uninverted = 1
}

public enum PFTGameUIType
{ 
    LoadScene = 1,
    StartMenu = 10,
    Settings = 11,
    LogoIntro = 20,
    LogoAlternate = 21,
    DialogYesNo = 100,
    DialoguePopUpLeft = 110,
    DialoguePopUpRight = 111,
    DialogueBlockingLeft = 112,
    DialogueBlockingRight = 113,

    Card = 200,
    PlayerHand = 300,

    Invalid = 999
}

public enum EventType
{
    OnPlayerSpawn = 1,
    OnUnitSpawn = 5,
    HandUISpawn = 10,
    OnCardSelected = 20,
    OnCardUnselected = 21,
    DrawStart = 101,
    DrawDone = 109,
}
public enum PFTUiFadeType
{
    None = 0,
    In = 1,
    Out = 2,
}

public enum TileTerrainType 
{
    Debug = -1,
    Default = 0,
    Dirt = 10,
    Water = 20,
    Grass = 30
}