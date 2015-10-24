using Arua_Camera_Editor.Common;
using Arua_Camera_Editor.Common.FileHandler;
using Arua_Camera_Editor.Common.GraphicsHandler;
using Arua_Camera_Editor.Properties;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Arua_Camera_Editor
{
	public class MainForm : Form
	{
		public class MapInfo
		{
			public string name;

			public int id;

			public MapInfo(string name, int id)
			{
				this.name = name;
				this.id = id;
			}

			public override string ToString()
			{
				return this.id + ") " + this.name;
			}
		}

		private CameraMotion camMotion;

		private Map map;

		private CameraMotion.Frame copiedFrame;

		private static Matrix roseCoordinate = Matrix.CreateRotationX(1.57079637f) * Matrix.CreateRotationZ(3.14159274f);

		private static Vector3 realPosition = new Vector3(5200f, 5200f, 0f);

		private IContainer components = null;

		private MenuStrip menuStrip;

		private ToolStripMenuItem fileToolStripMenuItem;

		private ToolStripMenuItem openFolderToolStripMenuItem;

		private ToolStripMenuItem exitToolStripMenuItem;

		private ToolStripMenuItem helpToolStripMenuItem;

		private ToolStripMenuItem aboutToolStripMenuItem;

		private StatusStrip statusStrip;

		private ToolStripStatusLabel toolStripStatusLabel;

		private GroupBox groupBoxMotion;

		private previewControl panelPreview;

		private TreeView treeViewFrame;

		private Button buttonBack;

		private Button buttonCapture;

		private Button buttonPlay;

		private Button buttonPause;

		private Button buttonSaveZMO;

		private Button buttonOpenZMO;

		private Label labelMotionName;

		private Label labelFPS;

		private NumericUpDown numericUpDownFPS;

		private Label labelMap;

		private ToolStripProgressBar toolStripProgressBar;

		private ListBox listBoxMap;

		private PropertyGrid propertyGrid;

		private Button walkButton;

		private ContextMenuStrip frameContextMenuStrip;

		private ToolStripMenuItem addMenuItem;

		private ToolStripMenuItem removeMenuItem;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem copyMenuItem;

		private ToolStripMenuItem pasteMenuItem;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripMenuItem setCurrentPositionMenuItem;

		private ToolStripMenuItem interpolationMenuItem;

		private ToolStripMenuItem toolStripMenuItem2;

		private Button buttonNewZMO;

		public MainForm()
		{
			this.InitializeComponent();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.Description = "Select your client path. Like \"C:\\Games\\Rose\\\"";
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				ContentManager.SetRootPath(folderBrowserDialog.SelectedPath + "\\");
				this.LoadMapsName();
			}
		}

		private void LoadMapsName()
		{
			STB sTB = ContentManager.Instance().GetSTB("3DDATA\\STB\\LIST_ZONE.STB");
			STL sTL = ContentManager.Instance().GetSTL("3DDATA\\STB\\LIST_ZONE_S.STL");
			this.UpdateStatus("Loading Maps Name", -1);
			this.SetProgressCount(sTL.entryCount);
			for (int i = 0; i < sTL.entryCount; i++)
			{
				MainForm.MapInfo item = new MainForm.MapInfo(sTL.entry[i].text[1], (int)sTL.entry[i].ID);
				this.listBoxMap.Items.Add(item);
				this.PerformStepProgress();
			}
			this.ResetProgress();
		}

		public void LoadMap(int index)
		{
			try
			{
				this.map = new Map(this.panelPreview.GraphicsDevice);
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				this.map.Load(index);
				this.panelPreview.DrawMap(this.map);
				this.panelPreview.StartDraw();
				stopwatch.Stop();
				this.UpdateStatus("Loaded in " + stopwatch.ElapsedMilliseconds, -1);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error while opening map : " + ex.Message);
			}
		}

		public void UpdateStatus(string newStatus, int progress = -1)
		{
			this.toolStripStatusLabel.Text = "Status : " + newStatus;
			if (progress != -1)
			{
				this.toolStripProgressBar.ProgressBar.Step = progress;
			}
		}

		public void PerformStepProgress()
		{
			this.toolStripProgressBar.PerformStep();
		}

		public void SetProgressCount(int count)
		{
			this.toolStripProgressBar.Maximum = count;
		}

		public void ResetProgress()
		{
			this.toolStripProgressBar.ProgressBar.Step = 0;
		}

		private void panelPreview_Resize(object sender, EventArgs e)
		{
			this.panelPreview.previewResize();
		}

		private void buttonPlay_Click(object sender, EventArgs e)
		{
			this.panelPreview.StartMotion();
		}

		private void buttonOpenZMO_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "ZMO Files (*.ZMO)|*.ZMO";
			openFileDialog.InitialDirectory = ContentManager.GetRootPath();
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.camMotion = new CameraMotion(this.panelPreview.GraphicsDevice);
				this.camMotion.Load(openFileDialog.FileName, ClientType.IROSE);
				this.labelMotionName.Text = "Motion name : " + openFileDialog.SafeFileName;
				this.numericUpDownFPS.Value = this.camMotion.GetFPS();
				this.panelPreview.SetCameraMotion(this.camMotion);
				this.DisplayFrames();
				this.frameContextMenuStrip.Enabled = true;
				this.addMenuItem.Enabled = true;
				this.removeMenuItem.Enabled = true;
				this.setCurrentPositionMenuItem.Enabled = true;
				this.copyMenuItem.Enabled = true;
				this.pasteMenuItem.Enabled = true;
				this.interpolationMenuItem.Enabled = true;
			}
		}

		public void DisplayFrames()
		{
			this.treeViewFrame.Nodes.Clear();
			IEnumerable<CameraMotion.Frame> frames = this.camMotion.GetFrames();
			int num = 0;
			foreach (CameraMotion.Frame current in frames)
			{
				TreeNode node = new TreeNode(string.Format("Frame [{0}] : P[{1}] A[{2}] U[{3}]", new object[]
				{
					num,
					current.cameraPosition,
					current.LookAt,
					current.Up
				}));
				this.treeViewFrame.Nodes.Add(node);
				num++;
			}
		}

		private void listBoxMap_SelectedIndexChanged(object sender, EventArgs e)
		{
			MainForm.MapInfo mapInfo = (MainForm.MapInfo)this.listBoxMap.SelectedItem;
			this.LoadMap(mapInfo.id);
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Credits to Guegant Jean @Jiwan.\r\nThanks to exjam,brett,xadets and any other guys that reversed ROSE's files formats.\r\nI would like also to thanks exjam for his open source client and xadet with his map editor, that helped me to understand the HIM format");
		}

		private void buttonPause_Click(object sender, EventArgs e)
		{
			this.panelPreview.StopMotion();
		}

		private void buttonBack_Click(object sender, EventArgs e)
		{
			this.panelPreview.ResetMotion();
		}

		private void treeViewFrame_AfterSelect(object sender, TreeViewEventArgs e)
		{
			int level = this.treeViewFrame.SelectedNode.Level;
			if (level == 0)
			{
				this.panelPreview.SetMotionFrameIndex(this.treeViewFrame.SelectedNode.Index);
				this.propertyGrid.SelectedObject = this.panelPreview.GetCurrentMotionFrame();
			}
		}

		private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			int index = this.treeViewFrame.SelectedNode.Index;
			List<CameraMotion.Frame> frames = this.camMotion.GetFrames();
			CameraMotion.Frame frame = frames.ElementAt(index);
			this.treeViewFrame.SelectedNode.Text = string.Format("Frame [{0}] : P[{1}] A[{2}] U[{3}]", new object[]
			{
				index,
				frame.cameraPosition,
				frame.LookAt,
				frame.Up
			});
			this.panelPreview.UpdateCameraMatrices();
			this.camMotion.GenerateVertices();
		}

		private void walkButton_Click(object sender, EventArgs e)
		{
			this.panelPreview.StopMotion();
			this.panelPreview.FreeRoam();
		}

		private void setCurrentPositionMenuItem_Click(object sender, EventArgs e)
		{
			if (this.treeViewFrame.SelectedNode != null)
			{
				int index = this.treeViewFrame.SelectedNode.Index;
				CameraMotion.Frame motionFrame = this.panelPreview.GetMotionFrame(index);
				Vector3 freeRoamPosition = this.panelPreview.GetFreeRoamPosition();
				Matrix freeRoamMatrix = this.panelPreview.GetFreeRoamMatrix();
				Vector3 lookAt = freeRoamPosition - new Vector3(freeRoamMatrix.M13, freeRoamMatrix.M23, freeRoamMatrix.M33);
				Vector3 up = new Vector3(freeRoamMatrix.M12, freeRoamMatrix.M22, freeRoamMatrix.M32);
				up.Normalize();
				motionFrame.cameraPosition = freeRoamPosition;
				motionFrame.LookAt = lookAt;
				motionFrame.Up = up;
				this.treeViewFrame.SelectedNode.Text = string.Format("Frame [{0}] : P[{1}] A[{2}] U[{3}]", new object[]
				{
					index,
					motionFrame.cameraPosition,
					motionFrame.LookAt,
					motionFrame.Up
				});
				this.panelPreview.UpdateCameraMatrices();
				this.camMotion.GenerateVertices();
			}
		}

		private void buttonSaveZMO_Click(object sender, EventArgs e)
		{
			this.panelPreview.SaveMotion();
			this.toolStripStatusLabel.Text = "Status :" + this.camMotion.motionFile.Path + "saved";
		}

		private void addMenuItem_Click(object sender, EventArgs e)
		{
			if (this.treeViewFrame.SelectedNode != null)
			{
				AddFrameMessageBox addFrameMessageBox = new AddFrameMessageBox();
				if (addFrameMessageBox.ShowDialog() == DialogResult.OK)
				{
					int framePosition = addFrameMessageBox.GetFramePosition();
					int index = this.treeViewFrame.SelectedNode.Index;
					CameraMotion.Frame motionFrame = this.panelPreview.GetMotionFrame(index);
					CameraMotion.Frame item = new CameraMotion.Frame();
					item = motionFrame;
					List<CameraMotion.Frame> frames = this.camMotion.GetFrames();
					frames.Insert(index + framePosition, item);
					this.panelPreview.UpdateCameraMatrices();
					this.camMotion.GenerateVertices();
					this.DisplayFrames();
				}
			}
		}

		private void removeMenuItem_Click(object sender, EventArgs e)
		{
			if (this.treeViewFrame.SelectedNode != null)
			{
				int index = this.treeViewFrame.SelectedNode.Index;
				List<CameraMotion.Frame> frames = this.camMotion.GetFrames();
				frames.RemoveAt(index);
				this.panelPreview.UpdateCameraMatrices();
				this.camMotion.GenerateVertices();
				this.DisplayFrames();
			}
		}

		private void copyMenuItem_Click(object sender, EventArgs e)
		{
			if (this.treeViewFrame.SelectedNode != null)
			{
				int index = this.treeViewFrame.SelectedNode.Index;
				this.copiedFrame = this.panelPreview.GetMotionFrame(index);
			}
		}

		private void pasteMenuItem_Click(object sender, EventArgs e)
		{
			if (this.copiedFrame != null)
			{
				if (this.treeViewFrame.SelectedNode != null)
				{
					int index = this.treeViewFrame.SelectedNode.Index;
					List<CameraMotion.Frame> frames = this.camMotion.GetFrames();
					frames[index] = this.copiedFrame;
					this.panelPreview.UpdateCameraMatrices();
					this.camMotion.GenerateVertices();
					this.DisplayFrames();
				}
			}
		}

		private void buttonNewZMO_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "ZMO Files (*.ZMO)|*.ZMO";
			saveFileDialog.AddExtension = true;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.camMotion = new CameraMotion(this.panelPreview.GraphicsDevice);
				List<CameraMotion.Frame> frames = this.camMotion.GetFrames();
				for (int i = 0; i < 5; i++)
				{
					frames.Add(new CameraMotion.Frame
					{
						cameraPosition = new Vector3(5200f, 5200f, 20f),
						LookAt = new Vector3(5200f, 5201f, 20f),
						Up = new Vector3(0f, 1f, 0f)
					});
				}
				this.camMotion.motionFile = new ZMO();
				this.numericUpDownFPS.Value = 30m;
				this.camMotion.motionFile.FPS = 30;
				this.camMotion.motionFile.Path = saveFileDialog.FileName;
				this.camMotion.GenerateVertices();
				this.camMotion.GenerateViewMatrices();
				this.panelPreview.SetCameraMotion(this.camMotion);
				this.DisplayFrames();
				this.labelMotionName.Text = "Motion name : " + saveFileDialog.FileName;
				this.frameContextMenuStrip.Enabled = true;
				this.addMenuItem.Enabled = true;
				this.removeMenuItem.Enabled = true;
				this.setCurrentPositionMenuItem.Enabled = true;
				this.copyMenuItem.Enabled = true;
				this.pasteMenuItem.Enabled = true;
				this.interpolationMenuItem.Enabled = true;
			}
		}

		private void interpolationMenuItem_Click(object sender, EventArgs e)
		{
			if (this.camMotion != null)
			{
				InterpolationMessageBox interpolationMessageBox = new InterpolationMessageBox(0, this.camMotion.GetFrames().Count - 1);
				if (interpolationMessageBox.ShowDialog() == DialogResult.OK)
				{
					this.camMotion.Interpolate(interpolationMessageBox.GetMinFrameIndex(), interpolationMessageBox.GetMaxFrameIndex(), interpolationMessageBox.GetSpeed());
					this.DisplayFrames();
				}
			}
		}

		private void numericUpDownFPS_ValueChanged(object sender, EventArgs e)
		{
			if (this.camMotion != null)
			{
				this.camMotion.SetFPS((int)this.numericUpDownFPS.Value);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			this.menuStrip = new MenuStrip();
			this.fileToolStripMenuItem = new ToolStripMenuItem();
			this.openFolderToolStripMenuItem = new ToolStripMenuItem();
			this.exitToolStripMenuItem = new ToolStripMenuItem();
			this.helpToolStripMenuItem = new ToolStripMenuItem();
			this.aboutToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripMenuItem2 = new ToolStripMenuItem();
			this.statusStrip = new StatusStrip();
			this.toolStripStatusLabel = new ToolStripStatusLabel();
			this.toolStripProgressBar = new ToolStripProgressBar();
			this.groupBoxMotion = new GroupBox();
			this.buttonNewZMO = new Button();
			this.walkButton = new Button();
			this.propertyGrid = new PropertyGrid();
			this.listBoxMap = new ListBox();
			this.labelMap = new Label();
			this.labelMotionName = new Label();
			this.labelFPS = new Label();
			this.numericUpDownFPS = new NumericUpDown();
			this.buttonSaveZMO = new Button();
			this.buttonOpenZMO = new Button();
			this.buttonCapture = new Button();
			this.buttonPlay = new Button();
			this.buttonPause = new Button();
			this.buttonBack = new Button();
			this.treeViewFrame = new TreeView();
			this.frameContextMenuStrip = new ContextMenuStrip(this.components);
			this.addMenuItem = new ToolStripMenuItem();
			this.removeMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.copyMenuItem = new ToolStripMenuItem();
			this.pasteMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.setCurrentPositionMenuItem = new ToolStripMenuItem();
			this.interpolationMenuItem = new ToolStripMenuItem();
			this.panelPreview = new previewControl();
			this.menuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.groupBoxMotion.SuspendLayout();
			((ISupportInitialize)this.numericUpDownFPS).BeginInit();
			this.frameContextMenuStrip.SuspendLayout();
			base.SuspendLayout();
			this.menuStrip.Items.AddRange(new ToolStripItem[]
			{
				this.fileToolStripMenuItem,
				this.helpToolStripMenuItem,
				this.toolStripMenuItem2
			});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new Size(1008, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip";
			this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.openFolderToolStripMenuItem,
				this.exitToolStripMenuItem
			});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
			this.openFolderToolStripMenuItem.Size = new Size(173, 22);
			this.openFolderToolStripMenuItem.Text = "Open Client Folder";
			this.openFolderToolStripMenuItem.Click += new EventHandler(this.openFolderToolStripMenuItem_Click);
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new Size(173, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new EventHandler(this.exitToolStripMenuItem_Click);
			this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.aboutToolStripMenuItem
			});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new Size(107, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new EventHandler(this.aboutToolStripMenuItem_Click);
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new Size(12, 20);
			this.statusStrip.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripStatusLabel,
				this.toolStripProgressBar
			});
			this.statusStrip.Location = new System.Drawing.Point(0, 708);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new Size(1008, 22);
			this.statusStrip.TabIndex = 1;
			this.statusStrip.Text = "statusStrip1";
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Size = new Size(45, 17);
			this.toolStripStatusLabel.Text = "Status :";
			this.toolStripProgressBar.Name = "toolStripProgressBar";
			this.toolStripProgressBar.Size = new Size(100, 16);
			this.groupBoxMotion.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.groupBoxMotion.Controls.Add(this.buttonNewZMO);
			this.groupBoxMotion.Controls.Add(this.walkButton);
			this.groupBoxMotion.Controls.Add(this.propertyGrid);
			this.groupBoxMotion.Controls.Add(this.listBoxMap);
			this.groupBoxMotion.Controls.Add(this.labelMap);
			this.groupBoxMotion.Controls.Add(this.labelMotionName);
			this.groupBoxMotion.Controls.Add(this.labelFPS);
			this.groupBoxMotion.Controls.Add(this.numericUpDownFPS);
			this.groupBoxMotion.Controls.Add(this.buttonSaveZMO);
			this.groupBoxMotion.Controls.Add(this.buttonOpenZMO);
			this.groupBoxMotion.Controls.Add(this.buttonCapture);
			this.groupBoxMotion.Controls.Add(this.buttonPlay);
			this.groupBoxMotion.Controls.Add(this.buttonPause);
			this.groupBoxMotion.Controls.Add(this.buttonBack);
			this.groupBoxMotion.Controls.Add(this.treeViewFrame);
			this.groupBoxMotion.Dock = DockStyle.Right;
			this.groupBoxMotion.Location = new System.Drawing.Point(708, 24);
			this.groupBoxMotion.Name = "groupBoxMotion";
			this.groupBoxMotion.Size = new Size(300, 684);
			this.groupBoxMotion.TabIndex = 2;
			this.groupBoxMotion.TabStop = false;
			this.groupBoxMotion.Text = "Motion :";
			this.buttonNewZMO.AutoSize = true;
			this.buttonNewZMO.Image = Resources.New_small;
			this.buttonNewZMO.Location = new System.Drawing.Point(198, 249);
			this.buttonNewZMO.Name = "buttonNewZMO";
			this.buttonNewZMO.Size = new Size(26, 26);
			this.buttonNewZMO.TabIndex = 14;
			this.buttonNewZMO.UseVisualStyleBackColor = true;
			this.buttonNewZMO.Click += new EventHandler(this.buttonNewZMO_Click);
			this.walkButton.AutoSize = true;
			this.walkButton.Image = Resources.Walk_small1;
			this.walkButton.Location = new System.Drawing.Point(134, 249);
			this.walkButton.Name = "walkButton";
			this.walkButton.Size = new Size(27, 26);
			this.walkButton.TabIndex = 13;
			this.walkButton.UseVisualStyleBackColor = true;
			this.walkButton.Click += new EventHandler(this.walkButton_Click);
			this.propertyGrid.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.propertyGrid.Location = new System.Drawing.Point(9, 461);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new Size(285, 217);
			this.propertyGrid.TabIndex = 12;
			this.propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
			this.listBoxMap.FormattingEnabled = true;
			this.listBoxMap.Location = new System.Drawing.Point(50, 35);
			this.listBoxMap.Name = "listBoxMap";
			this.listBoxMap.Size = new Size(237, 108);
			this.listBoxMap.TabIndex = 11;
			this.listBoxMap.SelectedIndexChanged += new EventHandler(this.listBoxMap_SelectedIndexChanged);
			this.labelMap.AutoSize = true;
			this.labelMap.Location = new System.Drawing.Point(5, 33);
			this.labelMap.Name = "labelMap";
			this.labelMap.Size = new Size(34, 13);
			this.labelMap.TabIndex = 10;
			this.labelMap.Text = "Map :";
			this.labelMotionName.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.labelMotionName.AutoSize = true;
			this.labelMotionName.Location = new System.Drawing.Point(6, 181);
			this.labelMotionName.Name = "labelMotionName";
			this.labelMotionName.Size = new Size(74, 13);
			this.labelMotionName.TabIndex = 8;
			this.labelMotionName.Text = "Motion name :";
			this.labelFPS.AutoSize = true;
			this.labelFPS.Location = new System.Drawing.Point(6, 211);
			this.labelFPS.Name = "labelFPS";
			this.labelFPS.Size = new Size(33, 13);
			this.labelFPS.TabIndex = 4;
			this.labelFPS.Text = "FPS :";
			this.numericUpDownFPS.Location = new System.Drawing.Point(45, 209);
			NumericUpDown arg_A8B_0 = this.numericUpDownFPS;
			int[] array = new int[4];
			array[0] = 60;
			arg_A8B_0.Maximum = new decimal(array);
			NumericUpDown arg_AA8_0 = this.numericUpDownFPS;
			array = new int[4];
			array[0] = 1;
			arg_AA8_0.Minimum = new decimal(array);
			this.numericUpDownFPS.Name = "numericUpDownFPS";
			this.numericUpDownFPS.Size = new Size(51, 20);
			this.numericUpDownFPS.TabIndex = 7;
			NumericUpDown arg_AF8_0 = this.numericUpDownFPS;
			array = new int[4];
			array[0] = 1;
			arg_AF8_0.Value = new decimal(array);
			this.numericUpDownFPS.ValueChanged += new EventHandler(this.numericUpDownFPS_ValueChanged);
			this.buttonSaveZMO.AutoSize = true;
			this.buttonSaveZMO.Image = Resources.Save_small;
			this.buttonSaveZMO.Location = new System.Drawing.Point(262, 249);
			this.buttonSaveZMO.Name = "buttonSaveZMO";
			this.buttonSaveZMO.Size = new Size(26, 26);
			this.buttonSaveZMO.TabIndex = 6;
			this.buttonSaveZMO.UseVisualStyleBackColor = true;
			this.buttonSaveZMO.Click += new EventHandler(this.buttonSaveZMO_Click);
			this.buttonOpenZMO.AutoSize = true;
			this.buttonOpenZMO.Image = Resources.Open_small;
			this.buttonOpenZMO.Location = new System.Drawing.Point(230, 249);
			this.buttonOpenZMO.Name = "buttonOpenZMO";
			this.buttonOpenZMO.Size = new Size(26, 26);
			this.buttonOpenZMO.TabIndex = 5;
			this.buttonOpenZMO.UseVisualStyleBackColor = true;
			this.buttonOpenZMO.Click += new EventHandler(this.buttonOpenZMO_Click);
			this.buttonCapture.AutoSize = true;
			this.buttonCapture.Image = Resources.Capture_small;
			this.buttonCapture.Location = new System.Drawing.Point(102, 249);
			this.buttonCapture.Name = "buttonCapture";
			this.buttonCapture.Size = new Size(26, 26);
			this.buttonCapture.TabIndex = 4;
			this.buttonCapture.UseVisualStyleBackColor = true;
			this.buttonPlay.AutoSize = true;
			this.buttonPlay.Image = Resources.Play_smal;
			this.buttonPlay.Location = new System.Drawing.Point(70, 249);
			this.buttonPlay.Name = "buttonPlay";
			this.buttonPlay.Size = new Size(26, 26);
			this.buttonPlay.TabIndex = 3;
			this.buttonPlay.UseVisualStyleBackColor = true;
			this.buttonPlay.Click += new EventHandler(this.buttonPlay_Click);
			this.buttonPause.AutoSize = true;
			this.buttonPause.Image = Resources.Pause_small;
			this.buttonPause.Location = new System.Drawing.Point(38, 249);
			this.buttonPause.Name = "buttonPause";
			this.buttonPause.Size = new Size(26, 26);
			this.buttonPause.TabIndex = 2;
			this.buttonPause.UseVisualStyleBackColor = true;
			this.buttonPause.Click += new EventHandler(this.buttonPause_Click);
			this.buttonBack.AutoSize = true;
			this.buttonBack.Image = Resources.Back_small;
			this.buttonBack.Location = new System.Drawing.Point(6, 249);
			this.buttonBack.Name = "buttonBack";
			this.buttonBack.Size = new Size(26, 26);
			this.buttonBack.TabIndex = 1;
			this.buttonBack.Tag = "sa";
			this.buttonBack.UseVisualStyleBackColor = true;
			this.buttonBack.Click += new EventHandler(this.buttonBack_Click);
			this.treeViewFrame.Anchor = AnchorStyles.None;
			this.treeViewFrame.ContextMenuStrip = this.frameContextMenuStrip;
			this.treeViewFrame.Location = new System.Drawing.Point(6, 295);
			this.treeViewFrame.Name = "treeViewFrame";
			this.treeViewFrame.Size = new Size(288, 160);
			this.treeViewFrame.TabIndex = 0;
			this.treeViewFrame.AfterSelect += new TreeViewEventHandler(this.treeViewFrame_AfterSelect);
			this.frameContextMenuStrip.Items.AddRange(new ToolStripItem[]
			{
				this.addMenuItem,
				this.removeMenuItem,
				this.toolStripSeparator1,
				this.copyMenuItem,
				this.pasteMenuItem,
				this.toolStripSeparator2,
				this.setCurrentPositionMenuItem,
				this.interpolationMenuItem
			});
			this.frameContextMenuStrip.Name = "frameContextMenuStrip";
			this.frameContextMenuStrip.Size = new Size(178, 148);
			this.addMenuItem.Enabled = false;
			this.addMenuItem.Name = "addMenuItem";
			this.addMenuItem.Size = new Size(177, 22);
			this.addMenuItem.Text = "Add";
			this.addMenuItem.Click += new EventHandler(this.addMenuItem_Click);
			this.removeMenuItem.Enabled = false;
			this.removeMenuItem.Name = "removeMenuItem";
			this.removeMenuItem.Size = new Size(177, 22);
			this.removeMenuItem.Text = "Remove";
			this.removeMenuItem.Click += new EventHandler(this.removeMenuItem_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(174, 6);
			this.copyMenuItem.Enabled = false;
			this.copyMenuItem.Name = "copyMenuItem";
			this.copyMenuItem.Size = new Size(177, 22);
			this.copyMenuItem.Text = "Copy";
			this.copyMenuItem.Click += new EventHandler(this.copyMenuItem_Click);
			this.pasteMenuItem.Enabled = false;
			this.pasteMenuItem.Name = "pasteMenuItem";
			this.pasteMenuItem.Size = new Size(177, 22);
			this.pasteMenuItem.Text = "Paste";
			this.pasteMenuItem.Click += new EventHandler(this.pasteMenuItem_Click);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new Size(174, 6);
			this.setCurrentPositionMenuItem.Enabled = false;
			this.setCurrentPositionMenuItem.Name = "setCurrentPositionMenuItem";
			this.setCurrentPositionMenuItem.Size = new Size(177, 22);
			this.setCurrentPositionMenuItem.Text = "Set current position";
			this.setCurrentPositionMenuItem.Click += new EventHandler(this.setCurrentPositionMenuItem_Click);
			this.interpolationMenuItem.Enabled = false;
			this.interpolationMenuItem.Name = "interpolationMenuItem";
			this.interpolationMenuItem.Size = new Size(177, 22);
			this.interpolationMenuItem.Text = "Interpolation";
			this.interpolationMenuItem.Click += new EventHandler(this.interpolationMenuItem_Click);
			this.panelPreview.Dock = DockStyle.Fill;
			this.panelPreview.Location = new System.Drawing.Point(0, 24);
			this.panelPreview.Name = "panelPreview";
			this.panelPreview.Size = new Size(708, 684);
			this.panelPreview.TabIndex = 3;
			this.panelPreview.Resize += new EventHandler(this.panelPreview_Resize);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(1008, 730);
			base.Controls.Add(this.panelPreview);
			base.Controls.Add(this.groupBoxMotion);
			base.Controls.Add(this.statusStrip);
			base.Controls.Add(this.menuStrip);
			base.MainMenuStrip = this.menuStrip;
			base.Name = "MainForm";
			this.Text = "Jiwan's Camera Editor";
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.groupBoxMotion.ResumeLayout(false);
			this.groupBoxMotion.PerformLayout();
			((ISupportInitialize)this.numericUpDownFPS).EndInit();
			this.frameContextMenuStrip.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
