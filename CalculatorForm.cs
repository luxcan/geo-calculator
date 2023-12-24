using GeographicLib;

namespace GeoCalculator {
    public partial class CalculatorForm : Form {
        public CalculatorForm() {
            InitializeComponent();
        }

        private void btnConvertGencentric_Click(object sender, EventArgs e) {
            _ = double.TryParse(txtXParam.Text, out double x);
            _ = double.TryParse(txtYParam.Text, out double y);
            _ = double.TryParse(txtZParam.Text, out double z);
            _ = double.TryParse(txtAddAltitude.Text, out double addAltitude);

            Geocentric earth = new Geocentric(Constants.WGS84_a, Constants.WGS84_f);
            double lat, lng, altitude;
            (lat, lng, altitude) = earth.Reverse(x, y, z);

            double newAltitude = altitude + addAltitude;
            double newX, newY, newZ;
            (newX, newY, newZ) = earth.Forward(lat, lng, newAltitude);

            txtXOutput.Text = newX.ToString();
            txtYOutput.Text = newY.ToString();
            txtZOutput.Text = newZ.ToString();

            txtLatOutput.Text = lat.ToString();
            txtLngOutput.Text = lng.ToString();
            txtAltOutput.Text = newAltitude.ToString();
        }

        private void btnConvertLatLng_Click(object sender, EventArgs e) {
            _ = double.TryParse(txtLatParam.Text, out double lat);
            _ = double.TryParse(txtLngParam.Text, out double lng);
            _ = double.TryParse(txtAltParam.Text, out double altitude);
            _ = double.TryParse(txtAddAltitude.Text, out double addAltitude);
            altitude += addAltitude;

            Geocentric earth = new Geocentric(Constants.WGS84_a, Constants.WGS84_f);
            var result = earth.Forward(lat, lng, altitude);

            txtXOutput.Text = result.X.ToString();
            txtYOutput.Text = result.Y.ToString();
            txtZOutput.Text = result.Z.ToString();

            txtLatOutput.Text = lat.ToString();
            txtLngOutput.Text = lng.ToString();
            txtAltOutput.Text = altitude.ToString();
        }
    }
}