---
uid: Troubleshooting_SLXML_exe
---

# SLXML.exe

## About SLXML

SLXML is a 32-bit process that manages all interaction with the different XML files found in a DataMiner System, i.e. SLXML reads and writes towards the XML files. If SLXML is abruptly interrupted, it is possible that only half an XML file is written, leading to incorrect configuration. Any issue pertaining to an XML file (e.g. the *MaintenanceSettings.xml* file) can be found here.

You can find the SLXML log file in the folder `C:\Skyline DataMiner\Logging`.

## SLXML troubleshooting flowchart

```mermaid
flowchart TD
%% Define styles %%
linkStyle default stroke:#cccccc
classDef LightBlue fill:#9DDAF5,stroke:#000070,stroke-width:0px, color:#1E5179;
classDef Blue fill:#4BAEEA,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef DarkBlue fill:#1E5179,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef DarkGray fill:#58595B,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef Gray fill:#999999,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef LightGray fill:#DDDDDD,stroke:#000070,stroke-width:0px, color:#1E5179;
%% Define blocks %%
START[SLXML]
Leak[Memory leak]
Crash[Process crash]
StartPage([Start page])
LogCollector([Log collector usage guide])
Ch[" \- Check Task Manager for high memory. <br/>\- Check Microsoft Platform element for gradual increase in memory over a period of time. "]
D1{{Are the conditions above met?}}
CrashdumpDetected{{"Crash dump found at issue time? C:\Skyline DataMiner\Logging\CrashDump"}}
%% Connect blocks %%
Leak --- |CONSEQUENCE| Crash
START --- Leak
START --- Crash
Leak --- |IDENTIFICATION| Ch
Ch --- D1
Crash --- CrashdumpDetected
%% Define hyperlinks %%
click D1 "#are-the-conditions-above-met"
click CrashdumpDetected "#crash-dump-found-at-issue-time"
click StartPage "/dataminer/Troubleshooting/Troubleshooting_Flowcharts/Finding_a_Root_Cause.html" "Go to Start Page"
click LogCollector "/dataminer/Reference/DataMiner_Tools/SLLogCollector.html" "SLLogCollector"
click Leak "#memory-leak"
%% Apply styles to blocks %%
class START,N2,N1,Y2,MinidumpNo,N3,Y3 DarkBlue;
class Sol1,MinidumpYes,Sol2 DarkGray;
class CrashdumpDetected,D1,Ch1,Minidump,Ch3 Blue;
class StartPage,LogCollector LightBlue;
class Crash,Ch,Y1,ProcessCrashed LightGray;
class Leak Gray;
```

## Are the conditions above met?

```mermaid
flowchart TD
%% Define styles %%
linkStyle default stroke:#cccccc
classDef LightBlue fill:#9DDAF5,stroke:#000070,stroke-width:0px, color:#1E5179;
classDef Blue fill:#4BAEEA,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef DarkBlue fill:#1E5179,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef DarkGray fill:#58595B,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef Gray fill:#999999,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef LightGray fill:#DDDDDD,stroke:#000070,stroke-width:0px, color:#1E5179;
%% Define blocks %%
D1{{Are the conditions above met?}}
N1([No memory leak.])
Y1[" \- Find the time when memory use started to grow. <br/>- Check logging, info events, Windows Event Viewer around that time. <br/>\- Installing Windows updates or updating Windows XML modules can trigger a leak too. "]
Sol1[" \- Check if Windows file updates are correctly installed. <br/>\- If an update is not correctly installed, a server restart might be needed to finish the installation. <br/>\- Check in the SLXml log file for a possible cause. "]
Ch1{{Is the leak still present?}}
N2([Issue resolved.])
Y2([Contact DataMiner Support with the required logging and memory dump.])
%% Connect blocks %%
D1 --- |YES| Y1
D1 --- |NO| N1
Y1 --- |SOLUTION| Sol1
Sol1 --- Ch1
Ch1 --- |YES| Y2
Ch1 --- |NO| N2
%% Apply styles to blocks %%
class START,N2,N1,Y2,MinidumpNo,N3,Y3 DarkBlue;
class Sol1,MinidumpYes,Sol2 DarkGray;
class CrashdumpDetected,D1,Ch1,Minidump,Ch3 Blue;
class StartPage,LogCollector LightBlue;
class Crash,Ch,Y1,ProcessCrashed LightGray;
class Leak Gray;
```

## Crash dump found at issue time?

```mermaid
flowchart TD
%% Define styles %%
linkStyle default stroke:#cccccc
classDef LightBlue fill:#9DDAF5,stroke:#000070,stroke-width:0px, color:#1E5179;
classDef Blue fill:#4BAEEA,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef DarkBlue fill:#1E5179,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef DarkGray fill:#58595B,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef Gray fill:#999999,stroke:#000070,stroke-width:0px, color:#FFFFFF;
classDef LightGray fill:#DDDDDD,stroke:#000070,stroke-width:0px, color:#1E5179;
%% Define blocks %%
ProcessCrashed["\- Save .high crashdump + note timestamp. <br/>\- Check the ErrorLog.txt file for possible causes. <br/>\- Check if DMA was missing production DVEs while restarting."]
Minidump{{"Minidump found at issue time? C:\Skyline DataMiner\Logging\MiniDump"}}
MinidumpNo(["Contact DataMiner Support with the required logging and memory dump."])
MinidumpYes["Identify the cause in the required log file: SLXML, SLErrors, SLDMS, etc."]
Sol2[Collect all the logs and restart the DMA.]
Ch3{{Does the crash happen after the restart?}}
N3([Issue resolved.])
Y3([Contact DataMiner Support with the required logging and crashdump files.])
CrashdumpDetected{{"Crash dump found at issue time? C:\Skyline DataMiner\Logging\CrashDump"}}
%% Connect blocks %%
CrashdumpDetected --- |YES| ProcessCrashed
CrashdumpDetected --- |NO| Minidump
Minidump --- |YES| MinidumpYes
Minidump --- |NO| MinidumpNo
ProcessCrashed --- |SOLUTION| Sol2
Sol2 --- Ch3
Ch3 --- |YES| Y3
Ch3 --- |NO| N3
%% Apply styles to blocks %%
class START,N2,N1,Y2,MinidumpNo,N3,Y3 DarkBlue;
class Sol1,MinidumpYes,Sol2 DarkGray;
class CrashdumpDetected,D1,Ch1,Minidump,Ch3 Blue;
class StartPage,LogCollector LightBlue;
class Crash,Ch,Y1,ProcessCrashed LightGray;
class Leak Gray;
```

## Memory leak

- As SLXML is a 32-bit process, memory leaks can grow up to 2 GB at most.

- SLXML uses Microsoft XML library to read and write into XML files; therefore it is important to have all Windows updates installed. Once the installation of updates is done, rebooting the server might also be necessary to finish the whole process.

- Another possible cause of memory leaks is the addition of a large LDAP user group to a DMS (for example over 900 users). DataMiner cannot cater to such a large number yet.

- Most other causes have been addressed in DataMiner versions > 10.

- Sometimes, a memory leak can trigger a process crash on SLXML too as the process goes out of memory (2 GB) and ultimately crashes.
