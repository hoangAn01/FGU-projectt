using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillMenuUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI skillStatusText;
    public Image skillIconImage;

    [Header("Skill Data")]
    public SkillData fireballSkill; // Kéo ScriptableObject của skill vào đây

    [Header("Visuals")]
    public Color lockedColor = new Color(0.3f, 0.3f, 0.3f, 0.8f); // Màu khi bị khóa (xám tối)
    public Color unlockedColor = Color.white; // Màu khi đã mở khóa (sáng bình thường)

    // Hàm này được gọi mỗi khi panel kỹ năng được bật lên
    void OnEnable()
    {
        UpdateSkillStatus();
    }

    void UpdateSkillStatus()
    {
        if (skillStatusText == null || fireballSkill == null || skillIconImage == null)
        {
            Debug.LogError("Chưa gán Text, Image hoặc SkillData cho SkillMenuUI!");
            return;
        }

        // Cập nhật ảnh và màu sắc của nó
        skillIconImage.sprite = fireballSkill.skillIcon;

        // Kiểm tra xem kỹ năng đã được mở khóa chưa từ ScriptableObject
        if (fireballSkill.IsUnlocked())
        {
            skillStatusText.text = $"{fireballSkill.skillName} (U): Đã mở khóa";
            skillIconImage.color = unlockedColor; // Hiển thị ảnh với màu bình thường
        }
        else
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            skillStatusText.text = $"{fireballSkill.skillName} (U): Yêu cầu {fireballSkill.requiredScore} điểm\n(Điểm cao nhất: {highScore})";
            skillIconImage.color = lockedColor; // Làm cho ảnh bị mờ đi
        }
    }
}