using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public static class HapticFeedback
{
    public static void SendHapticImpulse(float amplitude, float duration)
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller, devices);

        foreach (var device in devices)
        {
            if (!device.isValid) continue;

            if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities) && capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0u, Mathf.Clamp01(amplitude), duration);
            }
        }
    }
}
