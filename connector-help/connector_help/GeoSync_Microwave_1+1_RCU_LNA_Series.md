---
uid: Connector_help_GeoSync_Microwave_1+1_RCU_LNA_Series
---

# GeoSync Microwave 1+1 RCU LNA Series

This connector is used to monitor and control a GeoSync Microwave 1+1 RCU Low-Noise Amplifier (LNA).

## About

### Version Info

| **Range**            | **Key Features** | **Based on** | **System Impact** |
|----------------------|------------------|--------------|-------------------|
| 1.0.0.x \[SLC Main\] | Initial version  | \-           | \-                |

### Product Info

| **Range** | **Supported Firmware** |
|-----------|------------------------|
| 1.0.0.x   | 2.00_R100              |

### System Info

| **Range** | **DCF Integration** | **Cassandra Compliant** | **Linked Components** | **Exported Components** |
|-----------|---------------------|-------------------------|-----------------------|-------------------------|
| 1.0.0.x   | No                  | Yes                     | \-                    | \-                      |

## Configuration

### Connections

#### Serial Connection

This connector uses a serial connection and requires the following input during element creation:

SERIAL CONNECTION:

- **IP address/host**: The polling IP of the device e.g. *10.11.12.13*.
- **IP port**: The IP port of the device.
- **Bus address**: The address used by the serial interface. The default bus address is 64 (range: 64-95).

## How to Use

On the **General** page, you can find comprehensive information about the Low-Noise Amplifier (LNA) switch.

On the remaining pages, you can control and monitor the status of the LNA switch.

On the **Log** page, you can review all the recorded log entries generated by the device. The log table is updated at a low frequency. To manually update the table, you can use the **Refresh** button. With the **Clear** button, you can remove the log entries.