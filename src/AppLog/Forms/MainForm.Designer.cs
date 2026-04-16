namespace AppLog.Forms;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify.
    /// </summary>
    private void InitializeComponent()
    {
        // =========================================================================
        // Controls
        // =========================================================================

        // Port Config Group
        grpPortConfig = new GroupBox();
        cmbPort = new ComboBox();
        cmbBaudRate = new ComboBox();
        btnUpdatePort = new Button();
        btnOpenClose = new Button();

        // Log Display Group
        grpLogDisplay = new GroupBox();
        rtbLogDisplay = new RichTextBox();
        btnClearLog = new Button();
        btnCopyLog = new Button();

        // Send Data Group
        grpSendData = new GroupBox();
        txtSendData = new TextBox();
        btnSend = new Button();

        // Log File Group
        grpLogFile = new GroupBox();
        btnStartStopLog = new Button();

        // Status Bar
        lblStatus = new Label();

        // =========================================================================
        // GroupBox: Port Config
        // =========================================================================
        grpPortConfig.Text = "Port Config";
        grpPortConfig.Location = new System.Drawing.Point(12, 12);
        grpPortConfig.Size = new System.Drawing.Size(560, 60);
        grpPortConfig.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        cmbPort.Location = new System.Drawing.Point(10, 25);
        cmbPort.Size = new System.Drawing.Size(120, 23);
        cmbPort.DropDownStyle = ComboBoxStyle.DropDownList;

        cmbBaudRate.Location = new System.Drawing.Point(140, 25);
        cmbBaudRate.Size = new System.Drawing.Size(90, 23);
        cmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200", "230400", "460800", "921600" });
        cmbBaudRate.SelectedIndex = 4; // Default: 115200

        btnUpdatePort.Location = new System.Drawing.Point(240, 24);
        btnUpdatePort.Size = new System.Drawing.Size(100, 26);
        btnUpdatePort.Text = "Update Port";
        btnUpdatePort.UseVisualStyleBackColor = true;
        btnUpdatePort.Click += btnUpdatePort_Click;

        btnOpenClose.Location = new System.Drawing.Point(350, 24);
        btnOpenClose.Size = new System.Drawing.Size(100, 26);
        btnOpenClose.Text = "Open";
        btnOpenClose.UseVisualStyleBackColor = true;
        btnOpenClose.Click += btnOpenClose_Click;

        grpPortConfig.Controls.AddRange(new Control[] { cmbPort, cmbBaudRate, btnUpdatePort, btnOpenClose });

        // =========================================================================
        // GroupBox: Log Display
        // =========================================================================
        grpLogDisplay.Text = "Log Display";
        grpLogDisplay.Location = new System.Drawing.Point(12, 80);
        grpLogDisplay.Size = new System.Drawing.Size(560, 300);
        grpLogDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        rtbLogDisplay.Location = new System.Drawing.Point(10, 20);
        rtbLogDisplay.Size = new System.Drawing.Size(540, 240);
        rtbLogDisplay.ReadOnly = true;
        rtbLogDisplay.BackColor = System.Drawing.Color.White;
        rtbLogDisplay.Font = new System.Drawing.Font("Consolas", 9F);
        rtbLogDisplay.WordWrap = false;
        rtbLogDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        btnClearLog.Location = new System.Drawing.Point(370, 266);
        btnClearLog.Size = new System.Drawing.Size(85, 26);
        btnClearLog.Text = "Clear";
        btnClearLog.UseVisualStyleBackColor = true;
        btnClearLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnClearLog.Click += btnClearLog_Click;

        btnCopyLog.Location = new System.Drawing.Point(465, 266);
        btnCopyLog.Size = new System.Drawing.Size(85, 26);
        btnCopyLog.Text = "Copy";
        btnCopyLog.UseVisualStyleBackColor = true;
        btnCopyLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCopyLog.Click += btnCopyLog_Click;

        grpLogDisplay.Controls.AddRange(new Control[] { rtbLogDisplay, btnClearLog, btnCopyLog });

        // =========================================================================
        // GroupBox: Send Data
        // =========================================================================
        grpSendData.Text = "Send Data";
        grpSendData.Location = new System.Drawing.Point(12, 388);
        grpSendData.Size = new System.Drawing.Size(560, 60);
        grpSendData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        txtSendData.Location = new System.Drawing.Point(10, 25);
        txtSendData.Size = new System.Drawing.Size(430, 23);
        txtSendData.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtSendData.KeyDown += txtSendData_KeyDown;

        btnSend.Location = new System.Drawing.Point(450, 24);
        btnSend.Size = new System.Drawing.Size(100, 26);
        btnSend.Text = "Send";
        btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSend.UseVisualStyleBackColor = true;
        btnSend.Enabled = false;
        btnSend.Click += btnSend_Click;

        grpSendData.Controls.AddRange(new Control[] { txtSendData, btnSend });

        // =========================================================================
        // GroupBox: Log File
        // =========================================================================
        grpLogFile.Text = "Log File";
        grpLogFile.Location = new System.Drawing.Point(12, 456);
        grpLogFile.Size = new System.Drawing.Size(560, 55);
        grpLogFile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        btnStartStopLog.Location = new System.Drawing.Point(10, 20);
        btnStartStopLog.Size = new System.Drawing.Size(100, 26);
        btnStartStopLog.Text = "Start Log";
        btnStartStopLog.UseVisualStyleBackColor = true;
        btnStartStopLog.Click += btnStartStopLog_Click;

        grpLogFile.Controls.Add(btnStartStopLog);

        // =========================================================================
        // Status Bar
        // =========================================================================
        lblStatus.Location = new System.Drawing.Point(0, 520);
        lblStatus.Size = new System.Drawing.Size(584, 25);
        lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblStatus.Text = "● Disconnected | COM - | Not logging";
        lblStatus.ForeColor = System.Drawing.Color.Red;
        lblStatus.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
        lblStatus.BorderStyle = BorderStyle.Fixed3D;
        lblStatus.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);

        // =========================================================================
        // Form
        // =========================================================================
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(584, 545);
        this.MinimumSize = new System.Drawing.Size(600, 400);
        this.Text = "AppLog";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.Sizable;
        this.Controls.Add(grpPortConfig);
        this.Controls.Add(grpLogDisplay);
        this.Controls.Add(grpSendData);
        this.Controls.Add(grpLogFile);
        this.Controls.Add(lblStatus);
        this.Load += MainForm_Load;
        this.FormClosing += MainForm_FormClosing;
        this.PerformLayout();
    }

    #endregion

    // =========================================================================
    // UI Controls
    // =========================================================================

    /// <summary>
    /// GroupBox for port configuration controls.
    /// </summary>
    private GroupBox grpPortConfig;

    /// <summary>
    /// ComboBox for selecting COM port.
    /// </summary>
    private ComboBox cmbPort;

    /// <summary>
    /// Button to refresh the COM port list.
    /// </summary>
    private Button btnUpdatePort;

    /// <summary>
    /// ComboBox for selecting baud rate.
    /// </summary>
    private ComboBox cmbBaudRate;

    /// <summary>
    /// Button to toggle Open/Close serial port.
    /// </summary>
    private Button btnOpenClose;

    /// <summary>
    /// GroupBox for log display area.
    /// </summary>
    private GroupBox grpLogDisplay;

    /// <summary>
    /// RichTextBox for displaying TX/RX log entries.
    /// </summary>
    private RichTextBox rtbLogDisplay;

    /// <summary>
    /// Button to clear log display.
    /// </summary>
    private Button btnClearLog;

    /// <summary>
    /// Button to copy log to clipboard.
    /// </summary>
    private Button btnCopyLog;

    /// <summary>
    /// GroupBox for send data controls.
    /// </summary>
    private GroupBox grpSendData;

    /// <summary>
    /// TextBox for entering ASCII data to send.
    /// </summary>
    private TextBox txtSendData;

    /// <summary>
    /// Button to send data through UART.
    /// </summary>
    private Button btnSend;

    /// <summary>
    /// GroupBox for log file controls.
    /// </summary>
    private GroupBox grpLogFile;

    /// <summary>
    /// Button to toggle Start/Stop file logging.
    /// </summary>
    private Button btnStartStopLog;

    /// <summary>
    /// Label for status bar display.
    /// </summary>
    private Label lblStatus;
}