/*
Copyright (c) 2012, Phil Holmes
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met: 

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer. 
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies, 
either expressed or implied, of the FreeBSD Project.

==============================================================================

This program is used to convert Noteworthy text files (nwctxt) to LilyPond
format.  It is written in c# but was created from Delphi code using Delphi2CS.
The Delphi code was written by Mike Wiering and had the following licence:

==============================================================================

  nwc2ly - program to convert music from NoteWorthy Composer to Lilypond,
  to be used as a User Tool (requires NoteWorthy Composer 2)


  Copyright (c) 2004, Mike Wiering, Wiering Software
  All rights reserved.

  Redistribution and use in source and binary forms, with or without modification, 
  are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice, this
	  list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
	  this list of conditions and the following disclaimer in the documentation
	  and/or other materials provided with the distribution.
    * Neither the name of Wiering Software nor the names of its contributors may
	  be used to endorse or promote products derived from this software without
	  specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
  POSSIBILITY OF SUCH DAMAGE.

 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;


namespace NWCTXT2Ly
{
	// Use the staff properties to decide whether it's a chord staff or whatever - page 123 of notation reference
	// Options: remove empty staves
	//	font size
	//	Multivoice rest position
	
	// Fix bug over multiple runs - done
	
	public partial class MainForm : Form
	{
		StreamReader NWCTextFile;
		StreamWriter LilyPondFile;
		StreamWriter NWCStaffFile;
		List<StaffInfo> StaffNames = new List<StaffInfo>();
		List<string> InputLines = new List<string>();
		string CurrentDir;
		string FileNameArg;
		bool SetSlurComplex = false;

		public MainForm(string FileName)
		{
			FileNameArg = FileName;
			InitializeComponent();
		}

		private void ReadRegValue(Control ThisControl, string FileName)
		{
			RegistryKey AppKey = Application.UserAppDataRegistry;
			RegistryKey SubKey;
			if (FileName != "")
			{
				SubKey = AppKey.CreateSubKey(FileName);
			}
			else
			{
				SubKey = AppKey.CreateSubKey("Default");
			}
			string ControlName = ThisControl.Name; 
			switch (ThisControl.GetType().Name)
			{
				case "Label":
				case "Button":
					break;  // Do nothing
				case "TextBox":
					TextBox Text = (TextBox)ThisControl;
					if (SubKey.GetValue(ControlName) != null)
					{
						Text.Text = SubKey.GetValue(ControlName).ToString();
					}
					break;
				case "CheckBox":
					CheckBox Check = (CheckBox)ThisControl;
					if (SubKey.GetValue(ControlName) != null)
					{
						Check.Checked = bool.Parse(SubKey.GetValue(ControlName).ToString());
					}
					break;
				case "NumericUpDown":
					NumericUpDown UpDown = (NumericUpDown)ThisControl;
					if (SubKey.GetValue(ControlName) != null)
					{
						UpDown.Value = int.Parse(SubKey.GetValue(ControlName).ToString());
					}
					break;
				case "RadioButton":
					RadioButton Radio = (RadioButton)ThisControl; 
					if (SubKey.GetValue(ControlName) != null)
					{
						Radio.Checked = bool.Parse(SubKey.GetValue(ControlName).ToString());
					}

					break;
			}
		}
		private void FileChanged(object sender, EventArgs e)
		{
			GetRegData();
		}

		private void ControlChanged(object sender, EventArgs e)
		{
			RegistryKey AppKey = Application.UserAppDataRegistry;
			RegistryKey SubKey;

			if (txtFile.Text != "")
			{
				SubKey = AppKey.CreateSubKey(txtFile.Text);
			}
			else
			{
				SubKey = AppKey.CreateSubKey("Default");
			}

			Control ControlSender = new Control();
			try
			{
				ControlSender = (Control)sender;
			}
			catch (Exception eEx)
			{
#if DEBUG
				MessageBox.Show("Error in control handling: " + eEx.Message);
#endif
				return;
			}
			if (ControlSender.GetType() == typeof(CheckBox))
			{
				CheckBox Check = (CheckBox)ControlSender;
				bool Checked = Check.Checked;
				SubKey.SetValue(Check.Name, Checked.ToString());
				if (chkRemove.Checked)
				{
					chkFirstStave.Enabled = true;
				}
				else
				{
					chkFirstStave.Enabled = false;
				}
			}
			else if (ControlSender.GetType() == typeof(NumericUpDown))
			{
				NumericUpDown UpDown = (NumericUpDown)ControlSender;
				int Number = (int)UpDown.Value;
				SubKey.SetValue(UpDown.Name, Number.ToString());
			}
			else if (ControlSender.GetType() == typeof(RadioButton))
			{
				RadioButton Radio = (RadioButton)ControlSender;
				bool Checked = Radio.Checked;
				SubKey.SetValue(Radio.Name, Checked.ToString());
			}
			else if (ControlSender.GetType() == typeof(TextBox))
			{
				TextBox Text = (TextBox)ControlSender;
				if (!Text.ReadOnly)
				{
					if (Text.Name == "txtName")
					{
						CheckName();
					}
					string TextVal = Text.Text;
					SubKey.SetValue(Text.Name, TextVal);
				}
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (Application.UserAppDataRegistry.GetValue("Filename") != null)
			{
				txtFile.Text = Application.UserAppDataRegistry.GetValue("Filename").ToString();
				btnGo.Enabled = true;
			}
			GetRegData();
			if (chkRemove.Checked) chkFirstStave.Enabled = true;

			if (FileNameArg != "")
			{
				txtFile.Text = FileNameArg;
				ProcessFile();
				Application.Exit();
			}
		}
		private void GetRegData()
		{
			for (int i = 0; i < this.Controls.Count; i++)
			{
				if (this.Controls[i].GetType().Name == "GroupBox")
				{
					for (int j = 0; j < this.Controls[i].Controls.Count; j++)
					{
						ReadRegValue(this.Controls[i].Controls[j], txtFile.Text);
					}
				}
				else
				{
					ReadRegValue(this.Controls[i], txtFile.Text);
				}
			}
		}
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofnSource = new OpenFileDialog();
			ofnSource.Filter = "Noteworthy Text files (*.nwctxt)|*.nwctxt";
			if (ofnSource.ShowDialog() == DialogResult.OK)
			{
				txtFile.Text = ofnSource.FileName;
				Application.UserAppDataRegistry.SetValue("Filename", txtFile.Text);
				btnGo.Enabled = true;
			}

		}

		private void btnGo_Click(object sender, EventArgs e)
		{
			ProcessFile();
		}
		private void ProcessFile()
		{
			btnGo.Enabled = false;
			Cursor OldCursor = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			SetSlurComplex = false;
			try
			{
				Encoding isoIn = Encoding.GetEncoding(28591);
				NWCTextFile = new StreamReader(txtFile.Text.ToString(), isoIn);
			}
			catch (Exception)
			{
				MessageBox.Show("Unable to read file " + txtFile.Text.ToString(), "NWC2LY");
				return;
			}
			FileInfo Info = new FileInfo(txtFile.Text.ToString());
			string Line, CopyLine;
			string Command;
			string StaffName = "";
			string Headers = "";
			CurrentDir = Info.DirectoryName + "\\";
			char LyricName = 'A';

			StaffNames.Clear();

			string LilyPondFileName = txtFile.Text.ToString();
			LilyPondFileName = LilyPondFileName.Replace(Info.Extension, ".ly");

			while (NWCTextFile.Peek() >= 0)
			{
				Line = NWCTextFile.ReadLine();
				InputLines.Add(Line);
				if (Line.IndexOf("setSlurComplex") > -1)
				{
					SetSlurComplex=true;
				}
			}
			while (InputLines.Count > 0)
			{
				Line = CopyLine = InputLines[0];
				InputLines.RemoveAt(0);
				Command = nwc2ly.nwc2ly.GetCommand(ref Line);
				switch (Command)
				{
					case "SongInfo":
						{
							Headers = DoHeaders(Line);
							break;
						}
					case "PgSetup":
						{
							break;
						}
					case "Font":
						{
							break;
						}
					case "PgMargins":
						{
							break;
						}
					case "AddStaff":
						{
							StaffName = nwc2ly.nwc2ly.GetPar("Name", Line);
							if (StaffName == "")
							{
								// Do something here to create a name
							}
							StaffInfo Staff = new StaffInfo();
							Staff.Name = StaffName; // This is used for creating solo parts
							StaffName = StaffName.Replace("\"", "");
							StaffName = StaffName.Replace(" ", "");
							StaffName = StaffName.Replace("/", "");
							StaffName = StaffName.Replace("\\", "");
							Staff.Label = nwc2ly.nwc2ly.GetPar("Label", Line);
							Staff.FileName = txtName.Text + StaffName;
							StaffNames.Add(Staff);
							break;
						}
					case "StaffProperties":
						{
							string Layer = nwc2ly.nwc2ly.GetPar("Layer", Line);
							if (Layer != "")
							{
								StaffNames[StaffNames.Count - 1].Layer = Layer;
							}
							StaffNames[StaffNames.Count - 1].StaffName = StaffNames[StaffNames.Count - 1].Name;
							if (StaffNames.Count > 1)
							{
								if (StaffNames[StaffNames.Count - 2].Layer == "Y")
								{
									StaffNames[StaffNames.Count - 1].StaffName = StaffNames[StaffNames.Count - 2].StaffName;
								}
							}
							string StaffLabel = StaffNames[StaffNames.Count - 1].Name.ToLower();

							if (StaffLabel.IndexOf("solo") > 0)
							{
								StaffNames[StaffNames.Count - 1].Type = "Solo";
							}
							else
							{
								string Style = nwc2ly.nwc2ly.GetPar("Style", Line);
								if (Style != "")
								{
									StaffNames[StaffNames.Count - 1].Type = Style;
								}
							}
							if (Line.IndexOf("Visible:N") > -1)
							{
								StaffNames[StaffNames.Count - 1].Visible = false;
							}
							if (Line.IndexOf("EndingBar:Master Repeat Close") > -1)
							{
								StaffNames[StaffNames.Count - 1].MasterRepeatClose = true;
							}
							break;
						}
					case "StaffInstrument":
						{
							break;
						}
					case "Lyrics":
						{
							if (Line.IndexOf("Placement:Top") >= 0)
							{
								StaffNames[StaffNames.Count - 1].bLyricsTop = true;
							}
							if (StaffNames[StaffNames.Count - 1].Visible)
							{
								GetLyrics(ref LyricName);
							}
							break;
						}
					case "Clef":
						{
							NWCStaffFile = new StreamWriter(CurrentDir + txtName.Text + StaffName + ".nwcextract");
							GetNoteInfo(CopyLine);
							NWCStaffFile.Close();
							string DynamicDir = "Down";
							if (StaffNames[StaffNames.Count - 1].Type == "Standard" || StaffNames[StaffNames.Count - 1].Type == "Solo")
							{
								DynamicDir = "Up";
							}
							string[] args = { CurrentDir + txtName.Text + StaffName + ".nwcextract", CurrentDir + txtName.Text + StaffName + ".ly", DynamicDir, radAcc.Checked.ToString() };
							nwc2ly.nwc2ly.Main(args);
							break;
						}
				}
			}

			// The bits below are the ossia lyric processing code
			// Start by looping over all the staves.
			for (int i = 0; i < StaffNames.Count; i++)
			{
				StreamReader Staff = new StreamReader(CurrentDir + StaffNames[i].FileName + ".ly");
				string InputLine = Staff.ReadLine();
				// The nwctxt processor marks staves with an ossiaStave command with %OssiaStave as the first text
				if (InputLine.IndexOf("%OssiaStave") == 0)
				{
					StaffNames[i].Type = "Ossia";
					// It places the ossia names with spaces between, so we can use split.
					string[] OssiaMusic = InputLine.Split(' ');
					for (int j = 1; j < OssiaMusic.Length; j++)
					{
						// Store the ossia names provided by this staff.
						StaffNames[i].OssiaMusic.Add(OssiaMusic[j]);
					}
				}
				if (InputLine.IndexOf("%OssiaInclude") == 0)
				{
					// These staves have ossia music included in them
					StaffNames[i].OssiaInclude = true;
					while (InputLine.IndexOf("%OssiaInclude") == 0)
					{
						// The included ossia music is listed at the top of the file, one per line
						StaffNames[i].OssiaMusic.Add(InputLine.Substring(14));  // Store the name, less %OssiaInclude
						InputLine = Staff.ReadLine();
					}
				}
				Staff.Close();
			}
			// OK, now we've found them all, let's process them
			for (int i = 0; i < StaffNames.Count; i++)  // Just doing this separate from the loop above for easier reading
			{
				if (StaffNames[i].MasterRepeatClose == true)
				{
					StreamReader InputFile = new StreamReader(CurrentDir + StaffNames[i].FileName + ".ly");
					// Get the contents of the file which includes the ossia
					string FileContents = InputFile.ReadToEnd();
					InputFile.Close();
					FileContents=FileContents.Replace("|.", ":|");
					StreamWriter FileOutput = new StreamWriter(CurrentDir + StaffNames[i].FileName + ".ly");
					FileOutput.Write(FileContents);
					FileOutput.Flush();
					FileOutput.Close();
				}
			}
			for (int i = 0; i < StaffNames.Count; i++)  // Just doing this separate from the loop above for easier reading
			{
				if (StaffNames[i].OssiaInclude)
				{
					// i.e. this staff has ossia music included in it
					List<OssiaMusicInfo> OssiaInfo = new List<OssiaMusicInfo>();
					int OssiaPieces = 0;
					for (int j = 0; j < StaffNames[i].OssiaMusic.Count; j++)
					{
						// Looping over all the included ossia parts by name
						string OssiaMusic = StaffNames[i].OssiaMusic[j];
						if (OssiaMusic.IndexOf(" Staff ") > -1)
						{
							OssiaMusic = OssiaMusic.Substring(0, OssiaMusic.Length - 7);
							OssiaPieces++;
						}
						for (int k = 0; k < StaffNames.Count; k++)  // Look for the stave(s) that contain the actual ossia music
						{
							if (StaffNames[k].Type == "Ossia")  // Ignore non-ossia staves
							{
								for (int l = 0; l < StaffNames[k].OssiaMusic.Count; l++)  // Loop over all the ossia names
								{
									string OssiaInclude = StaffNames[k].OssiaMusic[l];
									if (OssiaMusic == OssiaInclude)
									{
										// Found the staff with the included ossia music
										OssiaMusicInfo ThisOssiaDetails = new OssiaMusicInfo();
										ThisOssiaDetails.StaffName = StaffNames[k].Name;
										ThisOssiaDetails.StaffNum = k;
										ThisOssiaDetails.Layer = (StaffNames[k].Layer == "Y");
										ThisOssiaDetails.OssiaMusicName = OssiaMusic;
										ThisOssiaDetails.OssiaPiece = OssiaPieces;
										OssiaInfo.Add(ThisOssiaDetails);

									}
								}
							}
						}
					}
					Console.WriteLine(""); // Process the OssiaMusicIncludes here
					for (int j = 0; j < OssiaInfo.Count; j++)
					{
						int OssiaPiece = OssiaInfo[j].OssiaPiece;
						int LastBottom = -1;
						int LayerCount = 0;
						// Check if there are other simultaneous pieces
						for (int k = 0; k < OssiaInfo.Count; k++)
						{
							if (j != k)
							{
								if (OssiaInfo[k].OssiaPiece == OssiaPiece)
								{
									// Yes, there are - so check whether they're layered
									if (OssiaInfo[j].Layer)
									{
										// Layered - but need to check whether the next stave is the one below
										if (OssiaInfo[k].StaffNum - OssiaInfo[j].StaffNum == LayerCount + 1)
										{
											// Yes - so these are 2 voices on a single stave - but there could still be more
											LayerCount++;
											OssiaInfo[Math.Min(j, k)].TopOfLayer = true;
											OssiaInfo[Math.Min(j, k)].BottomOfLayer = false;
											OssiaInfo[Math.Max(j, k)].BottomOfLayer = true;
											OssiaInfo[Math.Max(j, k)].TopOfLayer = false;
											if (LayerCount > 1)
											{
												OssiaInfo[LastBottom].BottomOfLayer = false;
											}
											LastBottom = Math.Max(j, k);
										}
									}
								}
							}
						}
						j += LayerCount;
					}
					// OK - so now we have identified which ossias start staves and which finish them.  Loop and write
					StreamReader OssiaFile = new StreamReader(CurrentDir + StaffNames[i].FileName + ".ly");
					// Get the contents of the file which includes the ossia
					string FileContents = OssiaFile.ReadToEnd();
					OssiaFile.Close();
						
					string OssiaStaff = "";
					for (int j = 0; j < OssiaInfo.Count; j++)
					{
						string MusicDetails = "";
						if (OssiaInfo[j].TopOfLayer)
						{
							MusicDetails += "\\new Staff = \"" + OssiaInfo[j].OssiaMusicName + "OssiaStaff\" \\with {\r\n";
							MusicDetails += "\\remove \"Time_signature_engraver\"\r\n";
							MusicDetails += "alignAboveContext = #" + StaffNames[i].Name + "\r\n";
							MusicDetails += "\\override StaffSymbol #'staff-space = #(magstep -3) % Sets the staff line spacing\r\n";
							MusicDetails += "fontSize = #-2\r\n";
							MusicDetails += " } \r\n";
							MusicDetails += " << \r\n";
							OssiaStaff = OssiaInfo[j].OssiaMusicName;
						}
						MusicDetails += "\\new Voice = \"" + OssiaInfo[j].OssiaMusicName + "Ossia\" { \r\n";
						MusicDetails += "\\autoBeamOff\r\n";
						MusicDetails += "\\" + OssiaInfo[j].OssiaMusicName + "\r\n";
						MusicDetails += " } \r\n";

						if (OssiaInfo[j].BottomOfLayer)
						{
							MusicDetails += ">>\r\n";
						}

						// Replace the marker with the music string
						FileContents = FileContents.Replace("%" + OssiaInfo[j].OssiaMusicName + "Music", MusicDetails);

						// Process the lyrics
						string LyricString = "";
						// Loop over all the lyric lines in this included music, in reverse order (lily requires this)
						for (int m = StaffNames[OssiaInfo[j].StaffNum].LyricLines - 1; m >= 0; m--)
						{
							LyricString += "\\new Lyrics \\with { alignBelowContext = \"";
							LyricString += OssiaStaff + "OssiaStaff";
							LyricString += "\" } \\lyricsto \"" + OssiaInfo[j].OssiaMusicName + "Ossia\" { \\teeny ";
							StreamReader OssiaLyricsFile = new StreamReader(CurrentDir + StaffNames[OssiaInfo[j].StaffNum].LyricFileNames[m]);
							string OssiaLyrics = OssiaLyricsFile.ReadToEnd();
							OssiaLyricsFile.Close();
							Regex FindOssiaLyrics = new Regex("(<oss\\(" + OssiaInfo[j].OssiaMusicName + "\\)>)([-_\\'\\,\\.\\;\\:\\!\\)\\?\\w\\s\\\"#={}\\\\]+)(</oss>)([\\\"]?)", RegexOptions.Multiline);
							Match LyricMatch = FindOssiaLyrics.Match(OssiaLyrics);
							string ThisLyric = "";
							if (LyricMatch.Success)
							{
								ThisLyric = LyricMatch.Result("$2" + "$4");
							}
							LyricString += ThisLyric;
							LyricString += " }";
						}
						// Replace the marker with the lyric string
						FileContents = FileContents.Replace("%" + OssiaInfo[j].OssiaMusicName + "Lyrics", LyricString);
					}


					// And write it out again
					StreamWriter OssiaFileOutput = new StreamWriter(CurrentDir + StaffNames[i].FileName + ".ly");
					OssiaFileOutput.Write(FileContents);
					OssiaFileOutput.Flush();
					OssiaFileOutput.Close();
				}
			}
			// End of ossia code

			LilyPondFile = new StreamWriter(LilyPondFileName, false);
			string ver = Application.ProductVersion;
			LilyPondFile.WriteLine("% Generated by NWCTXT2Ly C# version " + ver + " by Phil Holmes");
			LilyPondFile.WriteLine("% Based on nwc2ly by Mike Wiering");
			LilyPondFile.WriteLine();
			LilyPondFile.WriteLine("\\version \"2.15.26\"");
			if (SetSlurComplex)
			{
				LilyPondFile.WriteLine(Resource1.SetSlurCode.ToString());
				LilyPondFile.WriteLine();
			}
			LilyPondFile.WriteLine("\\pointAndClickOff");
			LilyPondFile.WriteLine("#(set-global-staff-size " + udFontSize.Value.ToString() + ")");
			LilyPondFile.WriteLine("");

			LilyPondFile.WriteLine(Headers);

			LilyPondFile.WriteLine("\\paper{");
			LilyPondFile.WriteLine("  top-margin = " + udTopMargin.Value.ToString());
			LilyPondFile.WriteLine("  bottom-margin = " + udBottomMargin.Value.ToString());
			LilyPondFile.WriteLine("  left-margin = " + udLeftMargin.Value.ToString());
			LilyPondFile.WriteLine("  line-width = " + udLineWidth.Value.ToString());
			if (chkLast.Checked)
			{
				LilyPondFile.WriteLine("  ragged-last-bottom = ##f");
			}
			LilyPondFile.WriteLine("}");
			LilyPondFile.WriteLine("");

			for (int i = 0; i < StaffNames.Count; i++)
			{
				if (StaffNames[i].Type == "Ossia")
				{
					LilyPondFile.WriteLine("\\include \"" + StaffNames[i].FileName + ".ly\"");

				}
			}

			if (chkRemove.Checked)
			{
				LilyPondFile.WriteLine("\\layout {");
				LilyPondFile.WriteLine("  \\context {");
				LilyPondFile.WriteLine("    \\RemoveEmptyStaffContext");
				if (chkFirstStave.Checked)
				{
					LilyPondFile.WriteLine("    \\override VerticalAxisGroup #'remove-first = ##t");
				}
				LilyPondFile.WriteLine("  }");
				LilyPondFile.WriteLine("}");
			}

			LilyPondFile.WriteLine("\\new Score \\with {");
			if (chkForcePage.Checked)
			{
				LilyPondFile.WriteLine("  \\override NonMusicalPaperColumn #'page-break-permission = ##f");
			}
			LilyPondFile.WriteLine("  \\override PaperColumn #'keep-inside-line = ##t");
			LilyPondFile.WriteLine("  \\override NonMusicalPaperColumn #'keep-inside-line = ##t");

			LilyPondFile.WriteLine("}");
			LilyPondFile.WriteLine("{");

			if (chkCompress.Checked)
			{
				LilyPondFile.WriteLine("  \\compressFullBarRests");
				LilyPondFile.WriteLine("  \\override Score.MultiMeasureRest #'expand-limit = #1");
			}

			LilyPondFile.WriteLine("  <<");

			WriteVoices();

			LilyPondFile.WriteLine("  >>");

			LilyPondFile.WriteLine("}");
			LilyPondFile.Close();
			NWCTextFile.Close();
			this.Cursor = OldCursor;
			btnGo.Enabled = true;
		}

		private void WriteLyricDetails(StaffInfo StaffName, ref char LyricName, int StaveNumber)
		{
			string FontSize = "";
			if (StaffName.Type == "Solo")
			{
				if (chkSmall.Checked == true)
				{
					FontSize = " \\tiny ";
				}
			}
			if (StaffName.LyricLines > 0)
			{
				for (int i = 0; i < StaffName.LyricLines; i++)
				{
					if (StaffName.bLyricsTop)
					{
						LilyPondFile.WriteLine("        \\new Lyrics \\with { alignAboveContext = " + StaffName.StaffName + " } \\lyricsto \"" + StaffName.FileName + "\" { " + FontSize + " << \\include \"" + StaffName.LyricFileNames[i] + "\" >> }");
					}
					else
					{
						LilyPondFile.WriteLine("        \\new Lyrics \\lyricsto \"" + StaffName.FileName + "\" { " + FontSize + " << \\include \"" + StaffName.LyricFileNames[i] + "\" >> }");
					}
					LyricName++;
				}
			}
		}
		private void WriteVoices()
		{
			string Layer;
			bool Layering = false;
			char Name1 = 'A', Name2 = 'A';
			char LyricName = 'A';
			string[] VoiceNames = { "\\voiceOne", "\\voiceTwo", "\\voiceThree", "\\voiceFour" };
			int VoiceNumber = 0;
			int Staves = 0;
			string StaveType = "";
			string NewStaveType;
			string ThisVoiceName;
			string FontSize = "";
			string FurnitureSize = "";
			bool PianoStaff=false;
			int StavesWritten = 0;
			int NonOssiaStaves = 0;

			for (int i = 0; i < StaffNames.Count; i++)
			{
				if (StaffNames[i].Type != "Ossia")
				{
					if (StaffNames[i].Visible)
					{
						if (StaffNames[i].Layer == "N")
						{

							NonOssiaStaves++;
						}
					}
				}
			}

			for (int i = 0; i < StaffNames.Count; i++)
			{
				PianoStaff = false;
				if (StaffNames[i].Visible && StaffNames[i].Type != "Ossia") 
				{
					NewStaveType = StaffNames[i].Type;
					
					FontSize = "";
					FurnitureSize = "";
					if (NewStaveType == "Solo" && chkSmall.Checked == true)
					{
						FontSize = " \\tiny ";
						FurnitureSize = @" \with { \override KeySignature #'font-size = #-2 \override TimeSignature #'font-size = #-2 \override StaffSymbol #'staff-space = #0.8 \override Clef #'font-size = #-2 } ";
					}
					if (NewStaveType.IndexOf("Grand Staff") > -1) PianoStaff = true;
					if (StaveType != NewStaveType)
					{
						if (StaveType != "Upper Grand Staff" && NewStaveType != "Lower Grand Staff")
						{
							if (StavesWritten > 0)
							{
								LilyPondFile.WriteLine("    >>");
							}
							switch (NewStaveType)
							{
								case "Solo":
									{
										LilyPondFile.WriteLine("    <<");
										break;
									}
								case "Standard":
									{
										LilyPondFile.WriteLine("    \\new ChoirStaff <<");
										if (NonOssiaStaves != StaffNames.Count())
										{
											LilyPondFile.Write(@"      \set ChoirStaff.systemStartDelimiterHierarchy = #'(SystemStartBar (SystemStartBracket ");
											for (int j = 0; j < NonOssiaStaves; j++)
											{
												LilyPondFile.Write(j.ToString() + " ");
											}
											LilyPondFile.WriteLine(") )");
										}
										LilyPondFile.WriteLine("      \\override ChoirStaff.SystemStartBracket #'collapse-height = #1");
										LilyPondFile.WriteLine("      \\override Score.SystemStartBar #'collapse-height = #1");
										break;
									}
								case "Orchestral":
									{
										LilyPondFile.WriteLine("    \\new StaffGroup <<");
										break;
									}
								case "Upper Grand Staff":
								case "Lower Grand Staff":
									{
										LilyPondFile.WriteLine("    \\new PianoStaff <<");
										LilyPondFile.WriteLine("      \\set PianoStaff.connectArpeggios = ##t");
										if (chkLabel.Checked)
										{
											LilyPondFile.WriteLine("      \\set PianoStaff.instrumentName = #\"Piano\"");
										}
										break;
									}
								default:
									{
										break;
									}

							}
						}
						StaveType = NewStaveType;
					}
					ThisVoiceName = "";
					Layer = StaffNames[i].Layer;
					string MelodyEngraver = "";
					string NeutralDirection = "";
					if (chkMelody.Checked)
					{
						if (StaffNames[i].Type == "Solo" || StaffNames[i].Type == "Standard")
						{
							MelodyEngraver = "\\with { \\consists \"Melody_engraver\" }";
							NeutralDirection = "\\override Stem #'neutral-direction = #'()";
						}
					}
					if (Layer == "N" && Layering)  // New voice on existing stave - layering = false - need to end stave
					{
						VoiceNumber++;
						if (chkVoice.Checked)
						{
							ThisVoiceName = VoiceNames[VoiceNumber];
						}
						LilyPondFile.WriteLine("        \\new Voice = \"" + StaffNames[i].FileName + "\" " + MelodyEngraver + " { " + FontSize + ThisVoiceName + NeutralDirection + " << \\include \"" + StaffNames[i].FileName + ".ly\" >> }");
						WriteLyricDetails(StaffNames[i], ref LyricName, Staves);
						LilyPondFile.WriteLine("      >>");
						Layering = false;
						Staves++;
					}
					else if (Layer == "Y" && Layering) // New voice on existing stave - layering = true
					{
						VoiceNumber++;
						if (chkVoice.Checked)
						{
							ThisVoiceName = VoiceNames[VoiceNumber];
						}
						LilyPondFile.WriteLine("        \\new Voice = \"" + StaffNames[i].FileName + "\" " + MelodyEngraver + " { " + FontSize + ThisVoiceName + NeutralDirection + " << \\include \"" + StaffNames[i].FileName + ".ly\" >> }");
						WriteLyricDetails(StaffNames[i], ref LyricName, Staves);
					}
					else if (Layer == "N" && !Layering) // Only voice on new stave - layering = false
					{
						VoiceNumber = 0;
						LilyPondFile.WriteLine("      \\new Staff = " + StaffNames[i].Name + FurnitureSize);
						LilyPondFile.WriteLine("      <<");

						if (chkLabel.Checked)
						{
							if (!PianoStaff)
							{
								LilyPondFile.WriteLine("        \\set Staff.instrumentName = #" + StaffNames[i].Label);
							}
						}
						if (chkShort.Checked)
						{
							if (!PianoStaff)
							{
								LilyPondFile.WriteLine("        \\set Staff.shortInstrumentName = #" + StaffNames[i].StaffName);
							}
						}

						if (PianoStaff)
						{
							if (!chkPiano.Checked)
							{
								LilyPondFile.WriteLine("        \\override Staff.VerticalAxisGroup #'remove-empty = ##f"); // Ensures piano parts don't lose their empty staves
							}
						}
						LilyPondFile.WriteLine("        \\new Voice = \"" + StaffNames[i].FileName + "\" " + MelodyEngraver + " { " + FontSize + NeutralDirection + " << \\include \"" + StaffNames[i].FileName + ".ly\" >> }");
						WriteLyricDetails(StaffNames[i], ref LyricName, Staves);
						LilyPondFile.WriteLine("      >>");
						Layering = false;
						Staves++;
					}
					else if (Layer == "Y" && !Layering)  // First voice on new stave - layering = true
					{
						VoiceNumber = 0;
						if (chkVoice.Checked)
						{
							ThisVoiceName = VoiceNames[VoiceNumber];
						}
						LilyPondFile.WriteLine("      \\new Staff = " + StaffNames[i].Name + FurnitureSize);
						LilyPondFile.WriteLine("      <<");

						if (chkLabel.Checked)
						{
							if (!PianoStaff)
							{
								LilyPondFile.WriteLine("        \\set Staff.instrumentName = #" + StaffNames[i].Label);
							}
						}
						if (chkShort.Checked)
						{
							if (!PianoStaff)
							{
								LilyPondFile.WriteLine("        \\set Staff.shortInstrumentName = #" + StaffNames[i].StaffName);
							}
						}
						if (PianoStaff)
						{
							if (!chkPiano.Checked)
							{
								LilyPondFile.WriteLine("        \\override Staff.VerticalAxisGroup #'remove-empty = ##f"); // Ensures piano parts don't lose their empty staves
							}
						}
						LilyPondFile.WriteLine("        \\new Voice = \"" + StaffNames[i].FileName + "\" " + MelodyEngraver + " { " + FontSize + ThisVoiceName + NeutralDirection + " << \\include \"" + StaffNames[i].FileName + ".ly\" >> }");
						WriteLyricDetails(StaffNames[i], ref LyricName, Staves);
						Layering = true;
					}
					else
					{  // Bizarre - can't happen
					}
					Name1++;
					if (Name1 == 'Z')
					{
						Name2++;
						Name1 = 'A';
					}
					StavesWritten++;
				}
			}
			LilyPondFile.WriteLine("    >>");
			txtResult.Text = Staves.ToString() + " staves written with " + StaffNames.Count.ToString() + " voices.";
		}
		private void GetNoteInfo(string FirstLine)
		{
			string Input, InputCopy;
			string Command;

			NWCStaffFile.WriteLine("!NoteWorthyComposerClip(2.0,Single)");
			NWCStaffFile.WriteLine(FirstLine);
	
			Input = InputCopy = InputLines[0];
			InputLines.RemoveAt(0);
			Command = nwc2ly.nwc2ly.GetCommand(ref InputCopy);
			while (Command != "AddStaff" && InputLines.Count > 0)
			{
				NWCStaffFile.WriteLine(Input);

				Input = InputCopy = InputLines[0];
				InputLines.RemoveAt(0);
				Command = nwc2ly.nwc2ly.GetCommand(ref InputCopy);
			}
			InputLines.Insert(0, Input);
			NWCStaffFile.Close();
		}
		private void GetLyrics(ref char Name)
		{
			string Input;
			string Command;
			int StanzaNum = 1;
			Regex FindParen = new Regex(@"(\s)(\([\w\\',\.;:\)\!]+)(\s)");   // Replaces (text with "(text"
			// Regex FindNumbers = new Regex(@"([^\s]+([0-9])[^\s]+)");
			Regex FindNumbers = new Regex(@"([^\s]*[0-9]+[^\s]*)");
			Regex FindUnderscoreInQuotes = new Regex("(\"[0-9a-zA-Z.,;:']*)(_)([0-9a-zA-Z.,;:']*\")");
			// Regex FindUnderscore = new Regex("_\\s");
			Regex FindDoubleUnderscore = new Regex(@"([\w\\',\.;:\)\!\?\x96-\x97]+)__");
			Regex FindUnicode = new Regex(@"(\w*)([\u0090-\u00ff]+)(\w*)\S");
			Regex FindEmDash = new Regex(@"([\'\w\,\.\;\:\!\)\?]*)([\x96-\x97]+)([\'\w\,\.\;\:\!\)]*)");
			Regex FinalHyphen = new Regex(@"([A-Za-z]*)(\s*--\s*})");
			Regex FirstHyphen = new Regex(@"({\s*--\s*)([A-Za-z,\.;:\)\!]*)");
			Regex FindMarkup = new Regex(@"(<m)([1-9])(>)([^\s^-]*)");
			Regex FindVerse = new Regex(@"<verse\(([0-9])\)>");
			Regex FindSuspendOutput = new Regex(@"<sus>.*?</sus>", RegexOptions.Singleline);

			int LyricCount = 0;

			Input = InputLines[0];
			while (Input.Substring(0, 6) == "|Lyric")
			{
				string LyricFileName = StaffNames[StaffNames.Count - 1].FileName + Name + "lyrics.ly";
				StaffNames[StaffNames.Count - 1].LyricFileNames.Add(LyricFileName);
				Command = nwc2ly.nwc2ly.GetCommand(ref Input);
				if (Command.Substring(0, 5) != "Lyric")
				{
					// Shouldn't happen
				}
				Match Markup = FindMarkup.Match(Input);
				while (Markup.Success)
				{
					string MarkupNumber = Markup.Result("$2");
					int MarkupStart = Input.IndexOf("<markup" + MarkupNumber.ToString() + ">");
					if (MarkupStart > -1)
					{
						MarkupStart += 10;
						int MarkupEnd = Input.IndexOf("</markup" + MarkupNumber.ToString() + ">");
						if (MarkupEnd > -1)
						{
							MarkupEnd -= MarkupStart;
							string MarkupString = Input.Substring(MarkupStart, MarkupEnd);
							Input = Input.Substring(0, MarkupStart - 10) + Input.Substring(MarkupStart + MarkupEnd + 10);
							MarkupString = MarkupString.Replace("\\\"", "quoteMark");
							Input = FindMarkup.Replace(Input, @"\markup { " + MarkupString + "$4" + " } ");
						}
						else
						{
							Input = FindMarkup.Replace(Input, "MarkupError");
						}
					}
					else
					{
						Input = FindMarkup.Replace(Input, "MarkupError");
					}
					Markup = FindMarkup.Match(Input);
				}
				Input = Input.Replace("\\'", "'");  // Gets rid of NXCTXT escaping '
				Input = Input.Replace("|Text:\"", " ");  // Just removing the opening of the line - space makes it easier to find quoted text at the start of the lyric text.
				Input = Input.Replace("\\r\\n", " \\r\\n ");  // Gets around the problem of newlines not counting as whitespace.
				Input = FindUnderscoreInQuotes.Replace(Input, "$1 $3");  // Replace underscore within inverted commas with space 
				// Line above doesn't actually do anything, since there is never a quote mark - it's always \"
				Input = Input.Replace("-", " -- ");  // Lilypond requires space double-hyphen space
				//Input = Input.Replace("\\\"", "''"); //Replaces quote marks with Lily-style quote
				Input = Input.Replace("\\r", "\r"); // NXCTXT marks CR as \r, so convert it to an actual CR
				Input = Input.Replace("\\n", "\n"); // Ditto LF
				Input = Input.Replace(" __", " __ _");  // I think this is to allow extender lines on non-melisma notes
				Input = FindDoubleUnderscore.Replace(Input, "$1 __");  // Replace text-double underscore with text-space double underscore for extenders
				Input = Input.Replace("\"\"", "\""); // Gets rid of double quotation marks.  Not really sure why...

				Input = Input.Replace("<left>", @" \once \override LyricText #'self-alignment-X = #LEFT "); // Implements left-align
				Input = Input.Replace("<right>", @" \once \override LyricText #'self-alignment-X = #RIGHT "); // Right
				Input = Input.Replace("<centre>", @" \once \override LyricText #'self-alignment-X = #CENTER "); //Centre
				Input = Input.Replace("<i>", " \\override LyricText #'font-shape = #'italic "); // Italic text
				Input = Input.Replace(@"<\\i>", " \\override LyricText #'font-shape = #'normal "); // Revert italic
				Input = Input.Replace(@"</i>", " \\override LyricText #'font-shape = #'normal ");  //Grr.  Must remember slash, not backslash

				Input = Input.Replace("<h>", " \\override LyricText #'stencil = ##f "); //Implement <h> to hide text
				Input = Input.Replace(@"</h>", " \\revert LyricText #'stencil "); // Revert

				string FindWordRegex = "(\\s\\\\\\\")";  // Yes - you really do need all those backslashes.
				// We're looking for a word starting \" and so to tell the regtest parser to look for that, it needs \\ \"
				// So we need to escape those for the c# text parser, so need \\ \\ \\ \"
				FindWordRegex += @"([\w\',\.;:\(\)\!_]+)";
				FindWordRegex += "(\\\\\\\"\\s)";
				Regex FindWordInQuotes = new Regex(FindWordRegex, RegexOptions.Multiline);
				Match FindWordWithQuotesMatch = FindWordInQuotes.Match(Input);
				while (FindWordWithQuotesMatch.Success)
				{
					string Word = FindWordWithQuotesMatch.Result("$2");
					Input = Input.Replace("\\\"" + Word + "\\\"", "\"\\\""+ Word +"\\\"\"");
					FindWordWithQuotesMatch = FindWordInQuotes.Match(Input);
				}

				Regex FindStartingQuote = new Regex("(\\s\\\\\\\")([\\w,\\.;:\\~\\)\\!\\']+)(\\s)");
				FindWordWithQuotesMatch = FindStartingQuote.Match(Input);
				while (FindWordWithQuotesMatch.Success)
				{
					string Start = FindWordWithQuotesMatch.Result("$1");
					string Word = FindWordWithQuotesMatch.Result("$2");
					Input = Input.Replace(Start + Word, " \"\\\"" + Word + "\"");
					FindWordWithQuotesMatch = FindStartingQuote.Match(Input);
				}

				Regex FindEndingQuote = new Regex("(\\s)([\\w,\\.;:\\)\\!\\']+)(\\\\\\\"[\\s_]*)");
				FindWordWithQuotesMatch = FindEndingQuote.Match(Input);
				while (FindWordWithQuotesMatch.Success)
				{
					string End = FindWordWithQuotesMatch.Result("$3");
					string Word = FindWordWithQuotesMatch.Result("$2");
					Input = Input.Replace(Word + End, "\"" + Word + "\\\"\" ");
					FindWordWithQuotesMatch = FindEndingQuote.Match(Input);
				}

				Regex FindQuoteInWord = new Regex(@"(\s)([\w\.~,:\?]*"+ "\\\\\\\"" + @"[\w\.~,:\?]*)(\s)");
				FindWordWithQuotesMatch = FindQuoteInWord.Match(Input);
				while (FindWordWithQuotesMatch.Success)
				{
					string Word = FindWordWithQuotesMatch.Result("$2");
					Input = Input.Replace(Word, "\"" + Word + "\"");
					FindWordWithQuotesMatch = FindEndingQuote.Match(Input);
				}
				Match FindNumbersMatch = FindNumbers.Match(Input);  // Lily interprets a number in lyrics as a duration, so need to put them in quotes
				// ([^\s]*[0-9]+[^\s]*)
				string InputNumbersEsc = "";
				int OldLoc = 0;
				bool NumbersFound = false;
				while (FindNumbersMatch.Success)
				{
					string ThisNum = FindNumbersMatch.Result("$&");
					if (!ThisNum.StartsWith("<verse("))  // Special case for verse numbers, so exclude them
					{
						int Location = FindNumbersMatch.Index;
						int MatchLen = FindNumbersMatch.Length;
						InputNumbersEsc += Input.Substring(OldLoc, Location - OldLoc) + "\"" + Input.Substring(Location, MatchLen) + "\"";
						OldLoc = Location + MatchLen;
						//Input = Input.Replace(ThisNum, "\"" + ThisNum + "\"");  // Puts numerals other than verse numbers in inverted commas
					}
					FindNumbersMatch = FindNumbersMatch.NextMatch();
					NumbersFound = true;
				}
				if (NumbersFound)
				{
					InputNumbersEsc += Input.Substring(OldLoc);
					Input = InputNumbersEsc;
				}

				Input = Input.Replace(" _ ", @" \skip 1 "); // Replaces space-single underscore-space with skip to avoid left align problems
				Input = Input.Replace(" _ ", @" \skip 1 "); // Need to do it twice because they overlap

				int VersePos = Input.IndexOf("<verse>");  // Converts <verse> to an incrementing stanza number
				while (VersePos > -1)
				{
					string NewInput = Input.Substring(0, VersePos);
					NewInput += " \\set stanza = #\"" + StanzaNum.ToString() + "\" \r\n";
					NewInput += Input.Substring(VersePos + "<verse>".Length);
					Input = NewInput;
					VersePos = Input.IndexOf("<verse>");
					StanzaNum++;
				}

				Match VersePosMatch = FindVerse.Match(Input);
				// <verse\(([0-9])\)>
				while (VersePosMatch.Success)
				{
					string NewStanzaNum = VersePosMatch.Result("$1");
					if (NewStanzaNum != "")
					{
						StanzaNum = int.Parse(VersePosMatch.Result("$1"));
						Input = Input.Replace("<verse(" + StanzaNum.ToString() + ")>", " \\set stanza = #\"" + StanzaNum.ToString() + "\" \r\n");
					}
					VersePosMatch = FindVerse.Match(Input);
				}

				Input = FindParen.Replace(Input, "$1\"$2\"$3");  // Replaces (text with "(text" - must be after verse stuff

				Input = Input.Replace("quoteMark", "\"");  // Not sure why I do this....
				//Input = Input.Replace("\\markup {", "\\once \\override LyricText #'self-alignment-X = #1 \r\n \\markup {");

				MatchCollection DashMatches = FindEmDash.Matches(Input);
				// ([\'\w\,\.\;\:\!\)\?]*)([\x96-\x97]+)([\'\w\,\.\;\:\!\)]*)
				foreach (Match DashMatch in DashMatches)
				{
					string emString = DashMatch.ToString();
					string ReplacementString = @" \markup{ \concat{ ";
					ReplacementString += DashMatch.Result("\"$1\"");
					char DashChar = DashMatch.Result("$2")[0];
					if (DashChar == 150)
					{
						ReplacementString += "\\char ##x2013 ";
					}
					else
					{
						ReplacementString += "\\char ##x2014 ";
					}
					ReplacementString += DashMatch.Result("$3");
					ReplacementString += "} } ";
					int iPos = Input.IndexOf(emString);
					Input = Input.Remove(iPos, emString.Length);
					Input = Input.Insert(iPos, ReplacementString);
				}

				MatchCollection UnicodeMatches = FindUnicode.Matches(Input);
				// (\w*)([\u0090-\u00ff]+)(\w*)\S
				foreach (Match UnicodeMatch in UnicodeMatches)
				{
					char UnicodeChar;
					string UnicodeString = UnicodeMatch.ToString();
					int UnicodeVal;
					string ReplacementString = @" \markup{ \concat{ ";
					for (int j = 0; j < UnicodeString.Length; j++)
					{
						UnicodeChar = UnicodeString[j];
						UnicodeVal = (int)UnicodeChar;
						if (UnicodeVal < 128)
						{
							ReplacementString += UnicodeChar.ToString();
						}
						else
						{
							ReplacementString += @"\char ##x00";
							ReplacementString += UnicodeVal.ToString("X2") + " ";
						}
					}
					ReplacementString += "} } ";
					int iPos = Input.IndexOf(UnicodeString);
					Input = Input.Remove(iPos, UnicodeString.Length);
					Input = Input.Insert(iPos, ReplacementString);
				}

				Input = Input.Substring(0, Input.Length - 1); //Not sure why this was here - it mucks up my "num" treatment
				Input += " }";
				Input = FirstHyphen.Replace(Input, "{ \" - $2\"");  // Replaces initial hyphen with quoted hyphen
				Input = FinalHyphen.Replace(Input, "\"$1 -\" }");  // Replaces final hyphen with quoted hyphen

				// This bit is the markup processing section

				Input = Input.Replace(@"\\markup", @"\markup");
				int MarkupPos = Input.IndexOf(@"\markup");
				while (MarkupPos > -1)
				{
					int CharPos = Input.IndexOf('{', MarkupPos);
					CharPos++;
					int BracketCount = 1;
					while (BracketCount != 0)
					{
						if (Input[CharPos] == '{') BracketCount++;
						if (Input[CharPos] == '}') BracketCount--;
						if (Input[CharPos] == '_')
							Input = Input.Substring(0, CharPos) + ' ' + Input.Substring(CharPos + 1);
						if (Input.Substring(CharPos, 2) == @"\\")
						{
							Input = Input.Remove(CharPos, 1);
						}
						CharPos++;
					}
					MarkupPos = Input.IndexOf(@"\markup", CharPos);
				}

				// End markup processing

				Input = FindSuspendOutput.Replace(Input, ""); // Gets rid of all text between <sus> and </sus>

				if (chkStanza.Checked)
				{
					Input = "\\set stanza = #\"" + StanzaNum.ToString() + "\" \r\n" + Input; ;
					StanzaNum++;
				}
				Input = "{\r\n" + Input;

				Input = Input.Replace("<skip>", "");
				Input = Input.Replace("<stop>", "\"  \"");

				StreamWriter LyricFile = new StreamWriter(CurrentDir + LyricFileName);
				LyricFile.Write(Input);
				LyricFile.Close();
				InputLines.RemoveAt(0);
				Input = InputLines[0];
				Name++;
				LyricCount++;
			}
			StaffNames[StaffNames.Count - 1].LyricLines = LyricCount;
		}
		private string DoHeaders(string SongInfo)
		{
			StringBuilder Header = new StringBuilder();
			string[] ParameterList;
			string[,] HeaderKey = {{"Title:", "subtitle = \\markup {\\fontsize #5 \"", "\"}" },
								  {"Author:", "composer = \\markup { \\center-column { \"Music: ", "\" \\vspace #1 } }"},
								  {"Lyricist:", "poet = \\markup { \\center-column { \"Lyrics: ", "\" \\vspace #1 } }"},
								  {"Copyright1:", "copyright = \\markup { \\vspace #2 \"", "\" } "},
								  {"Comments:", @"title = \markup \fill-line { \line {} \line { ", "} }"}};


			Header.AppendLine("\\header {");

			ParameterList = SongInfo.Split('|');

			string Param;
			for (int i = 0; i < ParameterList.Length; i++)
			{
				Param = ParameterList[i];
				for (int j = 0; j < HeaderKey.GetLength(0); j++)
				{
					if (Param.Length > 0)
					{
						if (Param.IndexOf(HeaderKey[j, 0]) >= 0)
						{
							Param = Param.Replace(HeaderKey[j, 0], "");
							Param = Param.Replace("\"", "");
							Param = Param.Replace("\\", "");
							if (Param.Length > 0)
							{
								Header.AppendLine("  " + HeaderKey[j, 1] + Param + HeaderKey[j, 2]);
							}
						}
					}
				}
			}

			Header.AppendLine("}");
			return Header.ToString();
		}
		private void CheckName()
		{
			bool Valid = true;
			string Name = txtName.Text;
			int Selection = txtName.SelectionStart;
			int i = 0;
			if (i > 0)
			{
				do
				{
					if (!char.IsLetter(Name[i]))
					{
						Valid = false;
						Name = Name.Remove(i, 1);
					}
					i++;
				} while (i < Name.Length);
			}
			if (!Valid)
			{
				MessageBox.Show("Name can only contain letters", "NWCTXT2Ly");
				txtName.Text = Name;
				txtName.SelectionStart = Selection - 1;
			}
		}
	}
	public class StaffInfo
	{
		public string FileName;
		public int Voice;
		public string Layer;
		public int LyricLines = 0;
		public List<string> LyricFileNames = new List<string>();
		public string Type;
		public string Name = "";
		public string Label = "";
		public bool bLyricsTop = false;
		public bool Visible = true;
		public string StaffName = "";
		public bool OssiaInclude = false;
		public List<string> OssiaMusic = new List<string>();
		public bool MasterRepeatClose = false;
	}
	public class OssiaMusicInfo
	{
		public string StaffName;
		public int StaffNum;
		public bool Layer;
		public string OssiaMusicName;
		public int OssiaPiece;
		public bool TopOfLayer = true;
		public bool BottomOfLayer = true;
	}
}
