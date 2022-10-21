using UnityEngine;
using System.Collections;

// This script is used for looping music and playing sound effects.
// To play an effect, instantiate the prefab with the parent as the game object source.
// Assign your clips to the clip list or a single clip to the m_Clip member.

[RequireComponent (typeof(AudioSource))]
public class JSAudioSource : MonoBehaviour
{
	public enum AudioType
	{
		SoundEffect,
		Music,
	}

	public AudioType m_AudioType = AudioType.SoundEffect;
	
	public bool m_PlayOnAwake = true;
	
	public AudioClip m_Clip;
	
	public AudioClip[] m_Clips;
	
	public float m_ClipVolume = 1.0f; // Should not exceed 1.0f
	
	public bool m_RandomizeClips = true;
	
	public float m_PlayDelay = 0f;
	
	public bool m_Loop = false;
	
	public float m_LoopDelay = 0f;
	
	public int m_LoopCount = 0; // Inifinite = 0
	
	public bool m_DestroyOnComplete = true;
	
	private int m_CurrentClip = 0;
	
	private JSPreferencesController m_PreferencesController;

	private bool m_IsPlaying = false;
	
	void Start()
	{
		Initialize();
	}
	
	void Update()
	{
	
	}
	
	private void Initialize()
	{
		if(m_PreferencesController == null && GameObject.Find("JSPreferencesController") != null)
		{
			m_PreferencesController = GameObject.Find("JSPreferencesController").GetComponent<JSPreferencesController>();
		}
		
		if(m_Clip == null && m_Clips.Length > 0)
		{
			if(m_RandomizeClips)
			{
				m_Clip = m_Clips[Random.Range(0, m_Clips.Length)];
			}
			else
			{
				m_Clip = m_Clips[0];
			}
		}
		
		
		if(m_PlayOnAwake)
		{
			StartCoroutine(AudioSequence());
		}
	}

	public IEnumerator AudioSequence()
	{
		if(m_Clip == null || m_IsPlaying)
		{
		    yield break;
		}
		
		if(m_PlayDelay > 0)
		{
			yield return new WaitForSeconds(m_PlayDelay);
		}

		SetVolume();

		GetComponent<AudioSource>().clip = m_Clip;
		
		GetComponent<AudioSource>().Play();

		m_IsPlaying = true;


		yield return new WaitForSeconds(m_Clip.length);
		
		m_IsPlaying = false;

		NextClip();
		
		// Notify myself that I have finished playing a clip, in case another script wants to switch out the clip being played...
		gameObject.SendMessage("ClipFinished", SendMessageOptions.DontRequireReceiver);
		
		if(m_Loop)
		{
			if(m_LoopCount > 0)
			{
				for(int i = 0; i < m_LoopCount; i++)
				{
					if(m_LoopDelay > 0)
					{
						yield return new WaitForSeconds(m_LoopDelay);
					}
								
					SetVolume();
			
					GetComponent<AudioSource>().clip = m_Clip;
					
					GetComponent<AudioSource>().Play();

					m_IsPlaying = true;

					yield return new WaitForSeconds(m_Clip.length);
					
					m_IsPlaying = false;

					NextClip();
					
					// Notify myself that I have finished playing a clip, in case another script wants to switch out the clip being played...
					gameObject.SendMessage("ClipFinished", SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				// Something else may stop this script...
				while(true)
				{
					if(m_LoopDelay > 0)
					{
						yield return new WaitForSeconds(m_LoopDelay);
					}
								
					SetVolume();
			
					GetComponent<AudioSource>().clip = m_Clip;
					
					GetComponent<AudioSource>().Play();

					m_IsPlaying = true;
					
					yield return new WaitForSeconds(m_Clip.length);
					
					m_IsPlaying = false;

					NextClip();
					
					// Notify myself that I have finished playing a clip, in case another script wants to switch out the clip being played...
					gameObject.SendMessage("ClipFinished", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
				
		GetComponent<AudioSource>().Stop();
		
		m_IsPlaying = false;

		if(m_DestroyOnComplete)
		{
			Destroy(gameObject);
		}
	}

	public bool IsPlaying()
	{
		return m_IsPlaying;
	}

	private void NextClip()
	{
		if(m_Clips != null && m_Clips.Length > 1)
		{
			if(m_RandomizeClips)
			{
				int tries = 10;
				
				int randomClip = Random.Range(0, m_Clips.Length);
				
				// Reduce the chance of playing the same clip twice in a row
				while(tries > 0 && randomClip == m_CurrentClip)
				{
					randomClip = Random.Range(0, m_Clips.Length);
				}
				
				m_CurrentClip = randomClip;
				
				m_Clip = m_Clips[m_CurrentClip];
			}
			else // Play sequentially
			{
				m_CurrentClip++;
				
				if(m_CurrentClip >= m_Clips.Length)
				{
					m_CurrentClip = 0;
				}
			}
		}
	}
	
	// If this object is set to not play on awake, then this function starts the sequence.
	public void Play()
	{
		Initialize();
		
		if(!m_PlayOnAwake)
		{
			StartCoroutine(AudioSequence());
		}
	}
	
	public void Stop()
	{
		GetComponent<AudioSource>().Stop();
	}
	
	// It is possible that options have changed between audio playing or during.
	public void SetVolume()
	{
		// Do not exceed the volume maximum for an audio source.
		if(m_ClipVolume > 1.0f)
		{
			m_ClipVolume = 1.0f;
		}

		/* TODO: Find out what UserData is?
		// Multiply the volume preference by this clip's volume level.
		if(UserData.Data != null)
		{
			if(m_AudioType == AudioType.Music)
			{
				GetComponent<AudioSource>().volume = UserData.Data.MusicVolume * m_ClipVolume;
			}
			else
			{
				GetComponent<AudioSource>().volume = UserData.Data.EffectsVolume * m_ClipVolume;
			}
		}
        */
	}
	
	public void PreferencesChanged()
	{
		SetVolume();
	}
}
