﻿// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser hardware class for MaxScarzellaLX200
//
// Description:	 <To be completed by driver developer>
//
// Implements:	ASCOM Focuser interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>

// TODO: Customise the SetConnected and InitialiseHardware methods as needed for your hardware

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Astrometry.NOVAS;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASCOM.MaxScarzellaLX200.Focuser
{
    //
    // TODO Customise the InitialiseHardware() method with code to set up a communication path to your hardware and validate that the hardware exists
    //
    // TODO Customise the SetConnected() method with code to connect to and disconnect from your hardware
    // NOTE You should not need to customise the code in the Connecting, Connect() and Disconnect() members as these are already fully implemented and call SetConnected() when appropriate.
    //
    // TODO Replace the not implemented exceptions with code to implement the functions or throw the appropriate ASCOM exceptions.
    //

    /// <summary>
    /// ASCOM Focuser hardware class for MaxScarzellaLX200.
    /// </summary>
    [HardwareClass()] // Class attribute flag this as a device hardware class that needs to be disposed by the local server when it exits.
    public static class FocuserHardware
    {
        // Constants used for Profile persistence
        internal const string comPortProfileName = "COM Port";
        internal const string comPortDefault = "COM1";
        internal const string traceStateProfileName = "Trace Level";
        internal const string traceStateDefault = "true";

        private static string DriverProgId = "ASCOM.MaxScarzellaLX200.Focuser"; // Imposta il tuo ProgID qui
        private static string DriverDescription = ""; // The value is set by the driver's class initialiser.
        internal static string comPort; // COM port name (if required)
        internal static string baudRate { get; set; } = "9600";
        private static bool connectedState; // Local server's connected state
        private static bool runOnce = false; // Flag to enable "one-off" activities only to run once.
        internal static Util utilities; // ASCOM Utilities object for use as required
        internal static AstroUtils astroUtilities; // ASCOM AstroUtilities object for use as required
        internal static TraceLogger tl; // Local server's trace logger object for diagnostic log with information that you specify


        private static List<Guid> uniqueIds = new List<Guid>(); // List of driver instance unique IDs

        /// <summary>
        /// Initializes a new instance of the device Hardware class.
        /// </summary>
        static FocuserHardware()
        {
            try
            {
                tl = new TraceLogger("", "MaxScarzellaLX200.Hardware");

                // Verifica che Focuser.DriverProgId non sia null o vuoto prima di usarlo
                var progId = Focuser.DriverProgId;
                if (string.IsNullOrWhiteSpace(progId))
                    throw new InvalidOperationException("Focuser.DriverProgId non è stato impostato. Assicurati che sia valorizzato prima di inizializzare FocuserHardware.");

                DriverProgId = progId;

                ReadProfile();

                LogMessage("FocuserHardware", $"Static initialiser completed.");
            }
            catch (Exception ex)
            {
                try { LogMessage("FocuserHardware", $"Initialisation exception: {ex}"); } catch { }
                MessageBox.Show($"FocuserHardware - {ex.Message}\r\n{ex}", $"Exception creating {Focuser.DriverProgId}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Place device initialisation code here
        /// </summary>
        /// <remarks>Called every time a new instance of the driver is created.</remarks>
        internal static void InitialiseHardware()
        {
            // This method will be called every time a new ASCOM client loads your driver
            LogMessage("InitialiseHardware", $"Start.");

            // Add any code that you want to run every time a client connects to your driver here

            // Add any code that you only want to run when the first client connects in the if (runOnce == false) block below
            if (runOnce == false)
            {
                LogMessage("InitialiseHardware", $"Starting one-off initialisation.");

                DriverDescription = Focuser.DriverDescription; // Get this device's Chooser description

                LogMessage("InitialiseHardware", $"ProgID: {DriverProgId}, Description: {DriverDescription}");

                connectedState = false; // Initialise connected to false
                utilities = new Util(); //Initialise ASCOM Utilities object
                astroUtilities = new AstroUtils(); // Initialise ASCOM Astronomy Utilities object

                LogMessage("InitialiseHardware", "Completed basic initialisation");

                // Add your own "one off" device initialisation here e.g. validating existence of hardware and setting up communications
                // If you are using a serial COM port you will find the COM port name selected by the user through the setup dialogue in the comPort variable.

                LogMessage("InitialiseHardware", $"One-off initialisation complete.");
                runOnce = true; // Set the flag to ensure that this code is not run again
            }
        }

        // PUBLIC COM INTERFACE IFocuserV4 IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public static void SetupDialog()
        {
            // Don't permit the setup dialogue if already connected
            if (IsConnected)
                MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(tl))
            {
                var result = F.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public static ArrayList SupportedActions
        {
            get
            {
                LogMessage("SupportedActions Get", "Returning empty ArrayList");
                return new ArrayList();
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public static string Action(string actionName, string actionParameters)
        {
            LogMessage("Action", $"Action {actionName}, parameters {actionParameters} is not implemented");
            throw new ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public static void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBlind must send the supplied command to the mount and return immediately without waiting for a response

            throw new MethodNotImplementedException($"CommandBlind - Command:{command}, Raw: {raw}.");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public static bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            // TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

            throw new MethodNotImplementedException($"CommandBool - Command:{command}, Raw: {raw}.");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public static string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

            throw new MethodNotImplementedException($"CommandString - Command:{command}, Raw: {raw}.");
        }

        /// <summary>
        /// Deterministically release both managed and unmanaged resources that are used by this class.
        /// </summary>
        /// <remarks>
        /// TODO: Release any managed or unmanaged resources that are used in this class.
        /// 
        /// Do not call this method from the Dispose method in your driver class.
        ///
        /// This is because this hardware class is decorated with the <see cref="HardwareClassAttribute"/> attribute and this Dispose() method will be called 
        /// automatically by the  local server executable when it is irretrievably shutting down. This gives you the opportunity to release managed and unmanaged 
        /// resources in a timely fashion and avoid any time delay between local server close down and garbage collection by the .NET runtime.
        ///
        /// For the same reason, do not call the SharedResources.Dispose() method from this method. Any resources used in the static shared resources class
        /// itself should be released in the SharedResources.Dispose() method as usual. The SharedResources.Dispose() method will be called automatically 
        /// by the local server just before it shuts down.
        /// 
        /// </remarks>
        public static void Dispose()
        {
            try { LogMessage("Dispose", $"Disposing of assets and closing down."); } catch { }

            try
            {
                // Clean up the trace logger and utility objects
                tl.Enabled = false;
                tl.Dispose();
                tl = null;
            }
            catch { }

            try
            {
                utilities.Dispose();
                utilities = null;
            }
            catch { }

            try
            {
                astroUtilities.Dispose();
                astroUtilities = null;
            }
            catch { }
        }

        /// <summary>
        /// Synchronously connects to or disconnects from the hardware
        /// </summary>
        /// <param name="uniqueId">Driver's unique ID</param>
        /// <param name="newState">New state: Connected or Disconnected</param>
        public static void SetConnected(Guid uniqueId, bool newState)
        {
            // Check whether we are connecting or disconnecting
            if (newState) // We are connecting
            {
                // Check whether this driver instance has already connected
                if (uniqueIds.Contains(uniqueId)) // Instance already connected
                {
                    // Ignore the request, the unique ID is already in the list
                    LogMessage("SetConnected", $"Ignoring request to connect because the device is already connected.");
                }
                else // Instance not already connected, so connect it
                {
                    // Check whether this is the first connection to the hardware
                    if (uniqueIds.Count == 0) // This is the first connection to the hardware so initiate the hardware connection
                    {
                        //
                        // Add hardware connect logic here
                        //
                        LogMessage("SetConnected", $"Connecting to hardware.");
                    }
                    else // Other device instances are connected so the hardware is already connected
                    {
                        // Since the hardware is already connected no action is required
                        LogMessage("SetConnected", $"Hardware already connected.");
                    }

                    // The hardware either "already was" or "is now" connected, so add the driver unique ID to the connected list
                    uniqueIds.Add(uniqueId);
                    LogMessage("SetConnected", $"Unique id {uniqueId} added to the connection list.");
                }
            }
            else // We are disconnecting
            {
                // Check whether this driver instance has already disconnected
                if (!uniqueIds.Contains(uniqueId)) // Instance not connected so ignore request
                {
                    // Ignore the request, the unique ID is not in the list
                    LogMessage("SetConnected", $"Ignoring request to disconnect because the device is already disconnected.");
                }
                else // Instance currently connected so disconnect it
                {
                    // Remove the driver unique ID to the connected list
                    uniqueIds.Remove(uniqueId);
                    LogMessage("SetConnected", $"Unique id {uniqueId} removed from the connection list.");

                    // Check whether there are now any connected driver instances 
                    if (uniqueIds.Count == 0) // There are no connected driver instances so disconnect from the hardware
                    {
                        //
                        // Add hardware disconnect logic here
                        //
                    }
                    else // Other device instances are connected so do not disconnect the hardware
                    {
                        // No action is required
                        LogMessage("SetConnected", $"Hardware already connected.");
                    }
                }
            }

            // Log the current connected state
            LogMessage("SetConnected", $"Currently connected driver ids:");
            foreach (Guid id in uniqueIds)
            {
                LogMessage("SetConnected", $" ID {id} is connected");
            }
        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public static string Description
        {
            // TODO customise this device description if required
            get
            {
                LogMessage("Description Get", DriverDescription);
                return DriverDescription;
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public static string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description if required
                string driverInfo = $"Information about the driver itself. Version: {version.Major}.{version.Minor}";
                LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public static string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = $"{version.Major}.{version.Minor}";
                LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public static short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "4");
                return Convert.ToInt16("4");
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public static string Name
        {
            // TODO customise this device name as required
            get
            {
                string name = "Short driver name - please customise";
                LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IFocuser Implementation

        private static int focuserPosition = 0; // Class level variable to hold the current focuser position
        private const int focuserSteps = 10000;

        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        internal static bool Absolute
        {
            get
            {
                LogMessage("Absolute Get", true.ToString());
                return true; // This is an absolute focuser
            }
        }

        /// <summary>
        /// Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
        /// </summary>
        internal static void Halt()
        {
            LogMessage("Halt", "Stop command sent");
            using (var serial = new ASCOM.Utilities.Serial())
            {
                serial.PortName = comPort;
                serial.Speed = SerialSpeed.ps9600; // Corretto: utilizza la proprietà 'Speed' invece di 'BaudRate'
                serial.Connected = true;
                serial.Transmit(":FQ#"); // Comando di stop, verifica se il tuo hardware usa questo
            }
        }

        /// <summary>
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        internal static bool IsMoving
        {
            get
            {
                LogMessage("IsMoving Get", false.ToString());
                return false; // This focuser always moves instantaneously so no need for IsMoving ever to be True
            }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser;
        /// i.e. the maximum number of steps allowed in one move operation.
        /// </summary>
        internal static int MaxIncrement
        {
            get
            {
                LogMessage("MaxIncrement Get", focuserSteps.ToString());
                return focuserSteps; // Maximum change in one move
            }
        }

        /// <summary>
        /// Maximum step position permitted.
        /// </summary>
        internal static int MaxStep
        {
            get
            {
                LogMessage("MaxStep Get", focuserSteps.ToString());
                return focuserSteps; // Maximum extent of the focuser, so position range is 0 to 10,000
            }
        }

        /// <summary>
        /// Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
        /// </summary>
        /// <param name="Position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
        internal static void Move(int Position)
        {
            LogMessage("Move", Position.ToString());
            int delta = Position - focuserPosition;
            if (delta == 0) return;

            string command;
            if (delta > 0)
                command = $":F+{delta}#";
            else
                command = $":F-{Math.Abs(delta)}#";

            using (var serial = new ASCOM.Utilities.Serial())
            {
                serial.PortName = comPort;
                serial.Speed = SerialSpeed.ps9600;
                serial.Connected = true;
                serial.Transmit(command);
            }
            focuserPosition = Position;
        }

        /// <summary>
        /// Current focuser position, in steps.
        /// </summary>
        internal static int Position
        {
            get
            {
                return focuserPosition; // Return the focuser position
            }
        }


        /// <summary>
        /// Step size (microns) for the focuser.
        /// </summary>
        internal static double StepSize
        {
            get
            {
                LogMessage("StepSize Get", "Not implemented");
                throw new PropertyNotImplementedException("StepSize", false);
            }
        }

        /// <summary>
        /// The state of temperature compensation mode (if available), else always False.
        /// </summary>
        internal static bool TempComp
        {
            get
            {
                LogMessage("TempComp Get", false.ToString());
                return false;
            }
            set
            {
                LogMessage("TempComp Set", "Not implemented");
                throw new PropertyNotImplementedException("TempComp", false);
            }
        }

        /// <summary>
        /// True if focuser has temperature compensation available.
        /// </summary>
        internal static bool TempCompAvailable
        {
            get
            {
                LogMessage("TempCompAvailable Get", false.ToString());
                return false; // Temperature compensation is not available in this driver
            }
        }

        /// <summary>
        /// Current ambient temperature in degrees Celsius as measured by the focuser.
        /// </summary>
        internal static double Temperature
        {
            get
            {
                LogMessage("Temperature Get", "Not implemented");
                throw new PropertyNotImplementedException("Temperature", false);
            }
        }

        #endregion

        #region Private properties and methods
        // Useful methods that can be used as required to help with driver development

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private static bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private static void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal static void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(DriverProgId, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(DriverProgId, comPortProfileName, string.Empty, comPortDefault);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal static void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                driverProfile.WriteValue(DriverProgId, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(DriverProgId, comPortProfileName, comPort.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes identifier and message strings
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        internal static void LogMessage(string identifier, string message)
        {
            tl.LogMessageCrLf(identifier, message);
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            LogMessage(identifier, msg);
        }
        #endregion
    }
}

