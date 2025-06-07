using System;
using System.Diagnostics;
using UnityEngine.Rendering;

public static class PlayerEvents
{
    public static event Action<KillingObjectType> OnKilled;
    

    public static void Kill(KillingObjectType killingobject)
    {
        OnKilled?.Invoke(killingobject);
    }

}