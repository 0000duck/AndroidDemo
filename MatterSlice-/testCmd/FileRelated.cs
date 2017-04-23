using System;
using System.IO;
using System.Windows.Forms;

namespace myconn
{
    public class FileRelated
    {
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
            }
        }

        private void OpenFilePath(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
            }
        }


        public void SaveFile(string data)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.InitialDirectory = "d:\\";
                save.Filter = "文本文件*.txt|*.txt";
                save.FilterIndex = 1;
                save.RestoreDirectory = true;
                if (save.ShowDialog() == DialogResult.OK)
                {
                    string path = save.FileName;
                    save.DefaultExt = ".txt";
                    StreamWriter sw = new StreamWriter(path);
                    sw.Write(data);
                    sw.Close();
                }
            }
            catch(Exception)
            {
                MessageBox.Show("保存文件失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        public void CopyFile(string srcFile, string dstFile)
        {
            if (File.Exists(dstFile) == true)
            {
                MessageBox.Show("目标文件夹已有此文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {                
                File.Copy(srcFile, dstFile);
                MessageBox.Show("复制成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }


        public void MoveFile(string srcFile, string dstFile)
        {
            if (File.Exists(dstFile) == true)
            {
                MessageBox.Show("目标文件夹已有此文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                File.Move(srcFile, dstFile);
                MessageBox.Show("移动成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }


        public void DeleteFile(string srcFile)
        {
            if (Directory.Exists(srcFile) == false)
            {
                MessageBox.Show("目标文件夹不存在此文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                try
                {
                    Directory.Delete(srcFile);
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch(Exception)
                {
                    MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }                
            }
        }

    }
}
