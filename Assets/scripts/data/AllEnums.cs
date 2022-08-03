
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

    Invalid = 999
}
public enum PFTUiFadeType
{
    None = 0,
    In = 1,
    Out = 2,
}