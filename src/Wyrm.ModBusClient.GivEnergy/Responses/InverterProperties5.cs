namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the fifth input data set from a GivEnergy inverter.
/// </summary>
public record InverterProperties5 : IResponseData
{
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg241 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg242 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 1.
    /// </summary>
    public ushort ChargeTargetStateOfCharge1 { get; init; }
    /// <summary>
    /// Gets the charge slot 2 X.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot2X { get; init; }
    /// <summary>
    /// Gets the charge target SOC 2.
    /// </summary>
    public ushort ChargeTargetStateOfCharge2 { get; init; }
    /// <summary>
    /// Gets the charge slot 3.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot3 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 3.
    /// </summary>
    public ushort ChargeTargetStateOfCharge3 { get; init; }
    /// <summary>
    /// Gets the charge slot 4.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot4 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 4.
    /// </summary>
    public ushort ChargeTargetStateOfCharge4 { get; init; }
    /// <summary>
    /// Gets the charge slot 5.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot5 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 5.
    /// </summary>
    public ushort ChargeTargetStateOfCharge5 { get; init; }
    /// <summary>
    /// Gets the charge slot 6.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot6 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 6.
    /// </summary>
    public ushort ChargeTargetStateOfCharge6 { get; init; }
    /// <summary>
    /// Gets the charge slot 7.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot7 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 7.
    /// </summary>
    public ushort ChargeTargetStateOfCharge7 { get; init; }
    /// <summary>
    /// Gets the charge slot 8.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot8 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 8.
    /// </summary>
    public ushort ChargeTargetStateOfCharge8 { get; init; }
    /// <summary>
    /// Gets the charge slot 9.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot9 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 9.
    /// </summary>
    public ushort ChargeTargetStateOfCharge9 { get; init; }
    /// <summary>
    /// Gets the charge slot 10.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot10 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 10.
    /// </summary>
    public ushort ChargeTargetStateOfCharge10 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg271 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg272 { get; init; }
    /// <summary>
    /// Gets the discharge target SOC 1.
    /// </summary>
    public ushort DischargeTargetStateOfCharge1 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg274 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg275 { get; init; }
    /// <summary>
    /// Gets the discharge target SOC 2.
    /// </summary>
    public ushort DischargeTargetStateOfCharge2 { get; init; }
    /// <summary>
    /// Gets the discharge slot 3.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? DischargeSlot3 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 3.
    /// </summary>
    public ushort DischargeTargetStateOfCharge3 { get; init; }
    /// <summary>
    /// Gets the discharge slot 4.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? DischargeSlot4 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 4.
    /// </summary>
    public ushort DischargeTargetStateOfCharge4 { get; init; }
    /// <summary>
    /// Gets the discharge slot 5.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? DischargeSlot5 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 5.
    /// </summary>
    public ushort DischargeTargetStateOfCharge5 { get; init; }
    /// <summary>
    /// Gets the discharge slot 6.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? DischargeSlot6 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 6.
    /// </summary>
    public ushort DischargeTargetStateOfCharge6 { get; init; }
    /// <summary>
    /// Gets the discharge slot 7.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? DischargeSlot7 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 7.
    /// </summary>
    public ushort DischargeTargetStateOfCharge7 { get; init; }
    /// <summary>
    /// Gets the discharge slot 8.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? DischargeSlot8 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 8.
    /// </summary>
    public ushort DischargeTargetStateOfCharge8 { get; init; }
    /// <summary>
    /// Gets the discharge slot 9.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? DischargeSlot9 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 9.
    /// </summary>
    public ushort DischargeTargetStateOfCharge9 { get; init; }
    /// <summary>
    /// Gets the discharge slot 10.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? DischargeSlot10 { get; init; }
    /// <summary>
    /// Gets the charge target SOC 10.
    /// </summary>
    public ushort DischargeTargetStateOfCharge10 { get; init; }
}
