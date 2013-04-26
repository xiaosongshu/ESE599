using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.IO;

namespace howto_ftp_upload_text
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Upload the text.
        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                lblStatus.Text = "Working...";
                Application.DoEvents();

                FtpUploadString(
                    DateTime.Now.ToString() + ": " + txtText.Text, 
                    txtUri.Text, txtUsername.Text, txtPassword.Text);

                lblStatus.Text = "Done";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // Use FTP to upload a string into a file.
        private void FtpUploadString(string text, string to_uri, string user_name, string password)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(to_uri);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Get network credentials.
            request.Credentials = new NetworkCredential(user_name, password);

            // Write the text's bytes into the request stream.
            request.ContentLength = text.Length;
            using (Stream request_stream = request.GetRequestStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                request_stream.Write(bytes, 0, text.Length);
                request_stream.Close();
            }
        }
    }
}
