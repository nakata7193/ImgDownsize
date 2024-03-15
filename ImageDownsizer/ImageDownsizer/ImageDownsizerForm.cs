using System.Diagnostics;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace imgDownsizer
{
	public partial class ImgDownsizerForm : Form	
	{

		private Bitmap uploadedImg;
		private ImageResultForm imgResultForm;
        private Bitmap currentImg;

        public ImgDownsizerForm()
		{
			InitializeComponent();
		}

		public void imgSelect()
		{
			OpenFileDialog selectImg = new OpenFileDialog();
			selectImg.Filter = "Img Files|*.jpg;*.jpeg;*.png;*.tiff";
			if (selectImg.ShowDialog() == DialogResult.OK)
			{
				if (imgResultForm == null || imgResultForm.IsDisposed)
				{
					imgResultForm = new ImageResultForm();
					imgResultForm.Show();
				}

				uploadedImg = new Bitmap(selectImg.FileName);
				currentImg = new Bitmap(uploadedImg);
				imgResultForm.imageResultBox.Image = currentImg;
			}
		}

		public void ImgDownsize()
		{
			int scale = Convert.ToInt32(downsizeScaleValue.Value);

			Thread imgDownsizeThread = new Thread(delegate ()
			{
				if (currentImg != null && scale >= 1 && scale < 100)
				{
					int newWidth = (int)(uploadedImg.Width * (100 - scale) / 100.0);
					int newHeight = (int)(uploadedImg.Height * (100 - scale) / 100.0);

					if (newWidth > 0 && newHeight > 0)
					{
                        currentImg = new Bitmap(uploadedImg, newWidth, newHeight);
						imgResultForm.imageResultBox.Image = currentImg;
					}
				}
			});
			imgDownsizeThread.Start();
		}

		private void ImageDownsizerForm_Load(object sender, EventArgs e)
		{

		}

		private void browseImageBtn_Click(object sender, EventArgs e)
		{
			imgSelect();
		}

		private void downsizeImageBtn_Click(object sender, EventArgs e)
		{
			ImgDownsize();
		}

		private void downsizeScaleValue_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				ImgDownsize();
				e.SuppressKeyPress = true;
			}
		}
	}
}