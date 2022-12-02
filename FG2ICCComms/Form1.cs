using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace FG2ICCComms
{
	// Token: 0x02000002 RID: 2
	public partial class Form1 : Form
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000280C File Offset: 0x00000A0C
		public void update_coms()
		{
			string text = this.comboBox1.SelectedItem as string;
			this.comboBox1.Items.Clear();
			foreach (string text2 in SerialPort.GetPortNames())
			{
				string text3 = text2;
				if (!char.IsDigit(text3[text3.Length - 1]))
				{
					text3 = text3.Substring(0, text3.Length - 1);
				}
				this.comboBox1.Items.Add(text3);
			}
			if (text != null && this.comboBox1.Items.Contains(text))
			{
				this.comboBox1.SelectedItem = text;
				return;
			}
			if (this.comboBox1.Items.Count > 0)
			{
				this.comboBox1.SelectedIndex = this.comboBox1.Items.Count - 1;
				return;
			}
			this.button1.Enabled = false;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000028F0 File Offset: 0x00000AF0
		private bool ishexpair(string s)
		{
			return s.Length >= 2 && ((s[0] >= '0' && s[1] <= '9') || s[0] >= 'A' || s[1] <= 'F' || s[0] >= 'a' || s[1] <= 'f');
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000294C File Offset: 0x00000B4C
		private int key_from_seed(int s)
		{
			byte b = (byte)(s >> 16 & 255);
			byte b2 = (byte)(s >> 8 & 255);
			byte b3 = (byte)(s & 255);
			int num = 112;
			int num2 = 76;
			int num3 = 97;
			int num4 = 82;
			int num5 = 77;
			int num6 = ((int)b << 16) + ((int)b2 << 8) + (int)b3;
			int num7 = (num6 & 16711680) >> 16 | (num6 & 65280) | num << 24 | (num6 & 255) << 16;
			int num8 = 12927401;
			for (int i = 0; i < 32; i++)
			{
				int num9 = ((num7 >> i & 1) ^ (num8 & 1)) << 23;
				int num11;
				int num10;
				num8 = (((num10 = (num11 = (num9 | num8 >> 1))) & 15691735) | ((num10 & 1048576) >> 20 ^ (num11 & 8388608) >> 23) << 20 | ((num8 >> 1 & 32768) >> 15 ^ (num11 & 8388608) >> 23) << 15 | ((num8 >> 1 & 4096) >> 12 ^ (num11 & 8388608) >> 23) << 12 | 32 * ((num8 >> 1 & 32) >> 5 ^ (num11 & 8388608) >> 23) | 8 * ((num8 >> 1 & 8) >> 3 ^ (num11 & 8388608) >> 23));
			}
			for (int i = 0; i < 32; i++)
			{
				int num9 = (((num5 << 24 | num4 << 16 | num2 | num3 << 8) >> i & 1) ^ (num8 & 1)) << 23;
				int num14;
				int num13;
				int num12 = num13 = (num14 = (num9 | num8 >> 1));
				num8 = ((num13 & 15691735) | ((num12 & 1048576) >> 20 ^ (num14 & 8388608) >> 23) << 20 | ((num8 >> 1 & 32768) >> 15 ^ (num14 & 8388608) >> 23) << 15 | ((num8 >> 1 & 4096) >> 12 ^ (num14 & 8388608) >> 23) << 12 | 32 * ((num8 >> 1 & 32) >> 5 ^ (num14 & 8388608) >> 23) | 8 * ((num8 >> 1 & 8) >> 3 ^ (num14 & 8388608) >> 23));
			}
			return (num8 & 983040) >> 16 | 16 * (num8 & 15) | ((num8 & 15728640) >> 20 | (num8 & 61440) >> 8) << 8 | (num8 & 4080) >> 4 << 16;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002BA0 File Offset: 0x00000DA0
		private int h2d(char c)
		{
			if (c <= '0')
			{
				return 0;
			}
			if (c > '9' && c < 'A')
			{
				return 0;
			}
			if (c > 'F')
			{
				return 0;
			}
			if (c <= '9')
			{
				return (int)(c - '0');
			}
			return (int)('\n' + (c - 'A'));
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002BD0 File Offset: 0x00000DD0
		private void addTxt1(string m)
		{
			TextBox textBox = this.textBox1;
			textBox.Text += m;
			this.textBox1.SelectionStart = this.textBox1.Text.Length;
			this.textBox1.SelectionLength = 0;
			this.textBox1.ScrollToCaret();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002C28 File Offset: 0x00000E28
		private string printerr(int error)
		{
			if (error <= 38)
			{
				switch (error)
				{
				case 16:
					return "General reject\r\n";
				case 17:
					return "Service not supported\r\n";
				case 18:
					return "Subfunction not supported\r\n";
				case 19:
					return "Incorrect message length or invalid format\r\n";
				case 20:
					return "Response too long\r\n";
				default:
					switch (error)
					{
					case 33:
						return "Busy repeat request\r\n";
					case 34:
						return "Condition not correct\r\n";
					case 36:
						return "Request sequence error\r\n";
					case 37:
						return "No response from subnet component\r\n";
					case 38:
						return "Failure prevents execution of requested action\r\n";
					}
					break;
				}
			}
			else
			{
				switch (error)
				{
				case 49:
					return "Request out of range\r\n";
				case 50:
				case 52:
					break;
				case 51:
					return "Security access denied\r\n";
				case 53:
					return "Invalid key\r\n";
				case 54:
					return "Exceeded number of attempts\r\n";
				case 55:
					return "Required time delay not expired\r\n";
				default:
					switch (error)
					{
					case 112:
						return "Upload/download not accepted\r\n";
					case 113:
						return "Transfer data suspended\r\n";
					case 114:
						return "General programming failure\r\n";
					case 115:
						return "Wrong block sequence counter\r\n";
					case 116:
					case 117:
					case 118:
					case 119:
						break;
					case 120:
						return "";
					default:
						switch (error)
						{
						case 126:
							return "Subfunction not supported in active session\r\n";
						case 127:
							return "Service not supported in active session\r\n";
						}
						break;
					}
					break;
				}
			}
			return "";
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002D70 File Offset: 0x00000F70
		private void delayloop(int ms)
		{
			long ticks = DateTime.UtcNow.Ticks;
			long num = ticks;
			while (this.waitfor != 0 && num - ticks < (long)(ms * 10000))
			{
				Thread.Sleep(1);
				int num2 = this.waitfor2;
				Application.DoEvents();
				int num3 = this.inerror;
				this.timer2_Tick(null, null);
				if (num3 != 0)
				{
					this.inerror = num3;
				}
				if (num2 != this.waitfor2)
				{
					return;
				}
				num = DateTime.UtcNow.Ticks;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002DEC File Offset: 0x00000FEC
		private void WaitCmdRdy()
		{
			long ticks = DateTime.UtcNow.Ticks;
			long num = ticks;
			this.timeout = 0;
			while (this.cmdready == 0 && num - ticks < 50000000L)
			{
				Thread.Sleep(1);
				Application.DoEvents();
				int num2 = this.inerror;
				this.timer2_Tick(null, null);
				if (num2 != 0)
				{
					this.inerror = num2;
				}
				num = DateTime.UtcNow.Ticks;
			}
			if (num - ticks >= 50000000L)
			{
				this.addTxt1("5 second ELM prompt timer expired, try to disconnect & reconnect\r\n");
				this.timeout = 1;
			}
			this.cmdready = 0;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002E7C File Offset: 0x0000107C
		private void Write2(ref byte[] b)
		{
			string str = "";
			for (int i = 0; i < b.Length; i++)
			{
				str += b[i].ToString("X2");
			}
			this.WaitCmdRdy();
			if (this.timeout != 0)
			{
				return;
			}
			this.g_comms.com.Write(str + "\r");
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002EE4 File Offset: 0x000010E4
		private void send_data(ref string di)
		{
			byte[] array = new byte[8];
			bool flag = false;
			int num = 0;
			di = di.Substring(0, di.Length - 1);
			int i = di.Length / 2;
			byte[] array2 = new byte[i];
			int j = 0;
			int num2 = 0;
			while (j < array2.Length)
			{
				array2[j] = (byte)(this.h2d(di[num2]) << 4);
				byte[] array3 = array2;
				int num3 = j;
				array3[num3] |= (byte)this.h2d(di[num2 + 1]);
				j++;
				num2 += 2;
			}
			array[0] = (byte)(16 + (i >> 8 & 15));
			array[1] = (byte)(i & 255);
			int k = 0;
			int l = 2;
			while (k < 6)
			{
				array[l++] = array2[k++];
			}
			i -= 6;
			j = 6;
			int num4 = 1;
			this.waitfor2 = 1;
			do
			{
				this.Write2(ref array);
				if (this.timeout == 0)
				{
					this.delayloop(1000);
				}
				if (this.timeout == 0 && this.waitfor2 == 0 && this.inerror == 0)
				{
					goto IL_1AB;
				}
			}
			while (this.waitfor2 != 0 && ++num < 4);
			this.addTxt1(string.Concat(new object[]
			{
				"*** Timeout in send data *** (",
				this.waitfor.ToString("X2"),
				",",
				this.timeout,
				",",
				this.waitfor2,
				",",
				this.inerror,
				" :- ",
				di,
				")\r\n"
			}));
			return;
			IL_1AB:
			if (i > 7)
			{
				flag = true;
				this.Write("ATR0\r");
			}
			while (i > 0)
			{
				if (i > 7)
				{
					num2 = 7;
				}
				else
				{
					num2 = i;
				}
				array[0] = (byte)(32 + (num4++ & 15));
				if (num2 != 7)
				{
					l = 1;
					while (l < 8)
					{
						array[l++] = 0;
					}
				}
				k = j;
				l = 1;
				for (int m = 0; m < num2; m++)
				{
					array[l++] = array2[k++];
				}
				j += num2;
				i -= num2;
				if (flag && i == 0)
				{
					this.Write("ATR1\r");
				}
				this.Write2(ref array);
				if (this.timeout != 0)
				{
					this.addTxt1("*** Timeout in send data ***\r\n");
					return;
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00003150 File Offset: 0x00001350
		private void Write(string s)
		{
			if (s.Length <= 1)
			{
				return;
			}
			if (s.Length >= 2 && s.Substring(0, 2) == "AT")
			{
				this.WaitCmdRdy();
				if (this.timeout != 0)
				{
					return;
				}
				this.g_comms.com.Write(s);
				return;
			}
			else
			{
				int num = s.Length / 2;
				if (num > 7)
				{
					this.send_data(ref s);
					return;
				}
				this.WaitCmdRdy();
				if (this.timeout != 0)
				{
					return;
				}
				this.g_comms.com.Write(num.ToString("X2") + s);
				return;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000031EC File Offset: 0x000013EC
		private void reset()
		{
			this.timeout = 0;
			this.waitfor2 = 0;
			this.waitfor = 0;
			this.inerror = 0;
			this.hidesend = 0;
			this.msg = "";
			this.newpacket = false;
			this.rxBuf[0] = 0;
			this.rxBuf[1] = 0;
			this.lastchecksum = 0;
			this.enabled = 0;
			this.WaitCmdRdy();
			if (this.timeout == 0)
			{
				this.cmdready = 1;
				this.Write("ATSTE0\r");
				if (this.timeout == 0)
				{
					this.Write("ATSH7A6\r");
					if (this.timeout == 0)
					{
						this.Write("ATR1\r");
						int num = this.timeout;
					}
				}
			}
			this.WaitCmdRdy();
			if (this.timeout == 0)
			{
				this.cmdready = 1;
			}
			this.enabled = 1;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000032B7 File Offset: 0x000014B7
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (!this.g_comms.com.IsOpen && SerialPort.GetPortNames().Length != this.comboBox1.Items.Count)
			{
				this.update_coms();
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000032EC File Offset: 0x000014EC
		private bool security()
		{
			int num = 0;
			this.hidesend = 1;
			this.addTxt1("Increasing the Security Level\r\n");
			this.waitfor = 80;
			this.Write("1081\r");
			if (this.timeout == 0)
			{
				this.delayloop(250);
			}
			Thread.Sleep(450);
			do
			{
				this.waitfor = 80;
				this.Write("10FA\r");
				if (this.timeout == 0)
				{
					this.delayloop(1000);
				}
				if (this.timeout == 0 && this.waitfor == 0 && this.inerror == 0)
				{
					goto IL_9E;
				}
			}
			while (num++ < 5);
			this.addTxt1("Failed to enter secure mode\r\n");
			this.hidesend = 0;
			return false;
			IL_9E:
			Thread.Sleep(450);
			this.waitfor = 103;
			this.Write("2701\r");
			if (this.timeout == 0)
			{
				this.delayloop(1000);
			}
			if (this.timeout != 0 || this.waitfor != 0 || this.inerror != 0)
			{
				this.addTxt1("Failed to request security level\r\n");
				this.hidesend = 0;
				return false;
			}
			int num2 = 0;
			num2 += (int)this.rxBuf[3] << 16;
			num2 += (int)this.rxBuf[4] << 8;
			num2 += (int)this.rxBuf[5];
			string s = "2702" + this.key_from_seed(num2).ToString("X6") + "\r";
			this.waitfor = 103;
			this.Write(s);
			if (this.timeout == 0)
			{
				this.delayloop(1000);
			}
			if (this.timeout != 0 || this.waitfor != 0 || this.inerror != 0)
			{
				this.addTxt1("Failed to raise security level\r\n");
				this.hidesend = 0;
				return false;
			}
			this.addTxt1("OK\r\n");
			this.hidesend = 0;
			return true;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000034A0 File Offset: 0x000016A0
		private void loop_task()
		{
			this.inerror = 0;
			if (this.rxBuf[0] <= 7 && this.rxBuf[1] == 127)
			{
				if (this.rxBuf[3] != 120)
				{
					this.inerror = 1;
					this.waitfor = 0;
					this.g_comms.com.Write(" ");
				}
			}
			else if ((this.rxBuf[0] < 16 || this.rxBuf[0] >= 48) && (this.rxBuf[0] != 2 || this.rxBuf[1] != 126))
			{
				if (this.rxBuf[1] == 117)
				{
					this.g_comms.com.Write(" ");
					this.waitfor = 118;
					this.Write("3601\r");
				}
				else
				{
					if (this.rxBuf[1] == 119)
					{
						this.lastchecksum = ((int)this.rxBuf[2] << 8 | (int)this.rxBuf[3]);
					}
					if ((int)this.rxBuf[1] == this.waitfor)
					{
						this.waitfor = 0;
						this.g_comms.com.Write(" ");
					}
				}
			}
			if (this.rxBuf[0] == 48)
			{
				this.waitfor2 = 0;
				this.g_comms.com.Write(" ");
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000035E8 File Offset: 0x000017E8
		private void timer2_Tick(object sender, EventArgs e)
		{
			if (!this.g_comms.com.IsOpen)
			{
				return;
			}
			for (;;)
			{
				if (this.newpacket)
				{
					this.newpacket = false;
					this.loop_task();
				}
				if (this.g_comms.com.HasData() == 0)
				{
					break;
				}
				bool flag = false;
				string text = "";
				while (this.g_comms.com.HasData() != 0)
				{
					char c = this.g_comms.com.ReadByte();
					if (c == '\r')
					{
						if (this.checking)
						{
							if (this.msg.Contains("2A:38 N"))
							{
								this.NVMcheck1 = true;
							}
							if (this.msg.Contains("2C:81 N") && this.msg.Contains("2D:04 N"))
							{
								this.NVMcheck2 = true;
							}
							if (this.msg.Contains("2C:"))
							{
								this.waitfor = 0;
							}
						}
						if (this.msg != "" && this.msg != "NO DATA" && this.msg != "STOPPED" && this.enabled != 0 && !this.msg.StartsWith("30") && (this.hidesend == 0 || this.msg != "OK"))
						{
							text = text + this.msg + "\r\n";
							flag = true;
						}
						if (this.msg.Length == 16 && this.ishexpair(this.msg))
						{
							this.rxBuf[0] = (byte)(this.h2d(this.msg[0]) << 4 | this.h2d(this.msg[1]));
							this.rxBuf[1] = (byte)(this.h2d(this.msg[2]) << 4 | this.h2d(this.msg[3]));
							this.rxBuf[2] = (byte)(this.h2d(this.msg[4]) << 4 | this.h2d(this.msg[5]));
							this.rxBuf[3] = (byte)(this.h2d(this.msg[6]) << 4 | this.h2d(this.msg[7]));
							this.rxBuf[4] = (byte)(this.h2d(this.msg[8]) << 4 | this.h2d(this.msg[9]));
							this.rxBuf[5] = (byte)(this.h2d(this.msg[10]) << 4 | this.h2d(this.msg[11]));
							this.rxBuf[6] = (byte)(this.h2d(this.msg[12]) << 4 | this.h2d(this.msg[13]));
							this.rxBuf[7] = (byte)(this.h2d(this.msg[14]) << 4 | this.h2d(this.msg[15]));
							if (this.rxBuf[0] <= 7 && this.rxBuf[1] == 127)
							{
								flag = true;
								text += this.printerr((int)this.rxBuf[3]);
							}
							this.newpacket = true;
						}
						this.msg = "";
						break;
					}
					if (c != '\n')
					{
						if (c == '>')
						{
							this.cmdready = 1;
						}
						else
						{
							this.msg += c;
						}
					}
				}
				if (flag)
				{
					this.addTxt1(text);
				}
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003978 File Offset: 0x00001B78
		private void button1_Click(object sender, EventArgs e)
		{
			if (!this.g_comms.com.IsOpen)
			{
				this.textBox1.Text = "";
				this.timer2.Enabled = false;
				this.Refresh();
				this.g_comms.com.Open();
				if (!this.g_comms.com.IsOpen)
				{
					return;
				}
				Thread.Sleep(750);
				while (this.g_comms.com.HasData() != 0)
				{
					this.g_comms.com.ReadByte();
				}
				this.timer2.Enabled = true;
				this.button1.Text = "Disconnect";
				this.enabled = 1;
				this.cmdready = 1;
				this.Write("ATZ\r");
				this.Write("ATM0\r");
				if (this.timeout != 0)
				{
					this.addTxt1("Hints: Are you using an ELM327? Do you have the correct BAUD rate selected?\r\n");
				}
				else
				{
					this.WaitCmdRdy();
					if (this.timeout == 0)
					{
						this.cmdready = 1;
						this.enabled = 0;
						this.checking = true;
						this.NVMcheck1 = false;
						this.NVMcheck2 = false;
						this.Write("ATPPS\r");
						if (this.timeout == 0)
						{
							this.waitfor = -1;
							this.delayloop(1000);
							this.waitfor = 0;
							this.checking = false;
							if (!this.NVMcheck1 || !this.NVMcheck2)
							{
								this.addTxt1("NVM settings incorrect.\r\n");
								this.addTxt1("Hints: Did you run Forscan first? Looking for ATPPS/NVM 2A:38 N, 2C:81 N, 2D:04 N\r\n");
							}
							else
							{
								this.Write("ATE0\r");
								if (this.timeout == 0)
								{
									this.Write("ATL0\r");
									if (this.timeout == 0)
									{
										this.Write("ATS0\r");
										if (this.timeout == 0)
										{
											this.Write("ATCF7AE\r");
											if (this.timeout == 0)
											{
												this.Write("ATCM7FF\r");
												if (this.timeout == 0)
												{
													this.Write("ATSH7A6\r");
													if (this.timeout == 0)
													{
														this.Write("ATTPB\r");
														if (this.timeout == 0)
														{
															this.Write("ATAT0\r");
															if (this.timeout == 0)
															{
																this.Write("ATSTE0\r");
																if (this.timeout == 0)
																{
																	this.Write("ATCAF0\r");
																	if (this.timeout == 0)
																	{
																		this.Write("ATV0\r");
																		if (this.timeout == 0)
																		{
																			this.Write("ATBI\r");
																			this.WaitCmdRdy();
																			if (this.timeout == 0)
																			{
																				this.cmdready = 1;
																				this.enabled = 1;
																				this.button2.Enabled = true;
																				this.button3.Enabled = true;
																				this.button4.Enabled = true;
																				return;
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			this.g_comms.com.Close();
			this.button1.Text = "Connect";
			this.button2.Enabled = false;
			this.button3.Enabled = false;
			this.button4.Enabled = false;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003C74 File Offset: 0x00001E74
		private void button2_Click(object sender, EventArgs e)
		{
			base.Enabled = false;
			this.reset();
			if (this.security())
			{
				this.hidesend = 1;
				this.addTxt1("Running Recore from USB\r\n");
				this.Write("31AB01\r");
				this.addTxt1("OK\r\n");
				this.hidesend = 0;
			}
			base.Enabled = true;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00003CCC File Offset: 0x00001ECC
		private void button3_Click(object sender, EventArgs e)
		{
			this.Write("1101\r");
			this.addTxt1("OK\r\n");
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00003CE4 File Offset: 0x00001EE4
		private void button4_Click(object sender, EventArgs e)
		{
			base.Enabled = false;
			this.reset();
			if (this.security())
			{
				this.hidesend = 1;
				this.addTxt1("Updating Data ID E610 - High Series\r\n");
				this.Write("2EE610415237392D3134443031352D464300000000000000000000\r");
				this.Write("1081\r");
				this.WaitCmdRdy();
				if (this.timeout == 0)
				{
					this.cmdready = 1;
				}
				this.addTxt1("OK\r\n");
				this.hidesend = 0;
			}
			base.Enabled = true;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003D5C File Offset: 0x00001F5C
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.g_comms.com.IsOpen)
			{
				this.g_comms.com.Close();
				this.button1.Text = "Connect";
				this.button2.Enabled = false;
				this.button3.Enabled = false;
				this.button4.Enabled = false;
			}
			if (this.comboBox1.SelectedItem != null && this.comboBox2.SelectedItem != null)
			{
				this.g_comms.com.Configure(this.comboBox1.SelectedItem.ToString(), Convert.ToInt32(this.comboBox2.SelectedItem.ToString()));
				this.button1.Enabled = true;
				return;
			}
			this.button1.Enabled = false;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003E28 File Offset: 0x00002028
		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.g_comms.com.IsOpen)
			{
				this.g_comms.com.Close();
				this.button1.Text = "Connect";
				this.button2.Enabled = false;
				this.button3.Enabled = false;
				this.button4.Enabled = false;
			}
			if (this.comboBox2.SelectedItem != null)
			{
				if (!this.updating)
				{
					Form1.Settings.SetObject("Serial Speed", this.comboBox2.SelectedItem.ToString());
					Form1.Settings.Save();
				}
				if (this.comboBox1.SelectedItem != null)
				{
					this.g_comms.com.Configure(this.comboBox1.SelectedItem.ToString(), Convert.ToInt32(this.comboBox2.SelectedItem.ToString()));
					this.button1.Enabled = true;
					return;
				}
				this.button1.Enabled = false;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003F20 File Offset: 0x00002120
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.g_comms.com.IsOpen)
			{
				this.g_comms.com.Close();
				this.button1.Text = "Connect";
				this.button2.Enabled = false;
				this.button3.Enabled = false;
				this.button4.Enabled = false;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003F84 File Offset: 0x00002184
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (this.updating)
			{
				return;
			}
			if (this.checkBox1.Checked)
			{
				Form1.reset_using_usb = true;
				Form1.Settings.SetObject("Serial DTR", "1");
				Form1.Settings.Save();
				return;
			}
			Form1.reset_using_usb = false;
			Form1.Settings.SetObject("Serial DTR", "0");
			Form1.Settings.Save();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003FDC File Offset: 0x000021DC
		private void Form1_LocationChanged(object sender, EventArgs e)
		{
			if (this.updating || base.WindowState != FormWindowState.Normal)
			{
				return;
			}
			Form1.Settings.SetObject("Form X", base.Location.X.ToString());
			Form1.Settings.SetObject("Form Y", base.Location.Y.ToString());
			Form1.Settings.Save();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00004040 File Offset: 0x00002240
		private void Form1_Resize(object sender, EventArgs e)
		{
			if (this.updating || base.WindowState != FormWindowState.Normal)
			{
				return;
			}
			Form1.Settings.SetObject("Form Width", base.Width.ToString());
			Form1.Settings.SetObject("Form Height", base.Height.ToString());
			Form1.Settings.Save();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00004094 File Offset: 0x00002294
		public Form1()
		{
			this.InitializeComponent();
			this.g_comms.com = new Form1.UsbSerial();
			this.update_coms();
			this.updating = true;
			this.comboBox2.SelectedItem = Form1.Settings.GetObject("Serial Speed", "38400");
			base.Width = Convert.ToInt32(Form1.Settings.GetObject("Form Width", base.Width.ToString()));
			base.Height = Convert.ToInt32(Form1.Settings.GetObject("Form Height", base.Height.ToString()));
			Rectangle bounds = Screen.GetBounds(new Point(base.Location.X, base.Location.Y));
			string defval = Convert.ToString(bounds.Width / 2 - base.Width / 2);
			string defval2 = Convert.ToString(bounds.Height / 2 - base.Height / 2);
			base.Left = Convert.ToInt32(Form1.Settings.GetObject("Form X", defval));
			base.Top = Convert.ToInt32(Form1.Settings.GetObject("Form Y", defval2));
			string a = Form1.Settings.GetObject("Serial DTR", "0").ToString();
			if (a == "1")
			{
				Form1.reset_using_usb = true;
				this.checkBox1.Checked = true;
			}
			this.updating = false;
		}

		// Token: 0x04000001 RID: 1
		private const string CANTimeout = "ATSTE0\r";

		// Token: 0x04000010 RID: 16
		public static bool reset_using_usb;

		// Token: 0x04000011 RID: 17
		public Form1.Comms g_comms;

		// Token: 0x04000012 RID: 18
		private bool updating;

		// Token: 0x04000013 RID: 19
		private bool checking;

		// Token: 0x04000014 RID: 20
		private bool NVMcheck1;

		// Token: 0x04000015 RID: 21
		private bool NVMcheck2;

		// Token: 0x04000016 RID: 22
		private int timeout;

		// Token: 0x04000017 RID: 23
		private int cmdready;

		// Token: 0x04000018 RID: 24
		private int waitfor2;

		// Token: 0x04000019 RID: 25
		private int waitfor;

		// Token: 0x0400001A RID: 26
		private int inerror;

		// Token: 0x0400001B RID: 27
		private int enabled = 1;

		// Token: 0x0400001C RID: 28
		private int hidesend;

		// Token: 0x0400001D RID: 29
		private string msg = "";

		// Token: 0x0400001E RID: 30
		private bool newpacket;

		// Token: 0x0400001F RID: 31
		private byte[] rxBuf = new byte[8];

		// Token: 0x04000020 RID: 32
		private int lastchecksum;

		// Token: 0x02000003 RID: 3
		public static class Settings
		{
			// Token: 0x17000001 RID: 1
			// (get) Token: 0x0600001F RID: 31 RVA: 0x0000420C File Offset: 0x0000240C
			public static string DataPath
			{
				get
				{
					if (Form1.Settings.mDataPath == null)
					{
						Form1.Settings.mDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FG2ICCComms");
						if (!Directory.Exists(Form1.Settings.mDataPath))
						{
							Directory.CreateDirectory(Form1.Settings.mDataPath);
						}
					}
					return Form1.Settings.mDataPath;
				}
			}

			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000020 RID: 32 RVA: 0x00004248 File Offset: 0x00002448
			private static string filename
			{
				get
				{
					string text = "FG2ICCComms.Settings.bin";
					string text2 = Path.Combine(Form1.Settings.DataPath, text);
					if (!File.Exists(text2) && File.Exists(text))
					{
						File.Copy(text, text2);
					}
					return text2;
				}
			}

			// Token: 0x06000021 RID: 33 RVA: 0x00004280 File Offset: 0x00002480
			static Settings()
			{
				try
				{
					if (File.Exists(Form1.Settings.filename))
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
						using (FileStream fileStream = new FileStream(Form1.Settings.filename, FileMode.Open, FileAccess.Read, FileShare.None))
						{
							Form1.Settings.dic = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
							fileStream.Close();
						}
					}
				}
				catch
				{
				}
				if (Form1.Settings.dic == null)
				{
					Form1.Settings.dic = new Dictionary<string, object>();
				}
			}

			// Token: 0x06000022 RID: 34 RVA: 0x0000430C File Offset: 0x0000250C
			public static object GetObject(string key, object defval)
			{
				if (!Form1.Settings.dic.ContainsKey(key) || Form1.Settings.dic[key] == null)
				{
					return defval;
				}
				return Form1.Settings.dic[key];
			}

			// Token: 0x06000023 RID: 35 RVA: 0x00004338 File Offset: 0x00002538
			public static object GetAndDeleteObject(string key, object defval)
			{
				object result = (Form1.Settings.dic.ContainsKey(key) && Form1.Settings.dic[key] != null) ? Form1.Settings.dic[key] : defval;
				Form1.Settings.DeleteObject(key);
				return result;
			}

			// Token: 0x06000024 RID: 36 RVA: 0x00004375 File Offset: 0x00002575
			public static void SetObject(string key, object value)
			{
				if (Form1.Settings.dic.ContainsKey(key))
				{
					Form1.Settings.dic[key] = value;
					return;
				}
				Form1.Settings.dic.Add(key, value);
			}

			// Token: 0x06000025 RID: 37 RVA: 0x000043A0 File Offset: 0x000025A0
			public static void Save()
			{
				try
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
					using (FileStream fileStream = new FileStream(Form1.Settings.filename, FileMode.Create, FileAccess.Write, FileShare.None))
					{
						binaryFormatter.Serialize(fileStream, Form1.Settings.dic);
						fileStream.Close();
					}
				}
				catch
				{
				}
			}

			// Token: 0x06000026 RID: 38 RVA: 0x00004408 File Offset: 0x00002608
			internal static void DeleteObject(string key)
			{
				if (Form1.Settings.dic.ContainsKey(key))
				{
					Form1.Settings.dic.Remove(key);
				}
			}

			// Token: 0x06000027 RID: 39 RVA: 0x00004423 File Offset: 0x00002623
			internal static bool ExistObject(string key)
			{
				return Form1.Settings.dic.ContainsKey(key);
			}

			// Token: 0x04000021 RID: 33
			private static Dictionary<string, object> dic;

			// Token: 0x04000022 RID: 34
			private static string mDataPath;
		}

		// Token: 0x02000004 RID: 4
		public class UsbSerial
		{
			// Token: 0x06000028 RID: 40 RVA: 0x00004430 File Offset: 0x00002630
			public void Configure(string port, int baud)
			{
				this.mPortName = port;
				this.mBaudRate = baud;
			}

			// Token: 0x06000029 RID: 41 RVA: 0x00004440 File Offset: 0x00002640
			public void Open()
			{
				if (!this.com.IsOpen)
				{
					try
					{
						this.com.DataBits = 8;
						this.com.Parity = Parity.None;
						this.com.StopBits = StopBits.One;
						this.com.Handshake = Handshake.None;
						this.com.PortName = this.mPortName;
						this.com.BaudRate = this.mBaudRate;
						this.com.NewLine = "\n";
						this.com.WriteTimeout = 1000;
						this.com.DtrEnable = Form1.reset_using_usb;
						this.com.Open();
						this.com.DiscardOutBuffer();
						this.com.DiscardInBuffer();
					}
					catch (IOException ex)
					{
						if (!char.IsDigit(this.mPortName[this.mPortName.Length - 1]) || !char.IsDigit(this.mPortName[this.mPortName.Length - 2]))
						{
							throw ex;
						}
						this.com.PortName = this.mPortName.Substring(0, this.mPortName.Length - 1);
						this.com.Open();
						this.com.DiscardOutBuffer();
						this.com.DiscardInBuffer();
					}
					catch
					{
						MessageBox.Show("COM Port might be busy, please check it and retry", "Warning");
					}
				}
			}

			// Token: 0x0600002A RID: 42 RVA: 0x000045C0 File Offset: 0x000027C0
			public void Close()
			{
				if (this.com.IsOpen)
				{
					this.com.DiscardOutBuffer();
					this.com.DiscardInBuffer();
					try
					{
						this.com.Close();
					}
					catch
					{
					}
				}
			}

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600002B RID: 43 RVA: 0x00004610 File Offset: 0x00002810
			public bool IsOpen
			{
				get
				{
					return this.com.IsOpen;
				}
			}

			// Token: 0x0600002C RID: 44 RVA: 0x00004620 File Offset: 0x00002820
			public void Write(char b)
			{
				this.com.Write(new byte[]
				{
					(byte)b
				}, 0, 1);
			}

			// Token: 0x0600002D RID: 45 RVA: 0x00004648 File Offset: 0x00002848
			public void Write(string s)
			{
				int i = 0;
				char[] array = s.ToCharArray();
				while (i < s.Length)
				{
					this.com.Write(new byte[]
					{
						(byte)array[i]
					}, 0, 1);
					i++;
				}
			}

			// Token: 0x0600002E RID: 46 RVA: 0x0000468C File Offset: 0x0000288C
			public char ReadByte()
			{
				int num = this.com.ReadByte();
				if (num == -1)
				{
					throw new EndOfStreamException("No COM data was available.");
				}
				return (char)num;
			}

			// Token: 0x0600002F RID: 47 RVA: 0x000046B6 File Offset: 0x000028B6
			public int HasData()
			{
				return this.com.BytesToRead;
			}

			// Token: 0x04000023 RID: 35
			private SerialPort com = new SerialPort();

			// Token: 0x04000024 RID: 36
			private string mPortName;

			// Token: 0x04000025 RID: 37
			private int mBaudRate;
		}

		// Token: 0x02000005 RID: 5
		public struct Comms
		{
			// Token: 0x04000026 RID: 38
			public Form1.UsbSerial com;
		}
	}
}
