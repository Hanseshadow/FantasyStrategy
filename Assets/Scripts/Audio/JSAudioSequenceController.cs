using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class JSAudioSequenceEntry
{
	public int m_SequenceID;

	private int m_ClipIndex;

	public List<AudioClip> m_Clips;

	public AudioClip GetClipInSequence()
	{
		if(m_Clips == null || m_Clips.Count <= 0)
		{
			return null;
		}

		if(m_ClipIndex >= m_Clips.Count)
		{
			m_ClipIndex = 0;
		}

		AudioClip clip = m_Clips[m_ClipIndex];

		m_ClipIndex++;

		return clip;
	}
}

public class JSAudioSequenceController : MonoBehaviour
{
/*
	public List<JSAudioSequenceEntry> m_AudioSequences;

	void Start()
	{
	
	}
	
	void Update()
	{
	
	}

	public AudioClip GetNextClipByID(int id)
	{
		if(m_AudioSequences == null || m_AudioSequences.Count <= 0)
		{
			return null;
		}

		for(int i = 0; i < m_AudioSequences.Count; i++)
		{
			if(m_AudioSequences[i] == null)
			{
				continue;
			}

			if(m_AudioSequences[i].m_SequenceID == id)
			{
				return m_AudioSequences[i].GetClipInSequence();
			}
		}

		return null;
	}
*/
}
