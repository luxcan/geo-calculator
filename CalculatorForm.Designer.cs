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
            lblXParam = new Label();
            txtXParam = new TextBox();
            lblYParam = new Label();
            txtYParam = new TextBox();
            lblZParam = new Label();
            txtZParam = new TextBox();
            lblAddAltitude = new Label();
            txtAddAltitude = new TextBox();
            lblXOutput = new Label();
            lblZOutput = new Label();
            txtZOutput = new TextBox();
            txtXOutput = new TextBox();
            txtYOutput = new TextBox();
            lblYOutput = new Label();
            btnConvertGeocentric = new Button();
            groupInputs = new GroupBox();
            groupOutputs = new GroupBox();
            grpLatLngInputs = new GroupBox();
            txtAltParam = new TextBox();
            lblAltParam = new Label();
            txtLngParam = new TextBox();
            lblLngParam = new Label();
            txtLatParam = new TextBox();
            lblLatParam = new Label();
            btnConvertLatLng = new Button();
            txtLatOutput = new TextBox();
            txtLngOutput = new TextBox();
            txtAltOutput = new TextBox();
            lblLatOutput = new Label();
            lblLngOutput = new Label();
            lblAltOutput = new Label();
            grpAddConditions = new GroupBox();
            groupInputs.SuspendLayout();
            groupOutputs.SuspendLayout();
            grpLatLngInputs.SuspendLayout();
            grpAddConditions.SuspendLayout();
            SuspendLayout();
            // 
            // lblXParam
            // 
            lblXParam.AutoSize = true;
            lblXParam.Location = new Point(49, 22);
            lblXParam.Name = "lblXParam";
            lblXParam.Size = new Size(17, 15);
            lblXParam.TabIndex = 0;
            lblXParam.Text = "X:";
            // 
            // txtXParam
            // 
            txtXParam.Location = new Point(72, 19);
            txtXParam.Name = "txtXParam";
            txtXParam.Size = new Size(100, 23);
            txtXParam.TabIndex = 1;
            // 
            // lblYParam
            // 
            lblYParam.AutoSize = true;
            lblYParam.Location = new Point(49, 51);
            lblYParam.Name = "lblYParam";
            lblYParam.Size = new Size(17, 15);
            lblYParam.TabIndex = 2;
            lblYParam.Text = "Y:";
            // 
            // txtYParam
            // 
            txtYParam.Location = new Point(72, 48);
            txtYParam.Name = "txtYParam";
            txtYParam.Size = new Size(100, 23);
            txtYParam.TabIndex = 3;
            // 
            // lblZParam
            // 
            lblZParam.AutoSize = true;
            lblZParam.Location = new Point(49, 80);
            lblZParam.Name = "lblZParam";
            lblZParam.Size = new Size(17, 15);
            lblZParam.TabIndex = 4;
            lblZParam.Text = "Z:";
            // 
            // txtZParam
            // 
            txtZParam.Location = new Point(72, 77);
            txtZParam.Name = "txtZParam";
            txtZParam.Size = new Size(100, 23);
            txtZParam.TabIndex = 5;
            // 
            // lblAddAltitude
            // 
            lblAddAltitude.AutoSize = true;
            lblAddAltitude.Location = new Point(14, 22);
            lblAddAltitude.Name = "lblAddAltitude";
            lblAddAltitude.Size = new Size(77, 15);
            lblAddAltitude.TabIndex = 9;
            lblAddAltitude.Text = "Add Altitude:";
            // 
            // txtAddAltitude
            // 
            txtAddAltitude.Location = new Point(97, 19);
            txtAddAltitude.Name = "txtAddAltitude";
            txtAddAltitude.Size = new Size(100, 23);
            txtAddAltitude.TabIndex = 8;
            // 
            // lblXOutput
            // 
            lblXOutput.AutoSize = true;
            lblXOutput.Location = new Point(45, 22);
            lblXOutput.Name = "lblXOutput";
            lblXOutput.Size = new Size(17, 15);
            lblXOutput.TabIndex = 0;
            lblXOutput.Text = "X:";
            // 
            // lblZOutput
            // 
            lblZOutput.AutoSize = true;
            lblZOutput.Location = new Point(45, 80);
            lblZOutput.Name = "lblZOutput";
            lblZOutput.Size = new Size(17, 15);
            lblZOutput.TabIndex = 4;
            lblZOutput.Text = "Z:";
            // 
            // txtZOutput
            // 
            txtZOutput.Location = new Point(68, 77);
            txtZOutput.Name = "txtZOutput";
            txtZOutput.Size = new Size(100, 23);
            txtZOutput.TabIndex = 5;
            // 
            // txtXOutput
            // 
            txtXOutput.Location = new Point(68, 19);
            txtXOutput.Name = "txtXOutput";
            txtXOutput.Size = new Size(100, 23);
            txtXOutput.TabIndex = 1;
            // 
            // txtYOutput
            // 
            txtYOutput.Location = new Point(68, 48);
            txtYOutput.Name = "txtYOutput";
            txtYOutput.Size = new Size(100, 23);
            txtYOutput.TabIndex = 3;
            // 
            // lblYOutput
            // 
            lblYOutput.AutoSize = true;
            lblYOutput.Location = new Point(45, 51);
            lblYOutput.Name = "lblYOutput";
            lblYOutput.Size = new Size(17, 15);
            lblYOutput.TabIndex = 2;
            lblYOutput.Text = "Y:";
            // 
            // btnConvertGeocentric
            // 
            btnConvertGeocentric.Location = new Point(214, 53);
            btnConvertGeocentric.Name = "btnConvertGeocentric";
            btnConvertGeocentric.Size = new Size(64, 25);
            btnConvertGeocentric.TabIndex = 9;
            btnConvertGeocentric.Text = "Convert";
            btnConvertGeocentric.UseVisualStyleBackColor = true;
            btnConvertGeocentric.Click += btnConvertGencentric_Click;
            // 
            // groupInputs
            // 
            groupInputs.Controls.Add(txtXParam);
            groupInputs.Controls.Add(lblXParam);
            groupInputs.Controls.Add(lblZParam);
            groupInputs.Controls.Add(lblYParam);
            groupInputs.Controls.Add(txtZParam);
            groupInputs.Controls.Add(txtYParam);
            groupInputs.Location = new Point(13, 12);
            groupInputs.Name = "groupInputs";
            groupInputs.Size = new Size(185, 114);
            groupInputs.TabIndex = 10;
            groupInputs.TabStop = false;
            groupInputs.Text = "Geocentric";
            // 
            // groupOutputs
            // 
            groupOutputs.Controls.Add(lblAltOutput);
            groupOutputs.Controls.Add(lblLngOutput);
            groupOutputs.Controls.Add(lblLatOutput);
            groupOutputs.Controls.Add(txtAltOutput);
            groupOutputs.Controls.Add(txtLngOutput);
            groupOutputs.Controls.Add(txtLatOutput);
            groupOutputs.Controls.Add(lblZOutput);
            groupOutputs.Controls.Add(lblXOutput);
            groupOutputs.Controls.Add(txtZOutput);
            groupOutputs.Controls.Add(txtXOutput);
            groupOutputs.Controls.Add(txtYOutput);
            groupOutputs.Controls.Add(lblYOutput);
            groupOutputs.Location = new Point(295, 12);
            groupOutputs.Name = "groupOutputs";
            groupOutputs.Size = new Size(183, 236);
            groupOutputs.TabIndex = 11;
            groupOutputs.TabStop = false;
            groupOutputs.Text = "Outputs";
            // 
            // grpLatLngInputs
            // 
            grpLatLngInputs.Controls.Add(txtAltParam);
            grpLatLngInputs.Controls.Add(lblAltParam);
            grpLatLngInputs.Controls.Add(txtLngParam);
            grpLatLngInputs.Controls.Add(lblLngParam);
            grpLatLngInputs.Controls.Add(txtLatParam);
            grpLatLngInputs.Controls.Add(lblLatParam);
            grpLatLngInputs.Location = new Point(13, 132);
            grpLatLngInputs.Name = "grpLatLngInputs";
            grpLatLngInputs.Size = new Size(185, 116);
            grpLatLngInputs.TabIndex = 14;
            grpLatLngInputs.TabStop = false;
            grpLatLngInputs.Text = "Lat/Lng";
            // 
            // txtAltParam
            // 
            txtAltParam.Location = new Point(72, 80);
            txtAltParam.Name = "txtAltParam";
            txtAltParam.Size = new Size(100, 23);
            txtAltParam.TabIndex = 18;
            // 
            // lblAltParam
            // 
            lblAltParam.AutoSize = true;
            lblAltParam.Location = new Point(14, 83);
            lblAltParam.Name = "lblAltParam";
            lblAltParam.Size = new Size(52, 15);
            lblAltParam.TabIndex = 17;
            lblAltParam.Text = "Altitude:";
            // 
            // txtLngParam
            // 
            txtLngParam.Location = new Point(72, 51);
            txtLngParam.Name = "txtLngParam";
            txtLngParam.Size = new Size(100, 23);
            txtLngParam.TabIndex = 16;
            // 
            // lblLngParam
            // 
            lblLngParam.AutoSize = true;
            lblLngParam.Location = new Point(36, 54);
            lblLngParam.Name = "lblLngParam";
            lblLngParam.Size = new Size(30, 15);
            lblLngParam.TabIndex = 6;
            lblLngParam.Text = "Lng:";
            // 
            // txtLatParam
            // 
            txtLatParam.Location = new Point(72, 22);
            txtLatParam.Name = "txtLatParam";
            txtLatParam.Size = new Size(100, 23);
            txtLatParam.TabIndex = 15;
            // 
            // lblLatParam
            // 
            lblLatParam.AutoSize = true;
            lblLatParam.Location = new Point(40, 25);
            lblLatParam.Name = "lblLatParam";
            lblLatParam.Size = new Size(26, 15);
            lblLatParam.TabIndex = 15;
            lblLatParam.Text = "Lat:";
            // 
            // btnConvertLatLng
            // 
            btnConvertLatLng.Location = new Point(214, 176);
            btnConvertLatLng.Name = "btnConvertLatLng";
            btnConvertLatLng.Size = new Size(64, 25);
            btnConvertLatLng.TabIndex = 15;
            btnConvertLatLng.Text = "Convert";
            btnConvertLatLng.UseVisualStyleBackColor = true;
            btnConvertLatLng.Click += btnConvertLatLng_Click;
            // 
            // txtLatOutput
            // 
            txtLatOutput.Location = new Point(68, 142);
            txtLatOutput.Name = "txtLatOutput";
            txtLatOutput.Size = new Size(100, 23);
            txtLatOutput.TabIndex = 16;
            // 
            // txtLngOutput
            // 
            txtLngOutput.Location = new Point(68, 171);
            txtLngOutput.Name = "txtLngOutput";
            txtLngOutput.Size = new Size(100, 23);
            txtLngOutput.TabIndex = 17;
            // 
            // txtAltOutput
            // 
            txtAltOutput.Location = new Point(68, 200);
            txtAltOutput.Name = "txtAltOutput";
            txtAltOutput.Size = new Size(100, 23);
            txtAltOutput.TabIndex = 19;
            // 
            // lblLatOutput
            // 
            lblLatOutput.AutoSize = true;
            lblLatOutput.Location = new Point(36, 145);
            lblLatOutput.Name = "lblLatOutput";
            lblLatOutput.Size = new Size(26, 15);
            lblLatOutput.TabIndex = 20;
            lblLatOutput.Text = "Lat:";
            // 
            // lblLngOutput
            // 
            lblLngOutput.AutoSize = true;
            lblLngOutput.Location = new Point(32, 174);
            lblLngOutput.Name = "lblLngOutput";
            lblLngOutput.Size = new Size(30, 15);
            lblLngOutput.TabIndex = 21;
            lblLngOutput.Text = "Lng:";
            // 
            // lblAltOutput
            // 
            lblAltOutput.AutoSize = true;
            lblAltOutput.Location = new Point(10, 203);
            lblAltOutput.Name = "lblAltOutput";
            lblAltOutput.Size = new Size(52, 15);
            lblAltOutput.TabIndex = 22;
            lblAltOutput.Text = "Altitude:";
            // 
            // grpAddConditions
            // 
            grpAddConditions.Controls.Add(lblAddAltitude);
            grpAddConditions.Controls.Add(txtAddAltitude);
            grpAddConditions.Location = new Point(13, 264);
            grpAddConditions.Name = "grpAddConditions";
            grpAddConditions.Size = new Size(466, 54);
            grpAddConditions.TabIndex = 16;
            grpAddConditions.TabStop = false;
            grpAddConditions.Text = "Additional Conditions";
            // 
            // CalculatorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(491, 335);
            Controls.Add(grpAddConditions);
            Controls.Add(btnConvertLatLng);
            Controls.Add(grpLatLngInputs);
            Controls.Add(groupOutputs);
            Controls.Add(groupInputs);
            Controls.Add(btnConvertGeocentric);
            Name = "CalculatorForm";
            Text = "Geo Calculator";
            groupInputs.ResumeLayout(false);
            groupInputs.PerformLayout();
            groupOutputs.ResumeLayout(false);
            groupOutputs.PerformLayout();
            grpLatLngInputs.ResumeLayout(false);
            grpLatLngInputs.PerformLayout();
            grpAddConditions.ResumeLayout(false);
            grpAddConditions.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblXParam;
        private TextBox txtXParam;
        private Label lblYParam;
        private TextBox txtYParam;
        private Label lblZParam;
        private TextBox txtZParam;
        private TextBox txtAddAltitude;
        private Label lblAddAltitude;
        private Label lblXOutput;
        private Label lblZOutput;
        private TextBox txtZOutput;
        private TextBox txtXOutput;
        private TextBox txtYOutput;
        private Label lblYOutput;
        private Button btnConvertGeocentric;
        private GroupBox groupInputs;
        private GroupBox groupOutputs;
        private GroupBox grpLatLngInputs;
        private Label lblLngParam;
        private TextBox txtLatParam;
        private Label lblLatParam;
        private Label lblAltParam;
        private TextBox txtLngParam;
        private TextBox txtAltParam;
        private Button btnConvertLatLng;
        private TextBox txtAltOutput;
        private TextBox txtLngOutput;
        private TextBox txtLatOutput;
        private Label lblLatOutput;
        private Label lblAltOutput;
        private Label lblLngOutput;
        private GroupBox grpAddConditions;
    }
}