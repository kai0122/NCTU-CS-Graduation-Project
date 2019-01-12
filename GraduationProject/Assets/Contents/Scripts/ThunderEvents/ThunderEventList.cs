using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ThunderEvents;

public class SimpleThunderEventList{}

public class ThunderEventListClass<T> : SimpleThunderEventList
{
	public List<T> List;
}

[System.Serializable]
public class ThunderEventList : ThunderEventListClass<ThunderEvent>
{
}