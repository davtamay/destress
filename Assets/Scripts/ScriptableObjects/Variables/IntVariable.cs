﻿using UnityEngine;

[CreateAssetMenu (fileName = "IntValue", menuName = "CustomSO/Variables/Int")]
public class IntVariable : ScriptableObject {

	#if UNITY_EDITOR
	[Multiline]
	public string DeveloperDescription = "";
	#endif
	public int Value;

	public void SetValue(int value)
	{
		Value = value;
	}

	public void SetValue(IntVariable value)
	{
		Value = value.Value;
	}

	public void ApplyChange(int amount)
	{
		Value += amount;
	}

	public void ApplyChange(IntVariable amount)
	{
		Value += amount.Value;
	}


//	public static implicit operator int(IntVariable reference){
//
//		return Value;
//	}
}

