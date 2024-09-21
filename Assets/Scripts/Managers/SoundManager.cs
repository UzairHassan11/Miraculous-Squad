using UnityEngine;

// namespace GameAssets.GameSet.GameDevUtils.Managers
// {
	public class SoundManager : MonoBehaviour
	{

		public static    SoundManager Instance { get; private set; }
		public AudioSource  bgSoundSource;
		// [SerializeField] AudioSource  bgAmbienceSoundSource;
		[SerializeField] AudioSource  sFXSoundSource;

		public AudioClip mainMenuBgClip, gameplayBgClip;
		// public AudioClip bgAmbienceClip;

		[Header("Clip names")] [SerializeField]
		private ClipName clipNames;

		[SerializeField] private AudioClip[] _audioClips;
		
		void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				DestroyImmediate(gameObject);
			}
		}

		void Start()
		{
			if (bgSoundSource.isPlaying)
				return;
			PlayMainMenyBG(true);
		    
			SetBgSoundSetting(GameSettings.Instance.toggleStatusBg);
			// bgAmbienceSoundSource.clip = bgAmbienceClip;
			// bgAmbienceSoundSource.loop = true;
			// bgAmbienceSoundSource.Play();
		}

	public void TurnOffBgSound()
    {
		bgSoundSource.Stop();
	}

	public void PlayMainMenyBG(bool state)
    {
		bgSoundSource.clip = state ? mainMenuBgClip : gameplayBgClip;
		bgSoundSource.loop = true;
		bgSoundSource.Play();
	}

	public void SetBgSoundSetting(bool toggle)
		{
			bgSoundSource.mute = !toggle;
			
		}

		public void SetSfxSoundSetting(bool toggle) => sFXSoundSource.mute = !toggle;

		void PlayOneShot(AudioClip clip, float volume)
		{
			if(GameSettings.Instance.toggleStatusSFX)
			    sFXSoundSource.PlayOneShot(clip, volume);
		}

		public void PlaySound(ClipName clipName, float volume = 1)
		{
			if(GameSettings.Instance.toggleStatusSFX)
				sFXSoundSource.PlayOneShot(_audioClips[(int)clipName], volume);
		}
		
		public void PlaySound(int index)
		{
			if(GameSettings.Instance.toggleStatusSFX)
				sFXSoundSource.PlayOneShot(_audioClips[index]);
		}
	}
// }
//class end
public enum ClipName
{
	Button,
	Reward,
	Win,
	Fail,
	PlayerHit,
	PlayerShieldHit,
	EnemyHit,
	GemPick,
	FootStep,
	CoinPick,
	PowerUpActive
}