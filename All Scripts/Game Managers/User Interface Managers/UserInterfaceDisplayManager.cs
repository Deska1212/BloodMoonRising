using System;
using UnityEngine;

/// <summary>
/// Master abstract class for all display managers, just acts as a contract making sure each element has consistent behaviour
/// Could have probably used an Interface here instead... But I will only really ever need to inherit this one class in children.
/// </summary>
public abstract class UserInterfaceDisplayManager : MonoBehaviour
{
	public abstract void UpdateDisplay<T>(T value);
}
