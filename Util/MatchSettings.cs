
public enum AtDeath { RemoveAllWeapons,RemoveLastWeapon};
[System.Serializable]
public class MatchSettings {

	public float respawnTime = 3f;
    public AtDeath atDeath;

}
