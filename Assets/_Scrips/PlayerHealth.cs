﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // Thêm dòng này

public class PlayerHealth : MonoBehaviour
{
	public float maxHp = 100f, currentHp;
	public ThanhHp thanhHp; // Tham chiếu đến script và thanh HP UI
	public Animator animator;
	public int score = 0;
	public TextMeshProUGUI scoreText; // Thêm biến này

	AudioManager audioManager;

	[Header("Skill Unlocks")]
	public SkillData fireballSkillToUnlock; // Kéo ScriptableObject của skill vào đây

	private void Awake()
	{
		// Tìm AudioManager trong scene
		audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
		if (audioManager == null)
			Debug.LogWarning("AudioManager not found in the scene!");
	}
	
	private void Start()
	{
		currentHp = maxHp;
		animator = GetComponent<Animator>();
		if (thanhHp != null) thanhHp.capNhatHp(currentHp, maxHp);
		
		UpdateScoreUI(); // Cập nhật UI khi bắt đầu
	}

	// Hàm trừ HP khi bị trúng
	public void TakeDamage(float damage)
	{
		if (audioManager != null && audioManager.takeDmg != null)
			audioManager.PlaySFX(audioManager.takeDmg);

		currentHp -= damage;
		currentHp = Mathf.Clamp(currentHp, 0, maxHp);

		if (thanhHp != null) thanhHp.capNhatHp(currentHp, maxHp);
		if (currentHp <= 0) Die();
	}

	private void Die()
	{
		Debug.Log("Player died!");
		animator.SetBool("isDead", true);

		// Lưu điểm hiện tại để màn hình "Play Again" có thể hiển thị
		PlayerPrefs.SetInt("LastScore", score);

		// Kiểm tra và lưu điểm cao
		int highScore = PlayerPrefs.GetInt("HighScore", 0);
		if (score > highScore)
		{
			PlayerPrefs.SetInt("HighScore", score);
			Debug.Log("New High Score: " + score);

			// --- LOGIC MỞ KHÓA KỸ NĂNG (đã cải tiến) ---
			if (fireballSkillToUnlock != null && score >= fireballSkillToUnlock.requiredScore && !fireballSkillToUnlock.IsUnlocked())
				fireballSkillToUnlock.Unlock();
			
		}
		StartCoroutine(WaitForDeathAnimation());
	}

	private IEnumerator WaitForDeathAnimation()
	{
		// Giả sử animation chết dài 2 giây,
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("PlayAgain");
	}

	public void Heal(int amount)
	{
		currentHp += amount;
		if (currentHp > maxHp) currentHp = maxHp;
		if (thanhHp != null)
			thanhHp.capNhatHp(currentHp, maxHp);

		Debug.Log("Đã hồi máu: " + amount + " | Máu hiện tại: " + currentHp);
	}

	public void AddScore(int amount)
	{
		score += amount;
		UpdateScoreUI(); // Cập nhật UI mỗi khi cộng điểm
		Debug.Log("Điểm hiện tại: " + score);
	}

	private void UpdateScoreUI(){
		if (scoreText != null) scoreText.text = "Điểm: " + score;
	}
}