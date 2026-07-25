# Wyrm.ModBusClient.GivEnergy
A client for accessing GivEnergy inverters using ModBus TCP.
This currently only supports reading register data.
IMPORTANT NOTE: This has only been tested on a GivEnergy Hybrid Gen3 Inverter with a single battery and single meter.
Thanks to Dewet22 for the work on the https://github.com/dewet22/givenergy-modbus/ project that was used to produce this.

## Usage
Add the GivEnergy client into dependency injection using the AddGivEnergyClient extension method.
```csharp
using Wyrm.ModBusClient.GivEnergy.DependencyInjection;
...
services.AddGivEnergyClient();
```
By default, it is added as a Singleton but you can specify the lifetime.
```csharp
services.AddGivEnergyClient(ServiceLifetime.Scoped);
```
Then inject the IGivEnergyClient interface into your class.

Call ConnectAsync before calling any of the methods.
Send requests on a different thread to the one receiving responses.
Make sure you Close the client when finished with it.

## Functions

### WaitForResponseAsync
This waits for a response from the inverter. It returns when one is received.
Each response indicates the Serial Number and WiFi Adapter to distinguish it.
The 'ResponseDataType' indicates what class the 'ResponseData' is.
You will need to cast the response data to the relevant class to access the data values.

### RequestInverterDataAsync
This requests that the inverter send specific data for a specific sub-device. Only a range of device indexes are allowed for each data type.

! Response Data Type  | Device Indices |
|---------------------|----------------|
| BatteryData2        | 0 to 5         |
| MeterData2          | 0 to 8         |
| LowVoltageBCUData2  | 0              !
| InverterData*       | 0              |
| InverterProperties* | 0              |

### SendReadRegistersAsync
This is a more flexible version of RequestInverterDataAsync.
You can specify the device address, whether to read holding registers (Inverter Properties) or input registers (Data), and the start address to read from.
It will issue a request to read 60 registers from the start address (usually that is itself a mnultiple of 60).

### SendCustomPduAsync
This allows a PDU that isn't a read registers one to be issued.
GivEnergy inverters send a response that apparently needs to be echoed back. You'll get a GivEnergyClientException thrown by the WaitForResponseAsync method if it receives something it doesn't recognise, and you can then send that bak using the SendCustomPduAsync method.
```csharp
...
catch (GivEnergyClientException ex)
{
    var dataSpan = ex.ErrorData.Span;
    if (dataSpan.Length == 13 && dataSpan[0] == 1 && dataSpan[1] == 1 && dataSpan[^1] == 1)
    {
        await client.SendCustomPduAsync(ex.ErrorData);
    }
    ...
}
```
