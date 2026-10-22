namespace GeoCalculator {
    partial class CalculatorForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculatorForm));
            lblConvertX = new Label();
            txtConvertX = new TextBox();
            lblConvertY = new Label();
            txtConvertY = new TextBox();
            lblConvertZ = new Label();
            txtConvertZ = new TextBox();
            lblAddAltitude = new Label();
            txtAddAltitude = new TextBox();
            lblConvertXOutput = new Label();
            lblConvertZOutput = new Label();
            txtConvertZOutput = new TextBox();
            txtConvertXOutput = new TextBox();
            txtConvertYOutput = new TextBox();
            lblConvertYOutput = new Label();
            btnConvertGeocentric = new Button();
            grpConvertGeocentricInputs = new GroupBox();
            grpConvertOutputs = new GroupBox();
            btnClipboardCopy = new Button();
            txtConvertDMSLngOutput = new TextBox();
            txtConvertDMSLatOutput = new TextBox();
            lblConvertAltOutput = new Label();
            lblConvertLngOutput = new Label();
            lblConvertLatOutput = new Label();
            txtConvertAltOutput = new TextBox();
            txtConvertLngOutput = new TextBox();
            txtConvertLatOutput = new TextBox();
            grpConvertLatLngInputs = new GroupBox();
            txtConvertAlt = new TextBox();
            lblConvertAlt = new Label();
            txtConvertLng = new TextBox();
            lblConvertLng = new Label();
            txtConvertLat = new TextBox();
            lblConvertLat = new Label();
            btnConvertLatLng = new Button();
            grpAddConditions = new GroupBox();
            mainTab = new TabControl();
            tabConverter = new TabPage();
            btnConvertDMS = new Button();
            grpConvertDMSInputs = new GroupBox();
            cmbConvertDMSLngHemisphere = new ComboBox();
            cmbConvertDMSLatHemisphere = new ComboBox();
            lblConvertDMSLngSec = new Label();
            txtConvertDMSLngSec = new TextBox();
            lblConvertDMSLngMin = new Label();
            txtConvertDMSLngMin = new TextBox();
            lblConvertDMSLngDeg = new Label();
            txtConvertDMSLngDeg = new TextBox();
            lblConvertDMSLatSec = new Label();
            txtConvertDMSLatSec = new TextBox();
            lblConvertDMSLatMin = new Label();
            txtConvertDMSLatMin = new TextBox();
            lblConvertDMSLatDeg = new Label();
            txtConvertDMSLatDeg = new TextBox();
            lblConvertDMSLat = new Label();
            lblConvertDMSLng = new Label();
            lblConvertDMSAlt = new Label();
            txtConvertDMSAlt = new TextBox();
            lblAddAltitudeHint = new Label();
            lblConvertDMSLatOutput = new Label();
            lblConvertDMSLngOutput = new Label();
            lblConvertStatus = new Label();
            tabDistance = new TabPage();
            grpDFReplaceText = new GroupBox();
            txtDFReplaceRunningNo = new TextBox();
            lblDFReplaceRunningNo = new Label();
            cbDFReplaceText = new CheckBox();
            txtDFReplaceText = new RichTextBox();
            grpDFOutput = new GroupBox();
            txtDFRichResult = new RichTextBox();
            grpDFConditions = new GroupBox();
            tipIconDFAzimuth = new PictureBox();
            txtDFElevation = new TextBox();
            lblDFElevation = new Label();
            btnDFCalculate = new Button();
            btnPasteFromClipboard = new Button();
            txtDFDistance = new TextBox();
            lblDFDistance = new Label();
            txtDFDeg = new TextBox();
            grpDFInputs = new GroupBox();
            txtDFLat = new TextBox();
            lblDFLat = new Label();
            lblDFAltitude = new Label();
            lblDFLng = new Label();
            txtDFAltitude = new TextBox();
            txtDFLng = new TextBox();
            txtClipboard = new TextBox();
            lblClipboard = new Label();
            lblGithub = new Label();
            tipDFReplaceOutput = new ToolTip(components);
            tipDFAzimuth = new ToolTip(components);
            tipInputs = new ToolTip(components);
            tipIconDFReplaceOutput = new PictureBox();
            grpDFAzimuth = new GroupBox();
            rbDFAzimuthList = new RadioButton();
            lblDFAzimuthListHint = new Label();
            lblDFAzimuthDivider = new Label();
            rbDFAzimuthRange = new RadioButton();
            txtDFAzimuthStart = new TextBox();
            lblDFAzimuthTo = new Label();
            txtDFAzimuthEnd = new TextBox();
            lblDFAzimuthEvery = new Label();
            txtDFAzimuthStep = new TextBox();
            lblDFAzimuthDirection = new Label();
            pnlDFAzimuthDirection = new Panel();
            rbDFAzimuthClockwise = new RadioButton();
            rbDFAzimuthAnticlockwise = new RadioButton();
            lblDFAzimuthRangeHint = new Label();
            btnDFAzimuthCopyToList = new Button();
            picDFAzimuthStatus = new PictureBox();
            lblDFAzimuthStatus = new Label();
            grpDFAzimuthPreview = new GroupBox();
            pnlDFAzimuthCompass = new Codes.Controls.AzimuthCompassPanel();
            lblDFAzimuthCount = new Label();
            lblDFAzimuthSpan = new Label();
            lvDFAzimuths = new ListView();
            colDFAzimuthNo = new ColumnHeader();
            colDFAzimuthValue = new ColumnHeader();
            colDFAzimuthLat = new ColumnHeader();
            colDFAzimuthLng = new ColumnHeader();
            lblDFOutputStatus = new Label();
            lblDFOutputShow = new Label();
            cbDFShowGeocentric = new CheckBox();
            cbDFShowGeodetic = new CheckBox();
            cbDFShowDMS = new CheckBox();
            tmrDFAzimuthPreview = new System.Windows.Forms.Timer(components);
            grpConvertGeocentricInputs.SuspendLayout();
            grpConvertOutputs.SuspendLayout();
            grpConvertLatLngInputs.SuspendLayout();
            grpAddConditions.SuspendLayout();
            mainTab.SuspendLayout();
            tabConverter.SuspendLayout();
            grpConvertDMSInputs.SuspendLayout();
            tabDistance.SuspendLayout();
            grpDFReplaceText.SuspendLayout();
            grpDFOutput.SuspendLayout();
            grpDFConditions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tipIconDFAzimuth).BeginInit();
            grpDFInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tipIconDFReplaceOutput).BeginInit();
            grpDFAzimuth.SuspendLayout();
            pnlDFAzimuthDirection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picDFAzimuthStatus).BeginInit();
            grpDFAzimuthPreview.SuspendLayout();
            SuspendLayout();
            // 
            // grpConvertGeocentricInputs
            // 
            grpConvertGeocentricInputs.Controls.Add(lblConvertX);
            grpConvertGeocentricInputs.Controls.Add(txtConvertX);
            grpConvertGeocentricInputs.Controls.Add(lblConvertY);
            grpConvertGeocentricInputs.Controls.Add(txtConvertY);
            grpConvertGeocentricInputs.Controls.Add(lblConvertZ);
            grpConvertGeocentricInputs.Controls.Add(txtConvertZ);
            grpConvertGeocentricInputs.Controls.Add(btnConvertGeocentric);
            grpConvertGeocentricInputs.Location = new Point(8, 6);
            grpConvertGeocentricInputs.Name = "grpConvertGeocentricInputs";
            grpConvertGeocentricInputs.Size = new Size(336, 150);
            grpConvertGeocentricInputs.TabIndex = 0;
            grpConvertGeocentricInputs.TabStop = false;
            grpConvertGeocentricInputs.Text = "Geocentric (ECEF)";
            // 
            // lblConvertX
            // 
            lblConvertX.AutoSize = true;
            lblConvertX.Location = new Point(46, 28);
            lblConvertX.Name = "lblConvertX";
            lblConvertX.TabIndex = 4;
            lblConvertX.Text = "X (m):";
            // 
            // txtConvertX
            // 
            txtConvertX.Location = new Point(93, 25);
            txtConvertX.Name = "txtConvertX";
            txtConvertX.Size = new Size(233, 23);
            txtConvertX.TabIndex = 0;
            // 
            // lblConvertY
            // 
            lblConvertY.AutoSize = true;
            lblConvertY.Location = new Point(46, 57);
            lblConvertY.Name = "lblConvertY";
            lblConvertY.TabIndex = 5;
            lblConvertY.Text = "Y (m):";
            // 
            // txtConvertY
            // 
            txtConvertY.Location = new Point(93, 54);
            txtConvertY.Name = "txtConvertY";
            txtConvertY.Size = new Size(233, 23);
            txtConvertY.TabIndex = 1;
            // 
            // lblConvertZ
            // 
            lblConvertZ.AutoSize = true;
            lblConvertZ.Location = new Point(46, 86);
            lblConvertZ.Name = "lblConvertZ";
            lblConvertZ.TabIndex = 6;
            lblConvertZ.Text = "Z (m):";
            // 
            // txtConvertZ
            // 
            txtConvertZ.Location = new Point(93, 83);
            txtConvertZ.Name = "txtConvertZ";
            txtConvertZ.Size = new Size(233, 23);
            txtConvertZ.TabIndex = 2;
            // 
            // btnConvertGeocentric
            // 
            btnConvertGeocentric.Location = new Point(246, 115);
            btnConvertGeocentric.Name = "btnConvertGeocentric";
            btnConvertGeocentric.Size = new Size(80, 27);
            btnConvertGeocentric.TabIndex = 3;
            btnConvertGeocentric.Text = "Convert";
            btnConvertGeocentric.UseVisualStyleBackColor = true;
            btnConvertGeocentric.Click += btnConvertGencentric_Click;
            // 
            // grpConvertLatLngInputs
            // 
            grpConvertLatLngInputs.Controls.Add(lblConvertLat);
            grpConvertLatLngInputs.Controls.Add(txtConvertLat);
            grpConvertLatLngInputs.Controls.Add(lblConvertLng);
            grpConvertLatLngInputs.Controls.Add(txtConvertLng);
            grpConvertLatLngInputs.Controls.Add(lblConvertAlt);
            grpConvertLatLngInputs.Controls.Add(txtConvertAlt);
            grpConvertLatLngInputs.Controls.Add(btnConvertLatLng);
            grpConvertLatLngInputs.Location = new Point(352, 6);
            grpConvertLatLngInputs.Name = "grpConvertLatLngInputs";
            grpConvertLatLngInputs.Size = new Size(336, 150);
            grpConvertLatLngInputs.TabIndex = 1;
            grpConvertLatLngInputs.TabStop = false;
            grpConvertLatLngInputs.Text = "Lat/Lng";
            // 
            // lblConvertLat
            // 
            lblConvertLat.AutoSize = true;
            lblConvertLat.Location = new Point(61, 28);
            lblConvertLat.Name = "lblConvertLat";
            lblConvertLat.TabIndex = 4;
            lblConvertLat.Text = "Lat:";
            // 
            // txtConvertLat
            // 
            txtConvertLat.Location = new Point(93, 25);
            txtConvertLat.Name = "txtConvertLat";
            txtConvertLat.Size = new Size(233, 23);
            txtConvertLat.TabIndex = 0;
            // 
            // lblConvertLng
            // 
            lblConvertLng.AutoSize = true;
            lblConvertLng.Location = new Point(57, 57);
            lblConvertLng.Name = "lblConvertLng";
            lblConvertLng.TabIndex = 5;
            lblConvertLng.Text = "Lng:";
            // 
            // txtConvertLng
            // 
            txtConvertLng.Location = new Point(93, 54);
            txtConvertLng.Name = "txtConvertLng";
            txtConvertLng.Size = new Size(233, 23);
            txtConvertLng.TabIndex = 1;
            // 
            // lblConvertAlt
            // 
            lblConvertAlt.AutoSize = true;
            lblConvertAlt.Location = new Point(13, 86);
            lblConvertAlt.Name = "lblConvertAlt";
            lblConvertAlt.TabIndex = 6;
            lblConvertAlt.Text = "Altitude (m):";
            // 
            // txtConvertAlt
            // 
            txtConvertAlt.Location = new Point(93, 83);
            txtConvertAlt.Name = "txtConvertAlt";
            txtConvertAlt.Size = new Size(233, 23);
            txtConvertAlt.TabIndex = 2;
            // 
            // btnConvertLatLng
            // 
            btnConvertLatLng.Location = new Point(246, 115);
            btnConvertLatLng.Name = "btnConvertLatLng";
            btnConvertLatLng.Size = new Size(80, 27);
            btnConvertLatLng.TabIndex = 3;
            btnConvertLatLng.Text = "Convert";
            btnConvertLatLng.UseVisualStyleBackColor = true;
            btnConvertLatLng.Click += btnConvertLatLng_Click;
            // 
            // grpConvertDMSInputs
            // 
            grpConvertDMSInputs.Controls.Add(lblConvertDMSLat);
            grpConvertDMSInputs.Controls.Add(txtConvertDMSLatDeg);
            grpConvertDMSInputs.Controls.Add(lblConvertDMSLatDeg);
            grpConvertDMSInputs.Controls.Add(txtConvertDMSLatMin);
            grpConvertDMSInputs.Controls.Add(lblConvertDMSLatMin);
            grpConvertDMSInputs.Controls.Add(txtConvertDMSLatSec);
            grpConvertDMSInputs.Controls.Add(lblConvertDMSLatSec);
            grpConvertDMSInputs.Controls.Add(cmbConvertDMSLatHemisphere);
            grpConvertDMSInputs.Controls.Add(lblConvertDMSLng);
            grpConvertDMSInputs.Controls.Add(txtConvertDMSLngDeg);
            grpConvertDMSInputs.Controls.Add(lblConvertDMSLngDeg);
            grpConvertDMSInputs.Controls.Add(txtConvertDMSLngMin);
            grpConvertDMSInputs.Controls.Add(lblConvertDMSLngMin);
            grpConvertDMSInputs.Controls.Add(txtConvertDMSLngSec);
            grpConvertDMSInputs.Controls.Add(lblConvertDMSLngSec);
            grpConvertDMSInputs.Controls.Add(cmbConvertDMSLngHemisphere);
            grpConvertDMSInputs.Controls.Add(lblConvertDMSAlt);
            grpConvertDMSInputs.Controls.Add(txtConvertDMSAlt);
            grpConvertDMSInputs.Controls.Add(btnConvertDMS);
            grpConvertDMSInputs.Location = new Point(696, 6);
            grpConvertDMSInputs.Name = "grpConvertDMSInputs";
            grpConvertDMSInputs.Size = new Size(348, 150);
            grpConvertDMSInputs.TabIndex = 2;
            grpConvertDMSInputs.TabStop = false;
            grpConvertDMSInputs.Text = "DMS";
            // 
            // lblConvertDMSLat
            // 
            lblConvertDMSLat.AutoSize = true;
            lblConvertDMSLat.Location = new Point(61, 28);
            lblConvertDMSLat.Name = "lblConvertDMSLat";
            lblConvertDMSLat.TabIndex = 11;
            lblConvertDMSLat.Text = "Lat:";
            // 
            // txtConvertDMSLatDeg
            // 
            txtConvertDMSLatDeg.Location = new Point(93, 25);
            txtConvertDMSLatDeg.Name = "txtConvertDMSLatDeg";
            txtConvertDMSLatDeg.Size = new Size(40, 23);
            txtConvertDMSLatDeg.TabIndex = 0;
            // 
            // lblConvertDMSLatDeg
            // 
            lblConvertDMSLatDeg.AutoSize = true;
            lblConvertDMSLatDeg.Location = new Point(133, 28);
            lblConvertDMSLatDeg.Name = "lblConvertDMSLatDeg";
            lblConvertDMSLatDeg.TabIndex = 12;
            lblConvertDMSLatDeg.Text = "°";
            // 
            // txtConvertDMSLatMin
            // 
            txtConvertDMSLatMin.Location = new Point(147, 25);
            txtConvertDMSLatMin.Name = "txtConvertDMSLatMin";
            txtConvertDMSLatMin.Size = new Size(40, 23);
            txtConvertDMSLatMin.TabIndex = 1;
            // 
            // lblConvertDMSLatMin
            // 
            lblConvertDMSLatMin.AutoSize = true;
            lblConvertDMSLatMin.Location = new Point(187, 28);
            lblConvertDMSLatMin.Name = "lblConvertDMSLatMin";
            lblConvertDMSLatMin.TabIndex = 13;
            lblConvertDMSLatMin.Text = "'";
            // 
            // txtConvertDMSLatSec
            // 
            txtConvertDMSLatSec.Location = new Point(199, 25);
            txtConvertDMSLatSec.Name = "txtConvertDMSLatSec";
            txtConvertDMSLatSec.Size = new Size(64, 23);
            txtConvertDMSLatSec.TabIndex = 2;
            // 
            // lblConvertDMSLatSec
            // 
            lblConvertDMSLatSec.AutoSize = true;
            lblConvertDMSLatSec.Location = new Point(263, 28);
            lblConvertDMSLatSec.Name = "lblConvertDMSLatSec";
            lblConvertDMSLatSec.TabIndex = 14;
            lblConvertDMSLatSec.Text = "\"";
            // 
            // cmbConvertDMSLatHemisphere
            // 
            cmbConvertDMSLatHemisphere.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbConvertDMSLatHemisphere.FormattingEnabled = true;
            cmbConvertDMSLatHemisphere.Items.AddRange(new object[] { "N", "S" });
            cmbConvertDMSLatHemisphere.Location = new Point(290, 25);
            cmbConvertDMSLatHemisphere.Name = "cmbConvertDMSLatHemisphere";
            cmbConvertDMSLatHemisphere.Size = new Size(48, 23);
            cmbConvertDMSLatHemisphere.TabIndex = 3;
            // 
            // lblConvertDMSLng
            // 
            lblConvertDMSLng.AutoSize = true;
            lblConvertDMSLng.Location = new Point(57, 57);
            lblConvertDMSLng.Name = "lblConvertDMSLng";
            lblConvertDMSLng.TabIndex = 15;
            lblConvertDMSLng.Text = "Lng:";
            // 
            // txtConvertDMSLngDeg
            // 
            txtConvertDMSLngDeg.Location = new Point(93, 54);
            txtConvertDMSLngDeg.Name = "txtConvertDMSLngDeg";
            txtConvertDMSLngDeg.Size = new Size(40, 23);
            txtConvertDMSLngDeg.TabIndex = 4;
            // 
            // lblConvertDMSLngDeg
            // 
            lblConvertDMSLngDeg.AutoSize = true;
            lblConvertDMSLngDeg.Location = new Point(133, 57);
            lblConvertDMSLngDeg.Name = "lblConvertDMSLngDeg";
            lblConvertDMSLngDeg.TabIndex = 16;
            lblConvertDMSLngDeg.Text = "°";
            // 
            // txtConvertDMSLngMin
            // 
            txtConvertDMSLngMin.Location = new Point(147, 54);
            txtConvertDMSLngMin.Name = "txtConvertDMSLngMin";
            txtConvertDMSLngMin.Size = new Size(40, 23);
            txtConvertDMSLngMin.TabIndex = 5;
            // 
            // lblConvertDMSLngMin
            // 
            lblConvertDMSLngMin.AutoSize = true;
            lblConvertDMSLngMin.Location = new Point(187, 57);
            lblConvertDMSLngMin.Name = "lblConvertDMSLngMin";
            lblConvertDMSLngMin.TabIndex = 17;
            lblConvertDMSLngMin.Text = "'";
            // 
            // txtConvertDMSLngSec
            // 
            txtConvertDMSLngSec.Location = new Point(199, 54);
            txtConvertDMSLngSec.Name = "txtConvertDMSLngSec";
            txtConvertDMSLngSec.Size = new Size(64, 23);
            txtConvertDMSLngSec.TabIndex = 6;
            // 
            // lblConvertDMSLngSec
            // 
            lblConvertDMSLngSec.AutoSize = true;
            lblConvertDMSLngSec.Location = new Point(263, 57);
            lblConvertDMSLngSec.Name = "lblConvertDMSLngSec";
            lblConvertDMSLngSec.TabIndex = 18;
            lblConvertDMSLngSec.Text = "\"";
            // 
            // cmbConvertDMSLngHemisphere
            // 
            cmbConvertDMSLngHemisphere.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbConvertDMSLngHemisphere.FormattingEnabled = true;
            cmbConvertDMSLngHemisphere.Items.AddRange(new object[] { "E", "W" });
            cmbConvertDMSLngHemisphere.Location = new Point(290, 54);
            cmbConvertDMSLngHemisphere.Name = "cmbConvertDMSLngHemisphere";
            cmbConvertDMSLngHemisphere.Size = new Size(48, 23);
            cmbConvertDMSLngHemisphere.TabIndex = 7;
            // 
            // lblConvertDMSAlt
            // 
            lblConvertDMSAlt.AutoSize = true;
            lblConvertDMSAlt.Location = new Point(13, 86);
            lblConvertDMSAlt.Name = "lblConvertDMSAlt";
            lblConvertDMSAlt.TabIndex = 19;
            lblConvertDMSAlt.Text = "Altitude (m):";
            // 
            // txtConvertDMSAlt
            // 
            txtConvertDMSAlt.Location = new Point(93, 83);
            txtConvertDMSAlt.Name = "txtConvertDMSAlt";
            txtConvertDMSAlt.Size = new Size(170, 23);
            txtConvertDMSAlt.TabIndex = 8;
            // 
            // btnConvertDMS
            // 
            btnConvertDMS.Location = new Point(258, 115);
            btnConvertDMS.Name = "btnConvertDMS";
            btnConvertDMS.Size = new Size(80, 27);
            btnConvertDMS.TabIndex = 9;
            btnConvertDMS.Text = "Convert";
            btnConvertDMS.UseVisualStyleBackColor = true;
            btnConvertDMS.Click += btnConvertDMS_Click;
            // 
            // grpAddConditions
            // 
            grpAddConditions.Controls.Add(lblAddAltitude);
            grpAddConditions.Controls.Add(txtAddAltitude);
            grpAddConditions.Controls.Add(lblAddAltitudeHint);
            grpAddConditions.Location = new Point(8, 162);
            grpAddConditions.Name = "grpAddConditions";
            grpAddConditions.Size = new Size(1036, 52);
            grpAddConditions.TabIndex = 3;
            grpAddConditions.TabStop = false;
            grpAddConditions.Text = "Additional Conditions";
            // 
            // lblAddAltitude
            // 
            lblAddAltitude.AutoSize = true;
            lblAddAltitude.Location = new Point(14, 23);
            lblAddAltitude.Name = "lblAddAltitude";
            lblAddAltitude.TabIndex = 1;
            lblAddAltitude.Text = "Add Altitude (m):";
            // 
            // txtAddAltitude
            // 
            txtAddAltitude.Location = new Point(120, 20);
            txtAddAltitude.Name = "txtAddAltitude";
            txtAddAltitude.PlaceholderText = "0";
            txtAddAltitude.Size = new Size(120, 23);
            txtAddAltitude.TabIndex = 0;
            // 
            // lblAddAltitudeHint
            // 
            lblAddAltitudeHint.AutoSize = true;
            lblAddAltitudeHint.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblAddAltitudeHint.ForeColor = SystemColors.GrayText;
            lblAddAltitudeHint.Location = new Point(250, 24);
            lblAddAltitudeHint.Name = "lblAddAltitudeHint";
            lblAddAltitudeHint.TabIndex = 2;
            lblAddAltitudeHint.Text = "Optional. Added to the altitude of every conversion, e.g. an antenna or sensor height. Blank = 0.";
            // 
            // grpConvertOutputs
            // 
            grpConvertOutputs.Controls.Add(lblConvertXOutput);
            grpConvertOutputs.Controls.Add(txtConvertXOutput);
            grpConvertOutputs.Controls.Add(lblConvertYOutput);
            grpConvertOutputs.Controls.Add(txtConvertYOutput);
            grpConvertOutputs.Controls.Add(lblConvertZOutput);
            grpConvertOutputs.Controls.Add(txtConvertZOutput);
            grpConvertOutputs.Controls.Add(lblConvertLatOutput);
            grpConvertOutputs.Controls.Add(txtConvertLatOutput);
            grpConvertOutputs.Controls.Add(lblConvertLngOutput);
            grpConvertOutputs.Controls.Add(txtConvertLngOutput);
            grpConvertOutputs.Controls.Add(lblConvertAltOutput);
            grpConvertOutputs.Controls.Add(txtConvertAltOutput);
            grpConvertOutputs.Controls.Add(lblConvertDMSLatOutput);
            grpConvertOutputs.Controls.Add(txtConvertDMSLatOutput);
            grpConvertOutputs.Controls.Add(lblConvertDMSLngOutput);
            grpConvertOutputs.Controls.Add(txtConvertDMSLngOutput);
            grpConvertOutputs.Controls.Add(lblConvertStatus);
            grpConvertOutputs.Controls.Add(btnClipboardCopy);
            grpConvertOutputs.Location = new Point(8, 220);
            grpConvertOutputs.Name = "grpConvertOutputs";
            grpConvertOutputs.Size = new Size(1036, 150);
            grpConvertOutputs.TabIndex = 4;
            grpConvertOutputs.TabStop = false;
            grpConvertOutputs.Text = "Outputs";
            // 
            // lblConvertXOutput
            // 
            lblConvertXOutput.AutoSize = true;
            lblConvertXOutput.Location = new Point(46, 28);
            lblConvertXOutput.Name = "lblConvertXOutput";
            lblConvertXOutput.TabIndex = 1;
            lblConvertXOutput.Text = "X (m):";
            // 
            // txtConvertXOutput
            // 
            txtConvertXOutput.Location = new Point(93, 25);
            txtConvertXOutput.Name = "txtConvertXOutput";
            txtConvertXOutput.ReadOnly = true;
            txtConvertXOutput.Size = new Size(233, 23);
            txtConvertXOutput.TabIndex = 2;
            txtConvertXOutput.TabStop = false;
            // 
            // lblConvertYOutput
            // 
            lblConvertYOutput.AutoSize = true;
            lblConvertYOutput.Location = new Point(46, 57);
            lblConvertYOutput.Name = "lblConvertYOutput";
            lblConvertYOutput.TabIndex = 3;
            lblConvertYOutput.Text = "Y (m):";
            // 
            // txtConvertYOutput
            // 
            txtConvertYOutput.Location = new Point(93, 54);
            txtConvertYOutput.Name = "txtConvertYOutput";
            txtConvertYOutput.ReadOnly = true;
            txtConvertYOutput.Size = new Size(233, 23);
            txtConvertYOutput.TabIndex = 4;
            txtConvertYOutput.TabStop = false;
            // 
            // lblConvertZOutput
            // 
            lblConvertZOutput.AutoSize = true;
            lblConvertZOutput.Location = new Point(46, 86);
            lblConvertZOutput.Name = "lblConvertZOutput";
            lblConvertZOutput.TabIndex = 5;
            lblConvertZOutput.Text = "Z (m):";
            // 
            // txtConvertZOutput
            // 
            txtConvertZOutput.Location = new Point(93, 83);
            txtConvertZOutput.Name = "txtConvertZOutput";
            txtConvertZOutput.ReadOnly = true;
            txtConvertZOutput.Size = new Size(233, 23);
            txtConvertZOutput.TabIndex = 6;
            txtConvertZOutput.TabStop = false;
            // 
            // lblConvertLatOutput
            // 
            lblConvertLatOutput.AutoSize = true;
            lblConvertLatOutput.Location = new Point(405, 28);
            lblConvertLatOutput.Name = "lblConvertLatOutput";
            lblConvertLatOutput.TabIndex = 7;
            lblConvertLatOutput.Text = "Lat:";
            // 
            // txtConvertLatOutput
            // 
            txtConvertLatOutput.Location = new Point(437, 25);
            txtConvertLatOutput.Name = "txtConvertLatOutput";
            txtConvertLatOutput.ReadOnly = true;
            txtConvertLatOutput.Size = new Size(233, 23);
            txtConvertLatOutput.TabIndex = 8;
            txtConvertLatOutput.TabStop = false;
            // 
            // lblConvertLngOutput
            // 
            lblConvertLngOutput.AutoSize = true;
            lblConvertLngOutput.Location = new Point(401, 57);
            lblConvertLngOutput.Name = "lblConvertLngOutput";
            lblConvertLngOutput.TabIndex = 9;
            lblConvertLngOutput.Text = "Lng:";
            // 
            // txtConvertLngOutput
            // 
            txtConvertLngOutput.Location = new Point(437, 54);
            txtConvertLngOutput.Name = "txtConvertLngOutput";
            txtConvertLngOutput.ReadOnly = true;
            txtConvertLngOutput.Size = new Size(233, 23);
            txtConvertLngOutput.TabIndex = 10;
            txtConvertLngOutput.TabStop = false;
            // 
            // lblConvertAltOutput
            // 
            lblConvertAltOutput.AutoSize = true;
            lblConvertAltOutput.Location = new Point(357, 86);
            lblConvertAltOutput.Name = "lblConvertAltOutput";
            lblConvertAltOutput.TabIndex = 11;
            lblConvertAltOutput.Text = "Altitude (m):";
            // 
            // txtConvertAltOutput
            // 
            txtConvertAltOutput.Location = new Point(437, 83);
            txtConvertAltOutput.Name = "txtConvertAltOutput";
            txtConvertAltOutput.ReadOnly = true;
            txtConvertAltOutput.Size = new Size(233, 23);
            txtConvertAltOutput.TabIndex = 12;
            txtConvertAltOutput.TabStop = false;
            // 
            // lblConvertDMSLatOutput
            // 
            lblConvertDMSLatOutput.AutoSize = true;
            lblConvertDMSLatOutput.Location = new Point(727, 28);
            lblConvertDMSLatOutput.Name = "lblConvertDMSLatOutput";
            lblConvertDMSLatOutput.TabIndex = 13;
            lblConvertDMSLatOutput.Text = "DMS Lat:";
            // 
            // txtConvertDMSLatOutput
            // 
            txtConvertDMSLatOutput.Location = new Point(781, 25);
            txtConvertDMSLatOutput.Name = "txtConvertDMSLatOutput";
            txtConvertDMSLatOutput.ReadOnly = true;
            txtConvertDMSLatOutput.Size = new Size(245, 23);
            txtConvertDMSLatOutput.TabIndex = 14;
            txtConvertDMSLatOutput.TabStop = false;
            // 
            // lblConvertDMSLngOutput
            // 
            lblConvertDMSLngOutput.AutoSize = true;
            lblConvertDMSLngOutput.Location = new Point(723, 57);
            lblConvertDMSLngOutput.Name = "lblConvertDMSLngOutput";
            lblConvertDMSLngOutput.TabIndex = 15;
            lblConvertDMSLngOutput.Text = "DMS Lng:";
            // 
            // txtConvertDMSLngOutput
            // 
            txtConvertDMSLngOutput.Location = new Point(781, 54);
            txtConvertDMSLngOutput.Name = "txtConvertDMSLngOutput";
            txtConvertDMSLngOutput.ReadOnly = true;
            txtConvertDMSLngOutput.Size = new Size(245, 23);
            txtConvertDMSLngOutput.TabIndex = 16;
            txtConvertDMSLngOutput.TabStop = false;
            // 
            // lblConvertStatus
            // 
            lblConvertStatus.Location = new Point(14, 120);
            lblConvertStatus.Name = "lblConvertStatus";
            lblConvertStatus.Size = new Size(700, 17);
            lblConvertStatus.TabIndex = 17;
            // 
            // btnClipboardCopy
            // 
            btnClipboardCopy.Location = new Point(886, 113);
            btnClipboardCopy.Name = "btnClipboardCopy";
            btnClipboardCopy.Size = new Size(140, 27);
            btnClipboardCopy.TabIndex = 0;
            btnClipboardCopy.Text = "Copy to Clipboard";
            btnClipboardCopy.UseVisualStyleBackColor = true;
            btnClipboardCopy.Click += btnClipboardCopy_Click;
            // 
            // mainTab
            // 
            mainTab.Controls.Add(tabConverter);
            mainTab.Controls.Add(tabDistance);
            mainTab.Location = new Point(12, 12);
            mainTab.Name = "mainTab";
            mainTab.SelectedIndex = 0;
            mainTab.Size = new Size(1060, 560);
            mainTab.TabIndex = 17;
            // 
            // tabConverter
            // 
            tabConverter.Controls.Add(grpConvertGeocentricInputs);
            tabConverter.Controls.Add(grpConvertLatLngInputs);
            tabConverter.Controls.Add(grpConvertDMSInputs);
            tabConverter.Controls.Add(grpAddConditions);
            tabConverter.Controls.Add(grpConvertOutputs);
            tabConverter.Location = new Point(4, 24);
            tabConverter.Name = "tabConverter";
            tabConverter.Padding = new Padding(3);
            tabConverter.Size = new Size(1052, 532);
            tabConverter.TabIndex = 0;
            tabConverter.Text = "Converter";
            tabConverter.UseVisualStyleBackColor = true;
            // 
            // tabDistance
            // 
            tabDistance.Controls.Add(grpDFReplaceText);
            tabDistance.Controls.Add(grpDFOutput);
            tabDistance.Controls.Add(grpDFAzimuthPreview);
            tabDistance.Controls.Add(btnDFCalculate);
            tabDistance.Controls.Add(grpDFAzimuth);
            tabDistance.Controls.Add(grpDFConditions);
            tabDistance.Controls.Add(grpDFInputs);
            tabDistance.Location = new Point(4, 24);
            tabDistance.Name = "tabDistance";
            tabDistance.Size = new Size(1052, 532);
            tabDistance.TabIndex = 1;
            tabDistance.Text = "Distance From";
            tabDistance.UseVisualStyleBackColor = true;
            // 
            // grpDFReplaceText
            // 
            grpDFReplaceText.Controls.Add(tipIconDFReplaceOutput);
            grpDFReplaceText.Controls.Add(txtDFReplaceRunningNo);
            grpDFReplaceText.Controls.Add(lblDFReplaceRunningNo);
            grpDFReplaceText.Controls.Add(cbDFReplaceText);
            grpDFReplaceText.Controls.Add(txtDFReplaceText);
            grpDFReplaceText.Location = new Point(352, 438);
            grpDFReplaceText.Name = "grpDFReplaceText";
            grpDFReplaceText.Size = new Size(692, 90);
            grpDFReplaceText.TabIndex = 6;
            grpDFReplaceText.TabStop = false;
            grpDFReplaceText.Text = "Replace Output";
            //
            // txtDFReplaceRunningNo
            //
            txtDFReplaceRunningNo.Location = new Point(562, 18);
            txtDFReplaceRunningNo.Name = "txtDFReplaceRunningNo";
            txtDFReplaceRunningNo.Size = new Size(122, 23);
            txtDFReplaceRunningNo.TabIndex = 1;
            txtDFReplaceRunningNo.TextChanged += txtDFInput_TextChanged;
            //
            // lblDFReplaceRunningNo
            //
            lblDFReplaceRunningNo.AutoSize = true;
            lblDFReplaceRunningNo.Location = new Point(486, 22);
            lblDFReplaceRunningNo.Name = "lblDFReplaceRunningNo";
            lblDFReplaceRunningNo.Size = new Size(74, 15);
            lblDFReplaceRunningNo.TabIndex = 4;
            lblDFReplaceRunningNo.Text = "Running No:";
            //
            // cbDFReplaceText
            //
            cbDFReplaceText.AutoSize = true;
            cbDFReplaceText.Location = new Point(8, 20);
            cbDFReplaceText.Name = "cbDFReplaceText";
            cbDFReplaceText.Size = new Size(195, 19);
            cbDFReplaceText.TabIndex = 0;
            cbDFReplaceText.Text = "Replace Output With Below Text";
            cbDFReplaceText.UseVisualStyleBackColor = true;
            cbDFReplaceText.CheckedChanged += cbDFReplaceText_CheckedChanged;
            //
            // txtDFReplaceText
            //
            txtDFReplaceText.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDFReplaceText.Location = new Point(8, 44);
            txtDFReplaceText.Name = "txtDFReplaceText";
            txtDFReplaceText.Size = new Size(676, 40);
            txtDFReplaceText.TabIndex = 2;
            txtDFReplaceText.Text = "";
            txtDFReplaceText.TextChanged += txtDFInput_TextChanged;
            //
            // grpDFOutput
            //
            grpDFOutput.Controls.Add(cbDFShowGeocentric);
            grpDFOutput.Controls.Add(cbDFShowGeodetic);
            grpDFOutput.Controls.Add(cbDFShowDMS);
            grpDFOutput.Controls.Add(lblDFOutputShow);
            grpDFOutput.Controls.Add(lblDFOutputStatus);
            grpDFOutput.Controls.Add(txtDFRichResult);
            grpDFOutput.Location = new Point(352, 246);
            grpDFOutput.Name = "grpDFOutput";
            grpDFOutput.Size = new Size(692, 186);
            grpDFOutput.TabIndex = 5;
            grpDFOutput.TabStop = false;
            grpDFOutput.Text = "Output";
            //
            // lblDFOutputStatus
            //
            lblDFOutputStatus.AutoEllipsis = true;
            lblDFOutputStatus.Location = new Point(8, 19);
            lblDFOutputStatus.Name = "lblDFOutputStatus";
            lblDFOutputStatus.Size = new Size(370, 17);
            lblDFOutputStatus.TabIndex = 4;
            //
            // txtDFRichResult
            //
            txtDFRichResult.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDFRichResult.Location = new Point(8, 38);
            txtDFRichResult.Name = "txtDFRichResult";
            txtDFRichResult.Size = new Size(676, 142);
            txtDFRichResult.TabIndex = 3;
            txtDFRichResult.Text = "";
            //
            // grpDFConditions
            //
            grpDFConditions.Controls.Add(txtDFElevation);
            grpDFConditions.Controls.Add(lblDFElevation);
            grpDFConditions.Controls.Add(txtDFDistance);
            grpDFConditions.Controls.Add(lblDFDistance);
            grpDFConditions.Location = new Point(8, 154);
            grpDFConditions.Name = "grpDFConditions";
            grpDFConditions.Size = new Size(336, 84);
            grpDFConditions.TabIndex = 1;
            grpDFConditions.TabStop = false;
            grpDFConditions.Text = "Distance and Elevation";
            //
            // tipIconDFAzimuth
            //
            tipIconDFAzimuth.Location = new Point(308, 25);
            tipIconDFAzimuth.Name = "tipIconDFAzimuth";
            tipIconDFAzimuth.Size = new Size(17, 17);
            tipIconDFAzimuth.TabIndex = 20;
            tipIconDFAzimuth.TabStop = false;
            //
            // txtDFElevation
            //
            txtDFElevation.Location = new Point(152, 51);
            txtDFElevation.Name = "txtDFElevation";
            txtDFElevation.PlaceholderText = "blank = along surface";
            txtDFElevation.Size = new Size(174, 23);
            txtDFElevation.TabIndex = 1;
            txtDFElevation.TextChanged += txtDFInput_TextChanged;
            //
            // lblDFElevation
            //
            lblDFElevation.AutoSize = true;
            lblDFElevation.Location = new Point(6, 54);
            lblDFElevation.Name = "lblDFElevation";
            lblDFElevation.Size = new Size(140, 15);
            lblDFElevation.TabIndex = 3;
            lblDFElevation.Text = "Elevation Angle (Degree):";
            //
            // btnDFCalculate
            //
            btnDFCalculate.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnDFCalculate.Location = new Point(184, 490);
            btnDFCalculate.Name = "btnDFCalculate";
            btnDFCalculate.Size = new Size(160, 34);
            btnDFCalculate.TabIndex = 3;
            btnDFCalculate.Text = "&Calculate";
            btnDFCalculate.UseVisualStyleBackColor = true;
            btnDFCalculate.Click += btnDFCalculate_Click;
            //
            // btnPasteFromClipboard
            //
            btnPasteFromClipboard.Location = new Point(93, 109);
            btnPasteFromClipboard.Name = "btnPasteFromClipboard";
            btnPasteFromClipboard.Size = new Size(146, 25);
            btnPasteFromClipboard.TabIndex = 3;
            btnPasteFromClipboard.Text = "Paste From Clipboard";
            btnPasteFromClipboard.UseVisualStyleBackColor = true;
            btnPasteFromClipboard.Click += btnPasteFromClipboard_Click;
            //
            // txtDFDistance
            //
            txtDFDistance.Location = new Point(152, 22);
            txtDFDistance.Name = "txtDFDistance";
            txtDFDistance.Size = new Size(174, 23);
            txtDFDistance.TabIndex = 0;
            txtDFDistance.TextChanged += txtDFInput_TextChanged;
            //
            // lblDFDistance
            //
            lblDFDistance.AutoSize = true;
            lblDFDistance.Location = new Point(31, 25);
            lblDFDistance.Name = "lblDFDistance";
            lblDFDistance.Size = new Size(115, 15);
            lblDFDistance.TabIndex = 2;
            lblDFDistance.Text = "Distance Apart (km):";
            //
            // txtDFDeg
            //
            txtDFDeg.Location = new Point(78, 22);
            txtDFDeg.Name = "txtDFDeg";
            txtDFDeg.PlaceholderText = "e.g. 0,90,180";
            txtDFDeg.Size = new Size(224, 23);
            txtDFDeg.TabIndex = 1;
            txtDFDeg.TextChanged += txtDFInput_TextChanged;
            //
            // grpDFInputs
            //
            grpDFInputs.Controls.Add(btnPasteFromClipboard);
            grpDFInputs.Controls.Add(txtDFLat);
            grpDFInputs.Controls.Add(lblDFLat);
            grpDFInputs.Controls.Add(lblDFAltitude);
            grpDFInputs.Controls.Add(lblDFLng);
            grpDFInputs.Controls.Add(txtDFAltitude);
            grpDFInputs.Controls.Add(txtDFLng);
            grpDFInputs.Location = new Point(8, 6);
            grpDFInputs.Name = "grpDFInputs";
            grpDFInputs.Size = new Size(336, 142);
            grpDFInputs.TabIndex = 0;
            grpDFInputs.TabStop = false;
            grpDFInputs.Text = "Target Coordinate";
            //
            // txtDFLat
            //
            txtDFLat.Location = new Point(93, 22);
            txtDFLat.Name = "txtDFLat";
            txtDFLat.Size = new Size(233, 23);
            txtDFLat.TabIndex = 0;
            txtDFLat.TextChanged += txtDFInput_TextChanged;
            //
            // lblDFLat
            //
            lblDFLat.AutoSize = true;
            lblDFLat.Location = new Point(61, 25);
            lblDFLat.Name = "lblDFLat";
            lblDFLat.Size = new Size(26, 15);
            lblDFLat.TabIndex = 4;
            lblDFLat.Text = "Lat:";
            //
            // lblDFAltitude
            //
            lblDFAltitude.AutoSize = true;
            lblDFAltitude.Location = new Point(13, 83);
            lblDFAltitude.Name = "lblDFAltitude";
            lblDFAltitude.Size = new Size(74, 15);
            lblDFAltitude.TabIndex = 6;
            lblDFAltitude.Text = "Altitude (m):";
            //
            // lblDFLng
            //
            lblDFLng.AutoSize = true;
            lblDFLng.Location = new Point(57, 54);
            lblDFLng.Name = "lblDFLng";
            lblDFLng.Size = new Size(30, 15);
            lblDFLng.TabIndex = 5;
            lblDFLng.Text = "Lng:";
            //
            // txtDFAltitude
            //
            txtDFAltitude.Location = new Point(93, 80);
            txtDFAltitude.Name = "txtDFAltitude";
            txtDFAltitude.Size = new Size(233, 23);
            txtDFAltitude.TabIndex = 2;
            txtDFAltitude.TextChanged += txtDFInput_TextChanged;
            //
            // txtDFLng
            //
            txtDFLng.Location = new Point(93, 51);
            txtDFLng.Name = "txtDFLng";
            txtDFLng.Size = new Size(233, 23);
            txtDFLng.TabIndex = 1;
            txtDFLng.TextChanged += txtDFInput_TextChanged;
            //
            // txtClipboard
            //
            txtClipboard.BackColor = SystemColors.GradientInactiveCaption;
            txtClipboard.Location = new Point(79, 581);
            txtClipboard.Margin = new Padding(2);
            txtClipboard.Name = "txtClipboard";
            txtClipboard.ReadOnly = true;
            txtClipboard.Size = new Size(560, 23);
            txtClipboard.TabIndex = 18;
            //
            // lblClipboard
            //
            lblClipboard.AutoSize = true;
            lblClipboard.Location = new Point(12, 584);
            lblClipboard.Name = "lblClipboard";
            lblClipboard.Size = new Size(62, 15);
            lblClipboard.TabIndex = 22;
            lblClipboard.Text = "Clipboard:";
            //
            // lblGithub
            //
            lblGithub.AutoSize = true;
            lblGithub.Location = new Point(842, 585);
            lblGithub.Name = "lblGithub";
            lblGithub.Size = new Size(230, 15);
            lblGithub.TabIndex = 23;
            lblGithub.Text = "https://github.com/luxcan/geo-calculator";
            //
            // tipIconDFReplaceOutput
            //
            tipIconDFReplaceOutput.Location = new Point(206, 21);
            tipIconDFReplaceOutput.Name = "tipIconDFReplaceOutput";
            tipIconDFReplaceOutput.Size = new Size(17, 17);
            tipIconDFReplaceOutput.TabIndex = 19;
            tipIconDFReplaceOutput.TabStop = false;
            //
            // grpDFAzimuth
            //
            grpDFAzimuth.Controls.Add(rbDFAzimuthList);
            grpDFAzimuth.Controls.Add(txtDFDeg);
            grpDFAzimuth.Controls.Add(tipIconDFAzimuth);
            grpDFAzimuth.Controls.Add(lblDFAzimuthListHint);
            grpDFAzimuth.Controls.Add(lblDFAzimuthDivider);
            grpDFAzimuth.Controls.Add(rbDFAzimuthRange);
            grpDFAzimuth.Controls.Add(txtDFAzimuthStart);
            grpDFAzimuth.Controls.Add(lblDFAzimuthTo);
            grpDFAzimuth.Controls.Add(txtDFAzimuthEnd);
            grpDFAzimuth.Controls.Add(lblDFAzimuthEvery);
            grpDFAzimuth.Controls.Add(txtDFAzimuthStep);
            grpDFAzimuth.Controls.Add(lblDFAzimuthDirection);
            grpDFAzimuth.Controls.Add(pnlDFAzimuthDirection);
            grpDFAzimuth.Controls.Add(lblDFAzimuthRangeHint);
            grpDFAzimuth.Controls.Add(btnDFAzimuthCopyToList);
            grpDFAzimuth.Controls.Add(picDFAzimuthStatus);
            grpDFAzimuth.Controls.Add(lblDFAzimuthStatus);
            grpDFAzimuth.Location = new Point(8, 244);
            grpDFAzimuth.Name = "grpDFAzimuth";
            grpDFAzimuth.Size = new Size(336, 238);
            grpDFAzimuth.TabIndex = 2;
            grpDFAzimuth.TabStop = false;
            grpDFAzimuth.Text = "Azimuth (Degree, clockwise from true north)";
            //
            // rbDFAzimuthList
            //
            rbDFAzimuthList.AutoSize = true;
            rbDFAzimuthList.Checked = true;
            rbDFAzimuthList.Location = new Point(10, 24);
            rbDFAzimuthList.Name = "rbDFAzimuthList";
            rbDFAzimuthList.Size = new Size(47, 19);
            rbDFAzimuthList.TabIndex = 0;
            rbDFAzimuthList.TabStop = true;
            rbDFAzimuthList.Text = "&List:";
            rbDFAzimuthList.UseVisualStyleBackColor = true;
            rbDFAzimuthList.CheckedChanged += rbDFAzimuthMode_CheckedChanged;
            //
            // lblDFAzimuthListHint
            //
            lblDFAzimuthListHint.AutoSize = true;
            lblDFAzimuthListHint.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblDFAzimuthListHint.ForeColor = SystemColors.GrayText;
            lblDFAzimuthListHint.Location = new Point(78, 48);
            lblDFAzimuthListHint.Name = "lblDFAzimuthListHint";
            lblDFAzimuthListHint.Size = new Size(214, 13);
            lblDFAzimuthListHint.TabIndex = 9;
            lblDFAzimuthListHint.Text = "Comma-separated · used in the order typed";
            //
            // lblDFAzimuthDivider
            //
            lblDFAzimuthDivider.BorderStyle = BorderStyle.Fixed3D;
            lblDFAzimuthDivider.Location = new Point(10, 68);
            lblDFAzimuthDivider.Name = "lblDFAzimuthDivider";
            lblDFAzimuthDivider.Size = new Size(316, 2);
            lblDFAzimuthDivider.TabIndex = 10;
            //
            // rbDFAzimuthRange
            //
            rbDFAzimuthRange.AutoSize = true;
            rbDFAzimuthRange.Location = new Point(10, 80);
            rbDFAzimuthRange.Name = "rbDFAzimuthRange";
            rbDFAzimuthRange.Size = new Size(64, 19);
            rbDFAzimuthRange.TabIndex = 2;
            rbDFAzimuthRange.Text = "&Range:";
            rbDFAzimuthRange.UseVisualStyleBackColor = true;
            rbDFAzimuthRange.CheckedChanged += rbDFAzimuthMode_CheckedChanged;
            //
            // txtDFAzimuthStart
            //
            txtDFAzimuthStart.Location = new Point(78, 78);
            txtDFAzimuthStart.Name = "txtDFAzimuthStart";
            txtDFAzimuthStart.PlaceholderText = "start";
            txtDFAzimuthStart.Size = new Size(58, 23);
            txtDFAzimuthStart.TabIndex = 3;
            txtDFAzimuthStart.TextChanged += txtDFInput_TextChanged;
            //
            // lblDFAzimuthTo
            //
            lblDFAzimuthTo.AutoSize = true;
            lblDFAzimuthTo.Location = new Point(139, 81);
            lblDFAzimuthTo.Name = "lblDFAzimuthTo";
            lblDFAzimuthTo.Size = new Size(18, 15);
            lblDFAzimuthTo.TabIndex = 11;
            lblDFAzimuthTo.Text = "to";
            //
            // txtDFAzimuthEnd
            //
            txtDFAzimuthEnd.Location = new Point(157, 78);
            txtDFAzimuthEnd.Name = "txtDFAzimuthEnd";
            txtDFAzimuthEnd.PlaceholderText = "end";
            txtDFAzimuthEnd.Size = new Size(58, 23);
            txtDFAzimuthEnd.TabIndex = 4;
            txtDFAzimuthEnd.TextChanged += txtDFInput_TextChanged;
            //
            // lblDFAzimuthEvery
            //
            lblDFAzimuthEvery.AutoSize = true;
            lblDFAzimuthEvery.Location = new Point(218, 81);
            lblDFAzimuthEvery.Name = "lblDFAzimuthEvery";
            lblDFAzimuthEvery.Size = new Size(34, 15);
            lblDFAzimuthEvery.TabIndex = 12;
            lblDFAzimuthEvery.Text = "every";
            //
            // txtDFAzimuthStep
            //
            txtDFAzimuthStep.Location = new Point(253, 78);
            txtDFAzimuthStep.Name = "txtDFAzimuthStep";
            txtDFAzimuthStep.PlaceholderText = "step";
            txtDFAzimuthStep.Size = new Size(49, 23);
            txtDFAzimuthStep.TabIndex = 5;
            txtDFAzimuthStep.TextChanged += txtDFInput_TextChanged;
            //
            // lblDFAzimuthDirection
            //
            lblDFAzimuthDirection.AutoSize = true;
            lblDFAzimuthDirection.Location = new Point(22, 110);
            lblDFAzimuthDirection.Name = "lblDFAzimuthDirection";
            lblDFAzimuthDirection.Size = new Size(58, 15);
            lblDFAzimuthDirection.TabIndex = 13;
            lblDFAzimuthDirection.Text = "Direction:";
            //
            // pnlDFAzimuthDirection
            //
            pnlDFAzimuthDirection.Controls.Add(rbDFAzimuthClockwise);
            pnlDFAzimuthDirection.Controls.Add(rbDFAzimuthAnticlockwise);
            pnlDFAzimuthDirection.Location = new Point(78, 106);
            pnlDFAzimuthDirection.Name = "pnlDFAzimuthDirection";
            pnlDFAzimuthDirection.Size = new Size(248, 24);
            pnlDFAzimuthDirection.TabIndex = 6;
            //
            // rbDFAzimuthClockwise
            //
            rbDFAzimuthClockwise.AutoSize = true;
            rbDFAzimuthClockwise.Checked = true;
            rbDFAzimuthClockwise.Location = new Point(0, 3);
            rbDFAzimuthClockwise.Name = "rbDFAzimuthClockwise";
            rbDFAzimuthClockwise.Size = new Size(77, 19);
            rbDFAzimuthClockwise.TabIndex = 0;
            rbDFAzimuthClockwise.TabStop = true;
            rbDFAzimuthClockwise.Text = "Clockwise";
            rbDFAzimuthClockwise.UseVisualStyleBackColor = true;
            rbDFAzimuthClockwise.CheckedChanged += rbDFAzimuthDirection_CheckedChanged;
            //
            // rbDFAzimuthAnticlockwise
            //
            rbDFAzimuthAnticlockwise.AutoSize = true;
            rbDFAzimuthAnticlockwise.Location = new Point(88, 3);
            rbDFAzimuthAnticlockwise.Name = "rbDFAzimuthAnticlockwise";
            rbDFAzimuthAnticlockwise.Size = new Size(98, 19);
            rbDFAzimuthAnticlockwise.TabIndex = 1;
            rbDFAzimuthAnticlockwise.Text = "Anticlockwise";
            rbDFAzimuthAnticlockwise.UseVisualStyleBackColor = true;
            rbDFAzimuthAnticlockwise.CheckedChanged += rbDFAzimuthDirection_CheckedChanged;
            //
            // lblDFAzimuthRangeHint
            //
            lblDFAzimuthRangeHint.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblDFAzimuthRangeHint.ForeColor = SystemColors.GrayText;
            lblDFAzimuthRangeHint.Location = new Point(78, 134);
            lblDFAzimuthRangeHint.Name = "lblDFAzimuthRangeHint";
            lblDFAzimuthRangeHint.Size = new Size(248, 30);
            lblDFAzimuthRangeHint.TabIndex = 14;
            lblDFAzimuthRangeHint.Text = "Start is always included; end is included when it falls on a step.";
            //
            // btnDFAzimuthCopyToList
            //
            btnDFAzimuthCopyToList.Location = new Point(196, 166);
            btnDFAzimuthCopyToList.Name = "btnDFAzimuthCopyToList";
            btnDFAzimuthCopyToList.Size = new Size(130, 25);
            btnDFAzimuthCopyToList.TabIndex = 7;
            btnDFAzimuthCopyToList.Text = "C&opy Range to List";
            btnDFAzimuthCopyToList.UseVisualStyleBackColor = true;
            btnDFAzimuthCopyToList.Click += btnDFAzimuthCopyToList_Click;
            //
            // picDFAzimuthStatus
            //
            picDFAzimuthStatus.Location = new Point(10, 202);
            picDFAzimuthStatus.Name = "picDFAzimuthStatus";
            picDFAzimuthStatus.Size = new Size(16, 16);
            picDFAzimuthStatus.SizeMode = PictureBoxSizeMode.Zoom;
            picDFAzimuthStatus.TabIndex = 15;
            picDFAzimuthStatus.TabStop = false;
            //
            // lblDFAzimuthStatus
            //
            lblDFAzimuthStatus.AutoEllipsis = true;
            lblDFAzimuthStatus.Location = new Point(30, 200);
            lblDFAzimuthStatus.Name = "lblDFAzimuthStatus";
            lblDFAzimuthStatus.Size = new Size(296, 32);
            lblDFAzimuthStatus.TabIndex = 16;
            //
            // grpDFAzimuthPreview
            //
            grpDFAzimuthPreview.Controls.Add(pnlDFAzimuthCompass);
            grpDFAzimuthPreview.Controls.Add(lblDFAzimuthCount);
            grpDFAzimuthPreview.Controls.Add(lblDFAzimuthSpan);
            grpDFAzimuthPreview.Controls.Add(lvDFAzimuths);
            grpDFAzimuthPreview.Location = new Point(352, 6);
            grpDFAzimuthPreview.Name = "grpDFAzimuthPreview";
            grpDFAzimuthPreview.Size = new Size(692, 234);
            grpDFAzimuthPreview.TabIndex = 4;
            grpDFAzimuthPreview.TabStop = false;
            grpDFAzimuthPreview.Text = "Azimuth Preview";
            //
            // pnlDFAzimuthCompass
            //
            pnlDFAzimuthCompass.BorderStyle = BorderStyle.FixedSingle;
            pnlDFAzimuthCompass.Location = new Point(8, 20);
            pnlDFAzimuthCompass.Name = "pnlDFAzimuthCompass";
            pnlDFAzimuthCompass.Size = new Size(196, 196);
            pnlDFAzimuthCompass.TabIndex = 1;
            pnlDFAzimuthCompass.RayClicked += pnlDFAzimuthCompass_RayClicked;
            //
            // lblDFAzimuthCount
            //
            lblDFAzimuthCount.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblDFAzimuthCount.Location = new Point(214, 20);
            lblDFAzimuthCount.Name = "lblDFAzimuthCount";
            lblDFAzimuthCount.Size = new Size(470, 22);
            lblDFAzimuthCount.TabIndex = 2;
            //
            // lblDFAzimuthSpan
            //
            lblDFAzimuthSpan.AutoEllipsis = true;
            lblDFAzimuthSpan.Location = new Point(214, 44);
            lblDFAzimuthSpan.Name = "lblDFAzimuthSpan";
            lblDFAzimuthSpan.Size = new Size(470, 18);
            lblDFAzimuthSpan.TabIndex = 3;
            //
            // lvDFAzimuths
            //
            lvDFAzimuths.Columns.AddRange(new ColumnHeader[] { colDFAzimuthNo, colDFAzimuthValue, colDFAzimuthLat, colDFAzimuthLng });
            lvDFAzimuths.FullRowSelect = true;
            lvDFAzimuths.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvDFAzimuths.HideSelection = false;
            lvDFAzimuths.Location = new Point(214, 66);
            lvDFAzimuths.MultiSelect = false;
            lvDFAzimuths.Name = "lvDFAzimuths";
            lvDFAzimuths.Size = new Size(470, 162);
            lvDFAzimuths.TabIndex = 0;
            lvDFAzimuths.UseCompatibleStateImageBehavior = false;
            lvDFAzimuths.View = View.Details;
            lvDFAzimuths.SelectedIndexChanged += lvDFAzimuths_SelectedIndexChanged;
            //
            // colDFAzimuthNo
            //
            colDFAzimuthNo.Text = "#";
            colDFAzimuthNo.Width = 36;
            //
            // colDFAzimuthValue
            //
            colDFAzimuthValue.Text = "Azimuth";
            colDFAzimuthValue.Width = 120;
            // 
            // colDFAzimuthLat
            // 
            colDFAzimuthLat.Text = "Latitude";
            colDFAzimuthLat.Width = 132;
            // 
            // colDFAzimuthLng
            // 
            colDFAzimuthLng.Text = "Longitude";
            colDFAzimuthLng.Width = 132;
            //
            // lblDFOutputShow
            //
            lblDFOutputShow.AutoSize = true;
            lblDFOutputShow.Location = new Point(384, 20);
            lblDFOutputShow.Name = "lblDFOutputShow";
            lblDFOutputShow.Size = new Size(39, 15);
            lblDFOutputShow.TabIndex = 5;
            lblDFOutputShow.Text = "Show:";
            //
            // cbDFShowGeocentric
            //
            cbDFShowGeocentric.AutoSize = true;
            cbDFShowGeocentric.Checked = true;
            cbDFShowGeocentric.CheckState = CheckState.Checked;
            cbDFShowGeocentric.Location = new Point(428, 18);
            cbDFShowGeocentric.Name = "cbDFShowGeocentric";
            cbDFShowGeocentric.Size = new Size(61, 19);
            cbDFShowGeocentric.TabIndex = 0;
            cbDFShowGeocentric.Text = "X, Y, Z";
            cbDFShowGeocentric.UseVisualStyleBackColor = true;
            cbDFShowGeocentric.CheckedChanged += cbDFShowOutput_CheckedChanged;
            //
            // cbDFShowGeodetic
            //
            cbDFShowGeodetic.AutoSize = true;
            cbDFShowGeodetic.Checked = true;
            cbDFShowGeodetic.CheckState = CheckState.Checked;
            cbDFShowGeodetic.Location = new Point(497, 18);
            cbDFShowGeodetic.Name = "cbDFShowGeodetic";
            cbDFShowGeodetic.Size = new Size(119, 19);
            cbDFShowGeodetic.TabIndex = 1;
            cbDFShowGeodetic.Text = "Lat, Lng, Altitude";
            cbDFShowGeodetic.UseVisualStyleBackColor = true;
            cbDFShowGeodetic.CheckedChanged += cbDFShowOutput_CheckedChanged;
            //
            // cbDFShowDMS
            //
            cbDFShowDMS.AutoSize = true;
            cbDFShowDMS.Checked = true;
            cbDFShowDMS.CheckState = CheckState.Checked;
            cbDFShowDMS.Location = new Point(624, 18);
            cbDFShowDMS.Name = "cbDFShowDMS";
            cbDFShowDMS.Size = new Size(50, 19);
            cbDFShowDMS.TabIndex = 2;
            cbDFShowDMS.Text = "DMS";
            cbDFShowDMS.UseVisualStyleBackColor = true;
            cbDFShowDMS.CheckedChanged += cbDFShowOutput_CheckedChanged;
            //
            // tmrDFAzimuthPreview
            //
            tmrDFAzimuthPreview.Tick += tmrDFAzimuthPreview_Tick;
            //
            // CalculatorForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 611);
            Controls.Add(lblGithub);
            Controls.Add(lblClipboard);
            Controls.Add(txtClipboard);
            Controls.Add(mainTab);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "CalculatorForm";
            Text = "Geo Calculator";
            grpConvertGeocentricInputs.ResumeLayout(false);
            grpConvertGeocentricInputs.PerformLayout();
            grpConvertOutputs.ResumeLayout(false);
            grpConvertOutputs.PerformLayout();
            grpConvertLatLngInputs.ResumeLayout(false);
            grpConvertLatLngInputs.PerformLayout();
            grpAddConditions.ResumeLayout(false);
            grpAddConditions.PerformLayout();
            mainTab.ResumeLayout(false);
            tabConverter.ResumeLayout(false);
            grpConvertDMSInputs.ResumeLayout(false);
            grpConvertDMSInputs.PerformLayout();
            tabDistance.ResumeLayout(false);
            grpDFReplaceText.ResumeLayout(false);
            grpDFReplaceText.PerformLayout();
            grpDFOutput.ResumeLayout(false);
            grpDFConditions.ResumeLayout(false);
            grpDFConditions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tipIconDFAzimuth).EndInit();
            grpDFInputs.ResumeLayout(false);
            grpDFInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tipIconDFReplaceOutput).EndInit();
            grpDFAzimuth.ResumeLayout(false);
            grpDFAzimuth.PerformLayout();
            pnlDFAzimuthDirection.ResumeLayout(false);
            pnlDFAzimuthDirection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picDFAzimuthStatus).EndInit();
            grpDFAzimuthPreview.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblConvertX;
        private TextBox txtConvertX;
        private Label lblConvertY;
        private TextBox txtConvertY;
        private Label lblConvertZ;
        private TextBox txtConvertZ;
        private TextBox txtAddAltitude;
        private Label lblAddAltitude;
        private Label lblConvertXOutput;
        private Label lblConvertZOutput;
        private TextBox txtConvertZOutput;
        private TextBox txtConvertXOutput;
        private TextBox txtConvertYOutput;
        private Label lblConvertYOutput;
        private Button btnConvertGeocentric;
        private GroupBox grpConvertGeocentricInputs;
        private GroupBox grpConvertOutputs;
        private GroupBox grpConvertLatLngInputs;
        private Label lblConvertLng;
        private TextBox txtConvertLat;
        private Label lblConvertLat;
        private Label lblConvertAlt;
        private TextBox txtConvertLng;
        private TextBox txtConvertAlt;
        private Button btnConvertLatLng;
        private TextBox txtConvertAltOutput;
        private TextBox txtConvertLngOutput;
        private TextBox txtConvertLatOutput;
        private Label lblConvertLatOutput;
        private Label lblConvertAltOutput;
        private Label lblConvertLngOutput;
        private GroupBox grpAddConditions;
        private TabControl mainTab;
        private TabPage tabConverter;
        private GroupBox grpConvertDMSInputs;
        private TextBox txtConvertDMSLatDeg;
        private Label lblConvertDMSLatDeg;
        private TextBox txtConvertDMSLatMin;
        private Label lblConvertDMSLatMin;
        private Label lblConvertDMSLatSec;
        private TextBox txtConvertDMSLatSec;
        private TextBox txtConvertDMSLngDeg;
        private Label lblConvertDMSLngDeg;
        private TextBox txtConvertDMSLngMin;
        private Label lblConvertDMSLngMin;
        private TextBox txtConvertDMSLngSec;
        private Label lblConvertDMSLngSec;
        private TextBox txtConvertDMSLatOutput;
        private TextBox txtConvertDMSLngOutput;
        private ComboBox cmbConvertDMSLatHemisphere;
        private ComboBox cmbConvertDMSLngHemisphere;
        private Label lblConvertDMSLat;
        private Label lblConvertDMSLng;
        private Label lblConvertDMSAlt;
        private TextBox txtConvertDMSAlt;
        private Label lblAddAltitudeHint;
        private Label lblConvertDMSLatOutput;
        private Label lblConvertDMSLngOutput;
        private Label lblConvertStatus;
        private Button btnConvertDMS;
        private TabPage tabDistance;
        private GroupBox grpDFInputs;
        private TextBox txtDFLat;
        private Label lblDFLat;
        private Label lblDFAltitude;
        private Label lblDFLng;
        private TextBox txtDFAltitude;
        private TextBox txtDFLng;
        private Label lblDFDistance;
        private TextBox txtDFDistance;
        private TextBox txtDFDeg;
        private Button btnDFCalculate;
        private TextBox txtClipboard;
        private Label lblClipboard;
        private Label lblGithub;
        private Button btnClipboardCopy;
        private Button btnPasteFromClipboard;
        private GroupBox grpDFConditions;
        private Label lblDFElevation;
        private TextBox txtDFElevation;
        private RichTextBox txtDFRichResult;
        private GroupBox grpDFOutput;
        private GroupBox grpDFReplaceText;
        private RichTextBox txtDFReplaceText;
        private CheckBox cbDFReplaceText;
        private TextBox txtDFReplaceRunningNo;
        private Label lblDFReplaceRunningNo;
        private ToolTip tipDFReplaceOutput;
        private ToolTip tipDFAzimuth;
        private ToolTip tipInputs;
        private PictureBox tipIconDFAzimuth;
        private PictureBox tipIconDFReplaceOutput;
        private GroupBox grpDFAzimuth;
        private RadioButton rbDFAzimuthList;
        private Label lblDFAzimuthListHint;
        private Label lblDFAzimuthDivider;
        private RadioButton rbDFAzimuthRange;
        private TextBox txtDFAzimuthStart;
        private Label lblDFAzimuthTo;
        private TextBox txtDFAzimuthEnd;
        private Label lblDFAzimuthEvery;
        private TextBox txtDFAzimuthStep;
        private Label lblDFAzimuthDirection;
        private Panel pnlDFAzimuthDirection;
        private RadioButton rbDFAzimuthClockwise;
        private RadioButton rbDFAzimuthAnticlockwise;
        private Label lblDFAzimuthRangeHint;
        private Button btnDFAzimuthCopyToList;
        private PictureBox picDFAzimuthStatus;
        private Label lblDFAzimuthStatus;
        private GroupBox grpDFAzimuthPreview;
        private Codes.Controls.AzimuthCompassPanel pnlDFAzimuthCompass;
        private Label lblDFAzimuthCount;
        private Label lblDFAzimuthSpan;
        private ListView lvDFAzimuths;
        private ColumnHeader colDFAzimuthNo;
        private ColumnHeader colDFAzimuthValue;
        private ColumnHeader colDFAzimuthLat;
        private ColumnHeader colDFAzimuthLng;
        private Label lblDFOutputStatus;
        private System.Windows.Forms.Timer tmrDFAzimuthPreview;
        private Label lblDFOutputShow;
        private CheckBox cbDFShowGeocentric;
        private CheckBox cbDFShowGeodetic;
        private CheckBox cbDFShowDMS;
    }
}