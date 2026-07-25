namespace Wyrm.ModBusClient.GivEnergy.Constants;

/// <summary>
/// Battery calibration stages.
/// </summary>
public enum BatteryCalibrationStage
{
    /// <summary>
    /// Off.
    /// </summary>
    Off,
    /// <summary>
    /// Discharging.
    /// </summary>
    Discharge,
    /// <summary>
    /// Setting lower limit.
    /// </summary>
    SetLowerLimit,
    /// <summary>
    /// Charging.
    /// </summary>
    Charge,
    /// <summary>
    /// Setting upper limit.
    /// </summary>
    SetUpperLimit,
    /// <summary>
    /// Balancing.
    /// </summary>
    Balance,
    /// <summary>
    /// Setting full capacity.
    /// </summary>
    SetFullCapacity,
    /// <summary>
    /// Finishing.
    /// </summary>
    Finish
}
