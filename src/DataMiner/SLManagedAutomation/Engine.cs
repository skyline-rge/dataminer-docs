﻿using System;
using System.Runtime.ExceptionServices;
using Skyline.DataMiner.Net;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Net.Exceptions;
using Skyline.DataMiner.Net.Profiles;
using System.Collections.Generic;
using Skyline.DataMiner.Net.Messages.SLDataGateway;
using Skyline.DataMiner.Net.Automation;

namespace Skyline.DataMiner.Automation
{
	/// <summary>
	/// Allows interaction with the DataMiner System from a C# code block of an Automation script.
	/// </summary>
	public class Engine : IDisposable, IEngine
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Engine"/> class.
		/// </summary>
		public Engine() { }

		~Engine() { }

		/// <summary>
		/// Gets the instance ID.
		/// </summary>
		/// <value>The instance ID.</value>
		public int InstanceId { get; }

		/// <summary>
		/// Gets a value indicating whether an interactive client is available for the script.
		/// </summary>
		/// <value><c>true</c> if an interactive client is available for the script; otherwise, <c>false</c>.</value>
		/// <example>
		/// <code>
		/// bool isInteractive = engine.IsInteractive;
		/// </code>
		/// </example>
		/// <remarks>Feature introduced in DataMiner 8.5.6 (RN 9775).</remarks>
		public bool IsInteractive { get; }

		/// <summary>
		/// Gets the Profile Manager.
		/// </summary>
		/// <value>The Profile Manager.</value>
		/// <example>
		/// <code>
		/// var profileManager = engine.ProfileManager;
		/// </code>
		/// </example>
		/// <remarks>
		/// <para><see cref="SLProfileManager"/> is now obsolete. Use <see cref="ProfileManagerHelper"/> for an up-to-date API.</para>
		/// <code>
		/// using System;
		/// using Skyline.DataMiner.Automation;
		/// using Skyline.DataMiner.Net.IManager.Helper;
		/// using Skyline.DataMiner.Net.Profiles;
		/// using System.Linq;
		/// 
		/// public class Script
		/// {
		/// 	// We use the ProfileManagerHelper object to communicate with the ProfileManager module.
		/// 	ProfileManagerHelper Helper;
		/// 	
		///		public Script()
		///		{
		///			// Initialize the ProfileManagerHelper
		///			Helper = new ProfileManagerHelper();
		///			
		///			//Handling the RequestResponseEvent gives the ProfileManagerHelper connection with SLNet.
		///			Helper.RequestResponseEvent += Helper_RequestResponseEvent;
		///		}
		///		
		///		//Handles the RequestResponseEvent. Any server call the Helper makes behind the scenes calls this method.
		///		private void Helper_RequestResponseEvent(object sender, IManagerRequestResponseEventArgs e)
		///		{
		///			e.responseMessage = Engine.SLNet.SendSingleResponseMessage(e.requestMessage);
		///		}
		///	
		///		public void Run(Engine engine)
		///		{
		///			// ...
		///		}
		///	}
		/// </code>
		/// </remarks>
		[Obsolete("Please use the ProfileManagerHelper for an up-to-date API")]
		public SLProfileManager ProfileManager { get; }

		/// <summary>
		/// Gets the Ticketing Gateway.
		/// </summary>
		/// <value>The Ticketing Gateway.</value>
		/// <example>
		/// <code>
		/// var ticketingGateway = engine.TicketingGateway;
		/// </code>
		/// </example>
		public SLTicketingGateway TicketingGateway { get; }

		/// <summary>
		/// Gets or sets the timeout for the current C# code block.
		/// </summary>
		/// <value>The timeout for the current C# code block.</value>
		/// <remarks>
		/// <note type="note">
		/// <para>From DataMiner 10.2.0/10.1.2 onwards, this property can also be used to determine when an interactive Automation script times out.</para>
		/// </note>
		/// </remarks>
		/// <example>
		/// <code>
		/// var timeout = engine.Timeout;
		/// </code>
		/// </example>
		public TimeSpan Timeout { get; set; }

		/// <summary>
		/// Gets the name of the user who triggered the script.
		/// </summary>
		/// <value>The name of the user who triggered the script.</value>
		/// <remarks>
		/// <note type="note">
		/// <para>When a scheduled task, a Correlation rule, or a redundancy group trigger a script to execute, this TriggeredByName will be filled in with "Scheduled task [name task]", "Correlation-rule [name rule]", or "Redundancy", respectively.</para>
		/// </note>
		/// <para>Feature introduced in DataMiner 10.2.6/10.3.0 (RN 33122).</para>
		/// </remarks>
		/// <example>
		/// <code>
		/// engine.Log(engine.TriggeredByName);
		/// </code>
		/// </example>
		public string TriggeredByName { get; }

        /// <summary>
        /// Gets the user cookie.
        /// </summary>
        /// <value>The user cookie.</value>
        /// <remarks>
        /// <para>Feature introduced in DataMiner 10.0.5 (RN 24475).</para>
        /// </remarks>
        public string UserCookie { get; }


		/// <summary>
		/// Gets the displayed name of the user who is executing the script.
		/// </summary>
		/// <value>The displayed name of the user who is executing the script.</value>
		/// <example>
		/// <code>
		/// engine.Log(engine.UserDisplayName);
		/// </code>
		/// </example>
		public string UserDisplayName { get; }

		/// <summary>
		/// Gets the login name of the user who is executing the script.
		/// </summary>
		/// <value>The login name of the user who is executing the script.</value>
		/// <example>
		/// <code>
		/// string name = engine.UserLoginName;
		/// </code>
		/// </example>
		public string UserLoginName { get; }

		/// <summary>
		/// Acknowledges the specified alarm tree using the provided comment message.
		/// </summary>
		/// <param name="alarmTreeID">The alarm tree to update</param>
		/// <param name="comment">A comment.</param>
		/// <remarks>If a user launches the script manually or attaches to it interactively, that user will become the owner of the alarm. If the script runs in the background, the alarm owner will become “Administrator”.</remarks>
		/// <example>
		/// <code>
		/// engine.AcknowledgeAlarm(new AlarmTreeID(7, 400, 304022), "Alarm acknowledged.");
		/// </code>
		/// </example>
		public void AcknowledgeAlarm(AlarmTreeID alarmTreeID, string comment) { }

        /// <summary>
        /// Acknowledges the specified alarm tree using the provided comment message.
        /// </summary>
        /// <param name="dataMinerID">The DataMiner Agent ID.</param>
        /// <param name="alarmID">The alarm ID.</param>
        /// <param name="comment">A comment.</param>
        /// <remarks>
        /// <para>If a user launches the script manually or attaches to it interactively, that user will become the owner of the alarm. If the script runs in the background, the alarm owner will become “Administrator”.</para>
        /// <para>Although this method is obsolete, it will still work on a DMS without Swarming enabled until 10.6.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// engine.AcknowledgeAlarm(7, 304022, "Alarm acknowledged.");
        /// </code>
        /// </example>
        [Obsolete("Please use the overload that takes an AlarmTreeID (still supported in <= 10.6 on non-swarming systems)")]
        public void AcknowledgeAlarm(int dataMinerID, int alarmID, string comment) { }

        /// <summary>
        /// Acknowledges the specified alarm tree using the provided comment message.
        /// </summary>
        /// <param name="dataMinerID">The DataMiner Agent ID.</param>
        /// <param name="elementID">The element ID.</param>
        /// <param name="alarmID">The alarm ID.</param>
        /// <param name="comment">A comment.</param>
        /// <remarks>
        /// <para>If a user launches the script manually or attaches to it interactively, that user will become the owner of the alarm. If the script runs in the background, the alarm owner will become “Administrator”.</para>
        /// <para>For performance reasons, we recommend using the overload that takes an AlarmTreeID instead.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// engine.AcknowledgeAlarm(7, 400, 304022, "Alarm acknowledged.");
        /// </code>
        /// </example>
        public void AcknowledgeAlarm(int dataMinerID, int elementID, int alarmID, string comment) { }

		/// <summary>
		/// Adds an error message to the Automation script, which will eventually cause the script to fail.
		/// </summary>
		/// <param name="error">The error message.</param>
		/// <example>
		/// <code>
		/// engine.AddError("A non-fatal error has occurred.");
		/// </code>
		/// </example>
		public void AddError(string error) { }

		/// <summary>
		/// Adds a key and value to the dictionary that will be passed to the parent script.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <remarks>Feature introduced in DataMiner 9.6.8 (RN 21952).</remarks>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null" /></exception>
		/// <exception cref="ArgumentException">An element with the same key already exists.</exception>
		public virtual void AddScriptOutput(string key, string value) { }

		/// <summary>
		/// Adds a key to the script output if it has not yet been added.
		/// </summary>
		/// <param name="key">The key to add or update.</param>
		/// <param name="value">The value.</param>
		/// <remarks>
		/// <para>If the key already exists, it will be updated with the specified value.</para>
		/// <para>Feature introduced in DataMiner 10.0.2 (RN 23936).</para>
		/// </remarks>
		public virtual void AddOrUpdateScriptOutput(string key, string value)
		{
			//if (this._scriptOutput == null)
			//	this._scriptOutput = new Dictionary<string, string>();
			//this._scriptOutput[key] = value;
		}

		/// <summary>
		/// Clears the script output.
		/// </summary>
		/// <remarks>Feature introduced in DataMiner 10.0.2 (RN 23936).</remarks>
		public virtual void ClearScriptResult()
		{
			//this._scriptOutput = new Dictionary<string, string>();
		}

		/// <summary>
		/// Removes the entry with the specified key from the script output.
		/// </summary>
		/// <param name="key">The key of the entry to clear.</param>
		/// <remarks>Feature introduced in DataMiner 10.0.2 (RN 23936).</remarks>
		public virtual void ClearScriptOutput(string key)
		{
			//Dictionary<string, string> scriptOutput = this._scriptOutput;
			//if (scriptOutput == null || !scriptOutput.ContainsKey(key))
			//	return;
			//this._scriptOutput.Remove(key);
		}

		/// <summary>
		/// Adds an additional dummy to the Automation script.
		/// </summary>
		/// <param name="dataMinerID">The DataMiner Agent ID.</param>
		/// <param name="elementID">The element ID.</param>
		/// <returns>The created dummy.</returns>
		/// <exception cref="DataMinerException">The specified element was not found.</exception>
		/// <example>
		/// <code>
		/// ScriptDummy extradummy1 = engine.CreateExtraDummy(366,22);
		/// </code>
		/// </example>
		public ScriptDummy CreateExtraDummy(int dataMinerID, int elementID) { return null; }

		/// <summary>
		/// Adds an additional dummy to the Automation script.
		/// </summary>
		/// <param name="dataMinerID">The DataMiner Agent ID.</param>
		/// <param name="elementID">The element ID.</param>
		/// <param name="key">The key.</param>
		/// <returns>The created dummy.</returns>
		/// <exception cref="DataMinerException">The specified element was not found.</exception>
		/// <example>
		/// <code>
		/// ScriptDummy extradummy1 = engine.CreateExtraDummy(366,22, "myDummy");
		/// </code>
		/// </example>
		public ScriptDummy CreateExtraDummy(int dataMinerID, int elementID, string key) { return null; }

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		//public sealed override void Dispose()
		public void Dispose()
		{
		}

		/// <summary>
		/// Aborts the Automation script, and indicates that it has failed.
		/// </summary>
		/// <param name="reason">Message describing the reason.</param>
		/// <example>
		/// <code>
		/// engine.ExitFail("A fatal error has occurred.");
		/// </code>
		/// </example>
		/// <remarks>
		/// <note type="caution">This method throws an <see cref="ScriptAbortException"/>. The script author is supposed to take this into account and make sure the script does not run endlessly, and that it does not swallow <see cref="ScriptAbortException"/> when calling <see cref="ExitFail(string)"/>.</note>
		/// </remarks>
		public void ExitFail(string reason) { }

		/// <summary>
		/// Aborts the Automation script, but does not indicate that it has failed.
		/// </summary>
		/// <param name="reason">Message describing the reason.</param>
		/// <example>
		/// <code>
		/// engine.ExitSuccess("The script has been stopped.");
		/// </code>
		/// </example>
		/// <remarks>
		/// <note type="caution">This method throws an <see cref="ScriptAbortException"/>. The script author is supposed to take this into account and make sure the script does not run endlessly, and that it does not swallow <see cref="ScriptAbortException"/> when calling <see cref="ExitSuccess(string)"/>.</note>
		/// </remarks>
		public void ExitSuccess(string reason) { }

		/// <summary>
		/// Retrieves the element with the specified name.
		/// </summary>
		/// <param name="name">The name of the element.</param>
		/// <returns>The element with the specified name or <see langword="null"/> if no element was found with the specified name.</returns>
		/// <example>
		/// <code>
		/// Element myElement = engine.FindElement("myElement");
		/// </code>
		/// </example>
		public Element FindElement(string name) { return null; }

		/// <summary>
		/// Retrieves the element with the specified DataMiner Agent ID/element ID.
		/// </summary>
		/// <param name="dmaID">The DataMiner Agent ID of the element.</param>
		/// <param name="elementID">The element ID.</param>
		/// <returns>The element with the specified DataMiner Agent ID/element ID or <see langword="null"/> if no element was found with the specified ID.</returns>
		/// <example>
		/// <code>
		/// Element myElement = engine.FindElement(7,32);
		/// </code>
		/// </example>
		public Element FindElement(int dmaID, int elementID) { return null; }

		/// <summary>
		/// Retrieves the element with the specified key.
		/// </summary>
		/// <param name="key">The key of the element. The key must be formatted as follows: "DataMiner Agent ID/element ID".</param>
		/// <returns>The element with the specified key or <see langword="null"/> if no element was found with the specified key.</returns>
		/// <example>
		/// <code>
		/// Element myElement = engine.FindElementByKey("7/32");
		/// </code>
		/// </example>
		public Element FindElementByKey(string key) { return null; }

		/// <summary>
		/// Retrieves all elements matching the specified filter.
		/// </summary>
		/// <param name="filter">The element filter.</param>
		/// <returns>The elements matching the specified filter.</returns>
		/// <example>
		///	<code>
		///	ElementFilter myElementFilter = new ElementFilter { MajorOnly=true };
		///	Element[] elements = engine.FindElements(myElementFilter);
		/// </code>
		/// </example>
		public Element[] FindElements(ElementFilter filter) { return null; }

		/// <summary>
		/// Retrieves all elements of which the name matches the specified name filter.
		/// </summary>
		/// <param name="nameFilter">The name filter.</param>
		/// <returns>The elements of which the name matches the specified name filter.</returns>
		/// <remarks><para>Name masks can contain the following wildcards:</para>
		/// <list type="bullet">
		/// <item><description>* : Any string of characters</description></item>
		/// <item><description>? : Any single character</description></item>
		/// </list>
		/// </remarks>
		/// <example>
		/// <code>
		/// Element[] elements = engine.FindElementsByName("Test*");
		/// </code>
		/// </example>
		public Element[] FindElementsByName(string nameFilter) { return null; }

		/// <summary>
		/// Retrieves all elements executing the specified protocol.
		/// </summary>
		/// <param name="name">The name of the protocol.</param>
		/// <returns>The elements executing the specified protocol.</returns>
		/// <example>
		/// <code>
		/// Elements[] elements = engine.FindElementsByProtocol("Microsoft Platform");
		/// </code>
		/// </example>
		public Element[] FindElementsByProtocol(string name) { return null; }

		/// <summary>
		/// Retrieves all elements sharing the specified protocol and protocol version.
		/// </summary>
		/// <param name="name">The protocol name.</param>
		/// <param name="version">The protocol version.</param>
		/// <returns>The elements executing the specified version of the specified protocol.</returns>
		/// <example>
		/// <code>
		/// Elements[] elements = engine.FindElementsByProtocol("Microsoft Platform","1.1.0.48");
		/// </code>
		/// </example>
		public Element[] FindElementsByProtocol(string name, string version) { return null; }

		/// <summary>
		/// Retrieves all elements in the specified view.
		/// </summary>
		/// <param name="viewID">The view ID.</param>
		/// <returns>The elements in the specified view.</returns>
		/// <example>
		/// <code>
		/// Elements[] elements = engine.FindElementsInView(14);
		/// </code>
		/// </example>
		public Element[] FindElementsInView(int viewID) { return null; }

		/// <summary>
		/// Retrieves all elements in the specified view.
		/// </summary>
		/// <param name="viewName">The view name.</param>
		/// <returns>The elements in the specified view.</returns>
		/// <example>
		/// <code>
		/// Elements[] elements = engine.FindElementsInView("MySpecialElements");
		/// </code>
		/// </example>
		public Element[] FindElementsInView(string viewName) { return null; }

		/// <summary>
		/// Retrieves all elements executing the specified protocol in the specified view.
		/// </summary>
		/// <param name="viewName">The view name.</param>
		/// <param name="protocolName">The protocol name.</param>
		/// <param name="protocolVersion">The protocol version.</param>
		/// <returns>The elements running the specified protocol in the specified view.</returns>
		/// <example>
		/// <code>
		/// Elements[] elements = engine.FindElementsInView("MySpecialElements","Microsoft Platform","1.1.0.48");
		/// </code>
		/// </example>
		public Element[] FindElementsInView(string viewName, string protocolName, string protocolVersion) { return null; }

		/// <summary>
		/// Retrieves all elements executing the specified protocol in the specified view.
		/// </summary>
		/// <param name="viewID">The view ID.</param>
		/// <param name="protocolName">The protocol name.</param>
		/// <param name="protocolVersion">The protocol version.</param>
		/// <returns>The elements running the specified protocol in the specified view.</returns>
		/// <example>
		/// <code>
		/// Elements[] elements = engine.FindElementsInView(14,"Microsoft Platform","1.1.0.48");
		/// </code>
		/// </example>
		public Element[] FindElementsInView(int viewID, string protocolName, string protocolVersion) { return null; }

		/// <summary>
		/// Asks input from a user.
		/// </summary>
		/// <param name="message">The message to be shown.</param>
		/// <param name="timeoutTime">A timeout (in seconds). When this timeout expires, the script will continue and the FindInteractiveClient method will return “False”. The script can then decide how to deal with this result: issue a new request, fail, or execute automatic actions.</param>
		/// <exception cref="DataMinerException">Failed to find interactive client.</exception>
		/// <returns><c>true</c> if attaching to the interactive client succeeded; otherwise, <c>false</c>.</returns>
		/// <remarks>
		/// <para>In an Automation script executed from e.g. a scheduled background task or as a Correlation action, you can use the FindInteractiveClient method to ask for input from a user.</para>
		/// <para>In a message box, the user will be asked to click either Attach or Ignore.</para>
		/// <list type="bullet">
		/// <item><description>If the user clicks Attach, the script will start in a pop-up window.</description></item>
		/// <item><description>If the user clicks Ignore, the message box will be closed.</description></item>
		/// </list>
		/// <note type="note">In DataMiner Cube, you can also use the script action Find interactive client, instead of using C#. For more information, see <see href="xref:AutomationActionFindInteractiveClient">Find interactive client</see>.</note>
		/// </remarks>
		/// <example>
		/// <code>
		/// string allowedGroups = "grpA;grpB";
		/// bool ok = engine.FindInteractiveClient("Hello world", 100 , allowedGroups, AutomationScriptAttachOptions.None);
		/// 
		/// if(!ok)
		/// {
		///		engine.GenerateInformation("Could not attach");
		/// }
		/// else
		/// {
		///		engine.GenerateInformation("Attached! As " + engine.UserDisplayName);
		///		engine.ShowProgress("A message");
		///		engine.ShowUI("Another message", true);
		/// }
		/// </code>
		/// </example>
		public bool FindInteractiveClient(string message, int timeoutTime) { return false; }

		/// <summary>
		/// Asks input from a user.
		/// </summary>
		/// <param name="message">The message to be shown.</param>
		/// <param name="timeoutTime">A timeout (in seconds). When this timeout expires, the script will continue and the FindInteractiveClient method will return “False”. The script can then decide how to deal with this result: issue a new request, fail, or execute automatic actions.</param>
		/// <param name="allowedGroups">Indication of which users will receive the request, i.e. a list of DataMiner security group names, separated by semicolons. In this list of groups, you can also specify individual users. To do so, specify “user:”, followed by the user name.</param>
		/// <exception cref="DataMinerException">Failed to find interactive client.</exception>
		/// <returns><c>true</c> if attaching to the interactive client succeeded; otherwise, <c>false</c>.</returns>
		/// <remarks>
		/// <para>In an Automation script executed from e.g. a scheduled background task or as a Correlation action, you can use the FindInteractiveClient method to ask for input from a user.</para>
		/// <para>In a message box, the user will be asked to click either Attach or Ignore.</para>
		/// <list type="bullet">
		/// <item><description>If the user clicks Attach, the script will start in a pop-up window.</description></item>
		/// <item><description>If the user clicks Ignore, the message box will be closed.</description></item>
		/// </list>
		/// <note type="note">In DataMiner Cube, you can also use the script action Find interactive client, instead of using C#. For more information, see <see href="xref:AutomationActionFindInteractiveClient">Find interactive client</see>.</note>
		/// <para>From DataMiner 9.6.9 (RN 22227) onwards, it is possible to find an interactive client by user cookie instead of by user name.</para>
		/// <code>bool ok = engine.FindInteractiveClient("Some text", 100, "userCookie:" + connection.ConnectionID);</code>
		/// </remarks>
		/// <example>
		/// <code>
		/// string allowedGroups = "grpA;grpB";
		/// bool ok = engine.FindInteractiveClient("Hello world", 100 , allowedGroups, AutomationScriptAttachOptions.None);
		/// if (!ok)
		/// {
		///		engine.GenerateInformation("Could not attach");
		/// }
		/// else
		/// {
		///		engine.GenerateInformation("Attached! As " + engine.UserDisplayName);
		///		engine.ShowProgress("A message");
		///		engine.ShowUI("Another message", true);
		/// }
		/// </code>
		/// </example>
		public bool FindInteractiveClient(string message, int timeoutTime, string allowedGroups) { return false; }

		/// <summary>
		/// Asks input from a user.
		/// </summary>
		/// <param name="message">The message to be shown.</param>
		/// <param name="timeoutTime">A timeout (in seconds). When this timeout expires, the script will continue and the FindInteractiveClient method will return “False”. The script can then decide how to deal with this result: issue a new request, fail, or execute automatic actions.</param>
		/// <param name="allowedGroups">Indication of which users will receive the request, i.e. a list of DataMiner security group names, separated by semicolons. In this list of groups, you can also specify individual users. To do so, specify “user:”, followed by the user name.</param>
		/// <param name="options">Options in the form of a set of binary flags.</param>
		/// <exception cref="DataMinerException">Failed to find interactive client.</exception>
		/// <returns><c>true</c> if attaching to the interactive client succeeded; otherwise, <c>false</c>.</returns>
		/// <remarks>
		/// <para>In an Automation script executed from e.g. a scheduled background task or as a Correlation action, you can use the FindInteractiveClient method to ask for input from a user.</para>
		/// <para>In a message box, the user will be asked to click either Attach or Ignore.</para>
		/// <list type="bullet">
		/// <item><description>If the user clicks Attach, the script will start in a pop-up window.</description></item>
		/// <item><description>If the user clicks Ignore, the message box will be closed.</description></item>
		/// </list>
		/// <note type="note">In DataMiner Cube, you can also use the script action Find interactive client, instead of using C#. For more information, see Find interactive client.</note>
		/// <para>From DataMiner 9.6.9 (RN 22227) onwards, it is possible to find an interactive client by user cookie instead of by user name.</para>
		/// <code>bool ok = engine.FindInteractiveClient("Some text", 100, "userCookie:" + connection.ConnectionID, AutomationScriptAttachOptions.None);</code>
		/// </remarks>
		/// <example>
		/// <code>
		/// string allowedGroups = "grpA;grpB";
		/// bool ok = engine.FindInteractiveClient("Hello world", 100 , allowedGroups, AutomationScriptAttachOptions.None);
		/// if (!ok)
		/// {
		///		engine.GenerateInformation("Could not attach");
		/// }
		/// else
		/// {
		///		engine.GenerateInformation("Attached! As " + engine.UserDisplayName);
		///		engine.ShowProgress("A message");
		///		engine.ShowUI("Another message", true);
		/// }
		/// </code>
		/// </example>
		public bool FindInteractiveClient(string message, int timeoutTime, string allowedGroups, AutomationScriptAttachOptions options) { return false; }

		/// <summary>
		/// Retrieves the redundancy group with the specified name.
		/// </summary>
		/// <param name="name">The redundancy group name.</param>
		/// <returns>The redundancy group with the specified name or <see langword="null"/> if no redundancy group with the specified name exists..</returns>
		/// <remarks>
		/// <para>Name masks can contain the following wildcards:</para>
		/// <list type="bullet">
		/// <item><description>* : Any string of characters</description></item>
		/// <item><description>? : Any single character</description></item>
		/// </list>
		/// </remarks>
		/// <example>
		/// <code>
		/// RedundancyGroup group = engine.FindRedundancyGroup("MySpecialRedundancyGroup");
		/// </code>
		/// </example>
		public RedundancyGroup FindRedundancyGroup(string name) { return null; }

		/// <summary>
		/// Retrieves the redundancy group with the specified DMA ID and group ID.
		/// </summary>
		/// <param name="dmaID">The DMA ID of the redundancy group.</param>
		/// <param name="groupID">The group ID of the redundancy group.</param>
		/// <returns>The redundancy group with the specified DMA ID and group ID or <see langword="null"/> if no redundancy group with the specified ID exists.</returns>
		/// <example>
		/// <code>
		/// RedundancyGroup group = engine.FindRedundancyGroup(7,24);
		/// </code>
		/// </example>
		public RedundancyGroup FindRedundancyGroup(int dmaID, int groupID) { return null; }

		/// <summary>
		/// Retrieves the redundancy group with the specified key.
		/// </summary>
		/// <param name="key">The key of the redundancy group. The key must be formatted as follows: "DataMiner Agent ID/redundancy group ID".</param>
		/// <returns>The redundancy group with the specified key or <see langword="null"/> if no redundancy group with the specified key exists.</returns>
		/// <example>
		/// <code>
		/// RedundancyGroup group = engine.FindRedundancyGroupByKey("7/24");
		/// </code>
		/// </example>
		public RedundancyGroup FindRedundancyGroupByKey(string key) { return null; }

		/// <summary>
		/// Retrieves all redundancy groups matching the specified filter.
		/// </summary>
		/// <param name="filter">The redundancy group filter.</param>
		/// <returns>The redundancy groups matching the specified filter.</returns>
		/// <example>
		/// <code>
		/// RedundancyGroupFilter myGroupFilter = RedundancyGroupFilter.ByView("MyView");
		/// RedundancyGroup[] groups = engine.FindRedundancyGroups(myGroupFilter);
		/// </code>
		/// </example>
		public RedundancyGroup[] FindRedundancyGroups(RedundancyGroupFilter filter) { return null; }

		/// <summary>
		/// Retrieves all redundancy groups of which the name matches the specified name mask.
		/// </summary>
		/// <param name="nameFilter">The name filter.</param>
		/// <returns>The redundancy groups of which the name matches the specified name mask.</returns>
		/// <remarks>
		/// <para>Name masks can contain the following wildcards:</para>
		/// <list type="bullet">
		/// <item><description>* : Any string of characters</description></item>
		/// <item><description>? : Any single character</description></item>
		/// </list>
		/// </remarks>
		/// <example><code>RedundancyGroup[] groups = engine.FindRedundancyGroupsByName("Test*");</code></example>
		public RedundancyGroup[] FindRedundancyGroupsByName(string nameFilter) { return null; }

		/// <summary>
		/// Retrieves all redundancy groups in the specified view.
		/// </summary>
		/// <param name="viewName">The view name.</param>
		/// <returns>The redundancy groups in the specified view.</returns>
		/// <example>
		/// <code>
		/// RedundancyGroup[] groups = engine.FindRedundancyGroupsInView("MySpecialView");
		/// </code>
		/// </example>
		public RedundancyGroup[] FindRedundancyGroupsInView(string viewName) { return null; }

		/// <summary>
		/// Retrieves all redundancy groups in the specified view.
		/// </summary>
		/// <param name="viewID">The view ID.</param>
		/// <returns>The redundancy groups in the specified view.</returns>
		/// <example>
		/// <code>
		/// RedundancyGroup[] groups = engine.FindRedundancyGroupsInView(17);
		/// </code>
		/// </example>
		public RedundancyGroup[] FindRedundancyGroupsInView(int viewID) { return null; }

		/// <summary>
		/// Retrieves the service with the specified name.
		/// </summary>
		/// <param name="name">The service name.</param>
		/// <returns>The service with the specified name or <see langword="null"/> if the service was not found.</returns>
		/// <example>
		/// <code>
		/// Service myService = engine.FindService("MyService");
		/// </code>
		/// </example>
		public Service FindService(string name) { return null; }

		/// <summary>
		/// Retrieves the service with the specified ID. 
		/// </summary>
		/// <param name="dmaID">The DMA ID of the service.</param>
		/// <param name="serviceID">The ID of the service.</param>
		/// <returns>The service with the specified ID or <see langword="null"/> if the service was not found.</returns>
		/// <example>
		/// <code>
		/// Service myService = engine.FindService(3, 56);
		/// </code>
		/// </example>
		public Service FindService(int dmaID, int serviceID) { return null; }

		/// <summary>
		/// Retrieves the service with the specified key.
		/// </summary>
		/// <param name="key">The key of the service. The key must be formatted as follows: "DMA ID/service ID".</param>
		/// <returns>The service with the specified key or <see langword="null"/> if the service was not found.</returns>
		/// <example>
		/// <code>
		/// Service myService = engine.FindServiceByKey("3/56");
		/// </code>
		/// </example>
		public Service FindServiceByKey(string key) { return null; }

		/// <summary>
		/// Retrieves all services matching the specified filter.
		/// </summary>
		/// <param name="filter">The service filter.</param>
		/// <returns>The services matching the specified filter.</returns>
		/// <example>
		/// <code>
		/// ServiceFilter myServiceFilter = ServiceFilter.ByView("MyView");
		/// Service[] services = engine.FindServices(myServiceFilter);
		/// </code>
		/// </example>
		public Service[] FindServices(ServiceFilter filter) { return null; }

		/// <summary>
		/// Retrieves all services of which the name matches the specified name filter.
		/// </summary>
		/// <param name="nameFilter">The name filter.</param>
		/// <returns>The services of which the name matches the specified name filter.</returns>
		/// <remarks>
		/// <para>Name masks can contain the following wildcards:</para>
		/// <list type="bullet">
		/// <item><description>* : Any string of characters</description></item>
		/// <item><description>? : Any single character</description></item>
		/// </list>
		/// </remarks>
		/// <example>
		/// <code>
		/// Service[] services = engine.FindServicesByName("Test*");
		/// </code>
		/// </example>
		public Service[] FindServicesByName(string nameFilter) { return null; }

		/// <summary>
		/// Retrieves all services in the view with the specified name.
		/// </summary>
		/// <param name="viewName">The view name.</param>
		/// <returns>The services in the specified view.</returns>
		/// <example>
		/// <code>
		/// Service[] services = engine.FindServicesInView("MySpecialView");
		/// </code>
		/// </example>
		public Service[] FindServicesInView(string viewName) { return null; }

		/// <summary>
		/// Retrieves all services in the view with the specified ID.
		/// </summary>
		/// <param name="viewID">The view ID.</param>
		/// <returns>The services in the specified view.</returns>
		/// <example>
		/// <code>
		/// Service[] services = engine.FindServicesInView(19);
		/// </code>
		/// </example>
		public Service[] FindServicesInView(int viewID) { return null; }

		/// <summary>
		/// Retrieves the service template with the specified DataMiner Agent ID and service template ID.
		/// </summary>
		/// <param name="dmaId">The DataMiner Agent ID.</param>
		/// <param name="serviceTemplateId">The service template ID.</param>
		/// <returns>The service matching the specified DataMiner Agent ID and service template ID or <see langword="null"/> if the service template is not found.</returns>
		/// <example>
		/// <code>
		/// Service service = engine.FindServiceTemplate(200, 400);
		/// </code>
		/// </example>
		public Service FindServiceTemplate(int dmaId, int serviceTemplateId) { return null; }

		/// <summary>
		/// Retrieves the service templates matching the specified service filter.
		/// </summary>
		/// <param name="filter">The service filter.</param>
		/// <returns>The service templates matching the specified service filter.</returns>
		/// <example>
		/// <code>
		/// ServiceFilter filter = new ServiceFilter{ CriticalOnly = true};
		/// Service[] services = engine.FindServiceTemplates(filter);
		/// </code>
		/// </example>
		public Service[] FindServiceTemplates(ServiceFilter filter) { return null; }

		/// <summary>
		/// Generates an information message with the specified text.
		/// </summary>
		/// <param name="text">Message to be shown in the information message.</param>
		/// <exception cref="DataMinerException">Information event generation failed.</exception>
		/// <example>
		/// <code>
		/// engine.GenerateInformation("Hello World!");
		/// </code>
		/// </example>
		/// <remarks>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</remarks>
		public void GenerateInformation(string text) { }

        /// <summary>
        /// Retrieves the value of the specified custom alarm property.
        /// </summary>
        /// <param name="alarmID">The alarm ID</param>
        /// <param name="propertyName">The name of the alarm property.</param>
        /// <exception cref="ArgumentException">Alarm not found.</exception> 
        /// <returns>The value of the specified alarm property.</returns>
        /// <example>
        /// <code>
        /// string propertyValue = engine.GetAlarmProperty(new AlarmID(new AlarmTreeID(200, 400, 59851), 59853), "SourceDetail");
        /// </code>
        /// </example>
        public string GetAlarmProperty(AlarmID alarmID, string propertyName) { return null; }

        /// <summary>
        /// Retrieves the value of the specified custom alarm property.
        /// </summary>
        /// <param name="dataMinerID">The DataMiner Agent ID.</param>
        /// <param name="alarmID">The alarm ID.</param>
        /// <param name="propertyName">The name of the alarm property.</param>
        /// <exception cref="ArgumentException">Alarm not found.</exception>
        /// <returns>The value of the specified alarm property.</returns>
        /// <remarks>Although this method is obsolete, it will still work on a DMS without Swarming enabled until 10.6.</remarks>
        /// <example>
        /// <code>
        /// string propertyValue = engine.GetAlarmProperty(200, 59851, "SourceDetail");
        /// </code>
        /// </example>
        [Obsolete("Please use the overload that takes an AlarmID object (still supported in <= 10.6 on non-swarming systems)")]
        public string GetAlarmProperty(int dataMinerID, int alarmID, string propertyName) { return null; }

        /// <summary>
        /// Retrieves the value of the specified custom alarm property.
        /// </summary>
        /// <param name="dataMinerID">The DataMiner Agent ID.</param>
        /// <param name="elementID">The element ID.</param>
        /// <param name="alarmID">The alarm ID.</param>
        /// <param name="propertyName">The name of the alarm property.</param>
        /// <exception cref="ArgumentException">Alarm not found.</exception> 
        /// <returns>The value of the specified alarm property.</returns>
        /// <remarks>For performance reasons, we recommend using the overload that takes an AlarmID instead.</remarks>
        /// <example>
        /// <code>
        /// string propertyValue = engine.GetAlarmProperty(200, 400, 59851, "SourceDetail");
        /// </code>
        /// </example>
        public string GetAlarmProperty(int dataMinerID, int elementID, int alarmID, string propertyName) { return null; }

		/// <summary>
		/// Retrieves an object representing one of the script dummies.
		/// </summary>
		/// <param name="id">The dummy ID.</param>
		/// <returns>The object representing the specified script dummy or <see langword="null"/> if the dummy with the specified ID is not found.</returns>
		/// <remarks><para>Through this object, actions like "set parameter" can be executed.</para></remarks>
		/// <example>
		/// <code>
		/// ScriptDummy dummyTest = engine.GetDummy(5);
		/// </code>
		/// </example>
		public ScriptDummy GetDummy(int id) { return null; }

		/// <summary>
		/// Retrieves an object representing one of the script dummies.
		/// </summary>
		/// <param name="name">The dummy name.</param>
		/// <returns>The object representing the specified script dummy or <see langword="null"/> if the dummy with the specified name is not found.</returns>
		/// <remarks><para>Through this object, actions like "set parameter" can be executed.</para></remarks>
		/// <example>
		/// <code>
		/// ScriptDummy dummyTest = engine.GetDummy("matrix");
		/// </code>
		/// </example>
		public ScriptDummy GetDummy(string name) { return null; }

		/// <summary>
		/// Retrieves an object representing one of the script’s memory files. Through this object, data can be read from or written into the memory file.
		/// </summary>
		/// <param name="name">The name of the memory file.</param>
		/// <returns>The object representing the specified script memory file or <see langword="null"/> if the memory with the specified name is not found.</returns>
		/// <example>
		/// <code>
		/// ScriptMemory memoryData = engine.GetMemory("name");
		/// </code>
		/// </example>
		public ScriptMemory GetMemory(string name) { return null; }

		/// <summary>
		/// Retrieves an object representing one of the script’s memory files. Through this object, data can be read from or written into the memory file.
		/// </summary>
		/// <param name="id">The ID of the memory file.</param>
		/// <returns>The object representing the specified script memory file or <see langword="null"/> if the memory with the specified ID is not found.</returns>
		/// <example>
		/// <code>
		/// ScriptMemory memoryData = engine.GetMemory(3);
		/// </code>
		/// </example>
		public ScriptMemory GetMemory(int id) { return null; }

		/// <summary>
		/// Retrieves an object representing a script parameter. Through this object, its value can be retrieved.
		/// </summary>
		/// <param name="name">The name of the script parameter.</param>
		/// <returns>The specified script parameter or <see langword="null"/> if the script parameter with the specified name is not found.</returns>
		/// <example>
		/// <code>
		/// ScriptParam param = engine.GetScriptParam("input");
		/// </code>
		/// </example>
		public ScriptParam GetScriptParam(string name) { return null; }

		/// <summary>
		/// Retrieves an object representing a script parameter. Through this object, its value can be retrieved.
		/// </summary>
		/// <param name="id">The ID of the script parameter.</param>
		/// <returns>The specified script parameter or <see langword="null"/> if the script parameter with the specified ID is not found..</returns>
		/// <example>
		/// <code>
		/// ScriptParam param = engine.GetScriptParam(5);
		/// </code>
		/// </example>
		public ScriptParam GetScriptParam(int id) { return null; }

		/// <summary>
		/// Returns a copy of the script output of the current script and, if the <see cref="SubScriptOptions.InheritScriptOutput"/> option is set to “true”, the child scripts.
		/// </summary>
		/// <returns>The script results.</returns>
		/// <remarks>
		/// <para>Feature introduced in DataMiner 10.0.2 (RN 23936).</para>
		/// <para>When a subscript fails or throws an exception, its script output will still be filled in.</para>
		/// </remarks>
		public virtual Dictionary<string, string> GetScriptResult()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			//Dictionary<string, string>.KeyCollection.Enumerator enumerator = this._scriptOutput.Keys.GetEnumerator();
			//if (enumerator.MoveNext())
			//{
			//	do
			//	{
			//		string current = enumerator.Current;
			//		dictionary.Add(current, this._scriptOutput[current]);
			//	}
			//	while (enumerator.MoveNext());
			//}
			return dictionary;
		}

		/// <summary>
		/// Returns the script output of the specified key.
		/// </summary>
		/// <param name="key">The key of the entry for which the value should be retrieved.</param>
		/// <returns>The value of the specified key or <see langword="null"/> when the specified key cannot be found.</returns>
		/// <remarks>
		/// <para>Feature introduced in DataMiner 10.0.2 (RN 23936).</para>
		/// <para>When a subscript fails or throws an exception, its script output will still be filled in.</para>
		/// </remarks>
		public virtual string GetScriptOutput(string key)
		{
			//Dictionary<string, string> scriptOutput = this._scriptOutput;
			//if (scriptOutput == null)
			//	return (string)null;
			//if (!scriptOutput.ContainsKey(key))
			//	return (string)null;
			//return this._scriptOutput[key];

			return null;
		}

		/// <summary>
		/// Retrieves a connection representing the user that executed the Automation script.
		/// </summary>
		/// <returns>A connection representing the user that executed the Automation script</returns>
		/// <remarks>
		/// <para>In case of an interactive Automation script, the connection represents the user that is interacting with the interactive script. If the script was triggered by DataMiner instead of a user, the connection represents the built-in ManagedAutomation user.</para>
		/// <para>Feature introduced in DataMiner 10.0.10 (RN 26434).</para>
		/// </remarks>
		/// <example>
		/// <code>
		/// using(var logHelper = LogHelper.Create(engine.GetUserConnection()))
		/// {
		///		// ...
		///	}
		/// </code>
		/// </example>
		public IConnection GetUserConnection() { return null; }

		/// <summary>
		/// Hides a custom-made dialog box of an interactive Automation script.
		/// </summary>
		/// <exception cref="InteractiveUserDetachedException">The interactive client was detached.</exception>
		/// <exception cref="DataMinerException">Hide UI failed.</exception>
		/// <para>Feature introduced in DataMiner 10.4.7 (RN 39451/RN 39638).</para>
		/// <para>This feature is only available for interactive Automation scripts executed in a web environment.</para>
		/// <example>
		/// <code>
		/// // Build and display a form
		/// var formUi = BuildFormUi();
		/// var results = engine.ShowUI(formUi);
		/// 
		/// // Process UI results
		/// 
		/// // Hide the UI before starting a lengthy operation
		/// engine.HideUI();
		/// 
		/// // Build and display issue information
		/// var issueUi = BuildIssueUi();
		/// var issueResults = engine.ShowUI(issueUi);
		/// </code>
		/// </example>
		public void HideUI() { }

		/// <summary>
		/// Resets the timeout timer, extending the time the Automation script is allowed to execute.
		/// The time can be specified via the <see cref="Timeout"/> property.
		/// </summary>
		/// <remarks>When a script reaches the timeout, a <see cref="ScriptTimeoutException"/> will be thrown to stop the execution.</remarks>
		public void KeepAlive() { }

		/// <summary>
		/// Retrieves a double value from a global script variable.
		/// </summary>
		/// <param name="name">The name of the global script variable.</param>
		/// <exception cref="DataMinerException">Load value failed.</exception>
		/// <returns>The value of the global script variable.</returns>
		/// <example>
		/// <code>
		/// double myValue = engine.LoadDoubleValue("MyVariable");
		/// </code>
		/// </example>
		public double LoadDoubleValue(string name) { return 0.0; }

		/// <summary>
		/// Retrieves a string value from a global script variable.
		/// </summary>
		/// <param name="name">The name of the global script variable.</param>
		/// <exception cref="DataMinerException">Load value failed.</exception>
		/// <returns>The value of the global script variable.</returns>
		/// <example>
		/// <code>
		/// string myValue = engine.LoadStringValue("MyVariable");
		/// </code>
		/// </example>
		public string LoadStringValue(string name) { return null; }

		/// <summary>
		/// Retrieves a value from a global script variable.
		/// </summary>
		/// <param name="name">The name of the global script variable.</param>
		/// <returns>The value of the global script variable.</returns>
		/// <exception cref="DataMinerException">Load value failed.</exception>
		/// <example>
		/// <code>
		/// object myValue = engine.LoadValue("MyVariable");
		/// </code>
		/// </example>
		public object LoadValue(string name) { return null; }

		/// <summary>
		/// Adds an entry in the SLAutomation.txt log file.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <exception cref="DataMinerException">Log failed.</exception>
		/// <example>
		/// <code>
		/// engine.Log("My log message");
		/// </code>
		/// </example>
		/// <remarks>
		/// <list type="bullet">
		/// <item><description>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</description></item>
		/// <item><description>From DataMiner 10.5.6/10.6.0 onwards (RN 42572), logging will also be added in a dedicated log file with the name of the Automation script. For additional information, see <see href="xref:Log">Log</see>.</description></item>
		/// <item><description>To configure a custom log level for a specific script in its dedicated log file, send an UpdateLogfileSettingMessage in which Name is set to "Automation\ScriptName". If no custom log configuration exists for a particular Automation script, the default configuration will be used.</description></item>
		/// </list>
		/// </remarks>
		public void Log(string message) { }

		/// <summary>
		/// Adds an entry in the SLAutomation.txt log file.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="type">The log type.</param>
		/// <param name="logLevel">The log level.</param>
		/// <exception cref="DataMinerException">Log failed.</exception>
		/// <example>
		/// <code>
		/// engine.Log("My log message", LogType.Always, 5);
		/// </code>
		/// </example>
		/// <remarks>
		/// <list type="bullet">
		/// <item><description>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</description></item>
		/// <item><description>From DataMiner 10.5.6/10.6.0 onwards (RN 42572), logging will also be added in a dedicated log file with the name of the Automation script. For additional information, see <see href="xref:Log">Log</see>.</description></item>
		/// <item><description>To configure a custom log level for a specific script in its dedicated log file, send an UpdateLogfileSettingMessage in which Name is set to "Automation\ScriptName". If no custom log configuration exists for a particular Automation script, the default configuration will be used.</description></item>
		/// </list>
		/// </remarks>
		public void Log(string message, LogType type, int logLevel) { }

		/// <summary>
		/// Adds an entry in the SLAutomation.txt log file.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="type">The log type.</param>
		/// <param name="logLevel">The log level.</param>
		/// <param name="method">The method name to include in the log entry.</param>
		/// <exception cref="DataMinerException">Log failed.</exception>
		/// <example>
		/// <code>
		/// engine.Log("My log message", LogType.Always, 5, "Initialize");
		/// </code>
		/// </example>
		/// <remarks>
		/// <list type="bullet">
		/// <item><description>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</description></item>
		/// <item><description>From DataMiner 10.5.6/10.6.0 onwards (RN 42572), logging will also be added in a dedicated log file with the name of the Automation script. For additional information, see <see href="xref:Log">Log</see>.</description></item>
		/// <item><description>To configure a custom log level for a specific script in its dedicated log file, send an UpdateLogfileSettingMessage in which Name is set to "Automation\ScriptName". If no custom log configuration exists for a particular Automation script, the default configuration will be used.</description></item>
		/// </list>
		/// </remarks>
		public void Log(string message, LogType type, int logLevel, string method) { }

		/// <summary>
		/// Returns a <see cref="MailReportOptions" /> object, which you can use to configure and launch an email report.
		/// </summary>
		/// <param name="mailReport">The mail report to prepare.</param>
		/// <returns>The <see cref="MailReportOptions" /> object.</returns>
		/// <exception cref="ArgumentException"><paramref name="mailReport"/> is <see langword="null"/> or the empty string ("").</exception>
		/// <example>
		/// <code>
		/// MailReportOptions reportOptions;
		/// reportOptions = engine.PrepareMailReport("myReportTemplate");
		/// reportOptions.EmailOptions.SendAsPlainText = true;
		/// ...
		/// engine.SendReport(reportOptions);
		/// </code>
		/// </example>
		public MailReportOptions PrepareMailReport(string mailReport) { return null; }

		/// <summary>
		/// Returns a <see cref="SubScriptOptions" /> object, which you can use to configure and launch a subscript.
		/// </summary>
		/// <param name="scriptName">The name of the script to prepare.</param>
		/// <exception cref="ArgumentNullException"><paramref name="scriptName"/> is <see langword="null"/>.</exception>
		/// <returns>The <see cref="SubScriptOptions" /> object.</returns>
		/// <example>
		/// <code>
		/// SubScriptOptions subscriptInfo;
		/// subscriptInfo = engine.PrepareSubScript("myOtherScript");
		/// subscriptInfo.Synchronous = true;
		/// ...
		/// subscriptInfo.StartScript();
		/// </code>
		/// </example>
		public SubScriptOptions PrepareSubScript(string scriptName) { return null; }

		/// <inheritdoc cref="IEngine.PrepareSubScript(string, RequestScriptInfoInput)"/>
		public RequestScriptInfoSubScriptOptions PrepareSubScript(string scriptName, RequestScriptInfoInput input) { return null; }

		/// <summary>
		/// Launches an application on the client in an interactive script.
		/// </summary>
		/// <param name="applicationPath">The application path.</param>
		/// <exception cref="InteractiveUserDetachedException">The interactive client was detached.</exception>
		/// <returns>The UI result.</returns>
		public UIResults RunClientProgram(string applicationPath) { return null; }

		/// <summary>
		/// Launches an application on the client in an interactive script.
		/// </summary>
		/// <param name="applicationPath">The application path.</param>
		/// <param name="waitForCompletion">When set to <c>true</c>, the script will halt until the client application has completed or has been closed. In the interactive window, users will see the message “Wait for client program to finish”.</param>
		/// <exception cref="InteractiveUserDetachedException">The interactive client was detached.</exception>
		/// <returns>The UI result.</returns>
		public UIResults RunClientProgram(string applicationPath, bool waitForCompletion) { return null; }

		/// <summary>
		/// Launches an application on the client in an interactive script.
		/// </summary>
		/// <param name="applicationPath">The application path.</param>
		/// <param name="arguments">The arguments.</param>
		/// <exception cref="InteractiveUserDetachedException">The interactive client was detached.</exception>
		/// <returns>The UI result.</returns>
		/// <example>
		/// <code>
		/// engine.RunClientProgram("notepad.exe", @"C:\skyline dataminer\logging\slerrors.txt");
		/// </code>
		/// </example>
		public UIResults RunClientProgram(string applicationPath, string arguments) { return null; }

		/// <summary>
		/// Launches an application on the client in an interactive script.
		/// </summary>
		/// <param name="applicationPath">The application path.</param>
		/// <param name="arguments">The arguments.</param>
		/// <param name="waitForCompletion">When set to <c>true</c>, the script will halt until the client application has completed or has been closed. In the interactive window, users will see the message “Wait for client program to finish”.</param>
		/// <exception cref="InteractiveUserDetachedException">The interactive client was detached.</exception>
		/// <returns>The UI result.</returns>
		/// <example>
		/// <code>
		/// engine.RunClientProgram(@"C:\Skyline DataMiner\Tools\SLTaskbarUtility\SLTaskbarUtility.exe", @"\h", true);
		/// </code>
		/// </example>
		public UIResults RunClientProgram(string applicationPath, string arguments, bool waitForCompletion) { return null; }

		/// <summary>
		/// Saves a value to a global script variable. This value can then be reused elsewhere in the same script.
		/// </summary>
		/// <param name="name">The name of the global script variable.</param>
		/// <param name="value">The value to set.</param>
		/// <exception cref="DataMinerException">Save value failed.</exception>
		/// <example>
		/// <code>
		/// engine.SaveValue("MyVariable", 10.8);
		/// </code>
		/// </example>
		public void SaveValue(string name, double value) { }

		/// <summary>
		/// Saves a value to a global script variable. This value can then be reused elsewhere in the same script.
		/// </summary>
		/// <param name="name">The name of the global script variable.</param>
		/// <param name="value">The value to set.</param>
		/// <exception cref="DataMinerException">Save value failed.</exception>
		/// <example>
		/// <code>
		/// engine.SaveValue("MyVariable", "MyValue");
		/// </code>
		/// </example>
		public void SaveValue(string name, string value) { }

		/// <summary>
		/// Sends an email message.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <exception cref="DataMinerException">Sending failed.</exception>
		/// <example>
		/// <code>
		/// EmailOptions myEmailOptions;
		/// ...
		/// engine.SendEmail(myEmailOptions);
		/// </code>
		/// </example>
		/// <remarks>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</remarks>
		public void SendEmail(EmailOptions options) { }

		/// <summary>
		/// Sends an email message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="title">The title.</param>
		/// <param name="to">The recipients.</param>
		/// <remarks>
		/// <exception cref="DataMinerException">Sending failed.</exception>
		/// <para>To specify the recipient, use</para>
		/// <list type="bullet">
		/// <item><description>an email address,</description></item>
		/// <item><description>USER:, followed by the name of a DataMiner user, or</description></item>
		/// <item><description>GROUP:, followed by the name of a DataMiner user group.</description></item>
		/// </list>
		/// </remarks>
		/// <example>
		/// <code>
		/// engine.SendEmail("The message I want to send.","The title of my message","support@gtc.com");
		/// </code>
		/// </example>
		/// <remarks>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</remarks>
		public void SendEmail(string message, string title, string to) { }

		/// <summary>
		/// Sends a pager message.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <exception cref="DataMinerException">Sending failed.</exception>
		/// <example>
		/// <code>
		/// PagerOptions myPagerOptions;
		/// ...
		/// engine.SendPager(myPagerOptions);
		/// </code>
		/// </example>
		/// <remarks>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</remarks>
		public void SendPager(PagerOptions options) { }

		/// <summary>
		/// Sends a pager message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="to">The recipients.</param>
		/// <exception cref="DataMinerException">Sending failed.</exception>
		/// <remarks>
		/// <para>To specify the recipient, use</para>
		/// <list type="bullet">
		/// <item><description>a pager number,</description></item>
		/// <item><description>USER:, followed by the name of a DataMiner user, or</description></item>
		/// <item><description>GROUP:, followed by the name of a DataMiner user group.</description></item>
		/// </list>
		/// </remarks>
		/// <example>
		/// <code>
		/// engine.SendPager("The message I want to send.","USER:ADMIN");
		/// </code>
		/// </example>
		/// <remarks>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</remarks>
		public void SendPager(string message, string to) { }

		/// <summary>
		/// Sends an email report.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <remarks>
		/// <para>To create a MailReportOptions object, call the PrepareMailReport method.</para>
		/// </remarks>
		/// <exception cref="DataMinerException">Report failed.</exception>
		/// <example>
		/// <code>
		/// MailReportOptions reportOptions;
		/// reportOptions = engine.PrepareMailReport("myReportTemplate");
		/// reportOptions.EmailOptions.SendAsPlainText = true;
		/// ...
		/// engine.SendReport(reportOptions);
		/// </code>
		/// </example>
		/// <remarks>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</remarks>
		public void SendReport(MailReportOptions options) { }

		/// <summary>
		/// Sends the specified message to the SLNet process.
		/// </summary>
		/// <param name="message">The SLNet message to send.</param>
		/// <returns>The response message(s) from SLNet.</returns>
		/// <remarks>
		/// <note type="warning">
		/// SLNet messages are subject to change. Therefore, it is not recommended to use these as breaking changes might be introduced in new versions of DataMiner.
		/// </note>
		/// </remarks>
		public virtual DMSMessage[] SendSLNetMessage(DMSMessage message) { return null; }

		/// <summary>
		/// Sends the specified messages to the SLNet process.
		/// </summary>
		/// <param name="messages">The SLNet messages to send.</param>
		/// <returns>The response message(s) from SLNet.</returns>
		/// <remarks>
		/// <note type="warning">
		/// SLNet messages are subject to change. Therefore, it is not recommended to use these as breaking changes might be introduced in new versions of DataMiner.
		/// </note>
		/// <para>Feature introduced in DataMiner 10.0.1 (RN 23981).</para>
		/// </remarks>
		public virtual DMSMessage[] SendSLNetMessages(DMSMessage[] messages)
        {
            return null;
        }

		/// <summary>
		/// Sends the specified message to the SLNet process.
		/// </summary>
		/// <param name="message">The SLNet message to send.</param>
		/// <returns>The response message from SLNet.</returns>
		/// <remarks>
		/// <note type="warning">
		/// SLNet messages are subject to change. Therefore, it is not recommended to use these as breaking changes might be introduced in new versions of DataMiner.
		/// </note>
		/// </remarks>
		public virtual DMSMessage SendSLNetSingleResponseMessage(DMSMessage message) { return null; }

		/// <summary>
		/// Sends a text message (SMS).
		/// </summary>
		/// <param name="options">The options.</param>
		/// <exception cref="DataMinerException">Sending failed.</exception>
		/// <example>
		/// <code>
		/// SmsOptions mySmsOptions;
		/// ...
		/// engine.SendSms(mySmsOptions);
		/// </code>
		/// </example>
		/// <remarks>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</remarks>
		public void SendSms(SmsOptions options) { }

		/// <summary>
		/// Sends a text message (SMS).
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="to">The recipients.</param>
		/// <exception cref="DataMinerException">Sending failed.</exception>
		/// <remarks>
		/// <list type="bullet">
		/// <item><description>a mobile number,</description></item>
		/// <item><description>USER:, followed by the name of a DataMiner user, or</description></item>
		/// <item><description>GROUP:, followed by the name of a DataMiner user group.</description></item>
		/// </list>
		/// </remarks>
		/// <example>
		/// <code>
		/// engine.SendSms("My Message","USER:ADMIN");
		/// </code>
		/// </example>
		/// <remarks>In DataMiner 10.4.0 [CU10]/10.5.1 (RN 41195), some changes are introduced in the locking behavior of this Automation script action. From these versions onward, text that supports the dummy placeholder will display the old element name if it is updated during the execution of a script, or it will still display the element name even if the element has been deleted in the meantime.</remarks>
		public void SendSms(string message, string to) { }

		/// <summary>
		/// Sets the specified custom alarm properties to the specified values.
		/// </summary>
		/// <param name="alarmTreeID">The alarm tree to update.</param>
		/// <param name="propertyNames">The names of the properties.</param>
		/// <param name="propertyValues">The values of the properties.</param>
		/// <remarks>
		/// <note type="note">
		/// <list type="bullet">
		/// <item><description>In DataMiner versions prior to 9.0, this method cannot be used to override alarm property values that are defined in the element protocol.</description></item>
		/// <item><description>When an alarm property value has been defined in the element protocol, and this method is used to explicitly assign a new value to the property, the new value will only be retained until the severity of the alarm changes. After this, the value from the protocol is used again.</description></item>
		/// </list>
		/// </note>
		/// </remarks>
		/// <example>
		/// <code>
		/// engine.SetAlarmProperties(new AlarmTreeID(200, 400, 521655), new string[]{"Property A", "Property B"}, new string[]{"Value A", "Value B"});
		/// </code>
		/// </example>
		public void SetAlarmProperties(AlarmTreeID alarmTreeID, string[] propertyNames, string[] propertyValues) { }

        /// <summary>
        /// Sets the specified custom alarm properties to the specified values.
        /// </summary>
        /// <param name="dataMinerID">The DataMiner Agent ID.</param>
        /// <param name="alarmID">The alarm ID.</param>
        /// <param name="propertyNames">The names of the properties.</param>
        /// <param name="propertyValues">The values of the properties.</param>
        /// <remarks>
        /// <note type="note">
        /// <list type="bullet">
        /// <item><description>Feature introduced in DataMiner 8.5.2 (RN 8347).</description></item>
        /// <item><description>In DataMiner versions prior to 9.0, this method cannot be used to override alarm property values that are defined in the element protocol.</description></item>
        /// <item><description>When an alarm property value has been defined in the element protocol, and this method is used to explicitly assign a new value to the property, the new value will only be retained until the severity of the alarm changes. After this, the value from the protocol is used again.</description></item>
        /// <item><description>Although this method is obsolete, it will still work on a DMS without Swarming enabled until 10.6.</description></item>
        /// </list>
        /// </note>
        /// </remarks>
        /// <example>
        /// <code>
        /// engine.SetAlarmProperties(200, 521655, new string[]{"Property A", "Property B"}, new string[]{"Value A", "Value B"});
        /// </code>
        /// </example>
        [Obsolete("Please use the overload that takes an AlarmTreeID (still supported in <= 10.6 on non-swarming systems)")]
        public void SetAlarmProperties(int dataMinerID, int alarmID, string[] propertyNames, string[] propertyValues) { }

        /// <summary>
        /// Sets the specified custom alarm properties to the specified values.
        /// </summary>
        /// <param name="dataMinerID">The DataMiner Agent ID.</param>
        /// <param name="elementID">The element ID.</param>
        /// <param name="alarmID">The alarm ID.</param>
        /// <param name="propertyNames">The names of the properties.</param>
        /// <param name="propertyValues">The values of the properties.</param>
        /// <remarks>
        /// <note type="note">
        /// <list type="bullet">
        /// <item><description>In DataMiner versions prior to 9.0, this method cannot be used to override alarm property values that are defined in the element protocol.</description></item>
        /// <item><description>When an alarm property value has been defined in the element protocol, and this method is used to explicitly assign a new value to the property, the new value will only be retained until the severity of the alarm changes. After this, the value from the protocol is used again.</description></item>
        /// <item><description>For performance reasons, we recommend using the overload that takes an AlarmTreeID instead.</description></item>
        /// </list>
        /// </note>
        /// </remarks>
        /// <example>
        /// <code>
        /// engine.SetAlarmProperties(200, 400, 521655, new string[]{"Property A", "Property B"}, new string[]{"Value A", "Value B"});
        /// </code>
        /// </example>
        public void SetAlarmProperties(int dataMinerID, int elementID, int alarmID, string[] propertyNames, string[] propertyValues) { }

        /// <summary>
        /// Updates a custom alarm property.
        /// </summary>
        /// <param name="alarmTreeID">The alarm tree to update.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="propertyValue">The value to set.</param>
        /// <remarks>
        /// <note type="note">
        /// <list type="bullet">
        /// <item><description>When an alarm property value has been defined in the element protocol, and this method is used to explicitly assign a new value to the property, the new value will only be retained until the severity of the alarm changes. After this, the value from the protocol is used again.</description></item>
        /// </list>
        /// </note>
        /// </remarks>
        /// <example>
        /// <code>
        /// engine.SetAlarmProperty(new AlarmTreeID(200, 456, 521655), "Property A", "Value A");
        /// </code>
        /// </example>
        public void SetAlarmProperty(AlarmTreeID alarmTreeID, string propertyName, string propertyValue) { }

        /// <summary>
        /// Updates a custom alarm property.
        /// </summary>
        /// <param name="dataMinerID">The DataMiner Agent ID.</param>
        /// <param name="alarmID">The alarm ID.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="propertyValue">The value to set.</param>
        /// <remarks>
        /// <note type="note">
        /// <list type="bullet">
        /// <item><description>In DataMiner versions prior to 9.0, this method cannot be used to override alarm property values that are defined in the element protocol.</description></item>
        /// <item><description>When an alarm property value has been defined in the element protocol, and this method is used to explicitly assign a new value to the property, the new value will only be retained until the severity of the alarm changes. After this, the value from the protocol is used again.</description></item>
        /// <item><description>Although this method is obsolete, it will still work on a DMS without Swarming enabled until 10.6.</description></item>
        /// </list>
        /// </note>
        /// </remarks>
        /// <example>
        /// <code>
        /// engine.SetAlarmProperty(200, 521655, "Property A", "Value A");
        /// </code>
        /// </example>
		[Obsolete("Please use the overload that takes an AlarmTreeID (still supported in <= 10.6 on non-swarming systems)")]
        public void SetAlarmProperty(int dataMinerID, int alarmID, string propertyName, string propertyValue) { }

        /// <summary>
        /// Updates a custom alarm property.
        /// </summary>
        /// <param name="dataMinerID">The DataMiner Agent ID.</param>
        /// <param name="elementID">The element ID.</param>
        /// <param name="alarmID">The alarm ID.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The value to set.</param>
        /// <remarks>
        /// <note type="note">
        /// <list type="bullet">
        /// <item><description>In DataMiner versions prior to 9.0, this method cannot be used to override alarm property values that are defined in the element protocol.</description></item>
        /// <item><description>When an alarm property value has been defined in the element protocol, and this method is used to explicitly assign a new value to the property, the new value will only be retained until the severity of the alarm changes. After this, the value from the protocol is used again.</description></item>
        /// <item><description>For performance reasons, we recommend using the overload that takes an AlarmTreeID instead.</description></item>
        /// </list>
        /// </note>
        /// </remarks>
        public void SetAlarmProperty(int dataMinerID, int elementID, int alarmID, string propertyName, string propertyValue) { }

		/// <summary>
		/// Allows setting <see cref="RunTimeFlags"/> at runtime.
		/// </summary>
		/// <param name="flag">The flags to set.</param>
		/// <example>
		/// <code>
		/// engine.SetFlag(RunTimeFlags.AllowUndef);
		/// </code>
		/// </example>
		public void SetFlag(RunTimeFlags flag) { }

		/// <summary>
		/// Displays a progress message during the execution of an interactive Automation script.
		/// </summary>
		/// <param name="uiData">The UI data.</param>
		/// <exception cref="InteractiveUserDetachedException">The interactive client was detached.</exception>
		/// <example>
		/// <code>
		/// string progress = "Session is successfully booked.";
		/// engine.ShowProgress(progress);
		/// </code>
		/// </example>
		/// <remarks>
                /// ShowProgress displays a message in a dialog with the name of the script as title. If you want the dialog to have a custom title you can use the UIBuilder:
		/// <code> UIBuilder uibDialogBox = new UIBuilder();
                /// uibDialogBox.Title = "My custom title";
                /// uibDialogBox.Append("Session is successfully booked.");
                /// engine.ShowUI(uibDialogBox);
		/// </code>
		/// <seealso cref="ShowUI"/>
                ///</remarks>
		public void ShowProgress(string uiData) { }

		/// <summary>
		/// Displays a custom-made dialog box of an interactive Automation script.
		/// </summary>
		/// <param name="uiData">The UI data.</param>
		/// <returns>The UI result.</returns>
		/// <exception cref="InteractiveUserDetachedException">The interactive client was detached.</exception>
		/// <exception cref="DataMinerException">Show UI failed.</exception>
		/// <example>
		/// <code>
		/// UIBuilder uib = new UIBuilder();
		/// ...
		/// // Add dialog box items to the dialog box
		/// UIBlockDefinition blockButton = new UIBlockDefinition();
		/// blockButton.Type = UIBlockType.Button;
		/// ...
		/// engine.ShowUI(uib.ToString());
		/// </code>
		/// </example>
		public UIResults ShowUI(string uiData) { return null; }

		/// <summary>
		/// Displays a custom-made dialog box of an interactive Automation script.
		/// </summary>
		/// <param name="uiBuilder">The <see cref="UIBuilder"/> instance representing a dialog box of an interactive automation script.</param>
		/// <exception cref="InteractiveUserDetachedException">The interactive client was detached.</exception>
		/// <exception cref="DataMinerException">Show UI failed.</exception>
		/// <returns>The UI result.</returns>
		/// <example>
		/// <code>
		/// UIBuilder uib = new UIBuilder();
		/// ...
		/// // Add dialog box items to the dialog box
		/// UIBlockDefinition blockButton = new UIBlockDefinition();
		/// blockButton.Type = UIBlockType.Button;
		/// ...
		/// engine.ShowUI(uib);
		/// </code>
		/// </example>
		public UIResults ShowUI(UIBuilder uiBuilder) { return null; }

		/// <summary>
		/// Displays a custom-made dialog box of an interactive Automation script.
		/// </summary>
		/// <param name="uiData">The UI data.</param>
		/// <param name="requireResponse">Indicates whether a response is required.</param>
		/// <exception cref="InteractiveUserDetachedException">The interactive client was detached.</exception>
		/// <exception cref="DataMinerException">Show UI failed.</exception>
		/// <returns>The UI result.</returns>
		/// <example>
		/// <code>
		/// UIBuilder uib = new UIBuilder();
		/// ...
		/// // Add dialog box items to the dialog box
		/// UIBlockDefinition blockButton = new UIBlockDefinition();
		/// blockButton.Type = UIBlockType.Button;
		/// ...
		/// engine.ShowUI(uib.ToString(), uib.RequireResponse);
		/// </code>
		/// </example>
		public UIResults ShowUI(string uiData, bool requireResponse) { return null; }

		/// <summary>
		/// Causes the Automation script to pause for the specified amount of time (in milliseconds).
		/// </summary>
		/// <param name="timeInMilliseconds">The time to sleep (in ms).</param>
		/// <exception cref="DataMinerException">Sleep failed.</exception>
		/// <example>
		/// <code>
		/// engine.Sleep(100);
		/// </code>
		/// </example>
                /// <remarks>
		/// <note type="note">
		/// <description>In DataMiner versions prior to 10.3.0 [CU18]/10.4.0 [CU6]/10.4.9, this method will throw a DataMinerException when a negative time is specified.</description>
		/// </note>
		/// </remarks>
		public void Sleep(int timeInMilliseconds) { }

		/// <summary>
		/// Clears the specified run-time flag.
		/// </summary>
		/// <param name="flag">The run-time flag to clear.</param>
		/// <remarks>Feature introduced in DataMiner 10.0.2 (RN 23961).</remarks>
		/// <example>
		/// <code>
		/// public void SetParameterSilent(int pid, object value) {
		///		// Set the NoInformationEvents flag to disable information events
		///		_engine.SetFlag(RunTimeFlags.NoInformationEvents);
		///		// Perform a silent parameter set without triggering an information event
		///		element.SetParameter(pid, value);
		///		// Re-enable information events by clearing the NoInformationEvents flag
		///		_engine.UnsetFlag(RunTimeFlags.NoInformationEvents);
		///	}
		/// </code>
		/// </example>
		public virtual void UnSetFlag(RunTimeFlags flag)
		{ }


		//[HandleProcessCorruptedStateExceptions]
		//protected virtual void Dispose(bool A_0) { }
	}
}
