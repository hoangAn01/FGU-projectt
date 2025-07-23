using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Game/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName = "New Skill";
    public string description = "Skill Description";
    public Sprite skillIcon; // Thêm dòng này để lưu ảnh của skill

    [Header("Unlock Logic")]
    public string playerPrefsKey = "SkillUnlocked_DefaultKey";
    public int requiredScore = 100;

    public bool IsUnlocked()
    {
        return PlayerPrefs.GetInt(playerPrefsKey, 0) == 1;
    }

    public void Unlock()
    {
        PlayerPrefs.SetInt(playerPrefsKey, 1);
        PlayerPrefs.Save(); // Lưu ngay lập tức
        Debug.Log($"Kỹ năng '{skillName}' đã được mở khóa!");
    }
}