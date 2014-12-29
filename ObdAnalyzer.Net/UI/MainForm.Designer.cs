namespace Tethys.OBD.ObdAnalyzer.Net.UI
{
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDashboard = new System.Windows.Forms.TabPage();
            this.lblDashRpm = new System.Windows.Forms.Label();
            this.gaugeEngineSpeed = new System.Windows.Forms.AGauge();
            this.lblDashSpeed = new System.Windows.Forms.Label();
            this.gaugeSpeed = new System.Windows.Forms.AGauge();
            this.tabSensorData = new System.Windows.Forms.TabPage();
            this.btnSaveStatistics = new System.Windows.Forms.Button();
            this.checkAutoRefreshSensorData = new System.Windows.Forms.CheckBox();
            this.btnRefreshSensorData = new System.Windows.Forms.Button();
            this.listViewSensor = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.tabReadCodes = new System.Windows.Forms.TabPage();
            this.lblMil = new System.Windows.Forms.Label();
            this.lblNumDtc = new System.Windows.Forms.Label();
            this.picMil = new System.Windows.Forms.PictureBox();
            this.btnClearDtcCodes = new System.Windows.Forms.Button();
            this.btnReadDtc = new System.Windows.Forms.Button();
            this.listViewDtc = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.tabInternal = new System.Windows.Forms.TabPage();
            this.btnListCapabilities = new System.Windows.Forms.Button();
            this.btnCreateRawReport = new System.Windows.Forms.Button();
            this.btnCreateFullReport = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPid = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSendPidCommand = new System.Windows.Forms.Button();
            this.txtReply = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRawCommand = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSendRawCommand = new System.Windows.Forms.Button();
            this.txtBatteryVoltage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGetBatteryVoltage = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolBtnStart = new System.Windows.Forms.ToolStripButton();
            this.toolBtnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolComboPorts = new System.Windows.Forms.ToolStripComboBox();
            this.toolBtnAbout = new System.Windows.Forms.ToolStripButton();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileStart = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolsDebugTrace = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripLabelConnected = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.rtfLogView = new Tethys.Logging.Controls.RtfLogView();
            this.label6 = new System.Windows.Forms.Label();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabDashboard.SuspendLayout();
            this.tabSensorData.SuspendLayout();
            this.tabReadCodes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMil)).BeginInit();
            this.tabInternal.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabDashboard);
            this.tabControl.Controls.Add(this.tabSensorData);
            this.tabControl.Controls.Add(this.tabReadCodes);
            this.tabControl.Controls.Add(this.tabInternal);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(3, 3);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(571, 343);
            this.tabControl.TabIndex = 0;
            // 
            // tabDashboard
            // 
            this.tabDashboard.Controls.Add(this.lblDashRpm);
            this.tabDashboard.Controls.Add(this.gaugeEngineSpeed);
            this.tabDashboard.Controls.Add(this.lblDashSpeed);
            this.tabDashboard.Controls.Add(this.gaugeSpeed);
            this.tabDashboard.Location = new System.Drawing.Point(4, 22);
            this.tabDashboard.Name = "tabDashboard";
            this.tabDashboard.Padding = new System.Windows.Forms.Padding(3);
            this.tabDashboard.Size = new System.Drawing.Size(563, 317);
            this.tabDashboard.TabIndex = 0;
            this.tabDashboard.Text = "Dashboard";
            this.tabDashboard.UseVisualStyleBackColor = true;
            // 
            // lblDashRpm
            // 
            this.lblDashRpm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDashRpm.Location = new System.Drawing.Point(276, 189);
            this.lblDashRpm.Name = "lblDashRpm";
            this.lblDashRpm.Size = new System.Drawing.Size(126, 13);
            this.lblDashRpm.TabIndex = 3;
            this.lblDashRpm.Text = "Engine Speed";
            this.lblDashRpm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gaugeEngineSpeed
            // 
            this.gaugeEngineSpeed.BackColor = System.Drawing.Color.White;
            this.gaugeEngineSpeed.BaseArcColor = System.Drawing.Color.Gray;
            this.gaugeEngineSpeed.BaseArcRadius = 80;
            this.gaugeEngineSpeed.BaseArcStart = 135;
            this.gaugeEngineSpeed.BaseArcSweep = 270;
            this.gaugeEngineSpeed.BaseArcWidth = 2;
            this.gaugeEngineSpeed.Center = new System.Drawing.Point(100, 100);
            this.gaugeEngineSpeed.Location = new System.Drawing.Point(240, 6);
            this.gaugeEngineSpeed.MaxValue = 8000F;
            this.gaugeEngineSpeed.MinValue = 0F;
            this.gaugeEngineSpeed.Name = "gaugeEngineSpeed";
            this.gaugeEngineSpeed.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.gaugeEngineSpeed.NeedleColor2 = System.Drawing.Color.DimGray;
            this.gaugeEngineSpeed.NeedleRadius = 80;
            this.gaugeEngineSpeed.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.gaugeEngineSpeed.NeedleWidth = 2;
            this.gaugeEngineSpeed.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.gaugeEngineSpeed.ScaleLinesInterInnerRadius = 73;
            this.gaugeEngineSpeed.ScaleLinesInterOuterRadius = 80;
            this.gaugeEngineSpeed.ScaleLinesInterWidth = 1;
            this.gaugeEngineSpeed.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.gaugeEngineSpeed.ScaleLinesMajorInnerRadius = 70;
            this.gaugeEngineSpeed.ScaleLinesMajorOuterRadius = 80;
            this.gaugeEngineSpeed.ScaleLinesMajorStepValue = 1000F;
            this.gaugeEngineSpeed.ScaleLinesMajorWidth = 2;
            this.gaugeEngineSpeed.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.gaugeEngineSpeed.ScaleLinesMinorInnerRadius = 75;
            this.gaugeEngineSpeed.ScaleLinesMinorOuterRadius = 80;
            this.gaugeEngineSpeed.ScaleLinesMinorTicks = 9;
            this.gaugeEngineSpeed.ScaleLinesMinorWidth = 1;
            this.gaugeEngineSpeed.ScaleNumbersColor = System.Drawing.Color.Black;
            this.gaugeEngineSpeed.ScaleNumbersFormat = null;
            this.gaugeEngineSpeed.ScaleNumbersRadius = 95;
            this.gaugeEngineSpeed.ScaleNumbersRotation = 0;
            this.gaugeEngineSpeed.ScaleNumbersStartScaleLine = 0;
            this.gaugeEngineSpeed.ScaleNumbersStepScaleLines = 1;
            this.gaugeEngineSpeed.Size = new System.Drawing.Size(211, 180);
            this.gaugeEngineSpeed.TabIndex = 2;
            this.gaugeEngineSpeed.Text = "Speed";
            this.gaugeEngineSpeed.Value = 0F;
            // 
            // lblDashSpeed
            // 
            this.lblDashSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDashSpeed.Location = new System.Drawing.Point(36, 189);
            this.lblDashSpeed.Name = "lblDashSpeed";
            this.lblDashSpeed.Size = new System.Drawing.Size(135, 13);
            this.lblDashSpeed.TabIndex = 1;
            this.lblDashSpeed.Text = "Speed";
            this.lblDashSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gaugeSpeed
            // 
            this.gaugeSpeed.BackColor = System.Drawing.Color.White;
            this.gaugeSpeed.BaseArcColor = System.Drawing.Color.Gray;
            this.gaugeSpeed.BaseArcRadius = 80;
            this.gaugeSpeed.BaseArcStart = 135;
            this.gaugeSpeed.BaseArcSweep = 270;
            this.gaugeSpeed.BaseArcWidth = 2;
            this.gaugeSpeed.Center = new System.Drawing.Point(100, 100);
            this.gaugeSpeed.Location = new System.Drawing.Point(6, 6);
            this.gaugeSpeed.MaxValue = 250F;
            this.gaugeSpeed.MinValue = 0F;
            this.gaugeSpeed.Name = "gaugeSpeed";
            this.gaugeSpeed.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.gaugeSpeed.NeedleColor2 = System.Drawing.Color.DimGray;
            this.gaugeSpeed.NeedleRadius = 80;
            this.gaugeSpeed.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.gaugeSpeed.NeedleWidth = 2;
            this.gaugeSpeed.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.gaugeSpeed.ScaleLinesInterInnerRadius = 73;
            this.gaugeSpeed.ScaleLinesInterOuterRadius = 80;
            this.gaugeSpeed.ScaleLinesInterWidth = 1;
            this.gaugeSpeed.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.gaugeSpeed.ScaleLinesMajorInnerRadius = 70;
            this.gaugeSpeed.ScaleLinesMajorOuterRadius = 80;
            this.gaugeSpeed.ScaleLinesMajorStepValue = 50F;
            this.gaugeSpeed.ScaleLinesMajorWidth = 2;
            this.gaugeSpeed.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.gaugeSpeed.ScaleLinesMinorInnerRadius = 75;
            this.gaugeSpeed.ScaleLinesMinorOuterRadius = 80;
            this.gaugeSpeed.ScaleLinesMinorTicks = 9;
            this.gaugeSpeed.ScaleLinesMinorWidth = 1;
            this.gaugeSpeed.ScaleNumbersColor = System.Drawing.Color.Black;
            this.gaugeSpeed.ScaleNumbersFormat = null;
            this.gaugeSpeed.ScaleNumbersRadius = 95;
            this.gaugeSpeed.ScaleNumbersRotation = 0;
            this.gaugeSpeed.ScaleNumbersStartScaleLine = 0;
            this.gaugeSpeed.ScaleNumbersStepScaleLines = 1;
            this.gaugeSpeed.Size = new System.Drawing.Size(205, 180);
            this.gaugeSpeed.TabIndex = 0;
            this.gaugeSpeed.Text = "Speed";
            this.gaugeSpeed.Value = 0F;
            // 
            // tabSensorData
            // 
            this.tabSensorData.Controls.Add(this.lblLastUpdate);
            this.tabSensorData.Controls.Add(this.label6);
            this.tabSensorData.Controls.Add(this.btnSaveStatistics);
            this.tabSensorData.Controls.Add(this.checkAutoRefreshSensorData);
            this.tabSensorData.Controls.Add(this.btnRefreshSensorData);
            this.tabSensorData.Controls.Add(this.listViewSensor);
            this.tabSensorData.Controls.Add(this.label1);
            this.tabSensorData.Location = new System.Drawing.Point(4, 22);
            this.tabSensorData.Name = "tabSensorData";
            this.tabSensorData.Padding = new System.Windows.Forms.Padding(3);
            this.tabSensorData.Size = new System.Drawing.Size(563, 317);
            this.tabSensorData.TabIndex = 1;
            this.tabSensorData.Text = "Sensor Data";
            this.tabSensorData.UseVisualStyleBackColor = true;
            // 
            // btnSaveStatistics
            // 
            this.btnSaveStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveStatistics.Location = new System.Drawing.Point(207, 268);
            this.btnSaveStatistics.Name = "btnSaveStatistics";
            this.btnSaveStatistics.Size = new System.Drawing.Size(142, 23);
            this.btnSaveStatistics.TabIndex = 4;
            this.btnSaveStatistics.Text = "Save Statistics to File";
            this.btnSaveStatistics.UseVisualStyleBackColor = true;
            this.btnSaveStatistics.Click += new System.EventHandler(this.BtnSaveStatisticsClick);
            // 
            // checkAutoRefreshSensorData
            // 
            this.checkAutoRefreshSensorData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkAutoRefreshSensorData.AutoSize = true;
            this.checkAutoRefreshSensorData.Location = new System.Drawing.Point(379, 272);
            this.checkAutoRefreshSensorData.Name = "checkAutoRefreshSensorData";
            this.checkAutoRefreshSensorData.Size = new System.Drawing.Size(88, 17);
            this.checkAutoRefreshSensorData.TabIndex = 3;
            this.checkAutoRefreshSensorData.Text = "Auto Refresh";
            this.checkAutoRefreshSensorData.UseVisualStyleBackColor = true;
            this.checkAutoRefreshSensorData.CheckedChanged += new System.EventHandler(this.CheckAutoRefreshSensorDataCheckedChanged);
            // 
            // btnRefreshSensorData
            // 
            this.btnRefreshSensorData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshSensorData.Location = new System.Drawing.Point(473, 268);
            this.btnRefreshSensorData.Name = "btnRefreshSensorData";
            this.btnRefreshSensorData.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshSensorData.TabIndex = 2;
            this.btnRefreshSensorData.Text = "Refresh";
            this.btnRefreshSensorData.UseVisualStyleBackColor = true;
            this.btnRefreshSensorData.Click += new System.EventHandler(this.BtnRefreshSensorDataClick);
            // 
            // listViewSensor
            // 
            this.listViewSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSensor.CheckBoxes = true;
            this.listViewSensor.FullRowSelect = true;
            this.listViewSensor.GridLines = true;
            this.listViewSensor.Location = new System.Drawing.Point(9, 19);
            this.listViewSensor.Name = "listViewSensor";
            this.listViewSensor.Size = new System.Drawing.Size(539, 243);
            this.listViewSensor.TabIndex = 1;
            this.listViewSensor.UseCompatibleStateImageBehavior = false;
            this.listViewSensor.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Car Sensor Data";
            // 
            // tabReadCodes
            // 
            this.tabReadCodes.Controls.Add(this.lblMil);
            this.tabReadCodes.Controls.Add(this.lblNumDtc);
            this.tabReadCodes.Controls.Add(this.picMil);
            this.tabReadCodes.Controls.Add(this.btnClearDtcCodes);
            this.tabReadCodes.Controls.Add(this.btnReadDtc);
            this.tabReadCodes.Controls.Add(this.listViewDtc);
            this.tabReadCodes.Controls.Add(this.label2);
            this.tabReadCodes.Location = new System.Drawing.Point(4, 22);
            this.tabReadCodes.Name = "tabReadCodes";
            this.tabReadCodes.Padding = new System.Windows.Forms.Padding(3);
            this.tabReadCodes.Size = new System.Drawing.Size(563, 317);
            this.tabReadCodes.TabIndex = 2;
            this.tabReadCodes.Text = "Read Codes";
            this.tabReadCodes.UseVisualStyleBackColor = true;
            // 
            // lblMil
            // 
            this.lblMil.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMil.AutoSize = true;
            this.lblMil.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMil.Location = new System.Drawing.Point(93, 289);
            this.lblMil.Name = "lblMil";
            this.lblMil.Size = new System.Drawing.Size(263, 16);
            this.lblMil.TabIndex = 8;
            this.lblMil.Text = "MIL (malfunction indicator lamp) is off";
            // 
            // lblNumDtc
            // 
            this.lblNumDtc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNumDtc.AutoSize = true;
            this.lblNumDtc.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumDtc.Location = new System.Drawing.Point(91, 245);
            this.lblNumDtc.Name = "lblNumDtc";
            this.lblNumDtc.Size = new System.Drawing.Size(106, 25);
            this.lblNumDtc.TabIndex = 7;
            this.lblNumDtc.Text = "No DTCs";
            // 
            // picMil
            // 
            this.picMil.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picMil.Image = global::Tethys.OBD.ObdAnalyzer.Net.Properties.Resources.Check_Engine_gray_70;
            this.picMil.Location = new System.Drawing.Point(12, 245);
            this.picMil.Name = "picMil";
            this.picMil.Size = new System.Drawing.Size(73, 60);
            this.picMil.TabIndex = 6;
            this.picMil.TabStop = false;
            // 
            // btnClearDtcCodes
            // 
            this.btnClearDtcCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearDtcCodes.Location = new System.Drawing.Point(371, 271);
            this.btnClearDtcCodes.Name = "btnClearDtcCodes";
            this.btnClearDtcCodes.Size = new System.Drawing.Size(90, 40);
            this.btnClearDtcCodes.TabIndex = 5;
            this.btnClearDtcCodes.Text = "Clear Codes";
            this.btnClearDtcCodes.UseVisualStyleBackColor = true;
            this.btnClearDtcCodes.Click += new System.EventHandler(this.BtnClearDtcCodesClick);
            // 
            // btnReadDtc
            // 
            this.btnReadDtc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReadDtc.Location = new System.Drawing.Point(467, 271);
            this.btnReadDtc.Name = "btnReadDtc";
            this.btnReadDtc.Size = new System.Drawing.Size(90, 40);
            this.btnReadDtc.TabIndex = 4;
            this.btnReadDtc.Text = "Read";
            this.btnReadDtc.UseVisualStyleBackColor = true;
            this.btnReadDtc.Click += new System.EventHandler(this.BtnReadDtcClick);
            // 
            // listViewDtc
            // 
            this.listViewDtc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewDtc.FullRowSelect = true;
            this.listViewDtc.GridLines = true;
            this.listViewDtc.Location = new System.Drawing.Point(12, 22);
            this.listViewDtc.Name = "listViewDtc";
            this.listViewDtc.Size = new System.Drawing.Size(545, 217);
            this.listViewDtc.TabIndex = 3;
            this.listViewDtc.UseCompatibleStateImageBehavior = false;
            this.listViewDtc.View = System.Windows.Forms.View.Details;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Diagnostic Troube Codes";
            // 
            // tabInternal
            // 
            this.tabInternal.Controls.Add(this.btnListCapabilities);
            this.tabInternal.Controls.Add(this.btnCreateRawReport);
            this.tabInternal.Controls.Add(this.btnCreateFullReport);
            this.tabInternal.Controls.Add(this.label9);
            this.tabInternal.Controls.Add(this.txtPid);
            this.tabInternal.Controls.Add(this.label8);
            this.tabInternal.Controls.Add(this.txtMode);
            this.tabInternal.Controls.Add(this.label7);
            this.tabInternal.Controls.Add(this.btnSendPidCommand);
            this.tabInternal.Controls.Add(this.txtReply);
            this.tabInternal.Controls.Add(this.label5);
            this.tabInternal.Controls.Add(this.txtRawCommand);
            this.tabInternal.Controls.Add(this.label4);
            this.tabInternal.Controls.Add(this.btnSendRawCommand);
            this.tabInternal.Controls.Add(this.txtBatteryVoltage);
            this.tabInternal.Controls.Add(this.label3);
            this.tabInternal.Controls.Add(this.btnGetBatteryVoltage);
            this.tabInternal.Location = new System.Drawing.Point(4, 22);
            this.tabInternal.Name = "tabInternal";
            this.tabInternal.Padding = new System.Windows.Forms.Padding(3);
            this.tabInternal.Size = new System.Drawing.Size(563, 317);
            this.tabInternal.TabIndex = 3;
            this.tabInternal.Text = "Internal";
            this.tabInternal.UseVisualStyleBackColor = true;
            // 
            // btnListCapabilities
            // 
            this.btnListCapabilities.Location = new System.Drawing.Point(291, 106);
            this.btnListCapabilities.Name = "btnListCapabilities";
            this.btnListCapabilities.Size = new System.Drawing.Size(135, 23);
            this.btnListCapabilities.TabIndex = 19;
            this.btnListCapabilities.Text = "List Capabilities";
            this.btnListCapabilities.UseVisualStyleBackColor = true;
            this.btnListCapabilities.Click += new System.EventHandler(this.BtnListCapabilitiesClick);
            // 
            // btnCreateRawReport
            // 
            this.btnCreateRawReport.Location = new System.Drawing.Point(9, 106);
            this.btnCreateRawReport.Name = "btnCreateRawReport";
            this.btnCreateRawReport.Size = new System.Drawing.Size(135, 23);
            this.btnCreateRawReport.TabIndex = 18;
            this.btnCreateRawReport.Text = "Create Raw OBD Report";
            this.btnCreateRawReport.UseVisualStyleBackColor = true;
            this.btnCreateRawReport.Click += new System.EventHandler(this.BtnCreateRawReportClick);
            // 
            // btnCreateFullReport
            // 
            this.btnCreateFullReport.Location = new System.Drawing.Point(150, 106);
            this.btnCreateFullReport.Name = "btnCreateFullReport";
            this.btnCreateFullReport.Size = new System.Drawing.Size(135, 23);
            this.btnCreateFullReport.TabIndex = 17;
            this.btnCreateFullReport.Text = "Create Full OBD Report";
            this.btnCreateFullReport.UseVisualStyleBackColor = true;
            this.btnCreateFullReport.Click += new System.EventHandler(this.BtnCreateFullReportClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(194, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "PID:";
            // 
            // txtPid
            // 
            this.txtPid.Location = new System.Drawing.Point(228, 79);
            this.txtPid.Name = "txtPid";
            this.txtPid.Size = new System.Drawing.Size(54, 20);
            this.txtPid.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(91, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Mode:";
            // 
            // txtMode
            // 
            this.txtMode.Location = new System.Drawing.Point(134, 79);
            this.txtMode.Name = "txtMode";
            this.txtMode.Size = new System.Drawing.Size(54, 20);
            this.txtMode.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "PID Command:";
            // 
            // btnSendPidCommand
            // 
            this.btnSendPidCommand.Location = new System.Drawing.Point(288, 77);
            this.btnSendPidCommand.Name = "btnSendPidCommand";
            this.btnSendPidCommand.Size = new System.Drawing.Size(75, 23);
            this.btnSendPidCommand.TabIndex = 8;
            this.btnSendPidCommand.Text = "Send";
            this.btnSendPidCommand.UseVisualStyleBackColor = true;
            this.btnSendPidCommand.Click += new System.EventHandler(this.BtnSendPidCommandClick);
            // 
            // txtReply
            // 
            this.txtReply.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReply.Location = new System.Drawing.Point(6, 218);
            this.txtReply.Multiline = true;
            this.txtReply.Name = "txtReply";
            this.txtReply.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReply.Size = new System.Drawing.Size(551, 93);
            this.txtReply.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Reply:";
            // 
            // txtRawCommand
            // 
            this.txtRawCommand.Location = new System.Drawing.Point(94, 45);
            this.txtRawCommand.Name = "txtRawCommand";
            this.txtRawCommand.Size = new System.Drawing.Size(137, 20);
            this.txtRawCommand.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Raw Command:";
            // 
            // btnSendRawCommand
            // 
            this.btnSendRawCommand.Location = new System.Drawing.Point(237, 45);
            this.btnSendRawCommand.Name = "btnSendRawCommand";
            this.btnSendRawCommand.Size = new System.Drawing.Size(75, 23);
            this.btnSendRawCommand.TabIndex = 3;
            this.btnSendRawCommand.Text = "Send";
            this.btnSendRawCommand.UseVisualStyleBackColor = true;
            this.btnSendRawCommand.Click += new System.EventHandler(this.BtnSendRawCommandClick);
            // 
            // txtBatteryVoltage
            // 
            this.txtBatteryVoltage.Location = new System.Drawing.Point(94, 9);
            this.txtBatteryVoltage.Name = "txtBatteryVoltage";
            this.txtBatteryVoltage.Size = new System.Drawing.Size(78, 20);
            this.txtBatteryVoltage.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Battery Voltage:";
            // 
            // btnGetBatteryVoltage
            // 
            this.btnGetBatteryVoltage.Location = new System.Drawing.Point(178, 7);
            this.btnGetBatteryVoltage.Name = "btnGetBatteryVoltage";
            this.btnGetBatteryVoltage.Size = new System.Drawing.Size(53, 23);
            this.btnGetBatteryVoltage.TabIndex = 0;
            this.btnGetBatteryVoltage.Text = "Get";
            this.btnGetBatteryVoltage.UseVisualStyleBackColor = true;
            this.btnGetBatteryVoltage.Click += new System.EventHandler(this.BtnGetBatteryVoltageClick);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnStart,
            this.toolBtnStop,
            this.toolStripLabel1,
            this.toolComboPorts,
            this.toolBtnAbout});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(571, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ToolStripItemClicked);
            // 
            // toolBtnStart
            // 
            this.toolBtnStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnStart.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnStart.Image")));
            this.toolBtnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnStart.Name = "toolBtnStart";
            this.toolBtnStart.Size = new System.Drawing.Size(23, 22);
            this.toolBtnStart.Text = "Start";
            // 
            // toolBtnStop
            // 
            this.toolBtnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnStop.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnStop.Image")));
            this.toolBtnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnStop.Name = "toolBtnStop";
            this.toolBtnStop.Size = new System.Drawing.Size(23, 22);
            this.toolBtnStop.Text = "Stop";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel1.Text = "Port:";
            // 
            // toolComboPorts
            // 
            this.toolComboPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolComboPorts.Name = "toolComboPorts";
            this.toolComboPorts.Size = new System.Drawing.Size(121, 25);
            // 
            // toolBtnAbout
            // 
            this.toolBtnAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnAbout.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnAbout.Image")));
            this.toolBtnAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnAbout.Name = "toolBtnAbout";
            this.toolBtnAbout.Size = new System.Drawing.Size(23, 22);
            this.toolBtnAbout.Text = "About";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuTools,
            this.menuHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(571, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileStart,
            this.menuFileStop,
            this.toolStripSeparator1,
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuFileStart
            // 
            this.menuFileStart.Name = "menuFileStart";
            this.menuFileStart.Size = new System.Drawing.Size(134, 22);
            this.menuFileStart.Text = "St&art";
            this.menuFileStart.Click += new System.EventHandler(this.MenuFileStartClick);
            // 
            // menuFileStop
            // 
            this.menuFileStop.Name = "menuFileStop";
            this.menuFileStop.Size = new System.Drawing.Size(134, 22);
            this.menuFileStop.Text = "St&op";
            this.menuFileStop.Click += new System.EventHandler(this.MenuFileStopClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuFileExit.Size = new System.Drawing.Size(134, 22);
            this.menuFileExit.Text = "E&xit";
            this.menuFileExit.Click += new System.EventHandler(this.MenuFileExitClick);
            // 
            // menuTools
            // 
            this.menuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolsDebugTrace});
            this.menuTools.Name = "menuTools";
            this.menuTools.Size = new System.Drawing.Size(48, 20);
            this.menuTools.Text = "&Tools";
            this.menuTools.Visible = false;
            // 
            // menuToolsDebugTrace
            // 
            this.menuToolsDebugTrace.Name = "menuToolsDebugTrace";
            this.menuToolsDebugTrace.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.menuToolsDebugTrace.Size = new System.Drawing.Size(224, 22);
            this.menuToolsDebugTrace.Text = "Debug Trace...";
            this.menuToolsDebugTrace.Click += new System.EventHandler(this.MenuToolsDebugTraceClick);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "&Help";
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(193, 22);
            this.menuHelpAbout.Text = "About OBD-Analyzer...";
            this.menuHelpAbout.Click += new System.EventHandler(this.MenuHelpAboutClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus,
            this.statusStripLabelConnected});
            this.statusStrip.Location = new System.Drawing.Point(0, 566);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(571, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(427, 17);
            this.toolStripStatus.Spring = true;
            this.toolStripStatus.Text = "Ready";
            this.toolStripStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStripLabelConnected
            // 
            this.statusStripLabelConnected.Name = "statusStripLabelConnected";
            this.statusStripLabelConnected.Size = new System.Drawing.Size(129, 17);
            this.statusStripLabelConnected.Text = "[Connected at COM99]";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 49);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rtfLogView);
            this.splitContainer.Size = new System.Drawing.Size(571, 517);
            this.splitContainer.SplitterDistance = 343;
            this.splitContainer.TabIndex = 4;
            // 
            // rtfLogView
            // 
            this.rtfLogView.AddAtTail = true;
            this.rtfLogView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfLogView.LabelText = "Status:";
            this.rtfLogView.Location = new System.Drawing.Point(0, 0);
            this.rtfLogView.MaxLogLength = -1;
            this.rtfLogView.Name = "rtfLogView";
            this.rtfLogView.ShowDebugEvents = false;
            this.rtfLogView.Size = new System.Drawing.Size(571, 170);
            this.rtfLogView.TabIndex = 0;
            this.rtfLogView.TextColor = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 273);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Last Update:";
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.Location = new System.Drawing.Point(82, 273);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(49, 13);
            this.lblLastUpdate.TabIndex = 6;
            this.lblLastUpdate.Text = "00:00:00";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 588);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(575, 350);
            this.Name = "MainForm";
            this.Text = "OBD-Analyzer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.tabControl.ResumeLayout(false);
            this.tabDashboard.ResumeLayout(false);
            this.tabSensorData.ResumeLayout(false);
            this.tabSensorData.PerformLayout();
            this.tabReadCodes.ResumeLayout(false);
            this.tabReadCodes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMil)).EndInit();
            this.tabInternal.ResumeLayout(false);
            this.tabInternal.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDashboard;
        private System.Windows.Forms.TabPage tabSensorData;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelConnected;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.TabPage tabReadCodes;
        private System.Windows.Forms.TabPage tabInternal;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.Label lblDashRpm;
        private System.Windows.Forms.AGauge gaugeEngineSpeed;
        private System.Windows.Forms.Label lblDashSpeed;
        private System.Windows.Forms.AGauge gaugeSpeed;
        private System.Windows.Forms.ToolStripButton toolBtnStart;
        private System.Windows.Forms.ToolStripButton toolBtnStop;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolComboPorts;
        private System.Windows.Forms.ToolStripButton toolBtnAbout;
        private System.Windows.Forms.ToolStripMenuItem menuFileStart;
        private System.Windows.Forms.ToolStripMenuItem menuFileStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuTools;
        private System.Windows.Forms.ToolStripMenuItem menuToolsDebugTrace;
        private System.Windows.Forms.TextBox txtBatteryVoltage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGetBatteryVoltage;
        private System.Windows.Forms.TextBox txtReply;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRawCommand;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSendRawCommand;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPid;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSendPidCommand;
        private System.Windows.Forms.Button btnCreateFullReport;
        private System.Windows.Forms.CheckBox checkAutoRefreshSensorData;
        private System.Windows.Forms.Button btnRefreshSensorData;
        private System.Windows.Forms.ListView listViewSensor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCreateRawReport;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Logging.Controls.RtfLogView rtfLogView;
        private System.Windows.Forms.Button btnClearDtcCodes;
        private System.Windows.Forms.Button btnReadDtc;
        private System.Windows.Forms.ListView listViewDtc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picMil;
        private System.Windows.Forms.Label lblNumDtc;
        private System.Windows.Forms.Label lblMil;
        private System.Windows.Forms.Button btnListCapabilities;
        private System.Windows.Forms.Button btnSaveStatistics;
        private System.Windows.Forms.Label lblLastUpdate;
        private System.Windows.Forms.Label label6;
    }
}

